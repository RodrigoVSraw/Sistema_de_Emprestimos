using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Usuario_CD.Models
{
    public class Usuario
    {
        public string Nome { get; private set; }
        public string Senha { get; set; }

        public Usuario(string nome, string senha)
        {
            if (string.IsNullOrWhiteSpace(nome))
            {
                throw new ArgumentException("O nome do usuário não pode ser vazio.");
            }

            if (string.IsNullOrWhiteSpace(senha))
            {
                throw new ArgumentException("A senha do usuário não pode ser vazia.");
            }   

            Nome = nome;
            Senha = senha;
        }
    }
}
