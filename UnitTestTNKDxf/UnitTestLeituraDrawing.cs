using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TNKDxf.TeklaManipulacao;

namespace UnitTestTNKDxf
{
    [TestClass]
    public class UnitTestLeituraDrawing
    {
        [TestMethod]
        public void TestMethod1()
        {
            LeitorDesenhoTekla leitor = new LeitorDesenhoTekla();
            leitor.LerDesenhos();



            
        }
    }
}
