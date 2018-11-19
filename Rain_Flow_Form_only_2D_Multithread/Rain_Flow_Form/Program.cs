using System;
using System.Collections.Generic;
using System.Windows.Forms;

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
            Console.Title = "Completed Percent " + 0 + "%";
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main_Form());
        }
    }
}
