using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarksManager
{
    class Nota
    {
        public string Nombre { set; get; }

        public string Destinatario { set; get; }
      
        public string Texto { set; get; }
        public Urgencia Urgencia { set; get; }

        public TipoNota Tipo { set; get; }

        public Nota(string nombre, string destinatario, string texto, Urgencia urgencia, TipoNota tipo)
        {
            Nombre = nombre;
            Destinatario = destinatario;
            Texto = texto;
            Urgencia = urgencia;
            Tipo = tipo;
        }

    }
}
