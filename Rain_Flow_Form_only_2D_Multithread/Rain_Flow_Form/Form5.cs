using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Rain_Flow_Form
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            if (Form1.Stress_1D_2D_3D == "1_D")
            {
                Box1_2D.Enabled = false;
                Box2_3D.Enabled = false;
            }
            else if (Form1.Stress_1D_2D_3D == "2_D")
            {
                Box1_2D.Enabled = true;
                Box2_3D.Enabled = false;
            }
            else
            {
                Box1_2D.Enabled = false;
                Box2_3D.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            Rain.Multiaxial_stress_Pr = "2D_1";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            Rain.Multiaxial_stress_Pr = "2D_2";
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            Rain.Multiaxial_stress_Pr = "2D_3";
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            Rain.Multiaxial_stress_Pr = "2D_4";
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            Rain.Multiaxial_stress_Pr = "2D_5";
            Form2.Text_3_Pr = true;
        }

        private void radioButton10_CheckedChanged(object sender, EventArgs e)
        {
            Rain.Multiaxial_stress_Pr = "3D_1";
        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            Rain.Multiaxial_stress_Pr = "3D_2";
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            Rain.Multiaxial_stress_Pr = "3D_3";
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            Rain.Multiaxial_stress_Pr = "3D_4";
            Form2.Text_3_Pr = true;
        }
        

        


    }
}
