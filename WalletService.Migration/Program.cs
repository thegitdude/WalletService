using DbUp;
using System;
using System.Linq;
using System.Reflection;

namespace WalletService.Migration
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var connectionString = args.FirstOrDefault()
           ?? System.Configuration.ConfigurationManager.ConnectionStrings["sql.connectionString"].ConnectionString;

            var upgrader =
                DeployChanges.To
                    .SqlDatabase(connectionString)
                    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                    .LogToConsole()
                    .Build();

            var result = upgrader.PerformUpgrade();

            if (!result.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Error);
                Console.ResetColor();
                Console.ReadLine();
                return -1;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success!");
            Console.ResetColor();
            return 0;
        }
    }
}
