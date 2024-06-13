using System;
using System.Windows.Forms;
using Fatige_Stress_Counting_Tool.Enums;

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
            if (Main_Form.StressType == StressTypeEnum.OneDimensional)
            {
                Box1_2D.Enabled = false;
            }
            else if (Main_Form.StressType == StressTypeEnum.TwoDimensional)
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

        // Sign equivalent (von Mises) stress:
        private void RadioButton1_Click(object sender, EventArgs e)
        {
            Engine.Multiaxial_stress = StressCalculationTypeEnum.SignVonMises2D;
            Element_Property_Form.Delta_enable_Pr = false;
        }

        // Sign maximum shear stress:
        private void RadioButton2_Click(object sender, EventArgs e)
        {
            Engine.Multiaxial_stress = StressCalculationTypeEnum.SignMaximumShearStress2D;
            Element_Property_Form.Delta_enable_Pr = false;
        }

        // Maximum principal stress:
        private void RadioButton3_Click(object sender, EventArgs e)
        {
            Engine.Multiaxial_stress = StressCalculationTypeEnum.MaxPrincipal2D;
            Element_Property_Form.Delta_enable_Pr = false;
        }

        // Equivalent (von Mises) stress:
        private void RadioButton4_Click(object sender, EventArgs e)
        {
            Engine.Multiaxial_stress = StressCalculationTypeEnum.EquivalentVonMisesStress2D;
            Element_Property_Form.Delta_enable_Pr = false;
        }


        // Critical plane:
        private void RadioButton5_Click(object sender, EventArgs e)
        {
            Element_Property_Form.Delta_enable_Pr = true;

            new CriticalPlane().Show();
        }
    }
}
