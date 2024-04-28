using Fatige_Stress_Counting_Tool.Enums;
using System;
using System.Windows.Forms;

namespace Fatige_Stress_Counting_Tool
{
    public partial class Mean_Stress_Correction_Class : Form
    {
        public static bool Choosing_equation_Pr { get; set; } = false;


        public Mean_Stress_Correction_Class()
        {
            InitializeComponent();
        }
        
        
        Cai_equation_Form cai_equation_form = new Cai_equation_Form();
        private void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            Engine.Stress_equation = MeanStressCorrectionEnum.Cai;

            if (radioButton1.Checked)
            {
                cai_equation_form = new Cai_equation_Form();
                cai_equation_form.Show();
            }
            else
                cai_equation_form.Close();

        }

        Walker_Equation_Form walker_equation_form = new Walker_Equation_Form();
        private void RadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            Engine.Stress_equation = MeanStressCorrectionEnum.Walker;
            if (radioButton2.Checked)
            {
                walker_equation_form = new Walker_Equation_Form();
                walker_equation_form.Show();
            }
            else
                walker_equation_form.Close();
        }

        Goodman_Equation_Form goodman_equation_form = new Goodman_Equation_Form();
        private void RadioButton3_CheckedChanged(object sender, EventArgs e)
        {
            Engine.Stress_equation = MeanStressCorrectionEnum.Goodman;
            if (radioButton3.Checked)
            {
                goodman_equation_form = new Goodman_Equation_Form();
                goodman_equation_form.Show();
            }
            else
                goodman_equation_form.Close();
        }

        private void RadioButton4_CheckedChanged(object sender, EventArgs e)
        {
            Engine.Stress_equation = MeanStressCorrectionEnum.Soderberg;
        }
                    

        private void RadioButton5_CheckedChanged(object sender, EventArgs e)
        {
            Engine.Stress_equation = MeanStressCorrectionEnum.Morro;
        }

        private void RadioButton6_CheckedChanged(object sender, EventArgs e)
        {
            Engine.Stress_equation = MeanStressCorrectionEnum.Gerber;
        }

        private void RadioButton7_CheckedChanged(object sender, EventArgs e)
        {
            Engine.Stress_equation = MeanStressCorrectionEnum.Asme;
        }

        private void RadioButton8_CheckedChanged(object sender, EventArgs e)
        {
            Engine.Stress_equation = MeanStressCorrectionEnum.Swt;
        }

        private void RadioButton9_CheckedChanged(object sender, EventArgs e)
        {
            Engine.Stress_equation = MeanStressCorrectionEnum.Stulen;
        }

        private void radioButton10_CheckedChanged(object sender, EventArgs e)
        {
            Engine.Stress_equation = MeanStressCorrectionEnum.Topper;
        }

        Cai_New_equation_Form cai_new_equation_form = new Cai_New_equation_Form();
        private void RadioButton11_CheckedChanged(object sender, EventArgs e)
        {
            Engine.Stress_equation = MeanStressCorrectionEnum.CaiNew;

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

        private void Button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked || radioButton2.Checked || radioButton3.Checked || 
                radioButton4.Checked || radioButton5.Checked || radioButton6.Checked || 
                radioButton7.Checked || radioButton8.Checked || radioButton9.Checked ||
                radioButton10.Checked || radioButton11.Checked)
                Choosing_equation_Pr = true;

            Close();
        }
    }
}
