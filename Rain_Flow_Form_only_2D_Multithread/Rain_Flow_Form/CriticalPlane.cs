using Fatige_Stress_Counting_Tool.Enums;
using System;
using System.Windows.Forms;

namespace Fatige_Stress_Counting_Tool
{
    public partial class CriticalPlane : Form
    {
        public CriticalPlane()
        {
            InitializeComponent();
            Engine.Multiaxial_stress = StressCalculationTypeEnum.CyclogramCriticalPlane;
        }

        private void radioButton1_Click(object sender, EventArgs e)
        {
            Engine.Multiaxial_stress = StressCalculationTypeEnum.CyclogramCriticalPlane;
            groupBox8.Enabled = false;
        }

        private void radioButton2_Click(object sender, EventArgs e)
        {
            Engine.Multiaxial_stress = StressCalculationTypeEnum.CriticalPlane;
            groupBox8.Enabled = true;
        }

        private void OK_Click(object sender, EventArgs e)
        {
            if(radioButton1.Checked)
            {
                Close();
                return;
            }

            if(radioButton2.Checked)
            {
                if(double.TryParse(Delta_angle.Text,out double  delta))
                {
                    Engine.Delta = delta;
                    Close();
                }
                else
                {
                    MessageBox.Show("Delta field must be given.");
                }
            }


        }
    }
}
