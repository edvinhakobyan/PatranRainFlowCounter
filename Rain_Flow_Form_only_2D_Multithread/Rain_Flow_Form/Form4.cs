using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Rain_Flow_Form
{
    public partial class Form4 : Form
    {

        public static bool choosing_equation = false;


        public Form4()
        {
            InitializeComponent();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            Rain.Stress_equation_Pr = "cai";
            if(radioButton1.Checked)
            {
                Cai_equation_Form cai = new Cai_equation_Form();
                cai.Show();
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            Rain.Stress_equation_Pr = "walker_";
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            Rain.Stress_equation_Pr = "goodman";
            if (radioButton3.Checked)
            {
                Formuls g = new Formuls();
                g.Show();
            }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            Rain.Stress_equation_Pr = "soderberg";
            if (radioButton4.Checked)
            {
                Formuls g = new Formuls();
                g.Show();
            }
        }
                    

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            Rain.Stress_equation_Pr = "morro";
            if (radioButton5.Checked)
            {
                Formuls g = new Formuls();
                g.Show();
            }
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            Rain.Stress_equation_Pr = "gerber";
            if (radioButton6.Checked)
            {
                Formuls g = new Formuls();
                g.Show();
            }
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            Rain.Stress_equation_Pr = "asme";
            if (radioButton7.Checked)
            {
                Formuls g = new Formuls();
                g.Show();
            }
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            Rain.Stress_equation_Pr = "swt";
            if (radioButton8.Checked)
            {
                Formuls g = new Formuls();
                g.Show();
            }
        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            Rain.Stress_equation_Pr = "stulen";
            if (radioButton9.Checked)
            {
                Formuls g = new Formuls();
                g.Show();
            }
        }

        private void radioButton10_CheckedChanged(object sender, EventArgs e)
        {
            Rain.Stress_equation_Pr = "topper";
            if (radioButton10.Checked)
            {
                Formuls g = new Formuls();
                g.Show();
            }
        } 

        private void radioButton11_CheckedChanged(object sender, EventArgs e)
        {
            Rain.Stress_equation_Pr = "walker";
            if (radioButton11.Checked)
            {
                Formuls g = new Formuls();
                g.Show();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked || radioButton2.Checked || radioButton3.Checked || 
                radioButton4.Checked || radioButton5.Checked || radioButton6.Checked || 
                radioButton7.Checked || radioButton8.Checked || radioButton9.Checked ||
                radioButton10.Checked || radioButton11.Checked)
                choosing_equation = true;
            Close();
        }

        private void Form4_Load(object sender, EventArgs e)
        {

        }








    }
}
