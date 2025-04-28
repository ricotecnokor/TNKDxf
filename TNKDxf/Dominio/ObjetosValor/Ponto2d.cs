using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNKDxf.Dominio.Extensoes;

namespace TNKDxf.Dominio.ObjetosValor
{
    public class Ponto2d
    {
        private double _x;
    private double _y;
    private double _fatorEscala;

    private Ponto2d(double x, double y, double fatorEscala)
    {
        _fatorEscala = fatorEscala;
        _x = x * _fatorEscala;
        _y = y * _fatorEscala;
    }

    public double X { get => _x; private set => _x = value; }
    public double Y { get => _y; private set => _y = value; }
    public double FatorEscala { get => _fatorEscala; private set => _fatorEscala = value; }

    public static Ponto2d CriarComEscala(double x, double y, double fatorEscala)
    {
        return new Ponto2d(x, y, fatorEscala);
    }

    public static Ponto2d CriarSemEscala(double x, double y)
    {
        return new Ponto2d(x, y, 1.0);
    }

    public bool MaiorOuIgual(Ponto2d ponto)
    {

        return this.X * _fatorEscala >= ponto.X * _fatorEscala && this.Y * _fatorEscala >= ponto.Y * _fatorEscala;


    }

    public bool MenorOuIgual(Ponto2d ponto)
    {


        return this.X * _fatorEscala <= ponto.X * _fatorEscala && this.Y * _fatorEscala <= ponto.Y * _fatorEscala;

    }

    public bool IgualComTolerancia(Ponto2d ponto, double tolerancia)
    {



        return ponto.X.IgualComTolerancia(this.X, tolerancia, _fatorEscala) == false ? false
            : ponto.Y.IgualComTolerancia(this.Y, tolerancia, _fatorEscala) == false ? false
            : true;



    }

    public bool MenorQue(Ponto2d ponto)
    {
        return ponto.X > this.X && ponto.Y > this.Y;
    }

    public bool MaiorQue(Ponto2d ponto)
    {
        return ponto.X < this.X && ponto.Y < this.Y;
    }

    public bool IgualComTolerancia(Ponto2d pontoInsercao, double tolerancia, double fatorEscala)
    {
        if (this.X.IgualComTolerancia(pontoInsercao.X, tolerancia, fatorEscala) && this.Y.IgualComTolerancia(pontoInsercao.Y, tolerancia, fatorEscala))
        {
            return true;
        }

        return false;
    }
}
}
