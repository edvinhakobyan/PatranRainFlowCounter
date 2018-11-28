using System;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace Fatige_Stress_Counting_Tool
{
    public partial class Main_Form : Form
    {
        string Cyclogramm_File_Name;
        static string stress_1D_2D_3D = "1_D";


        public static string Stress_1D_2D_3D
        {
            get { return stress_1D_2D_3D; }
        }

        public Main_Form()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string lic = File.ReadAllText("\\\\192.168.0.1\\Projects\\SCAC\\Work Data\\hakobyan.edvin.a\\Lic.txt");
            License(long.Parse(lic));
        }

        OpenFileDialog openfiledialog = new OpenFileDialog();
        SaveFileDialog savefiledialog = new SaveFileDialog();
        FolderBrowserDialog folder = new FolderBrowserDialog();
        
        engine engine_ = new engine();
        Form3 Forma_3 = new Form3();
        Mean_Stress_Correction_Class stress_correct = new Mean_Stress_Correction_Class();
        Form5 Forma_5 = new Form5();
        Report_File_Format_1D Forma_6 = new Report_File_Format_1D();
        Report_File_Format_2D Forma_7 = new Report_File_Format_2D();
        Report_File_Format_3D Forma_8 = new Report_File_Format_3D();
        Cyclogram_File_Format cyclogram_file_format = new Cyclogram_File_Format();
        Element_Property_Form elm_prop_elm = new Element_Property_Form();


        


        private void Atach_1D_Report_File_Click(object sender, EventArgs e)
        {
            openfiledialog.Title = "Atach Uniaxial Report File";
            openfiledialog.Filter = "Text Files (*.txt, *.dat, *.rpt) | *.txt; *.dat; *.rpt;  | All Files (*.*) | *.*";
            openfiledialog.CheckPathExists = true;
            openfiledialog.CheckFileExists = true;

            if (openfiledialog.ShowDialog() == DialogResult.OK)
            {
                engine.Report_File_Name_Pr = openfiledialog.FileName;
                stress_1D_2D_3D = "1_D";

                Element_Property_Enabling(true, true, false, false);

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
                engine.Report_File_Name_Pr = openfiledialog.FileName;
                stress_1D_2D_3D = "2_D";
                engine.Multiaxial_stress_Pr = "2D_1";

                Element_Property_Enabling(true, true, true, false);
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
                engine.Report_File_Name_Pr = openfiledialog.FileName;
                stress_1D_2D_3D = "3_D";
                engine.Multiaxial_stress_Pr = "3D_1";

                Element_Property_Enabling(true, true, false, false);
                
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
                engine.Ciclogramm_File_Name_Pr = Cyclogramm_File_Name;
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
                    engine.Templet_File_Name_Pr += (Templet_File_Name[i] + "\\");
                    engine.Temporary_File_Name_Pr += (Templet_File_Name[i] + "\\");
                }
                engine.Templet_File_Name_Pr += "Stress_Templ.res_tmpl";
                engine.Temporary_File_Name_Pr += "Report_Temporary.txt";

                engine.Result_File_Name_Pr = savefiledialog.FileName;
                
            }

        }

        private void Run_Click(object sender, EventArgs e)
        {

             engine.Console_Show_Or_No_Pr = Show_Consol.Checked;

            if (!File.Exists(engine.Report_File_Name_Pr))
            {
                MessageBox.Show("Select Report File !                                                  ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            if (!File.Exists(engine.Ciclogramm_File_Name_Pr))
            {
                MessageBox.Show("Select File With Cyclograms !                                         ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (engine.Result_File_Name_Pr == null)
            {
                MessageBox.Show("Select Path for Results !" + new string(' ',50), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            if (!Mean_Stress_Correction_Class.choosing_equation_Pr)
            {
                MessageBox.Show("Choose one of equation !                                              ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }



            if (engine.Stress_equation_Pr == "walker")
            {
                if (engine.Coef_a_walker_Pr == 0)
                {
                    MessageBox.Show("Coefficient A in Walker equation is not correct !                 ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (engine.Coef_gama_walker_Pr == 0)
                {
                    MessageBox.Show("Coefficient Gamma in Walker equation is not correct !             ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }


            if (engine.elm_prop_Pr.Count == 0)
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

            
            if (stress_1D_2D_3D == "1_D")
            {
                Thread d_1 = new Thread(new ThreadStart(engine_.Engine_for_1D));
                d_1.Start();
            }
            else if (stress_1D_2D_3D == "2_D")
            {
                if (engine.Multiaxial_stress_Pr == "2D_5")
                {
                    Thread d_2 = new Thread(new ThreadStart(engine_.Engine_for_2D_critical_plane));
                    d_2.Start();
                }
                else
                {
                    Thread d_2 = new Thread(new ThreadStart(engine_.Engine_for_2D));
                    d_2.Start();
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


        private void Element_Property_Enabling(bool elm_list, bool kof_m, bool kof_k, bool delta)
        {
            Element_Property_Form.Elm_list_enable_Pr = elm_list;
            Element_Property_Form.Koef_m_enable_Pr = kof_m;
            Element_Property_Form.Koef_k_enable_Pr = kof_k;
            Element_Property_Form.delta_enable_Pr = delta;
        }


        static void License(long a)
        {
            DateTime now = DateTime.Now;
            DateTime end = DateTime.FromBinary(0);

            try
            {
                end = DateTime.FromBinary(a);
            }
            catch
            {
                MessageBox.Show("!!!!????!!!!                                ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Environment.Exit(0);
            }

            //DateTime end1 = DateTime.Parse("30.12.2018"); //gtnel datain hamapatasxan long
            //long iii = end1.ToBinary();

 
            bool flag = (DateTime.Compare(now, end) > 0) ? true : false;
            if (flag)
            {
                MessageBox.Show("!!!!????!!!!                                ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Environment.Exit(0);
            }
        }

    }
}
