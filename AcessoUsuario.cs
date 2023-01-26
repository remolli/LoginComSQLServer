using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginComSQLServer
{
    public static class AcessoUsuario
    {
        public static void Interface(SqlConnection Connection)
        {
            Console.Clear();
            Console.WriteLine("Você deseja se registrar ou entrar no Sistema?");
            Console.WriteLine();
            Console.WriteLine("Escreva sua escolha:");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Entrar / Registrar");
            Console.ResetColor();
            Console.Write("> ");
            string escolha = Console.ReadLine();

            if (escolha.ToLower() == "entrar")
            {
                InterfaceEntrar(Connection);
                Thread.Sleep(3000);
                Console.Clear();
                Interface(Connection);
            }
            else if (escolha.ToLower() == "registrar")
            {
                InterfaceRegistro(Connection);
                Thread.Sleep(3000);
                Console.Clear();
                Interface(Connection);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Escreva Corretamente!");
                Console.ResetColor();
            }
        }
        public static void InterfaceRegistro(SqlConnection Connection)
        {
            try
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("Registrar Conta");
                Console.ResetColor();
                Console.WriteLine();
                Console.Write("Usuario: ");
                string usuario = Console.ReadLine();
                Console.Write("Senha: ");
                string senha = Console.ReadLine();

                if (usuario == "" || senha == "")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Usuário e Senha Inválidos!");
                    Console.ResetColor();
                }
                else
                {
                    Registrar(usuario, senha, Connection);
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Erro ao registrar conta.");
                Console.ResetColor();
                //Console.WriteLine(ex.Message);
                //Console.WriteLine(ex.StackTrace);
            }
        }
        public static void Registrar(string usuario, string senha, SqlConnection Connection)
        {
            string str = 
                "SELECT * FROM Contas WHERE Usuario = '" + usuario + "'AND Senha = '" + senha + "'";
            SqlCommand autentifica = new SqlCommand(str, Connection);
            SqlDataReader Reader = autentifica.ExecuteReader();
            if (Reader.Read() == true)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Esse usuário já existe");
                Console.ResetColor();
                Reader.Close();
            }
            else
            {
                try
                {
                    Reader.Close();
                    string insert = 
                        "INSERT INTO Contas VALUES('" + usuario + "', '" + senha + "')";
                    SqlCommand registrar = new SqlCommand(insert, Connection);
                    registrar.ExecuteNonQuery();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Conta Registrada!");
                    Console.ResetColor();
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Não foi possível registrar a conta.");
                    Console.ResetColor();
                    //Console.WriteLine(ex.Message);
                    //Console.WriteLine(ex.StackTrace);
                    
                }
            }
        }
        public static void InterfaceEntrar(SqlConnection Connection)
        {
            try
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("Entrar no Sistema");
                Console.ResetColor();
                Console.WriteLine();
                Console.Write("Usuario: ");
                string usuario = Console.ReadLine();
                Console.Write("Senha: ");
                string senha = Console.ReadLine();

                if (usuario == "" || senha == "")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Usuário e Senha Inválidos!");
                    Console.ResetColor();
                }
                else
                {
                    Entrar(usuario, senha, Connection);
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Erro ao entrar no sistema.");
                Console.ResetColor();
                //Console.WriteLine(ex.Message);
                //Console.WriteLine(ex.StackTrace);
            }
        }
        public static void Entrar(string usuario, string senha, SqlConnection Connection)
        {
            try
            {
                string str = 
                    "SELECT * FROM Contas WHERE Usuario = '" + usuario + "'AND Senha = '" + senha + "'";
                SqlCommand entrar = new SqlCommand(str, Connection);
                SqlDataReader Reader = entrar.ExecuteReader();
                if (Reader.Read())
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Acesso Concedido - Login efetuado");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Usuário e senha incorretos.");
                    Console.ResetColor();
                }
                Reader.Close();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Erro ao entrar!");
                Console.ResetColor();
                //Console.WriteLine(ex.Message);
                //Console.WriteLine(ex.StackTrace);

            }
        }
        public static void CriaTabelaContas(SqlConnection Connection)
        {
            try
            {
                Conexao.UseDatabase(Connection);
                string str = 
                    "CREATE TABLE Contas(" +
                    "ID int IDENTITY(1,1)," +
                    "Usuario varchar(15) NOT NULL," +
                    "Senha varchar(15) NOT NULL)";
                SqlCommand comando = new SqlCommand(str, Connection);
                comando.ExecuteNonQuery();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Tabela Contas criada.");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Tabela Contas já existe");
                Console.ResetColor();
                //Console.WriteLine(ex.Message);
                //Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
