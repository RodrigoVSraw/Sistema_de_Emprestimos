using System;
using System.Collections.Generic;
using System.Globalization;
using Usuario_CD.Models;
using Emprestimos.Models;

public class SistemaDeEmprestimos
{
    decimal jurosMensal = 0.1m;
    decimal juroDiario = 0.001m;
    List<Emprestimo> emprestimos = new List<Emprestimo>();
    List<Usuario> usuarios = new List<Usuario>();

    public SistemaDeEmprestimos()
    {
        Usuario joao = new Usuario("João", "2689");
        Usuario maria = new Usuario("Maria", "8090");
        Usuario antonio = new Usuario("Antonio", "9000");
        Usuario jose = new Usuario("José", "4576");

        usuarios = new List<Usuario>
        {
            joao,
            maria,
            antonio,
            jose
        };

        emprestimos = new List<Emprestimo>
        {
            new Emprestimo(joao, maria, new DateTime(2026, 02, 16), 10.00m),
            new Emprestimo(antonio, maria, new DateTime(2026, 02, 17), 12.00m),
            new Emprestimo(antonio, jose, new DateTime(2026, 02, 18), 5.00m),
            new Emprestimo(antonio, maria, new DateTime(2026, 02, 18), 3.00m),
            new Emprestimo(joao, maria, new DateTime(2026, 02, 19), 18.00m)
        };
    }

    public Usuario buscaUsuario(string nome)
    {
        foreach (Usuario usuario in usuarios)
        {
            if (usuario.Nome == nome)
            {
                return usuario;
            }
        }
        return null;
    }

    public void TrocaSenhaUsuario(string nomeUsuario, string senhaAntiga, string senhaNova)
    {
        Usuario usuarioNovaSenha = buscaUsuario(nomeUsuario);

        if(usuarioNovaSenha == null)
        {
            throw new ArgumentException("Usuário não Existe.");
        }
        else
        {
            if(senhaAntiga != usuarioNovaSenha.Senha)
            {
                throw new InvalidOperationException("Senha atual incorreta.");
            }
            else
            {
                usuarioNovaSenha.Senha = senhaNova;
            }
        }

    }

    public decimal CalculaDebitoCredito(List<Emprestimo> listaConsultada)
    {
        decimal total = 0;

        foreach(Emprestimo Emp in listaConsultada)
        {
            total += calculaValorReajuste(Emp);
        }

        return total;
    }

    public List<Emprestimo> ConsultaUsuarioComoCredor(string nomeUsuario)
    {
        List<Emprestimo> emprestimosCredor = new List<Emprestimo>();
        int id = 0;

        foreach (Emprestimo Emp in emprestimos)
        {
            if (nomeUsuario == Emp.Credor.Nome)
            {
                emprestimosCredor.Add(Emp);
            }
        }

        return emprestimosCredor;
    }

    public List<Emprestimo> ConsultaUsuarioComoDevedor(string nomeUsuario)
    {
        
        List<Emprestimo> emprestimosDevedor = new List<Emprestimo>();

        foreach (Emprestimo Emp in emprestimos)
        {
            if (nomeUsuario == Emp.Devedor.Nome)
            {
                emprestimosDevedor.Add(Emp);
            }
        }

        return emprestimosDevedor;
    }

    public bool registraEmprestimo(string nomeCredor, string nomeDevedor, DateTime data, decimal valor, string senhaDevedor)
    {
        Usuario usuarioCredor = buscaUsuario(nomeCredor);

        if (usuarioCredor == null)
        {
            return false;
        }

        Usuario usuarioDevedor = buscaUsuario(nomeDevedor);

        if (usuarioDevedor == null)
        {
            return false;
        }

        if (valor <= 0)
        {
            return false;
        }

        if (usuarioDevedor.Senha != senhaDevedor)
        {
            return false;
        }
        else
        {
            Emprestimo novoEmprestimo = new Emprestimo(usuarioCredor, usuarioDevedor, data, valor);
            emprestimos.Add(novoEmprestimo); 
            return true;
        }
    }
    
    public List<Emprestimo> buscaEmprestimosEntre(string nomeCredor, string nomeDevedor)
    {
        List<Emprestimo> empEncontrados = new List<Emprestimo>();
        
        foreach (Emprestimo emp in emprestimos)
        {
            
            if (emp.Credor.Nome == nomeCredor && emp.Devedor.Nome == nomeDevedor)
            { 
                empEncontrados.Add(emp);
            }

        }

        return empEncontrados;
    }

    public decimal calculaValorReajuste(Emprestimo emprestimo)
    {
        decimal valorReajustado = 0;

        DateTime hoje = DateTime.Today;
        DateTime dataEmprestimo = emprestimo.Data;

        int mesesCompletos = ((hoje.Year - dataEmprestimo.Year) * 12) + (hoje.Month - dataEmprestimo.Month);
        DateTime dataAposMeses = dataEmprestimo.AddMonths(mesesCompletos);

        int diasRestantes = (hoje - dataAposMeses).Days;

        decimal fatorMensal = (decimal)Math.Pow((double)(1 + jurosMensal), mesesCompletos);
        decimal fatorDiario = (decimal)Math.Pow((double)(1 + juroDiario), diasRestantes);

        valorReajustado = Math.Round(emprestimo.valorOriginal * fatorMensal * fatorDiario, 2);
    
        return valorReajustado;
    }

    public void quitarEmprestimo(string senhaCredor, Emprestimo emprestimoSelecionado)
    {

        if (emprestimoSelecionado.Credor.Senha == senhaCredor)
        {
            emprestimos.Remove(emprestimoSelecionado);
        }
    }
}