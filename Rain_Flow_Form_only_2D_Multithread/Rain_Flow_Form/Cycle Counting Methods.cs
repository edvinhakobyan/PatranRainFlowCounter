using System;
using System.Windows.Forms;

namespace Fatige_Stress_Counting_Tool
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                checkBox2.Checked = false;
                Engine.Cycle_method = "rain_flow";
            }
            else
                checkBox2.Checked = true;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                checkBox1.Checked = false;
                Engine.Cycle_method = "full_cycle";
            }
            else
                checkBox1.Checked = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
                Close();
        }

    }
}
