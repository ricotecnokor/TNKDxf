using netDxf;
using netDxf.Entities;
using netDxf.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using static Tekla.Structures.Model.ReferenceModel;

namespace ConsoleTNKDxf
{
    public class XDadosFormato
    {
        private const string APPNAME = "08478f494deb";
        DxfDocument _dxf;
        Desenho _desenho;
        //RelatorioMultiDesenhos _relatorio;
        //string _nomeArquivo;

        List<Line> _linhasHorizontais;
        List<Line> _linhasVerticais;
        //netDxf.Entities.Line _linhaVerticalMaisEsquerda;
        //netDxf.Entities.Line _linhaVerticalMaisDireita;

        public XDadosFormato(DxfDocument dxf, Desenho desenho)//, RelatorioMultiDesenhos relatorio)
        {
            _desenho = desenho;
            //_relatorio = relatorio;
            _dxf = dxf;
            var blocoLista = _dxf.Entities.Inserts.FirstOrDefault(x => x.Block.Name.StartsWith("FORMATO_DET_A1"));
            _linhasHorizontais = blocoLista.Block.Entities.OfType<netDxf.Entities.Line>().Where(x => x.StartPoint.Y == x.EndPoint.Y).ToList();
            _linhasVerticais = blocoLista.Block.Entities.OfType<netDxf.Entities.Line>().Where(x => x.StartPoint.X == x.EndPoint.X).ToList();
        }

        public void InserirInformacoes()
        {
            var linhaHorizontalMaisAlta = _linhasHorizontais.OrderByDescending(x => x.StartPoint.Y).FirstOrDefault();
            inserirCamposFormato(linhaHorizontalMaisAlta);

            var linhaVerticalMaisEsquerda = _linhasVerticais.OrderBy(x => x.StartPoint.X).FirstOrDefault();
            inserirRevisoes(linhaVerticalMaisEsquerda);

            var linhaVerticalMaisDireita = _linhasVerticais.OrderByDescending(x => x.StartPoint.X).FirstOrDefault();
            inserirDadosLM(linhaVerticalMaisDireita);

            var linhaHorizontalMaisBaixa = _linhasHorizontais.OrderBy(x => x.StartPoint.Y).FirstOrDefault();
            inserirDoQuadroAplicacao(linhaHorizontalMaisBaixa);
        }

        private void inserirDadosLM(Line linhaRef)
        {
            int numeroLinha = 0;
            inserirConjuntos(ref numeroLinha, linhaRef);
            var elementosFixacao = _desenho.ElementosFixacao;
            if (elementosFixacao.Parafusos.Count > 0)
            {
                insereLinhas(elementosFixacao.Parafusos, $"{APPNAME}_PF_L_", ref numeroLinha, linhaRef);
                insereLinhas(elementosFixacao.Porcas, $"{APPNAME}_PC_L_", ref numeroLinha, linhaRef);
                insereLinhas(elementosFixacao.Arruelas, $"{APPNAME}_AR_L_", ref numeroLinha, linhaRef);
            }
        }

        private void inserirConjuntos(ref int numeroLinha, Line linhaRef)
        {
            foreach (Conjunto conjunto in _desenho.ListaMateriais)
            {
                var posicaoConjunto = conjunto.Posicao;

                string appNameLinha = $"{APPNAME}_M_{posicaoConjunto}_{++numeroLinha}";
                ApplicationRegistry appReg;
                appReg = new ApplicationRegistry(appNameLinha);

                _dxf.ApplicationRegistries.Add(appReg);
                XData xdataConjunto = new XData(appReg);

                insereLinhaLM(conjunto, xdataConjunto, linhaRef);

                

                insereLinhas(conjunto.Pecas, $"{APPNAME}_I_{posicaoConjunto}_", ref numeroLinha, linhaRef);
            }

        }


        private void insereLinhas(List<ILinhaLM> linhas, string prefixo, ref int numeroLinha, Line linhaRef)
        {
            foreach (ILinhaLM linha in linhas)
            {
                var appNameLinha = $"{prefixo}_{++numeroLinha}";
                ApplicationRegistry appReg;
                if (!_dxf.ApplicationRegistries.Contains(appNameLinha))
                {
                    appReg = new ApplicationRegistry(appNameLinha);
                    _dxf.ApplicationRegistries.Add(appReg);
                    XData xdata = new XData(appReg);
                    insereLinhaLM(linha, xdata, linhaRef);
                }
                else
                {
                    appReg = _dxf.ApplicationRegistries[appNameLinha];
                }
            }

        }

        private void insereLinhaLM(ILinhaLM linha, XData xdata, Line linhaRef)
        {
            xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, linha.Posicao));
            xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, linha.Quantidade));
            xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, linha.Descricao));
            xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, linha.Observacao));
            xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, linha.Material));
            xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, linha.Peso));
            linhaRef.XData.Add(xdata);

        }

        private void inserirDoQuadroAplicacao(Line linhaRef)
        {
            QuadroAplicacao quadro = _desenho.QuadroAplicacao;
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

            Revisao revisao = _desenho.QuadroRevisao;

            string appNameLinha = $"{APPNAME}_Revisao";
            ApplicationRegistry appReg;
            if (!_dxf.ApplicationRegistries.Contains(appNameLinha))
            {
                appReg = new ApplicationRegistry(appNameLinha);
                _dxf.ApplicationRegistries.Add(appReg);
                XData xdata = new XData(appReg);
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, revisao.RevisionMark));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, revisao.Descricao));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, revisao.CriadoPor));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, revisao.DataCriacao));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, revisao.ChecadoPor));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, revisao.DataChecado));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, revisao.AprovadoPor));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, revisao.DataAprovacao));

                linhaRef.XData.Add(xdata);
            }
            else
            {
                appReg = _dxf.ApplicationRegistries[APPNAME];
            }

        }
        private void inserirCamposFormato(Line linhaRef)
        {

            //PropriedadesDesenho propriedades = _desenho.PropriedadesFormato; // _relatorio.PegaPropriedades(_nomeArquivo);

            ApplicationRegistry appReg;
            if (!_dxf.ApplicationRegistries.Contains(APPNAME))
            {
                appReg = new ApplicationRegistry(APPNAME);
                _dxf.ApplicationRegistries.Add(appReg);

                XData xdata = new XData(appReg);

                string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;


                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, userName));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, _desenho.NomeModelo));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, _desenho.NumeroProjeto));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, _desenho.Data));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, _desenho.NumeroContratada));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, _desenho.NumeroCliente));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, _desenho.DescricaoProjeto));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, _desenho.RevisaoFormato));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, _desenho.RevisaoFormatoCliente));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, _desenho.Escala));


                linhaRef.XData.Add(xdata);

            }
            else
            {
                appReg = _dxf.ApplicationRegistries[APPNAME];
            }
        }

    }
}
