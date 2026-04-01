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
        DesenhoDgt _desenhoDgt;


        public XDadosFormato(DxfDocument dxf, DesenhoDgt desenhoDgt)//, RelatorioMultiDesenhos relatorio)
        {


            _desenhoDgt = desenhoDgt;

            _dxf = dxf;
           
        }

        public RespostaModelo InserirInformacoes(string versaoTsep)
        {

            var blocoLista = _dxf.Entities.Inserts.FirstOrDefault(x => x.Block.Name.StartsWith("FORMATO_DET_A1"));

            if (blocoLista == null)
            {
                return new RespostaModelo(false,null,"Bloco de formato não encontrado.");
            }

            var linhasHorizontais = blocoLista.Block.Entities.OfType<netDxf.Entities.Line>().Where(x => x.StartPoint.Y == x.EndPoint.Y).ToList();
            var linhasVerticais = blocoLista.Block.Entities.OfType<netDxf.Entities.Line>().Where(x => x.StartPoint.X == x.EndPoint.X).ToList();

            var linhaHorizontalMaisAlta = linhasHorizontais.OrderByDescending(x => x.StartPoint.Y).FirstOrDefault();
            inserirCamposFormatoDgt(linhaHorizontalMaisAlta, versaoTsep);

            var linhaVerticalMaisEsquerda = linhasVerticais.OrderBy(x => x.StartPoint.X).FirstOrDefault();
            inserirRevisoes(linhaVerticalMaisEsquerda);



            if (_desenhoDgt.CriarLM != "NÃO")
            {
                var linhaVerticalMaisDireita = linhasVerticais.OrderByDescending(x => x.StartPoint.X).FirstOrDefault();
                inserirDadosLM(linhaVerticalMaisDireita);
                Console.WriteLine("Inserida lista de materiais.");
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Sem lista de materiais.");
                Console.ForegroundColor = ConsoleColor.Green;
            }
            



            if (_desenhoDgt.CriarLM != "NÃO")
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

            if(_desenhoDgt.ListarElementosObra != "NÃO")
            {
                var elementosFixacao = _desenhoDgt.ElementosFixacao;
                if (elementosFixacao.Parafusos.Count > 0)
                {
                    numeroLinhaConjunto++;
                    inserirElementosFixacaoDgt(elementosFixacao, ref numeroLinhaConjunto, linhaRef);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Desenho: {_desenhoDgt.Title1}");
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

        }

        private void inserirElementosFixacaoDgt(ElementosFixacaoDgt elementosFixacao, ref int numeroLinhaConjunto, Line linhaRef)
        {
           int numeroLinha = inserirParafusosDgt(elementosFixacao.Parafusos, $"{APPNAME}_PF_{numeroLinhaConjunto}_", linhaRef);
           numeroLinha = inserirPorcasDgt(elementosFixacao.Porcas, $"{APPNAME}_PC_{numeroLinhaConjunto}_", linhaRef, numeroLinha);
            inserirArruelasDgt(elementosFixacao.Arruelas, $"{APPNAME}_AR_{numeroLinhaConjunto}_", linhaRef, numeroLinha);
        }


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
                    xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, parafuso.Name == null ? "" : parafuso.Name));
                    xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, parafuso.Quantidade == null ? "" : parafuso.Quantidade.ToString()));
                    xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, parafuso.NameShort == null ? "" : parafuso.NameShort));
                    xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, parafuso.Weight == null ? "" : parafuso.Weight.ToString()));
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
                    xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, porca.NutName == null ? "" : porca.NutName));
                    xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, porca.Quantidade == null ? "" : porca.Quantidade.ToString()));
                    xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, porca.BoltStandard == null ? "" : porca.BoltStandard));
                    xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, porca.NutWeight == null ? "" : porca.NutWeight.ToString()));
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
                    xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, arruela.WasherName == null ? "" : arruela.WasherName));
                    xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, arruela.Quantidade == null ? "" : arruela.Quantidade.ToString()));
                    xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, arruela.BoltStandard == null ? "" : arruela.BoltStandard));
                    xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, arruela.WasherWeight == null ? "" : arruela.WasherWeight.ToString()));
                    linhaRef.XData.Add(xdata);
                }
                else
                {
                    appReg = _dxf.ApplicationRegistries[appNameItem];
                }
            }

        }

      

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

        private void insereConjuntoDgt(ConjuntoDgt conjunto, XData xdata, Line linhaRef)
        {
            xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, conjunto.AssemblyPos == null ? "" : conjunto.AssemblyPos));
            xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, conjunto.MainPartName == null ? "" : conjunto.MainPartName));
            xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, conjunto.Quantidade == null ? "" : conjunto.Quantidade.ToString()));
            xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, conjunto.Height == null ? "" : conjunto.Height.ToString()));
            xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, conjunto.Weigth == null ? "" : conjunto.Weigth.ToString()));
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
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, quadro.Tag == null ? "" : quadro.Tag));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, quadro.Desenho == null ? "" : quadro.Desenho));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, quadro.DesenhoCliente == null ? "" : quadro.DesenhoCliente));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, quadro.Familia == null ? "" : quadro.Familia));

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
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, revisao.RevisionLastMark == null ? "0" : revisao.RevisionLastMark));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, revisao.RevisionLastDescription == null ? "" : revisao.RevisionLastDescription));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, revisao.RevisionLastCreatedBy == null ? "" : revisao.RevisionLastCreatedBy));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, revisao.RevisionLastDateCreated == null ? "" : revisao.RevisionLastDateCreated));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, revisao.RevisionLastCheckedBy == null ? "" : revisao.RevisionLastCheckedBy));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, revisao.RevisionLastDateChecked == null ? "" : revisao.RevisionLastDateChecked));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, revisao.RevisionLastApprovedBy == null ? "" : revisao.RevisionLastApprovedBy));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, revisao.RevisionLastDateApproved == null ? "" : revisao.RevisionLastDateApproved));

                linhaRef.XData.Add(xdata);
            }
            else
            {
                appReg = _dxf.ApplicationRegistries[APPNAME];
            }

        }
       

        private void inserirCamposFormatoDgt(Line linhaRef, string versaoTsep)
        {

            ApplicationRegistry appReg;
            if (!_dxf.ApplicationRegistries.Contains(APPNAME))
            {
                appReg = new ApplicationRegistry(APPNAME);
                _dxf.ApplicationRegistries.Add(appReg);

                XData xdata = new XData(appReg);

                string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;


                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, userName));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, _desenhoDgt.Title == null ? "TITLE" : _desenhoDgt.Title));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, _desenhoDgt.Title1 == null ? "TITLE1" : _desenhoDgt.Title1));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, _desenhoDgt.Title2 == null ? "TITLE2" : _desenhoDgt.Title2));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, _desenhoDgt.Title3 == null ? "TITLE3" : _desenhoDgt.Title3));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, _desenhoDgt.ProjectObject == null ? "PROJECT OBJECT" : _desenhoDgt.ProjectObject));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, _desenhoDgt.RevisionMark == null ? "0" : _desenhoDgt.RevisionMark));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, _desenhoDgt.ProjectModel == null ? "MODELO" : _desenhoDgt.ProjectModel));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, _desenhoDgt.ProjectNumber == null ? "NUMERO PROJETO" : _desenhoDgt.ProjectNumber));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, _desenhoDgt.Scale1 == null ? "" : _desenhoDgt.Scale1));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, _desenhoDgt.Scale2 == null ? "" : _desenhoDgt.Scale2));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, _desenhoDgt.Scale3 == null ? "" : _desenhoDgt.Scale3));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, _desenhoDgt.Scale4 == null ? "" : _desenhoDgt.Scale4));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, _desenhoDgt.Scale5 == null ? "" : _desenhoDgt.Scale5));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, versaoTsep));

                linhaRef.XData.Add(xdata);

            }
            else
            {
                appReg = _dxf.ApplicationRegistries[APPNAME];
            }
        }

    }
}
