using netDxf;
using netDxf.Entities;
using netDxf.Tables;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleTNKDxf
{
    public class XDados
    {
        DxfDocument _dxf;
        Desenho _desenho;
        RelatorioMultiDesenhos _relatorio;
        string _nomeArquivo;
    
        netDxf.Entities.Line _linhaHorizontalMaisAlta;
        netDxf.Entities.Line _linhaVerticalMaisEsquerda;
        netDxf.Entities.Line _linhaVerticalMaisDireita;

        public XDados(DxfDocument dxf, string nomeArquivo, Desenho desenho, RelatorioMultiDesenhos relatorio)
        {
            _nomeArquivo = nomeArquivo;
            _desenho = desenho;
            _relatorio = relatorio;
            _dxf = dxf;
            var blocoLista = _dxf.Entities.Inserts.FirstOrDefault(x => x.Block.Name.StartsWith("FORMATO_DET_A1"));

            var linhasHorizontais = blocoLista.Block.Entities.OfType<netDxf.Entities.Line>().Where(x => x.StartPoint.Y == x.EndPoint.Y).ToList();
            _linhaHorizontalMaisAlta = linhasHorizontais.OrderByDescending(x => x.StartPoint.Y).FirstOrDefault();
            

            var linhasVerticais = blocoLista.Block.Entities.OfType<netDxf.Entities.Line>().Where(x => x.StartPoint.X == x.EndPoint.X).ToList();
            _linhaVerticalMaisEsquerda = linhasVerticais.OrderBy(x => x.StartPoint.X).FirstOrDefault();
            _linhaVerticalMaisDireita = linhasVerticais.OrderByDescending(x => x.StartPoint.X).FirstOrDefault();



        }

        public void InserirInformacoesDesenho()
        {
            var dxf = DxfDocument.Load(_nomeArquivo);
            inserirDoRelatorio("08478f494deb");
            inserirLM("08478f494deb");
            inserirRevisoes("08478f494deb");
        }

        private void inserirRevisoes(string appName)
        {
            //int numeroLinha = 0;
            //string appNameLinha = $"{appName}_R{numeroLinha}";
            Revisao revisao = _desenho.Revisao;

            string appNameLinha = $"{appName}_Revisao";
            ApplicationRegistry appReg;
            if (!_dxf.ApplicationRegistries.Contains(appNameLinha))
            {
                appReg = new ApplicationRegistry(appNameLinha);
                _dxf.ApplicationRegistries.Add(appReg);
                XData xdata = new XData(appReg);
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, revisao.RevisionMark));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, revisao.Descricao));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, revisao.CriadoPor));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, revisao.DataCriacao.ToString()));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, revisao.ChecadoPor));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, revisao.DataChecado.ToString()));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, revisao.AprovadoPor));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, revisao.DataAprovacao.ToString()));

                _linhaVerticalMaisEsquerda.XData.Add(xdata);
            }
            else
            {
                appReg = _dxf.ApplicationRegistries[appName];
            }

        }

        private void inserirLM(string appName)
        {
            int numeroLinha = 0;
            string appNameLinha = $"{appName}_L{numeroLinha}";

            foreach (Conjunto conjunto in _desenho.Conjuntos)
            {
                foreach (Peca peca in conjunto.Pecas)
                {
                    appNameLinha = $"{appName}_L{++numeroLinha}";
                    ApplicationRegistry appReg;
                    if (!_dxf.ApplicationRegistries.Contains(appNameLinha))
                    {
                        appReg = new ApplicationRegistry(appNameLinha);

                        _dxf.ApplicationRegistries.Add(appReg);
                        XData xdata = new XData(appReg);

                        xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, conjunto.Posicao));
                        xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, conjunto.Quantidade.ToString()));
                        xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, conjunto.Descricao));
                        xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, conjunto.PesoTotal.ToString()));

                        xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, peca.Posicao));
                        xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, peca.Quantidade.ToString()));
                        xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, peca.Descricao.Descricao));
                        xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, peca.Observacao));
                        xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, peca.Material.Descricao));
                        xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, peca.PesoTotal.ToString()));

                        _linhaVerticalMaisDireita.XData.Add(xdata);
                    }
                    else
                    {
                        appReg = _dxf.ApplicationRegistries[appName];

                    }

                }
            }

        }

        private void inserirDoRelatorio(string appName)
        {

            PropriedadesDesenho propriedades = _relatorio.PegaPropriedades(_nomeArquivo);

            ApplicationRegistry appReg;
            if (!_dxf.ApplicationRegistries.Contains(appName))
            {
                appReg = new ApplicationRegistry(appName);
                _dxf.ApplicationRegistries.Add(appReg);

                XData xdata = new XData(appReg);

                string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;


                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, userName));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, _relatorio.NomeModelo));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, _relatorio.NumeroProjeto));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, _relatorio.Data.ToString()));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, propriedades.NumeroContratada));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, propriedades.NumeroCliente));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, propriedades.DescricaoProjeto));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, propriedades.AreaSubarea));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, propriedades.TituloDesenho));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, propriedades.Subtitulo1Desenho));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, propriedades.Subtitulo2Desenho));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, propriedades.Revisao));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, propriedades.RevisaoCliente));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, propriedades.Escala));

                _linhaHorizontalMaisAlta.XData.Add(xdata);

            }
            else
            {
                appReg = _dxf.ApplicationRegistries[appName];
            }
        }

    }
}
