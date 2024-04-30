using System;
using System.Collections.Generic;
using System.Collections;
using System.Windows.Forms;
using RFC;

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
            //Rain rain = new Rain();

            //var t = rain.Rain_Flow(new List<double>() { 0, 5.3, 3.32, 6.25, 7.42, 2.3, 4.95 }, 4, 10.0);

            Console.Title = "Completed " + 0 + "%";
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main_Form());
        }
    }
}
