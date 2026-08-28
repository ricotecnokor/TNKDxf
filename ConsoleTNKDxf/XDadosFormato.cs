using ConsoleTNKDxf.Abstracoes;
using ConsoleTNKDxf.Dgts;
using netDxf;
using netDxf.Entities;
using netDxf.Tables;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleTNKDxf
{
    public class XDadosFormato<T> where T : ConjuntoAbstrato
    {
        protected LmAbs<T> _coletorLM;
        private const string APPNAME = "08478f494deb";
        DxfDocument _dxf;
        //DesenhoDgtAbs<T> _desenhoDgt;
        string _criarLM, _listarElementosObra;
        CamposDesenhoINP _camposFormatoTemplate;

        ElementosFixacaoDgt _elementosFixacaoDgt;


        public XDadosFormato(DxfDocument dxf, string criarLM, string listarElementosObra, CamposDesenhoINP camposFormato, ElementosFixacaoDgt elementosFixacaoDgt, LmAbs<T> coletorLM)//, DesenhoDgtAbs<T> desenhoDgt)//, RelatorioMultiDesenhos relatorio)
        {


            //_desenhoDgt = desenhoDgt;

            _dxf = dxf;
            _criarLM = criarLM;
            _listarElementosObra = listarElementosObra;
            _camposFormatoTemplate = camposFormato;
            _elementosFixacaoDgt = elementosFixacaoDgt;
            _coletorLM = coletorLM;
        }

        public RespostaModelo InserirInformacoes(string versaoTsep, string tipoDesenho)
        {

            var blocoLista = _dxf.Entities.Inserts.FirstOrDefault(x => x.Block.Name.StartsWith("FORMATO_DET_A1"));

            if (blocoLista == null)
            {
                return new RespostaModelo(false,null,"Bloco de formato não encontrado.");
            }

            var linhasHorizontais = blocoLista.Block.Entities.OfType<netDxf.Entities.Line>().Where(x => x.StartPoint.Y == x.EndPoint.Y).ToList();
            var linhasVerticais = blocoLista.Block.Entities.OfType<netDxf.Entities.Line>().Where(x => x.StartPoint.X == x.EndPoint.X).ToList();

            var linhaHorizontalMaisAlta = linhasHorizontais.OrderByDescending(x => x.StartPoint.Y).FirstOrDefault();
            inserirCamposFormatoDgt(linhaHorizontalMaisAlta, versaoTsep, tipoDesenho);

            var linhaVerticalMaisEsquerda = linhasVerticais.OrderBy(x => x.StartPoint.X).FirstOrDefault();
            //inserirRevisoes(linhaVerticalMaisEsquerda);



            if (_criarLM != "NÃO")
            {
                var linhaVerticalMaisDireita = linhasVerticais.OrderByDescending(x => x.StartPoint.X).FirstOrDefault();
                inserirDadosLM(linhaVerticalMaisDireita);
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.Green;
            }
            



            if (_criarLM != "NÃO")
            {
                var linhaHorizontalMaisBaixa = linhasHorizontais.OrderBy(x => x.StartPoint.Y).FirstOrDefault();
                inserirDoQuadroAplicacao(linhaHorizontalMaisBaixa, _camposFormatoTemplate);
            }
                

            return new RespostaModelo(true, null, "Informações do formato inseridas com sucesso.");
        }

        private void inserirDadosLM(Line linhaRef)
        {
            int numeroLinhaConjunto = 0;

           

            inserirConjuntosDgt(ref numeroLinhaConjunto, linhaRef);

           

            if (_listarElementosObra != "NÃO")
            {
                if (_elementosFixacaoDgt != null && _elementosFixacaoDgt.FixacaoObra.Count > 0)
                {
                    numeroLinhaConjunto++;
                    inserirElementosObraDgt(_elementosFixacaoDgt, ref numeroLinhaConjunto, linhaRef);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Desenho: {_camposFormatoTemplate.Title1}");
                    Console.WriteLine("Inserida lista de elementos de obra.");
                    Console.ForegroundColor = ConsoleColor.Green;
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Sem lista de elementos de obra.");
                Console.ForegroundColor = ConsoleColor.Green;
            }

            inserirElementosFabricaDgt(_elementosFixacaoDgt, ref numeroLinhaConjunto, linhaRef);

        }

        private void inserirElementosObraDgt(ElementosFixacaoDgt elementosFixacao, ref int numeroLinhaConjunto, Line linhaRef)
        {
           int numeroLinha = inserirParafusosDgt(elementosFixacao.FixacaoObra, $"{APPNAME}_PF_{numeroLinhaConjunto}_", linhaRef);
           numeroLinha = inserirPorcasDgt(elementosFixacao.FixacaoObra, $"{APPNAME}_PC_{numeroLinhaConjunto}_", linhaRef, numeroLinha);
            inserirArruelasDgt(elementosFixacao.FixacaoObra, $"{APPNAME}_AR_{numeroLinhaConjunto}_", linhaRef, numeroLinha);
        }

        private void inserirElementosFabricaDgt(ElementosFixacaoDgt elementosFixacao, ref int numeroLinhaConjunto, Line linhaRef)
        {
            int numeroLinha = inserirParafusosDgt(elementosFixacao.FixacaoFabrica, $"{APPNAME}_PF_FAB_{numeroLinhaConjunto}_", linhaRef);
            numeroLinha = inserirPorcasDgt(elementosFixacao.FixacaoFabrica, $"{APPNAME}_PC_FAB_{numeroLinhaConjunto}_", linhaRef, numeroLinha);
            inserirArruelasDgt(elementosFixacao.FixacaoFabrica, $"{APPNAME}_AR_FAB_{numeroLinhaConjunto}_", linhaRef, numeroLinha);
        }

        private void inserirConjuntosDgt(ref int numeroLinhaConjunto, Line linhaRef)
        {
            foreach (T conjunto in _coletorLM)
            {

                string appNameConjunto = $"{APPNAME}_M_{++numeroLinhaConjunto}";
                ApplicationRegistry appReg;
                appReg = new ApplicationRegistry(appNameConjunto);

                _dxf.ApplicationRegistries.Add(appReg);
                XData xdataConjunto = new XData(appReg);

                insereConjuntoDgt(conjunto as ConjuntoAbstrato, xdataConjunto, linhaRef);

                if(conjunto is ConjuntoDetalhadoDgt detalhado)
                {
                   int numeroLinha = insereListaPecasDgt(detalhado.Itens, $"{APPNAME}_I_{numeroLinhaConjunto}_", linhaRef);
                   insereListaFixacao(detalhado.FixacaoFabrica, $"{APPNAME}_I_{numeroLinhaConjunto}_", linhaRef, numeroLinha, "F");
                   insereListaFixacao(detalhado.FixacaoObra, $"{APPNAME}_I_{numeroLinhaConjunto}_", linhaRef, numeroLinha, "O");
                }



            }

        }

       

        private int inserirParafusosDgt(List<FixacaoDgt> elementosFixacao, string prefixo, Line linhaRef)
        {
            int numeroLinha = 0;
            foreach (FixacaoDgt elementoFixacao in elementosFixacao)
            {
                var appNameItem = $"{prefixo}{++numeroLinha}";
                ApplicationRegistry appReg;
                if (!_dxf.ApplicationRegistries.Contains(appNameItem))
                {
                    appReg = new ApplicationRegistry(appNameItem);
                    _dxf.ApplicationRegistries.Add(appReg);
                    XData xdata = new XData(appReg);
                    xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, elementoFixacao.Parafuso.Name == null ? "" : elementoFixacao.Parafuso.Name));
                    xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, elementoFixacao.Parafuso.Quantidade == null ? "" : elementoFixacao.Parafuso.Quantidade.ToString()));
                    xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, elementoFixacao.Parafuso.NameShort == null ? "" : elementoFixacao.Parafuso.NameShort));
                    xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, elementoFixacao.Parafuso.Weight == null ? "" : elementoFixacao.Parafuso.Weight.ToString()));
                    linhaRef.XData.Add(xdata);
                }
                else
                {
                    appReg = _dxf.ApplicationRegistries[appNameItem];
                }
            }

            return numeroLinha;

        }

        private int inserirPorcasDgt(List<FixacaoDgt> elementosFixacao, string prefixo, Line linhaRef, int numeroLinha)
        {
            foreach (FixacaoDgt elementoFixacao in elementosFixacao)
            {
                var appNameItem = $"{prefixo}{++numeroLinha}";
                ApplicationRegistry appReg;
                if (!_dxf.ApplicationRegistries.Contains(appNameItem))
                {
                    appReg = new ApplicationRegistry(appNameItem);
                    _dxf.ApplicationRegistries.Add(appReg);
                    XData xdata = new XData(appReg);
                    xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, elementoFixacao.Porca.NutName == null ? "" : elementoFixacao.Porca.NutName));
                    xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, elementoFixacao.Porca.Quantidade == null ? "" : elementoFixacao.Porca.Quantidade.ToString()));
                    xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, elementoFixacao.Porca.BoltStandard == null ? "" : elementoFixacao.Porca.BoltStandard));
                    xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, elementoFixacao.Porca.NutWeight == null ? "" : elementoFixacao.Porca.NutWeight.ToString()));
                    linhaRef.XData.Add(xdata);
                }
                else
                {
                    appReg = _dxf.ApplicationRegistries[appNameItem];
                }
            }

            return numeroLinha;
        }

        private void inserirArruelasDgt(List<FixacaoDgt> elementosFixacao, string prefixo, Line linhaRef, int numeroLinha)
        {

            foreach (FixacaoDgt elementoFixacao in elementosFixacao)
            {
                var appNameItem = $"{prefixo}{++numeroLinha}";
                ApplicationRegistry appReg;
                if (!_dxf.ApplicationRegistries.Contains(appNameItem))
                {
                    appReg = new ApplicationRegistry(appNameItem);
                    _dxf.ApplicationRegistries.Add(appReg);
                    XData xdata = new XData(appReg);
                    xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, elementoFixacao.Arruela.WasherName == null ? "" : elementoFixacao.Arruela.WasherName));
                    xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, elementoFixacao.Arruela.Quantidade == null ? "" : elementoFixacao.Arruela.Quantidade.ToString()));
                    xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, elementoFixacao.Arruela.BoltStandard == null ? "" : elementoFixacao.Arruela.BoltStandard));
                    xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, elementoFixacao.Arruela.WasherWeight == null ? "" : elementoFixacao.Arruela.WasherWeight.ToString()));
                    linhaRef.XData.Add(xdata);
                }
                else
                {
                    appReg = _dxf.ApplicationRegistries[appNameItem];
                }
            }

        }

      

        private int insereListaPecasDgt(List<PecaDgt> linhas, string prefixo, Line linhaRef)
        {
            int numeroLinha = 0;
            foreach (PecaDgt linha in linhas)
            {
                var appNameItem = $"{prefixo}{++numeroLinha}";
                ApplicationRegistry appReg;
                if (!_dxf.ApplicationRegistries.Contains(appNameItem))
                {
                    appReg = new ApplicationRegistry(appNameItem);
                    _dxf.ApplicationRegistries.Add(appReg);
                    XData xdata = new XData(appReg);
                    inserePecaDgt(linha, xdata, linhaRef);
                }
                else
                {
                    appReg = _dxf.ApplicationRegistries[appNameItem];
                }
            }

            return numeroLinha;
        }

        private void insereListaFixacao(List<FixacaoDgt> fixacoes, string prefixo, Line linhaRef, int numeroLinha, string tipoMontagem)
        {
            numeroLinha = insereParafusosFixacaoFabrica(prefixo + $"PF{tipoMontagem}_", linhaRef, numeroLinha, fixacoes.Select(f => f.Parafuso).ToList());
            numeroLinha = inserePorcasFixacaoFabrica(prefixo + $"PC{tipoMontagem}_", linhaRef, numeroLinha, fixacoes.Select(f => f.Porca).ToList());
            numeroLinha = insereArruelasFixacaoFabrica(prefixo + $"AR{tipoMontagem}_", linhaRef, numeroLinha, fixacoes.Select(f => f.Arruela).ToList());

        }

        private int insereArruelasFixacaoFabrica(string prefixo, Line linhaRef, int numeroLinha, List<ArruelaDgt> arruelaDgts)
        {
            foreach (ArruelaDgt arruela in arruelaDgts)
            {
                if(arruela == null)
                {
                    continue;
                }
                var appNameItem = $"{prefixo}{++numeroLinha}";
                ApplicationRegistry appReg;
                if (!_dxf.ApplicationRegistries.Contains(appNameItem))
                {
                    appReg = new ApplicationRegistry(appNameItem);
                    _dxf.ApplicationRegistries.Add(appReg);
                    XData xdata = new XData(appReg);
                    insereArruelaFabricaDgt(arruela, xdata, linhaRef);
                }
                else
                {
                    appReg = _dxf.ApplicationRegistries[appNameItem];
                }
            }

            return numeroLinha;
        }

        

        private int insereParafusosFixacaoFabrica(string prefixo, Line linhaRef, int numeroLinha, List<ParafusoDgt> parafusos)
        {
            foreach (ParafusoDgt paraf in parafusos)
            {
                var appNameItem = $"{prefixo}{++numeroLinha}";
                ApplicationRegistry appReg;
                if (!_dxf.ApplicationRegistries.Contains(appNameItem))
                {
                    appReg = new ApplicationRegistry(appNameItem);
                    _dxf.ApplicationRegistries.Add(appReg);
                    XData xdata = new XData(appReg);
                    insereParafusoFabricaDgt(paraf, xdata, linhaRef);
                }
                else
                {
                    appReg = _dxf.ApplicationRegistries[appNameItem];
                }
            }

            return numeroLinha;
        }

        private int inserePorcasFixacaoFabrica(string prefixo, Line linhaRef, int numeroLinha, List<PorcaDgt> porcas)
        {
            foreach (PorcaDgt porca in porcas)
            {
                if(porca == null)
                    continue;
                var appNameItem = $"{prefixo}{++numeroLinha}";
                ApplicationRegistry appReg;
                if (!_dxf.ApplicationRegistries.Contains(appNameItem))
                {
                    appReg = new ApplicationRegistry(appNameItem);
                    _dxf.ApplicationRegistries.Add(appReg);
                    XData xdata = new XData(appReg);
                    inserePorcaFabricaDgt(porca, xdata, linhaRef);
                }
                else
                {
                    appReg = _dxf.ApplicationRegistries[appNameItem];
                }
            }

            return numeroLinha;
        }

        private void insereParafusoFabricaDgt(ParafusoDgt linha, XData xdata, Line linhaRef)
        {
            xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, linha.Name == null ? "" : linha.Name));
            xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, linha.Quantidade == null ? "" : linha.Quantidade.ToString()));
            xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, linha.NameShort == null ? "" : linha.NameShort));
            xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, linha.Weight == null ? "" : linha.Weight.ToString()));
            xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, linha.Profile == null ? "" : linha.Profile));
            xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, linha.PecaChega.Posicao == null ? "" : linha.PecaChega.Posicao));
            xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, linha.PecaRecebe.Posicao == null ? "" : linha.PecaRecebe.Posicao));
            xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, linha.Montagem == null ? "" : linha.Montagem));
            linhaRef.XData.Add(xdata);

        }

        private void inserePorcaFabricaDgt(PorcaDgt linha, XData xdata, Line linhaRef)
        {
            xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, linha.NutName == null ? "" : linha.NutName));
            xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, linha.BoltStandard == null ? "" : linha.BoltStandard));
            xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, linha.Quantidade == null ? "" : linha.Quantidade.ToString()));
            xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, linha.NutWeight == null ? "" : linha.NutWeight.ToString()));
            linhaRef.XData.Add(xdata);
        }

        private void insereArruelaFabricaDgt(ArruelaDgt arruela, XData xdata, Line linhaRef)
        {
            xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, arruela.WasherName == null ? "" : arruela.WasherName));
            xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, arruela.BoltStandard == null ? "" : arruela.BoltStandard));
            xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, arruela.Quantidade == null ? "" : arruela.Quantidade.ToString()));
            xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, arruela.WasherWeight == null ? "" : arruela.WasherWeight.ToString()));
            linhaRef.XData.Add(xdata);
        }

        private void inserePecaDgt(PecaDgt linha, XData xdata, Line linhaRef)
        {
            xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, linha.PartPos == null ? "" : linha.PartPos));
            xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, linha.Quantidade == null ? "" : linha.Quantidade.ToString()));
            xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, linha.Finish == null ? "" : linha.Finish));
            xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, linha.Profile == null ? "" : linha.Profile));
            xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, linha.ProfileType == null ? "" : linha.ProfileType));
            xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, linha.Material == null ? "" : linha.Material));
            xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, linha.Height == null ? "" : linha.Height.ToString()));
            xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, linha.Length == null ? "" : linha.Length.ToString()));
            xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, linha.Width == null ? "" : linha.Width.ToString()));
            xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, linha.ProfileDiameter == null ? "" : linha.ProfileDiameter.ToString()));
            xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, linha.ProfilePlateThickness == null ? "" : linha.ProfilePlateThickness.ToString()));
            xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, linha.ProfileWeightPerUnitLength == null ? "" : linha.ProfileWeightPerUnitLength.ToString()));
            xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, linha.WeightNet == null ? "" : linha.WeightNet.ToString()));
            xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, linha.WeightGross == null ? "" : linha.WeightGross.ToString()));
            xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, linha.WeightM == null ? "" : linha.WeightM.ToString()));
            xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, linha.Weight == null ? "" : linha.Weight.ToString()));
            linhaRef.XData.Add(xdata);

        }

        private void insereConjuntoDgt(ConjuntoAbstrato conjunto, XData xdata, Line linhaRef)
        {
            xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, conjunto.AssemblyPos == null ? "" : conjunto.AssemblyPos));
            xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, conjunto.MainPartName == null ? "" : conjunto.MainPartName));
            xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, conjunto.Quantidade == null ? "" : conjunto.Quantidade.ToString()));
            xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, conjunto.Height == null ? "" : conjunto.Height.ToString()));
            xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, conjunto.Weigth == null ? "" : conjunto.Weigth.ToString()));
            linhaRef.XData.Add(xdata);
        }

        private void inserirDoQuadroAplicacao(Line linhaRef, CamposDesenhoINP camposFormato)
        {
            string appNameLinha = $"{APPNAME}_QA";
            ApplicationRegistry appReg;
            if (!_dxf.ApplicationRegistries.Contains(appNameLinha))
            {
                appReg = new ApplicationRegistry(appNameLinha);
                _dxf.ApplicationRegistries.Add(appReg);
                XData xdata = new XData(appReg);
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, camposFormato.TagQA == null ? "" : camposFormato.TagQA));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, camposFormato.QtdQA == 0 ? "" : camposFormato.QtdQA.ToString()));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, camposFormato.DesenhoQA == null ? "" : camposFormato.DesenhoQA));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, camposFormato.DesenhoClienteQA == null ? "" : camposFormato.DesenhoClienteQA));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, camposFormato.FamiliaQA == 0 ? "" : camposFormato.FamiliaQA.ToString()));

                linhaRef.XData.Add(xdata);
            }
            else
            {
                appReg = _dxf.ApplicationRegistries[APPNAME];
            }
        }

        
       

        private void inserirCamposFormatoDgt(Line linhaRef, string versaoTsep, string tipoDesenho)
        {

            ApplicationRegistry appReg;
            if (!_dxf.ApplicationRegistries.Contains(APPNAME))
            {
                appReg = new ApplicationRegistry(APPNAME);
                _dxf.ApplicationRegistries.Add(appReg);

                XData xdata = new XData(appReg);

                string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;


                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, userName));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, _camposFormatoTemplate.Title == null ? "TITLE" : _camposFormatoTemplate.Title));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, _camposFormatoTemplate.Title1 == null ? "TITLE1" : _camposFormatoTemplate.Title1));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, _camposFormatoTemplate.Title2 == null ? "TITLE2" : _camposFormatoTemplate.Title2));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, _camposFormatoTemplate.Title3 == null ? "TITLE3" : _camposFormatoTemplate.Title3));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, _camposFormatoTemplate.ProjectObject == null ? "PROJECT OBJECT" : _camposFormatoTemplate.ProjectObject));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, _camposFormatoTemplate.RevisionMark == null ? "0" : _camposFormatoTemplate.RevisionMark));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, _camposFormatoTemplate.ProjectModel == null ? "MODELO" : _camposFormatoTemplate.ProjectModel));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, _camposFormatoTemplate.ProjectNumber == null ? "NUMERO PROJETO" : _camposFormatoTemplate.ProjectNumber));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, _camposFormatoTemplate.Scale1 == null ? "" : _camposFormatoTemplate.Scale1));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, _camposFormatoTemplate.Scale2 == null ? "" : _camposFormatoTemplate.Scale2));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, _camposFormatoTemplate.Scale3 == null ? "" : _camposFormatoTemplate.Scale3));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, _camposFormatoTemplate.Scale4 == null ? "" : _camposFormatoTemplate.Scale4));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, _camposFormatoTemplate.Scale5 == null ? "" : _camposFormatoTemplate.Scale5));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, versaoTsep));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, tipoDesenho));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, _camposFormatoTemplate.Name == null ? "NAME" : _camposFormatoTemplate.Name));
                linhaRef.XData.Add(xdata);

            }
            else
            {
                appReg = _dxf.ApplicationRegistries[APPNAME];
            }
        }

    }
}
