using System;
using System.Windows.Forms;

namespace Fatige_Stress_Counting_Tool
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            if (Main_Form.Stress_1D_2D_3D == "1_D")
            {
                Box1_2D.Enabled = false;
            }
            else if (Main_Form.Stress_1D_2D_3D == "2_D")
            {
                Box1_2D.Enabled = true;
            }
            else
            {
                Box1_2D.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
           Close();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            engine.Multiaxial_stress_Pr = "2D_1";
            Element_Property_Form.delta_enable_Pr = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            engine.Multiaxial_stress_Pr = "2D_2";
            Element_Property_Form.delta_enable_Pr = false;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            engine.Multiaxial_stress_Pr = "2D_3";
            Element_Property_Form.delta_enable_Pr = false;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            engine.Multiaxial_stress_Pr = "2D_4";
            Element_Property_Form.delta_enable_Pr = false;
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            engine.Multiaxial_stress_Pr = "2D_5";
            Element_Property_Form.delta_enable_Pr = true;
        }

    }
}
