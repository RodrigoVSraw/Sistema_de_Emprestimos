using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Usuario_CD.Models;

namespace Emprestimos.Models
{
    public class Emprestimo
    {
        public Usuario Credor { get; private set; }
        public Usuario Devedor { get; private set; }
        public DateTime Data { get; private set; }
        public decimal valorOriginal { get; private set; }

        public Emprestimo(Usuario credor, Usuario devedor, DateTime data, decimal _valorOriginal)
        {
            Credor = credor;
            Devedor = devedor;
            valorOriginal = _valorOriginal;
            Data = data;
        }
    }
}
