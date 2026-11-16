using ConsoleTNKDxf.Abstracoes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TSM = Tekla.Structures.Model;

namespace ConsoleTNKDxf.Dgts
{
    public class ConjuntoDetalhadoDgt : ConjuntoAbstrato
    {
        private List<PecaDgt> _itens = new List<PecaDgt>();
        

        public List<PecaDgt> Itens => _itens;



        private readonly PecaDgt _pecaDgtMaiPart;
        private Dictionary<string, int> _contagemPartesAssembly;
        private HashSet<Tekla.Structures.Identifier> _boltIdsAssembly = new HashSet<Tekla.Structures.Identifier>();

        public int TotalPartesNoAssembly => _contagemPartesAssembly != null ? _contagemPartesAssembly.Values.Sum() : 0;
        public int TotalPartesColetadas => _itens.Sum(p => p.Quantidade);
        public int TotalParafusosNoAssembly => _boltIdsAssembly.Count;
        public HashSet<Tekla.Structures.Identifier> BoltIdsNoAssembly => _boltIdsAssembly;

        public ConjuntoDetalhadoDgt(TSM.Part part, PecaDgt pecaDgtMaiPart = null)
        {
            var assy = part.GetAssembly();
            _pecaDgtMaiPart = pecaDgtMaiPart;

            Hashtable stringProterties = new Hashtable();
            ArrayList stringReportProperties = new ArrayList { "ASSEMBLY_POS", "MAINPART.NAME" };
            assy.GetStringReportProperties(stringReportProperties, ref stringProterties);
            _mainPartName = stringProterties.ContainsKey("MAINPART.NAME") ? stringProterties["MAINPART.NAME"].ToString() : string.Empty;
            _assemblyPos = stringProterties.ContainsKey("ASSEMBLY_POS") ? stringProterties["ASSEMBLY_POS"].ToString() : string.Empty;

            Hashtable doubleProperties = new Hashtable();
            ArrayList doubleReportProperties = new ArrayList { "HEIGHT", "WEIGTH" };
            assy.GetDoubleReportProperties(doubleReportProperties, ref doubleProperties);
            _height = doubleProperties.ContainsKey("HEIGHT") ? doubleProperties["HEIGHT"].ToString().ConverterParaDouble() : 0;
            _weigth = doubleProperties.ContainsKey("WEIGTH") ? doubleProperties["WEIGTH"].ToString().ConverterParaDouble() : 0;

            Hashtable integerProperties = new Hashtable();
            ArrayList integerReportProperties = new ArrayList { "NUMBER", "ASSEMBLY.MODEL_TOTAL" };
            assy.GetIntegerReportProperties(integerReportProperties, ref integerProperties);
            int number = integerProperties.ContainsKey("NUMBER") ? int.Parse(integerProperties["NUMBER"].ToString()) : 1;
            int modelTotal = integerProperties.ContainsKey("ASSEMBLY.MODEL_TOTAL") ? int.Parse(integerProperties["ASSEMBLY.MODEL_TOTAL"].ToString()) : 1;

            int qtdModel = 0;
            part.GetReportProperty("ASSEMBLY.MODEL_TOTAL", ref qtdModel);

            _quantidade = number * qtdModel;

            inicializarContagemAssembly(assy);

            string partPos = part.ObterPropriedade("PART_POS")?.ToString() ?? string.Empty;
            int qtdPeca = obterContagemPeca(partPos, assy);
            PecaDgt peca = new PecaDgt(part, qtdPeca);
            
            _itens.Add(peca);
        }

        private void inicializarContagemAssembly(TSM.Assembly assy)
        {
            if (_contagemPartesAssembly != null || assy == null) return;
            _contagemPartesAssembly = new Dictionary<string, int>();

            var principal = assy.GetMainPart() as TSM.Part;
            if (principal != null)
            {
                string posPrincipal = principal.ObterPropriedade("PART_POS")?.ToString();
                if (!string.IsNullOrEmpty(posPrincipal))
                {
                    _contagemPartesAssembly[posPrincipal] = 1;
                }

                var mBolts = principal.GetBolts();
                while (mBolts.MoveNext())
                {
                    if (mBolts.Current is TSM.BoltArray b)
                    {
                        _boltIdsAssembly.Add(b.Identifier);
                    }
                }
            }

            var filhos = assy.GetSecondaries().GetEnumerator();
            while (filhos.MoveNext())
            {
                var filho = filhos.Current as TSM.Part;
                if (filho != null)
                {
                    string posFilho = filho.ObterPropriedade("PART_POS")?.ToString();
                    if (!string.IsNullOrEmpty(posFilho))
                    {
                        if (_contagemPartesAssembly.ContainsKey(posFilho))
                            _contagemPartesAssembly[posFilho]++;
                        else
                            _contagemPartesAssembly[posFilho] = 1;
                    }

                    var sBolts = filho.GetBolts();
                    while (sBolts.MoveNext())
                    {
                        if (sBolts.Current is TSM.BoltArray b)
                        {
                            _boltIdsAssembly.Add(b.Identifier);
                        }
                    }
                }
            }
        }

        private int obterContagemPeca(string posicaoPeca, TSM.Assembly assy)
        {
            if (_contagemPartesAssembly == null)
            {
                inicializarContagemAssembly(assy);
            }

            if (_contagemPartesAssembly != null && _contagemPartesAssembly.TryGetValue(posicaoPeca, out int qtd))
            {
                return qtd;
            }

            return 1;
        }

        public void AddItem(TSM.Part part, TSM.Assembly assembly)
        {
            string posicaoPeca = part.ObterPropriedade("PART_POS").ToString();

            if (!_itens.Any(c => c.PartPos == posicaoPeca))
            {
                int qtd = obterContagemPeca(posicaoPeca, assembly);
                PecaDgt item = new PecaDgt(part, qtd);
                _itens.Add(item);
            }
        }

        public void AddItemExistente(string posicaoPeca)
        {
            var existingItem = _itens.FirstOrDefault(c => c.PartPos == posicaoPeca);
            if (existingItem != null)
            {
                existingItem.IncrementarQuantidade();
            }
        }

        public void MultiplicaQtdConjuntoPorPeca(int quantidadeConjuntos)
        {
            //var itemAvulso = _itens.FirstOrDefault();
            //itemAvulso.MultiplicaQtdConjuntoPorPeca(_quantidade);
            foreach (var item in _itens)
            {
                item.MultiplicaQtdConjuntoPorPeca(quantidadeConjuntos);
            }
        }
    }
}
