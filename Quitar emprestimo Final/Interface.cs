using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Usuario_CD.Models;
using Emprestimos.Models;

namespace Interface_Emprestimos.Models
{
    public class Interface
    {
        static void Main(string[] args)
        {
            SistemaDeEmprestimos sistema = new SistemaDeEmprestimos();
            List<Emprestimo> listaEmpEncontrados = new List<Emprestimo>();
            var rodando = true;
            Emprestimo emprestimoSelecionado = null;

            while (rodando)
            {
                Console.Clear();
                string nomeCredor = null;
                string nomeDevedor = null;
                string nomeUsuario = null;
                DateTime data = DateTime.MinValue;
                decimal valor = 0;

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("---------------------------------------------");
                Console.WriteLine("## Bem vindo ao sistema de empréstimos Raw ##");
                Console.WriteLine("---------------------------------------------");
                Console.ResetColor();

                Console.WriteLine("\n1. Registrar Empréstimo.");
                Console.WriteLine("2. Quitar Empréstimo.");
                Console.WriteLine("3. Consultar Situação de Usuário.");
                Console.WriteLine("4. Trocar Senha de Usuário.");
                Console.WriteLine("0. Fechar sitema de empréstimos.");
                Console.ForegroundColor= ConsoleColor.Yellow;
                Console.Write("\nSelecione a opção desejada: ");
                Console.ResetColor();

                if (!int.TryParse(Console.ReadLine(), out int op))
                {
                    op = -1;
                }

                switch (op)
                {
                    case 0:
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine("\nObrigado por usar o Sitema de Empréstimos Raw");
                        Console.ReadKey();
                        rodando = false;

                        break;

                    case 1:
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("--------------------------");
                        Console.WriteLine("## Registrar Empréstimo ##");
                        Console.WriteLine("--------------------------");
                        Console.ResetColor();

                        Console.WriteLine("\nInsira os seguintes dados para efetuar o empréstimo:");
                        try
                        {
                            Console.Write("\nNome do Credor: ");
                            nomeCredor = Console.ReadLine();

                            Console.Write("Nome do Devedor: ");
                            nomeDevedor = Console.ReadLine();

                            Console.Write("Data (dd/MM/yyyy): ");
                            data = DateTime.ParseExact(Console.ReadLine(), "dd/MM/yyyy", CultureInfo.InvariantCulture);

                            Console.Write("Valor: ");
                            valor = decimal.Parse(Console.ReadLine());

                            Console.Write("\nDigite a senha do devedor para registrar o empréstimo: ");
                            string senhaDevedor = Console.ReadLine();

                            if (sistema.registraEmprestimo(nomeCredor, nomeDevedor, data, valor, senhaDevedor) == false)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("\nNão foi possível registrar o empréstimo. Verifique os dados e tente novamente.");
                                Console.ReadKey();
                                continue;
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("Empréstimo registrado com sucesso!");
                                Console.ReadKey();
                            }
                        }
                        catch (FormatException)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\nFormato de data ou valor inválido. Pressione qualquer tecla para tentar novamente.");
                            Console.ReadKey();
                            continue;
                        }

                        break;

                    case 2:
                        int id = 0;

                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine("-----------------------");
                        Console.WriteLine("## Quitar Empréstimo ##");
                        Console.WriteLine("-----------------------");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\nInsira os seguintes dados para quitar o empréstimo:");
                        Console.ResetColor();

                        try
                        {
                            Console.Write("\nNome do Credor: ");
                            nomeCredor = Console.ReadLine();

                            while (sistema.buscaUsuario(nomeCredor) == null)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("\nCredor não encontrado. Pressione qualquer tecla para tentar novamente.");
                                Console.ReadKey();
                                Console.Write("Nome do Credor: ");
                                nomeCredor = Console.ReadLine();
                            }

                            Console.Write("Nome do Devedor: ");
                            nomeDevedor = Console.ReadLine();

                            while (sistema.buscaUsuario(nomeDevedor) == null)
                            {
                                Console.WriteLine("\nDevedor não encontrado. Pressione qualquer tecla para tentar novamente.");
                                Console.ReadKey();
                                Console.Write("Nome do Devedor: ");
                                nomeDevedor = Console.ReadLine();
                            }

                            listaEmpEncontrados = sistema.buscaEmprestimosEntre(nomeCredor, nomeDevedor);

                            Console.WriteLine("\nEmprestimos encontrados: ");
                            foreach (Emprestimo emp in listaEmpEncontrados)
                            {
                                id++;
                                Console.WriteLine($"{id}. Data: {emp.Data}, Valor Original: {emp.valorOriginal}, Valor Reajustado: {sistema.calculaValorReajuste(emp)}");
                            }

                            Console.WriteLine("\nDigite o número do empréstimo que deseja quitar, ou pressione 0 para sair: ");
                            int opEmp = int.Parse(Console.ReadLine());

                            if (opEmp == 0)
                            {
                                Console.WriteLine("\nObrigado por utilizar o sistema!");
                                Console.ReadKey();
                                continue;
                            }
                            else
                            {
                                emprestimoSelecionado = listaEmpEncontrados[opEmp - 1];
                            }


                            Console.Write("\nDigite a senha para quitar o empréstimo: ");
                            string senhaCredor = Console.ReadLine();

                            while (sistema.buscaUsuario(nomeCredor).Senha != senhaCredor)
                            {
                                Console.WriteLine("\nSenha incorreta. Pressione qualquer tecla para tentar novamente.");
                                Console.ReadKey();
                                Console.Write("\nDigite a senha para quitar o empréstimo: ");
                                senhaCredor = Console.ReadLine();
                            }

                            sistema.quitarEmprestimo(senhaCredor, emprestimoSelecionado);
                            Console.WriteLine("Empréstimo quitado!");

                            Console.ReadKey();
                        }
                        catch (FormatException)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\nEntrada inválida. Pressione qualquer tecla para tentar novamente.");
                            Console.ReadKey();
                            continue;
                        }
                        break;

                    case 3:

                        int idC = 0;
                        int idD = 0;
                        decimal totalC = 0;
                        decimal totalD = 0;

                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("-----------------------------------");
                        Console.WriteLine("## Consultar Situação de Usuário ##");
                        Console.WriteLine("-----------------------------------");
                        Console.ResetColor();

                        Console.WriteLine("\nInsira os seguintes dados para consultar a situação do usuário:");

                        try
                        {
                            Console.Write("\nNome de Usuário: ");
                            nomeUsuario = Console.ReadLine();

                            while (sistema.buscaUsuario(nomeUsuario) == null)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("\nUsuário não encontrado. Pressione qualquer tecla para tentar novamente.");
                                Console.ReadKey();
                                Console.ResetColor();

                                Console.Write("\nNome de Usuário: ");
                                nomeUsuario = Console.ReadLine();
                            }
                        }
                        catch (FormatException)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\nEntrada inválida. Pressione qualquer tecla para tentar novamente.");
                            Console.ReadKey();
                            continue;
                        }


                        List<Emprestimo> emprestimosComoCredor = new List<Emprestimo>();
                        emprestimosComoCredor  = sistema.ConsultaUsuarioComoCredor(nomeUsuario);

                        Console.ForegroundColor = ConsoleColor.Magenta; 
                        Console.WriteLine("\n------------------------------------------------------");
                        Console.WriteLine("## Lista de Emprestimos em que o Usuário é Credor ##");
                        Console.WriteLine("\n");
                        Console.ResetColor();

                        if (emprestimosComoCredor.Count == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.WriteLine("Não há empréstimos em que o usuário é Credor.");
                            Console.ResetColor();   
                        }
                        else
                        {
                            
                            foreach (Emprestimo EmpC in emprestimosComoCredor)
                            {
                                idC++;
                                Console.WriteLine($"{idC}. Nome do Devedor: {EmpC.Devedor.Nome}, Data: {EmpC.Data}, Valor Original: {EmpC.valorOriginal}, Valor Reajustado: {sistema.calculaValorReajuste(EmpC)}");
                            }

                            totalC = sistema.CalculaDebitoCredito(emprestimosComoCredor);

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"\nTotal de Crédito do Usuário: {totalC}");
                            Console.ResetColor();
                        }
                            
                        List<Emprestimo> emprestimoComoDevedor = new List<Emprestimo>();
                        emprestimoComoDevedor = sistema.ConsultaUsuarioComoDevedor(nomeUsuario);
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine("\n-------------------------------------------------------");
                        Console.WriteLine("## Lista de Emprestimos em que o Usuário é Devedor ##");
                        Console.WriteLine("\n");
                        Console.ResetColor();

                        if (emprestimoComoDevedor.Count == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.WriteLine("Não há Empréstimos em que o usuário é Devedor.");
                            Console.ResetColor();
                        }
                        else
                        {
                            foreach (Emprestimo EmpD in emprestimoComoDevedor)
                            {
                                idD++;
                                Console.WriteLine($"{idD}. Nome do Credor: {EmpD.Credor.Nome}, Data: {EmpD.Data}, Valor Original: {EmpD.valorOriginal}, Valor Reajustado: {sistema.calculaValorReajuste(EmpD)}");
                            }

                            totalD = sistema.CalculaDebitoCredito(emprestimoComoDevedor);

                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.WriteLine($"\nTotal de Débito do Usuário: {totalD}");
                            Console.ResetColor();
                        }

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
                        Console.ReadKey();

                        break;

                    case 4:
                        

                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine("------------------");
                        Console.WriteLine("## Trocar Senha ##");
                        Console.WriteLine("------------------");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine("\nInsira os seguintes dados para trocar de senha:");
                        Console.ResetColor();

                        try
                        {
                            Console.Write("\nNome de Usuário: ");
                            nomeUsuario = Console.ReadLine();

                            Console.Write("Senha Atual: ");
                            string senhaAtual = Console.ReadLine();

                            Console.Write("Senha Nova: ");
                            string senhaNova = Console.ReadLine();

                            sistema.TrocaSenhaUsuario(nomeUsuario, senhaAtual, senhaNova);

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("\nSenha Alterada com sucesso!!!");
                            Console.ReadKey();

                        }
                        catch(Exception ex)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"\nNão foi possível trocar a senha devido ao seguinte erro: {ex.Message}");
                            Console.WriteLine("\nCancelando Operação...");
                            Console.ReadKey();
                        }

                        break;

                    default:
                        if (op < 0 || op > 4)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\nOpção inválida. Pressione qualquer tecla para tentar novamente.");
                            Console.ReadKey();
                        }

                        break;
                }
            }
        }
    }
}
