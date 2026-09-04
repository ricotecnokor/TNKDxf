using ConsoleTNKDxf.Dgts;
using System.Collections.Generic;

namespace ConsoleTNKDxf.Abstracoes
{
    public abstract class ConjuntoAbstrato
    {
        protected List<FixacaoDgt> _fixacaoFabrica = new List<FixacaoDgt>();
        protected List<FixacaoDgt> _fixacaoObra = new List<FixacaoDgt>();
        protected double _height;
        protected double _weigth;
        protected string _assemblyPos;
        protected string _mainPartName;
        protected int _quantidade;
        public List<FixacaoDgt> FixacaoFabrica => _fixacaoFabrica;
        public List<FixacaoDgt> FixacaoObra => _fixacaoObra;
        public string AssemblyPos => _assemblyPos;
        public string MainPartName => _mainPartName;
        public int Quantidade => _quantidade;
        public double Height => _height;
        public double Weigth => _weigth;
    }
}
