using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Tekla.Structures.Drawing;
using Tekla.Structures.Model;

namespace ConsoleTNKDxf
{
    public class CamposFormato
    {

        private readonly string _numeroContratada;
        private readonly string _numeroCliente;
        private readonly string _descricaoProjeto;
        private readonly string _subtitulo1;
        private readonly string _tituloProjeto;
        private readonly string _escala;
        private readonly string _revisao;
        private readonly string _revisaoCliente;
        private readonly string _nomeModelo = string.Empty;
        private readonly string _numeroProjeto = string.Empty;
        private readonly DateTime _data = DateTime.Now;

        public CamposFormato(MultiDrawing drawing)
        {

            PropertyInfo propInfo = drawing.GetType().GetProperty("Identifier",
                                        BindingFlags.Instance | BindingFlags.NonPublic);
            object value = propInfo.GetValue(drawing, null);
            Tekla.Structures.Identifier identifier = (Tekla.Structures.Identifier)value;

            Beam tempBeam = new Beam();
            tempBeam.Identifier = identifier;

            tempBeam.GetReportProperty("TITLE1", ref _numeroContratada);
            tempBeam.GetReportProperty("TITLE2", ref _numeroCliente);
            tempBeam.GetReportProperty("PROJECT.OBJECT", ref _descricaoProjeto);
            tempBeam.GetReportProperty("TITLE3", ref _subtitulo1);
            tempBeam.GetReportProperty("TITLE", ref _tituloProjeto);
            tempBeam.GetReportProperty("REVISION.MARK", ref _revisao);
            tempBeam.GetReportProperty("REVISION.MARK", ref _revisaoCliente);
            tempBeam.GetReportProperty("PROJECT.MODEL", ref _nomeModelo);
            tempBeam.GetReportProperty("PROJECT.NUMBER", ref _numeroProjeto);

            string currentTemplateFile = string.Empty;
            tempBeam.GetReportProperty("PADRÃO ARAUCO", ref currentTemplateFile);

            //string dia = string.Empty;
            //tempBeam.GetReportProperty("DATE", ref dia);
            //string hora = string.Empty;
            //tempBeam.GetReportProperty("TIME", ref hora);
            ////_data = new DateTime(
            //    Convert.ToInt32(dia.Split('.')[2]),
            //    Convert.ToInt32(dia.Split('.')[1]),
            //    Convert.ToInt32(dia.Split('.')[0]),
            //    Convert.ToInt32(hora.Split(':')[0]),
            //    Convert.ToInt32(hora.Split(':')[1]),
            //    Convert.ToInt32(hora.Split(':')[2])
            //);
            _data = DateTime.Now;

            Dictionary<string, double> escalas = new Dictionary<string, double>();
            var escala1 = string.Empty;
            var escala2 = string.Empty;
            var escala3 = string.Empty;
            var escala4 = string.Empty;
            var escala5 = string.Empty;
            tempBeam.GetReportProperty("SCALE1", ref escala1);
            tempBeam.GetReportProperty("SCALE2", ref escala2);
            tempBeam.GetReportProperty("SCALE3", ref escala3);
            tempBeam.GetReportProperty("SCALE4", ref escala4);
            tempBeam.GetReportProperty("SCALE5", ref escala5);
            if(!string.IsNullOrEmpty(escala1))  escalas.Add(escala1,  Convert.ToDouble(escala1.Split(':')[1]));
            if(!string.IsNullOrEmpty(escala2))  escalas.Add(escala2,  Convert.ToDouble(escala2.Split(':')[1]));
            if(!string.IsNullOrEmpty(escala3))  escalas.Add(escala3,  Convert.ToDouble(escala3.Split(':')[1]));
            if(!string.IsNullOrEmpty(escala4))  escalas.Add(escala4,  Convert.ToDouble(escala4.Split(':')[1]));
            if(!string.IsNullOrEmpty(escala5))  escalas.Add(escala5, Convert.ToDouble(escala5.Split(':')[1]));
            _escala = escalas.OrderBy(e => e.Value).LastOrDefault().Key;

        }

        public string NumeroContratada => _numeroContratada;

        public string NumeroCliente => _numeroCliente;

        public string DescricaoProjeto => _descricaoProjeto;

        public string Subtitulo1 => _subtitulo1;

        public string TituloProjeto => _tituloProjeto;

        public string Escala => _escala;

        public string Revisao => _revisao;

        public string RevisaoCliente => _revisaoCliente;

        public string NomeModelo => _nomeModelo;

        public string NumeroProjeto => _numeroProjeto;

        public DateTime Data => _data;

    }
}
