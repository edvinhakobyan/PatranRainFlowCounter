using System;
using System.Collections.Generic;
using System.Collections;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Reflection;

namespace Fatige_Stress_Counting_Tool
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            //CheckTrial();

            Console.Title = "Completed " + 0 + "%";
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main_Form());
        }


        private static void CheckTrial()
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            var settingsName = $@"SOFTWARE\Settings\{version}";
            var key1 = Registry.CurrentUser.OpenSubKey(settingsName, true);

            if (key1 == null)
            {
                var key = Registry.CurrentUser.CreateSubKey(settingsName);
                key.SetValue("Time", DateTime.UtcNow.Date.ToBinary(), RegistryValueKind.QWord);
                key.SetValue("Counter", 0, RegistryValueKind.DWord);
                key.Close();

                key1 = Registry.CurrentUser.OpenSubKey(settingsName, true);
            }

            if (key1 != null)
            {
                var time = key1.GetValue("Time");
                var counter = key1.GetValue("Counter");

                var regTime = DateTime.FromBinary((long)time);
                var regCounter = (int)counter;

                if (DateTime.UtcNow.Date < regTime || regCounter == 20)
                {
                    MessageBox.Show("-+-😭-+-");
                    return;
                }

                if (DateTime.UtcNow.Date != regTime)
                {
                    key1.SetValue("Time", DateTime.UtcNow.Date.ToBinary(), RegistryValueKind.QWord);
                    key1.SetValue("Counter", counter, RegistryValueKind.DWord);
                }
            }
            key1.Close();
        }

    }
}
