namespace ConsoleTNKDxf.Abstracoes
{
    public abstract class ConjuntoAbstrato
    {
        protected double _height;
        protected double _weigth;
        protected string _assemblyPos;
        protected string _mainPartName;
        protected int _quantidade;
        public string AssemblyPos => _assemblyPos;
        public string MainPartName => _mainPartName;
        public int Quantidade => _quantidade;
        public double Height => _height;
        public double Weigth => _weigth;
    }
}
