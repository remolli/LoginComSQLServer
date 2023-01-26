using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace LoginComSQLServer
{
    public static class Conexao
    {
        public static void Run()
        {
            SqlConnection Connection = new SqlConnection("Server=localhost;Integrated security=SSPI;database=master");

            try
            {
                //Conecta ao Banco de Dados master local
                ConectarDB(Connection);

                // Verifica existência do banco de dados local, se não existir, será criado
                TentaCriarDB(Connection);

                // Cria tabela Contas com as colunas ID - Usuario - Senha
                AcessoUsuario.CriaTabelaContas(Connection);

                PerguntaResetarDB(Connection);

                //Executar a Interface até o usuário se registrar ou entrar no sistema
                while (true)
                {
                    AcessoUsuario.Interface(Connection);
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Erro ao conectar no Banco de Dados!");
                Console.ResetColor();
                //Console.WriteLine(ex.Message);
                //Console.WriteLine(ex.StackTrace);
            }
            finally
            {
                DesconectarDB(Connection);
            }
        }

        //
        //  ----------- METODOS SIMPLIFICADOS -----------
        //

        public static void ConectarDB(SqlConnection Connection)
        {
            Connection.Open();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Banco de Dados Conectado");
            Console.ResetColor();
        }
        public static void DesconectarDB(SqlConnection Connection)
        {
            if (Connection.State == ConnectionState.Open)
            {
                Connection.Close();
            }
        }
        public static void ResetarDB(SqlConnection Connection)
        {
            try
            {
                UseDatabase(Connection);
                string strDropDB = 
                    "DROP TABLE Contas";
                SqlCommand dropdb = new SqlCommand(strDropDB, Connection);
                dropdb.ExecuteNonQuery();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("DATABASE RESETADA");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Erro no 'DropDB'");
                Console.ResetColor();
                //Console.WriteLine(ex.Message);
                //Console.WriteLine(ex.StackTrace);
            }
        }
        public static void PerguntaResetarDB(SqlConnection Connection)
        {
            Console.WriteLine();
            Console.WriteLine("Você deseja resetar o Banco de Dados?");
            Console.WriteLine("Sim / Nao");
            Console.Write("> ");
            string resetar = Console.ReadLine();
            if (resetar.ToLower() == "sim")
            {
                ResetarDB(Connection);
                AcessoUsuario.CriaTabelaContas(Connection);
            }
        }
        public static void TentaCriarDB(SqlConnection Connection)
        {
            try
            {
                CriaDB(Connection);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Banco de Dados Criado.");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Banco de Dados já existe.");
                Console.ResetColor();
                //Console.WriteLine(ex.Message);
                //Console.WriteLine(ex.StackTrace);
            }
        }
        public static void CriaDB(SqlConnection Connection)
        {
            string databasePath = DatabasePath();
            string strCriaDB;
            strCriaDB = "CREATE DATABASE MyDatabase ON PRIMARY " +
            "(NAME = MyDatabase, " +
            "FILENAME = '" + databasePath + "MyDatabase.mdf', " +
            "SIZE = 2MB, MAXSIZE = 10MB, FILEGROWTH = 10%)" +
            "LOG ON (NAME = MyDatabaseLog, " +
            "FILENAME = '" + databasePath + "MyDatabaseLog.ldf', " +
            "SIZE = 1MB, " +
            "MAXSIZE = 5MB, " +
            "FILEGROWTH = 10%)";

            SqlCommand CriaDB = new SqlCommand(strCriaDB, Connection);
            CriaDB.ExecuteNonQuery();
        }
        public static void UseDatabase(SqlConnection Connection)
        {
            // Executa o comando 'USE MyDatabase' para manipular dados dentro do Banco de Dados
            string strUse = "USE MyDatabase;";
            SqlCommand use = new SqlCommand(strUse, Connection);
            use.ExecuteNonQuery();
        }
        public static string DatabasePath()
        {
            var atualPath = AppDomain.CurrentDomain.BaseDirectory;
            string programPath = "";
            for (int i = atualPath.Length - 1; i > atualPath.Length - 19; i--)
            {
                programPath = atualPath.Remove(i);
            }
            var path = $"{programPath}\\Database\\";
            return path;
        }
    }
}
