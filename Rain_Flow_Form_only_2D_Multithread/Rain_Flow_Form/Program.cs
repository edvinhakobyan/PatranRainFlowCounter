using System;
using System.Windows.Forms;
using System.Collections.Generic;
namespace Fatige_Stress_Counting_Tool
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]

        static void Main()
        {
            Console.Title = "Completed " + 0 + "%";
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main_Form());
        }
    }
}
