using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Fatige_Stress_Counting_Tool
{
    public partial class Walker_Equation_Form : Form
    {
        public Walker_Equation_Form()
        {
            InitializeComponent();
        }


        double w_a;
        double w_g;

        private void button1_Click(object sender, EventArgs e)
        {

            if (textBox1.Text == "")
            {
                MessageBox.Show("Coefficient A is empty                  ", "Warning");
                return;
            }
            else if (w_a <= 0)
            {
                MessageBox.Show("Coefficient A can not be les or equal 0  ", "Warning");
                textBox1.Clear();
                return;
            }
            else if (textBox2.Text == "")
            {
                MessageBox.Show("Coefficient Gamma is empty               ", "Warning");
                return;
            }

            else if (!double.TryParse(textBox2.Text, out w_g))
            {
                MessageBox.Show("Coefficient Gamma is not correct            ", "Warning");
                textBox1.Clear();
                return;
            }
            else if (w_g > 1)
            {
                MessageBox.Show("Coefficient Gamma can not be bigger 1     ", "Warning");
                textBox2.Clear();
                return;
            }
            else if (w_g <= 0)
            {
                MessageBox.Show("Coefficient Gamma can not be les or equal 0", "Warning");
                textBox2.Clear();
                return;
            }
            else
            {
                engine.Coef_a_walker_Pr = w_a;
                engine.Coef_gama_walker_Pr = w_g;
            }
            Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!double.TryParse(textBox1.Text, out w_a) && textBox1.Text != "")
            {
                textBox1.Clear();
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (!double.TryParse(textBox2.Text, out w_g) && textBox2.Text != "")
            {
                textBox2.Clear();
            }
        }




    }
}
