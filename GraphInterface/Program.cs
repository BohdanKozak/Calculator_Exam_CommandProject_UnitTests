using AnalaizerClassLibrary;
using MySql.Data.EntityFramework;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraphInterface
{

    public class CalculatorValue
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public string Expected { get; set; }
    }

    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class CalculatorDbContext : DbContext
    {
        public CalculatorDbContext()
            : base()
        {

        }
        public CalculatorDbContext(DbConnection existingConnection, bool contextOwnsConnection)
      : base(existingConnection, contextOwnsConnection)
        {

        }
    public DbSet<CalculatorValue> CalculatorValues { get; set; }


    }


    static class Program
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool AllocConsole();
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool FreeConsole();

        [DllImport("kernel32.dll")]
        static extern bool AttachConsole(int dwProcessId);
        private const int ATTACH_PARENT_PROCESS = -1;
        private const int MAX_LENGHT_EXPRESSION = 65536;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {

            var context = new CalculatorDbContext();

            var Format_ValidTextShouldBeFormatted = new CalculatorValue()
            {
                Value = "1 + 1"
            };
            context.CalculatorValues.Add(Format_ValidTextShouldBeFormatted);
            var Format_InvalidStartCharacterShouldReturnError = new CalculatorValue()
            {
                Value = "#1 + 1"
            };
            context.CalculatorValues.Add(Format_InvalidStartCharacterShouldReturnError);
            var Format_EmptyTextShouldReturnEmptyString = new CalculatorValue()
            {
                Value = ""
            };
            context.CalculatorValues.Add(Format_EmptyTextShouldReturnEmptyString);
            var Format_TextExceedsMaxLengthShouldReturnError = new CalculatorValue()
            {
                Value = "1234567890" + new string('1', MAX_LENGHT_EXPRESSION)
            };
            context.CalculatorValues.Add(Format_TextExceedsMaxLengthShouldReturnError);

            context.SaveChanges();

            int argCount = args == null ? 0 : args.Length;
            if (argCount > 0)
            {
                // redirect console output to parent process;
                // must be before any calls to Console.WriteLine()
                AttachConsole(ATTACH_PARENT_PROCESS);


                AnalaizerClass.expression = args[0];

                int length = Console.CursorLeft;
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.WriteLine(new string(' ', length));
                Console.SetCursorPosition(0, Console.CursorTop);

                Console.WriteLine("Expression:" + AnalaizerClass.expression);
                string result = AnalaizerClass.Estimate();

                ConsoleColor color = ConsoleColor.Green;

                if (result.StartsWith("&"))
                {
                    result = result.TrimStart('&');
                    color = ConsoleColor.Red;
                }
                else
                    result = result + Environment.NewLine + "Error: 0";


                ConsoleColor current = Console.ForegroundColor;
                Console.ForegroundColor = color;
                Console.OutputEncoding = Encoding.UTF8;
                Console.WriteLine("Result: " + result);
                Console.ForegroundColor = current;

                return;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
