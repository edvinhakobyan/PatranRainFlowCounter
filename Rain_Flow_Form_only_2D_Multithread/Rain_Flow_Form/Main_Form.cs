using System;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using Fatige_Stress_Counting_Tool.Enums;
using System.Globalization;


namespace Fatige_Stress_Counting_Tool
{
    public partial class Main_Form : Form
    {
        OpenFileDialog openfiledialog = new OpenFileDialog();
        SaveFileDialog savefiledialog = new SaveFileDialog();

        Engine engine_ = new Engine();
        Form3 Forma_3 = new Form3();
        Mean_Stress_Correction_Class stress_correct = new Mean_Stress_Correction_Class();
        Form5 Forma_5 = new Form5();
        Report_File_Format_1D Forma_6 = new Report_File_Format_1D();
        Report_File_Format_2D Forma_7 = new Report_File_Format_2D();
        Report_File_Format_3D Forma_8 = new Report_File_Format_3D();
        Cyclogram_File_Format cyclogram_file_format = new Cyclogram_File_Format();
        Element_Property_Form elm_prop_elm = new Element_Property_Form();

        public static StressTypeEnum StressType { get; private set; } = StressTypeEnum.OneDimensional;
        public string Cyclogramm_File_Name { get; set; }

        public Main_Form()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //try
            //{
            //    var activTo = "03:06:2024";
            //    if (DateTime.Now > DateTime.ParseExact(activTo, "dd:MM:yyyy", CultureInfo.InvariantCulture))
            //    {
            //        throw new Exception();
            //    }
            //}
            //catch
            //{
            //    MessageBox.Show("Something was wrong.\nConfiguration File Not Found !", "Error",
            //        MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    throw;
            //}
        }


        private void Atach_1D_Report_File_Click(object sender, EventArgs e)
        {
            openfiledialog.Title = "Atach Uniaxial Report File";
            openfiledialog.Filter = "Text Files (*.txt, *.dat, *.rpt) | *.txt; *.dat; *.rpt;  | All Files (*.*) | *.*";
            openfiledialog.CheckPathExists = true;
            openfiledialog.CheckFileExists = true;

            if (openfiledialog.ShowDialog() == DialogResult.OK)
            {
                Engine.Report_File_Name = openfiledialog.FileName;
                StressType = StressTypeEnum.OneDimensional;

                Element_Property_Enabling(true, true, false, false, true, true);
            }
        }

        private void Atach_2D_Report_File_Click(object sender, EventArgs e)
        {
            openfiledialog.Title = "Atach Biaxial Report File";
            openfiledialog.Filter = "Text Files (*.txt, *.dat, *.rpt) | *.txt; *.dat; *.rpt;  | All Files (*.*) | *.*";
            openfiledialog.CheckPathExists = true;
            openfiledialog.CheckFileExists = true;

            if (openfiledialog.ShowDialog() == DialogResult.OK)
            {
                Engine.Report_File_Name = openfiledialog.FileName;
                StressType = StressTypeEnum.TwoDimensional;
                Engine.Multiaxial_stress = StressCalculationTypeEnum.SignVonMises2D;

                Element_Property_Enabling(true, true, true, false, false, false);
            }
        }

        private void Atach_3D_Report_File_Click(object sender, EventArgs e)
        {

            openfiledialog.Title = "Atach Volumetric Report File";
            openfiledialog.Filter = "Text Files (*.txt, *.dat, *.rpt) | *.txt; *.dat; *.rpt;  | All Files (*.*) | *.*";
            openfiledialog.CheckPathExists = true;
            openfiledialog.CheckFileExists = true;

            if (openfiledialog.ShowDialog() == DialogResult.OK)
            {
                Engine.Report_File_Name = openfiledialog.FileName;
                StressType = StressTypeEnum.ThreeDimensional;
                //Engine.Multiaxial_stress = StressTypeEnum.;

                Element_Property_Enabling(true, true, false, false, true, true);
            }
        }

        private void Atach_Cyclogram_File_Click(object sender, EventArgs e)
        {

            openfiledialog.Title = "Atach Cyclogram Report File";
            openfiledialog.Filter = "Text Files (*.txt, *.dat) | *.txt; *.dat;  | All Files (*.*) | *.*";
            openfiledialog.CheckPathExists = true;
            openfiledialog.CheckFileExists = true;

            if (openfiledialog.ShowDialog() == DialogResult.OK)
            {
                Cyclogramm_File_Name = openfiledialog.FileName;
                Engine.Ciclogramm_File_Name = Cyclogramm_File_Name;
            }
        }

        private void Save_Result_As_Click(object sender, EventArgs e)
        {

            savefiledialog.Title = "Save File";
            savefiledialog.DefaultExt = ".els";
            savefiledialog.Filter = "Text Files (*.els) | *.els";
            savefiledialog.OverwritePrompt = true;

            if (savefiledialog.ShowDialog() == DialogResult.OK)
            {
                char[] sep = new char[] { '\\' };
                string[] Templet_File_Name = savefiledialog.FileName.Split(sep);
                for (int i = 0; i < Templet_File_Name.Length - 1; i++)
                {
                    Engine.Templet_File_Name += (Templet_File_Name[i] + "\\");
                    Engine.Temporary_File_Name += (Templet_File_Name[i] + "\\");
                }
                Engine.Templet_File_Name += "Stress_Templ.res_tmpl";
                Engine.Temporary_File_Name += "Report_Temporary.txt";

                Engine.Result_File_Name = savefiledialog.FileName;

            }

        }

        private void Run_Click(object sender, EventArgs e)
        {
            Engine.ConsoleShow = Show_Consol.Checked;

            if (!File.Exists(Engine.Report_File_Name))
            {
                MessageBox.Show("Select Report File !                                                  ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!File.Exists(Engine.Ciclogramm_File_Name))
            {
                MessageBox.Show("Select File With Cyclograms !                                         ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (Engine.Result_File_Name == null)
            {
                MessageBox.Show("Select Path for Results !" + new string(' ', 50), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            if (Engine.Stress_equation == MeanStressCorrectionEnum.Walker)
            {
                if (Engine.Coef_a_walker == 0)
                {
                    MessageBox.Show("Coefficient A in Walker equation is not correct !                 ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (Engine.Coef_gama_walker == 0)
                {
                    MessageBox.Show("Coefficient Gamma in Walker equation is not correct !             ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }


            if (Engine.Elm_prop.Count == 0)
            {
                MessageBox.Show("Problem With Elements Properties !!!");
                return;
            }

            #region //elm_prop_check comment
            //foreach (TabPage tp in ((TabControl)elm_prop_elm.Controls[5]).TabPages) //elm_prop_check
            //{
            //    if (tp.Controls["Elm_list"].Text == "")
            //    {
            //        MessageBox.Show("Element List in <" + tp.Controls["Elm_list"].Parent.Text + "> is Empty");
            //        return;
            //    }

            //    if (tp.Controls["Kof_m"].Text == "")
            //    {
            //        MessageBox.Show("Coefficient m in <" + tp.Controls["Elm_list"].Parent.Text + "> is Empty");
            //        return;
            //    }

            //    if (tp.Controls["Kof_k"].Text == "")
            //    {
            //        MessageBox.Show("Coefficient k in <" + tp.Controls["Elm_list"].Parent.Text + "> is Empty");
            //        return;
            //    }
            //}
            #endregion


            if (StressType == StressTypeEnum.OneDimensional)
            {
                var thread = new Thread(new ParameterizedThreadStart(engine_.Engine_for_1D));
                thread.Start(progressBar);
            }
            else if (StressType == StressTypeEnum.TwoDimensional)
            {

                if (Engine.Multiaxial_stress == StressCalculationTypeEnum.CriticalPlane)
                {
                    var thread = new Thread(new ParameterizedThreadStart(engine_.Engine_for_2D_critical_plane_CycleAngle));
                    thread.Start(progressBar);
                }
                else if(Engine.Multiaxial_stress == StressCalculationTypeEnum.CyclogramCriticalPlane)
                {
                    var thread = new Thread(new ParameterizedThreadStart(engine_.Engine_for_2D_critical_plane_CycleAngle));
                    thread.Start(progressBar);
                }
                else
                {
                    var thread = new Thread(new ParameterizedThreadStart(engine_.Engine_for_2D));
                    thread.Start(progressBar);
                }
            }
        }

        private void Counting_Methods_Click(object sender, EventArgs e)
        {
            Forma_3.ShowDialog();
        }

        private void Mean_Stress_Correction_Click(object sender, EventArgs e)
        {
            stress_correct.ShowDialog();
        }

        private void Stress_Combination_Click(object sender, EventArgs e)
        {
            Forma_5.ShowDialog();
        }

        private void Uniaxial_Click(object sender, EventArgs e)
        {
            Forma_6.ShowDialog();
        }

        private void Biaxial_Click(object sender, EventArgs e)
        {
            Forma_7.ShowDialog();
        }

        private void Volumetric_Click(object sender, EventArgs e)
        {
            Forma_8.ShowDialog();
        }

        private void Cycl_File_Format_Click(object sender, EventArgs e)
        {
            cyclogram_file_format.ShowDialog();
        }

        private void Element_Property_Click(object sender, EventArgs e)
        {
            elm_prop_elm.ShowDialog();
        }

        private void Element_Property_Enabling(bool elm_list, bool kof_m, bool kof_k, bool delta, bool sig02, bool ktg)
        {
            Element_Property_Form.Elm_list_enable_Pr = elm_list;
            Element_Property_Form.Koef_m_enable_Pr = kof_m;
            Element_Property_Form.Koef_k_enable_Pr = kof_k;
            Element_Property_Form.Delta_enable_Pr = delta;
            Element_Property_Form.Sigma_02_enable = sig02;
            Element_Property_Form.Ktg_enable = ktg;
        }
    }
}
