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

        private void Button1_Click(object sender, EventArgs e)
        {
           Close();
        }

        private void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            Engine.Multiaxial_stress_Pr = "2D_1";
            Element_Property_Form.Delta_enable_Pr = false;
        }

        private void RadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            Engine.Multiaxial_stress_Pr = "2D_2";
            Element_Property_Form.Delta_enable_Pr = false;
        }

        private void RadioButton3_CheckedChanged(object sender, EventArgs e)
        {
            Engine.Multiaxial_stress_Pr = "2D_3";
            Element_Property_Form.Delta_enable_Pr = false;
        }

        private void RadioButton4_CheckedChanged(object sender, EventArgs e)
        {
            Engine.Multiaxial_stress_Pr = "2D_4";
            Element_Property_Form.Delta_enable_Pr = false;
        }

        private void RadioButton5_CheckedChanged(object sender, EventArgs e)
        {
            Engine.Multiaxial_stress_Pr = "2D_5";
            Element_Property_Form.Delta_enable_Pr = true;
        }

    }
}
