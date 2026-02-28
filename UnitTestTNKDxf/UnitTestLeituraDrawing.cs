using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using TNKDxf.TeklaManipulacao;
using TNKDxf.TeklaManipulacao.Adapters;

namespace UnitTestTNKDxf
{
    [TestClass]
    public class UnitTestLeituraDrawing
    {
        [TestMethod]
        public void TestLerRelatorioDesenhos()
        {
            LeitorRlatorioDesenhosTekla leitor = new LeitorRlatorioDesenhosTekla("testeMulti.rpt");
            var relatorio = leitor.Ler();

            Assert.IsNotNull(relatorio);
                Assert.IsTrue(relatorio.NomeModelo.Length > 0);
                Assert.IsTrue(relatorio.NumeroProjeto.Length > 0);
                Assert.IsTrue(relatorio.Data != default(DateTime));
                Assert.IsTrue(relatorio.GetEnumerator().MoveNext());


        }

        [TestMethod]
        public void SelecionarDesenhos()
        {
            DesenhoSelector desenhoSelector = new DesenhoSelector();
            var desenhosParaSelecionar = new List<string> { "PRJ-00019-D-00126", "PRJ-00019-D-00130" };
            var selecionados = desenhoSelector.Selecionar(desenhosParaSelecionar);
            Assert.IsNotNull(selecionados);
            Assert.IsTrue(selecionados.Count > 0);

        }

        [TestMethod]
        public void LerEntidadesDesenhos()
        {
            IAdapterDesenho adapter = new AdapterDesenho();    
            adapter.ColetarInformacoesDesenho();

            Assert.IsNotNull(adapter);

        }
    }
}
