using ConsoleTNKDxf.Dgts;
using netDxf;
using netDxf.Entities;
using netDxf.Tables;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleTNKDxf
{
    public class XDadosFormato
    {
        private const string APPNAME = "08478f494deb";
        DxfDocument _dxf;
        //Desenho _desenho;
        DesenhoDgt _desenhoDgt;

        //List<Line> _linhasHorizontais;
        //List<Line> _linhasVerticais;

        public XDadosFormato(DxfDocument dxf, DesenhoDgt desenhoDgt)//, RelatorioMultiDesenhos relatorio)
        {
            //_desenho = desenho;
            //_relatorio = relatorio;

            _desenhoDgt = desenhoDgt;

            _dxf = dxf;
           
        }

        public RespostaModelo InserirInformacoes()
        {

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Desenho: {_desenhoDgt.Title1}");

            var blocoLista = _dxf.Entities.Inserts.FirstOrDefault(x => x.Block.Name.StartsWith("FORMATO_DET_A1"));

            if (blocoLista == null)
            {
                return new RespostaModelo(false,null,"Bloco de formato não encontrado.");
            }

            var linhasHorizontais = blocoLista.Block.Entities.OfType<netDxf.Entities.Line>().Where(x => x.StartPoint.Y == x.EndPoint.Y).ToList();
            var linhasVerticais = blocoLista.Block.Entities.OfType<netDxf.Entities.Line>().Where(x => x.StartPoint.X == x.EndPoint.X).ToList();

            var linhaHorizontalMaisAlta = linhasHorizontais.OrderByDescending(x => x.StartPoint.Y).FirstOrDefault();
            inserirCamposFormatoDgt(linhaHorizontalMaisAlta);

            var linhaVerticalMaisEsquerda = linhasVerticais.OrderBy(x => x.StartPoint.X).FirstOrDefault();
            inserirRevisoes(linhaVerticalMaisEsquerda);

            if (_desenhoDgt.CriarLM == "SIM")
            {
                var linhaVerticalMaisDireita = linhasVerticais.OrderByDescending(x => x.StartPoint.X).FirstOrDefault();
                inserirDadosLM(linhaVerticalMaisDireita);
                Console.WriteLine("Inserida lista de materiais.");
                Console.ForegroundColor = ConsoleColor.Blue;
            }
            else
            {
                Console.WriteLine("Sem lista de materiais.");
                Console.ForegroundColor = ConsoleColor.Blue;
            }



            if (_desenhoDgt.CriarLM == "SIM")
            {
                var linhaHorizontalMaisBaixa = linhasHorizontais.OrderBy(x => x.StartPoint.Y).FirstOrDefault();
                inserirDoQuadroAplicacao(linhaHorizontalMaisBaixa);
            }
                

            return new RespostaModelo(true, null, "Informações do formato inseridas com sucesso.");
        }

        private void inserirDadosLM(Line linhaRef)
        {
            int numeroLinhaConjunto = 0;
            inserirConjuntosDgt(ref numeroLinhaConjunto, linhaRef);

            if(_desenhoDgt.ListarElementosObra == "SIM")
            {
                var elementosFixacao = _desenhoDgt.ElementosFixacao;
                if (elementosFixacao.Parafusos.Count > 0)
                {
                    numeroLinhaConjunto++;
                    inserirElementosFixacaoDgt(elementosFixacao, ref numeroLinhaConjunto, linhaRef);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Desenho: {_desenhoDgt.Title1}");
                    Console.WriteLine("Inserida lista de elementos de obra.");
                    Console.ForegroundColor = ConsoleColor.Blue;
                }
            }
            else
            {
               
                Console.WriteLine("Sem lista de elementos de obra.");
                Console.ForegroundColor = ConsoleColor.Blue;
            }

        }

        //private void inserirElementosFixacao(ElementosFixacao elementosFixacao, ref int numeroLinhaConjunto, Line linhaRef)
        //{
        //    var conjunto = new ConjuntoElementosFixacao();
        //    string pefixoFixacao = $"{APPNAME}_F_{++numeroLinhaConjunto}";
        //    ApplicationRegistry appReg;
        //    appReg = new ApplicationRegistry(pefixoFixacao);

        //    _dxf.ApplicationRegistries.Add(appReg);
        //    XData xdataConjuntoElementosFixacao = new XData(appReg);
        //    insereLinhaLM(conjunto, xdataConjuntoElementosFixacao, linhaRef);

        //    insereLinhas(elementosFixacao.Parafusos, $"{APPNAME}_PF_{numeroLinhaConjunto}_", linhaRef);
        //    insereLinhas(elementosFixacao.Porcas, $"{APPNAME}_PC_{numeroLinhaConjunto}_", linhaRef);
        //    insereLinhas(elementosFixacao.Arruelas, $"{APPNAME}_AR_{numeroLinhaConjunto}_", linhaRef);
        //}

        private void inserirElementosFixacaoDgt(ElementosFixacaoDgt elementosFixacao, ref int numeroLinhaConjunto, Line linhaRef)
        {
           int numeroLinha = inserirParafusosDgt(elementosFixacao.Parafusos, $"{APPNAME}_PF_{numeroLinhaConjunto}_", linhaRef);
           numeroLinha = inserirPorcasDgt(elementosFixacao.Porcas, $"{APPNAME}_PC_{numeroLinhaConjunto}_", linhaRef, numeroLinha);
            inserirArruelasDgt(elementosFixacao.Arruelas, $"{APPNAME}_AR_{numeroLinhaConjunto}_", linhaRef, numeroLinha);
        }


        //private void inserirConjuntos(ref int numeroLinhaConjunto, Line linhaRef)
        //{
        //    foreach (Conjunto conjunto in _desenho.ListaMateriais)
        //    {

        //        string appNameConjunto = $"{APPNAME}_M_{++numeroLinhaConjunto}";
        //        ApplicationRegistry appReg;
        //        appReg = new ApplicationRegistry(appNameConjunto);

        //        _dxf.ApplicationRegistries.Add(appReg);
        //        XData xdataConjunto = new XData(appReg);

        //        insereLinhaLM(conjunto, xdataConjunto, linhaRef);



        //        insereLinhas(conjunto.Pecas, $"{APPNAME}_I_{numeroLinhaConjunto}_", linhaRef);
        //    }

        //}
        private void inserirConjuntosDgt(ref int numeroLinhaConjunto, Line linhaRef)
        {
            foreach (ConjuntoDgt conjunto in _desenhoDgt.ListaMateriais)
            {

                string appNameConjunto = $"{APPNAME}_M_{++numeroLinhaConjunto}";
                ApplicationRegistry appReg;
                appReg = new ApplicationRegistry(appNameConjunto);

                _dxf.ApplicationRegistries.Add(appReg);
                XData xdataConjunto = new XData(appReg);

                insereConjuntoDgt(conjunto, xdataConjunto, linhaRef);



                insereListaPecasDgt(conjunto.Itens, $"{APPNAME}_I_{numeroLinhaConjunto}_", linhaRef);
            }

        }

        private int inserirParafusosDgt(List<ParafusoDgt> parafusos, string prefixo, Line linhaRef)
        {
            int numeroLinha = 0;
            foreach (ParafusoDgt parafuso in parafusos)
            {
                var appNameItem = $"{prefixo}{++numeroLinha}";
                ApplicationRegistry appReg;
                if (!_dxf.ApplicationRegistries.Contains(appNameItem))
                {
                    appReg = new ApplicationRegistry(appNameItem);
                    _dxf.ApplicationRegistries.Add(appReg);
                    XData xdata = new XData(appReg);
                    xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, parafuso.Name));
                    xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, parafuso.Quantidade.ToString()));
                    xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, parafuso.NameShort));
                    xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, parafuso.Weight.ToString()));
                    linhaRef.XData.Add(xdata);
                }
                else
                {
                    appReg = _dxf.ApplicationRegistries[appNameItem];
                }
            }

            return numeroLinha;

        }

        private int inserirPorcasDgt(List<PorcaDgt> porcas, string prefixo, Line linhaRef, int numeroLinha)
        {
            foreach (PorcaDgt porca in porcas)
            {
                var appNameItem = $"{prefixo}{++numeroLinha}";
                ApplicationRegistry appReg;
                if (!_dxf.ApplicationRegistries.Contains(appNameItem))
                {
                    appReg = new ApplicationRegistry(appNameItem);
                    _dxf.ApplicationRegistries.Add(appReg);
                    XData xdata = new XData(appReg);
                    xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, porca.NutName));
                    xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, porca.Quantidade.ToString()));
                    xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, porca.BoltStandard));
                    xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, porca.NutWeight.ToString()));
                    linhaRef.XData.Add(xdata);
                }
                else
                {
                    appReg = _dxf.ApplicationRegistries[appNameItem];
                }
            }

            return numeroLinha;
        }

        private void inserirArruelasDgt(List<ArruelaDgt> arruelas, string prefixo, Line linhaRef, int numeroLinha)
        {

            foreach (ArruelaDgt arruela in arruelas)
            {
                var appNameItem = $"{prefixo}{++numeroLinha}";
                ApplicationRegistry appReg;
                if (!_dxf.ApplicationRegistries.Contains(appNameItem))
                {
                    appReg = new ApplicationRegistry(appNameItem);
                    _dxf.ApplicationRegistries.Add(appReg);
                    XData xdata = new XData(appReg);
                    xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, arruela.WasherName));
                    xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, arruela.Quantidade.ToString()));
                    xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, arruela.BoltStandard));
                    xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, arruela.WasherWeight.ToString()));
                    linhaRef.XData.Add(xdata);
                }
                else
                {
                    appReg = _dxf.ApplicationRegistries[appNameItem];
                }
            }

        }

        //private void insereLinhas(List<ILinhaLM> linhas, string prefixo, Line linhaRef)
        //{
        //    int numeroLinha = 0;
        //    foreach (ILinhaLM linha in linhas)
        //    {
        //        var appNameItem = $"{prefixo}{++numeroLinha}";
        //        ApplicationRegistry appReg;
        //        if (!_dxf.ApplicationRegistries.Contains(appNameItem))
        //        {
        //            appReg = new ApplicationRegistry(appNameItem);
        //            _dxf.ApplicationRegistries.Add(appReg);
        //            XData xdata = new XData(appReg);
        //            insereLinhaLM(linha, xdata, linhaRef);
        //        }
        //        else
        //        {
        //            appReg = _dxf.ApplicationRegistries[appNameItem];
        //        }
        //    }

        //}

        private void insereListaPecasDgt(List<PecaDgt> linhas, string prefixo, Line linhaRef)
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

        }


        //private void insereLinhaLM(ILinhaLM linha, XData xdata, Line linhaRef)
        //{
        //    xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, linha.Posicao));
        //    xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, linha.Quantidade));
        //    xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, linha.Descricao));
        //    xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, linha.Observacao));
        //    xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, linha.Material));
        //    xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, linha.Peso));
        //    linhaRef.XData.Add(xdata);

        //}
        private void inserePecaDgt(PecaDgt linha, XData xdata, Line linhaRef)
        {
            xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, linha.PartPos));
            xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, linha.Quantidade.ToString()));
            xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, linha.Finish));
            xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, linha.Profile));
            xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, linha.ProfileType));
            xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, linha.Material));
            xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, linha.Height.ToString()));
            xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, linha.Length.ToString()));
            xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, linha.Width.ToString()));
            xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, linha.ProfileDiameter.ToString()));
            xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, linha.ProfilePlateThickness.ToString()));
            xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, linha.ProfileWeightPerUnitLength.ToString()));
            xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, linha.WeightNet.ToString()));
            xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, linha.WeightGross.ToString()));
            xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, linha.WeightM.ToString()));
            xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, linha.Weight.ToString()));
            linhaRef.XData.Add(xdata);

        }

        private void insereConjuntoDgt(ConjuntoDgt conjunto, XData xdata, Line linhaRef)
        {
            xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, conjunto.AssemblyPos));
            xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, conjunto.MainPartName));
            xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, conjunto.Quantidade.ToString()));
            xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, conjunto.Height.ToString()));
            xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, conjunto.Weigth.ToString()));
            linhaRef.XData.Add(xdata);
        }

        private void inserirDoQuadroAplicacao(Line linhaRef)
        {
            QuadroAplicacaoDgt quadro = _desenhoDgt.QuadroAplicacao;
            string appNameLinha = $"{APPNAME}_QA";
            ApplicationRegistry appReg;
            if (!_dxf.ApplicationRegistries.Contains(appNameLinha))
            {
                appReg = new ApplicationRegistry(appNameLinha);
                _dxf.ApplicationRegistries.Add(appReg);
                XData xdata = new XData(appReg);
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, quadro.Tag));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, quadro.Desenho));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, quadro.DesenhoCliente));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, quadro.Familia));

                linhaRef.XData.Add(xdata);
            }
            else
            {
                appReg = _dxf.ApplicationRegistries[APPNAME];
            }
        }

        private void inserirRevisoes(Line linhaRef)
        {

            RevisaoDgt revisao = _desenhoDgt.Revisao;

            string appNameLinha = $"{APPNAME}_Revisao";
            ApplicationRegistry appReg;
            if (!_dxf.ApplicationRegistries.Contains(appNameLinha))
            {
                appReg = new ApplicationRegistry(appNameLinha);
                _dxf.ApplicationRegistries.Add(appReg);
                XData xdata = new XData(appReg);
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, revisao.RevisionLastMark));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, revisao.RevisionLastDescription));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, revisao.RevisionLastCreatedBy));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, revisao.RevisionLastDateCreated));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, revisao.RevisionLastCheckedBy));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, revisao.RevisionLastDateChecked));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, revisao.RevisionLastApprovedBy));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, revisao.RevisionLastDateApproved));

                linhaRef.XData.Add(xdata);
            }
            else
            {
                appReg = _dxf.ApplicationRegistries[APPNAME];
            }

        }
        //private void inserirCamposFormato(Line linhaRef)
        //{

        //    //PropriedadesDesenho propriedades = _desenho.PropriedadesFormato; // _relatorio.PegaPropriedades(_nomeArquivo);

        //    ApplicationRegistry appReg;
        //    if (!_dxf.ApplicationRegistries.Contains(APPNAME))
        //    {
        //        appReg = new ApplicationRegistry(APPNAME);
        //        _dxf.ApplicationRegistries.Add(appReg);

        //        XData xdata = new XData(appReg);

        //        string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;


        //        xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, userName));
        //        xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, _desenho.NomeModelo));
        //        xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, _desenho.NumeroProjeto));
        //        xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, _desenho.Data));
        //        xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, _desenho.NumeroContratada));
        //        xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, _desenho.NumeroCliente));
        //        xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, _desenho.DescricaoProjeto));
        //        xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, _desenho.RevisaoFormato));
        //        xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, _desenho.RevisaoFormatoCliente));
        //        xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, _desenho.Escala));


        //        linhaRef.XData.Add(xdata);

        //    }
        //    else
        //    {
        //        appReg = _dxf.ApplicationRegistries[APPNAME];
        //    }
        //}

        private void inserirCamposFormatoDgt(Line linhaRef)
        {

            ApplicationRegistry appReg;
            if (!_dxf.ApplicationRegistries.Contains(APPNAME))
            {
                appReg = new ApplicationRegistry(APPNAME);
                _dxf.ApplicationRegistries.Add(appReg);

                XData xdata = new XData(appReg);

                string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;


                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, userName));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, _desenhoDgt.Title));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, _desenhoDgt.Title1));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, _desenhoDgt.Title2));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, _desenhoDgt.Title3));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, _desenhoDgt.ProjectObject));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, _desenhoDgt.RevisionMark));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, _desenhoDgt.ProjectModel));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, _desenhoDgt.ProjectNumber));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, _desenhoDgt.Scale1));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, _desenhoDgt.Scale2));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, _desenhoDgt.Scale3));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, _desenhoDgt.Scale4));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, _desenhoDgt.Scale5));

                linhaRef.XData.Add(xdata);

            }
            else
            {
                appReg = _dxf.ApplicationRegistries[APPNAME];
            }
        }

    }
}
