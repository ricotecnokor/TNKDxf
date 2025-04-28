using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNKDxf.Dominio.ObjetosValor
{
    public class Texto
    {


        double _fatorEscala;



        public Texto(string atributo, string campo, string valor, Ponto2d pontoInsercao, int? indiceCor)
    {
        Atributo = atributo;
        Campo = campo;
        Valor = valor;
        PontoInsercao = pontoInsercao;
        IndiceCor = indiceCor;
        _fatorEscala = pontoInsercao.FatorEscala;
    }

    public string Atributo { get; set; } = string.Empty;
    public string Campo { get; set; } = string.Empty;
    public string Valor { get; set; } = string.Empty;
    public Ponto2d PontoInsercao { get; set; }
    public int? IndiceCor { get; set; }

    public static Texto Default()
    {
        return new Texto("", "", "", Ponto2d.CriarSemEscala(0.0, 0.0), 0);
    }
}
}
