using System;
using System.Windows.Forms;

namespace Fatige_Stress_Counting_Tool
{
    public partial class Mean_Stress_Correction_Class : Form
    {

        static bool choosing_equation = false;

        public static bool choosing_equation_Pr
        {
            get { return choosing_equation; }
            set { choosing_equation = value; }
        }





        public Mean_Stress_Correction_Class()
        {
            InitializeComponent();
        }
        
        
        Cai_equation_Form cai_equation_form = new Cai_equation_Form();
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

            engine.Stress_equation_Pr = "cai";

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
            
            engine.Stress_equation_Pr = "walker";


            if (radioButton2.Checked)
            {
                walker_equation_form = new Walker_Equation_Form();
                walker_equation_form.Show();
            }
            else
                walker_equation_form.Close();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            engine.Stress_equation_Pr = "goodman";

        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            engine.Stress_equation_Pr = "soderberg";

        }
                    

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            engine.Stress_equation_Pr = "morro";

        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            engine.Stress_equation_Pr = "gerber";

        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            engine.Stress_equation_Pr = "asme";

        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            engine.Stress_equation_Pr = "swt";

        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            engine.Stress_equation_Pr = "stulen";

        }

        private void radioButton10_CheckedChanged(object sender, EventArgs e)
        {
            engine.Stress_equation_Pr = "topper";

        } 


        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked || radioButton2.Checked || radioButton3.Checked || 
                radioButton4.Checked || radioButton5.Checked || radioButton6.Checked || 
                radioButton7.Checked || radioButton8.Checked || radioButton9.Checked ||
                radioButton10.Checked)
                choosing_equation = true;

            Close();
        }













    }
}
