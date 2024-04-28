using System;
using System.Windows.Forms;

namespace Fatige_Stress_Counting_Tool
{
    public partial class Mean_Stress_Correction_Class : Form
    {
        public static bool choosing_equation_Pr { get; set; } = false;


        public Mean_Stress_Correction_Class()
        {
            InitializeComponent();
        }
        
        
        Cai_equation_Form cai_equation_form = new Cai_equation_Form();
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            Engine.Stress_equation = "cai";

            if (radioButton1.Checked)
            {
                cai_equation_form = new Cai_equation_Form();
                cai_equation_form.Show();
            }
            else
                cai_equation_form.Close();

        }

        Walker_Equation_Form walker_equation_form = new Walker_Equation_Form();
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            Engine.Stress_equation = "walker";
            if (radioButton2.Checked)
            {
                walker_equation_form = new Walker_Equation_Form();
                walker_equation_form.Show();
            }
            else
                walker_equation_form.Close();
        }

        Goodman_Equation_Form goodman_equation_form = new Goodman_Equation_Form();
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            Engine.Stress_equation = "goodman";
            if (radioButton3.Checked)
            {
                goodman_equation_form = new Goodman_Equation_Form();
                goodman_equation_form.Show();
            }
            else
                goodman_equation_form.Close();


        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            Engine.Stress_equation = "soderberg";

        }
                    

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            Engine.Stress_equation = "morro";

        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            Engine.Stress_equation = "gerber";

        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            Engine.Stress_equation = "asme";

        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            Engine.Stress_equation = "swt";

        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            Engine.Stress_equation = "stulen";

        }

        private void radioButton10_CheckedChanged(object sender, EventArgs e)
        {
            Engine.Stress_equation = "topper";

        }

        Cai_New_equation_Form cai_new_equation_form = new Cai_New_equation_Form();
        private void radioButton11_CheckedChanged(object sender, EventArgs e)
        {
            Engine.Stress_equation = "cai_new";

            if (radioButton11.Checked)
            {
                cai_new_equation_form = new Cai_New_equation_Form();
                cai_new_equation_form.Show();
            }
            else
                cai_new_equation_form.Close();

            Element_Property_Form.Sigma_02_enable = !Element_Property_Form.Sigma_02_enable;
            Element_Property_Form.Ktg_enable = !Element_Property_Form.Ktg_enable;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked || radioButton2.Checked || radioButton3.Checked || 
                radioButton4.Checked || radioButton5.Checked || radioButton6.Checked || 
                radioButton7.Checked || radioButton8.Checked || radioButton9.Checked ||
                radioButton10.Checked || radioButton11.Checked)
                choosing_equation_Pr = true;

            Close();
        }

    }
}
