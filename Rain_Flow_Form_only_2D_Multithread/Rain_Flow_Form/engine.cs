using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using System.Text;
using System.Linq;
using System.IO;
using System;

namespace Fatige_Stress_Counting_Tool
{
    public class engine
    {
        static string Report_File_Name;  // patranic stacvac report fayli anun@
        static string Cyclogramm_File_Name; // ciklogrammai fayli anun@
        static string Result_File_Name;
        static string Multiaxial_stress;
        static string Stress_equation;
        static string Counting_method = "rain_flow";
        static Dictionary<int, double[]> elm_prop = new Dictionary<int, double[]>();


        double Cof_k;
        double Cof_m;
        static double Delta;
        static double Coef_a_walker;
        static double Coef_gama_walker;
        static double Gudman_ult_stress = 1; //+
        static bool Console_Show_Or_No;
        static string Templet_File_Name;
        static string Temporary_File_Name;
        static char[] symbols = { ' ', '\t' };


        public static string Report_File_Name_Pr
        {
            get { return Report_File_Name; }
            set { Report_File_Name = value; }
        }
        public static string Ciclogramm_File_Name_Pr
        {
            get { return Cyclogramm_File_Name; }
            set { Cyclogramm_File_Name = value; }
        }
        public static string Result_File_Name_Pr
        {
            get { return Result_File_Name; }
            set { Result_File_Name = value; }
        }
        public static string Templet_File_Name_Pr
        {
            get { return Templet_File_Name; }
            set { Templet_File_Name = value; }

        }
        public static string Temporary_File_Name_Pr
        {
            get { return Temporary_File_Name; }
            set { Temporary_File_Name = value; }
        }
        public static double Delta_Pr
        {
            get { return Delta; }
            set { Delta = value; }
        }
        public static double Coef_a_walker_Pr
        {
            get { return Coef_a_walker; }
            set { Coef_a_walker = value; }
        }
        public static double Coef_gama_walker_Pr
        {
            get { return Coef_gama_walker; }
            set { Coef_gama_walker = value; }
        }
        public static string Multiaxial_stress_Pr
        {
            get { return Multiaxial_stress; }
            set { Multiaxial_stress = value; }
        }
        public static string Stress_equation_Pr
        {
            get { return Stress_equation; }
            set { Stress_equation = value; }
        }
        public static string Cycle_method_Pr
        {
            get { return Counting_method; }
            set { Counting_method = value; }
        }
        public static bool Console_Show_Or_No_Pr
        {
            get { return Console_Show_Or_No; }
            set { Console_Show_Or_No = value; }
        }
        public static Dictionary<int, double[]> elm_prop_Pr
        {
            get { return elm_prop; }
            set { elm_prop = value; }
        }


        public void Engine_for_1D()
        {

            #region Reading Cyclograms File !!!
            Console.WriteLine("Reading Cyclograms File !!!");

            int Cycle_Line_count = 0;
            int Cycle_Column_count = 0;

            double[,] cycl_matrix = returne_cyclograms(ref Cycle_Line_count, ref Cycle_Column_count);

            if (cycl_matrix == null)
            {
                MessageBox.Show("Cyclograms File Is Incorrect..." + new string(' ', 50), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Console.WriteLine("Cyclograms File Read Successfully !!!");
            #endregion

            #region Reading Patran Report File !!!
            Console.WriteLine("Reading Patran Report File !!!");

            int Load_Case_Count = 0;
            int Element_Count = 0;
            int tox_byate_lengt = 0;


            if (!Create_Temporary_Result_File(ref Load_Case_Count, ref Element_Count, ref tox_byate_lengt, 2))
            {
                MessageBox.Show("Report File is incorrect..." + new string(' ', 50), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            Console.WriteLine("Patran Report File Read Successfully !!!");
            #endregion

            #region IF Cycle_Column_count == Load_Case_Count
            if (Cycle_Column_count != Load_Case_Count)
            {
                MessageBox.Show("The Number of Load Cases is Not Equal to The Number of Cyclograms !", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            #endregion

            #region Element_Property_Check
            Console.WriteLine("Property check started !!!");

            BinaryReader element_id_from_report = new BinaryReader(File.OpenRead(Temporary_File_Name));
            List<int> error_list = new List<int>();

            int id;
            for (int i = 0; i < 16 * Element_Count; i += 16)
            {

                element_id_from_report.BaseStream.Position = i;
                id = (int)element_id_from_report.ReadInt64();

                if (!elm_prop.ContainsKey(id))
                    error_list.Add(id);
            }
            element_id_from_report.Close();


            if (error_list.Count != 0)
            {
                string path = Temporary_File_Name.Substring(0, Temporary_File_Name.IndexOf(".txt")) + "_Error_List.txt";
                StreamWriter Error_List_Stream = new StreamWriter(path);

                foreach (int elmid in error_list)
                    Error_List_Stream.Write(elmid + " ");

                Error_List_Stream.Close();

                MessageBox.Show("Elements in Error_List.txt" + "Has No Properties", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Console.WriteLine("Property check ended !!!");
            #endregion


            MessageBox.Show(string.Format("The Number Of Point In Ciclograms = {0}\nThe Number Of Load Cases = {1}\nThe Number Of Elements = {2}", Cycle_Line_count, Load_Case_Count, Element_Count));

            BinaryReader read = new BinaryReader(File.Open(Temporary_File_Name, FileMode.Open));

            StreamWriter equivalent_stress_file = new StreamWriter(Result_File_Name);

            Create_templit_1D(Templet_File_Name);

            equivalent_stress_file.WriteLine("Fatigue Stress:" + Environment.NewLine + "1" + Environment.NewLine +
                                                  "SUBTITLE 1" + Environment.NewLine + "SUBTITLE 2");

            Console.WriteLine("Start Solving....");

            string outtext = "";
            var time = DateTime.Now;
            int Element_id = 0;
            double Sigma_equiv = 0.0;
            double[] stress_x_direction = new double[Load_Case_Count];

            for (int i = 0; i < Element_Count; i++)
            {
                read.BaseStream.Position = i * 16;

                Element_id = (int)read.ReadInt64();
                Cof_m = elm_prop[Element_id][0];

                for (int j = 0; j < Load_Case_Count; j++)
                {
                    read.BaseStream.Position = (j * Element_Count + i) * 16 + 8; // (+8) araji long@ic heto ekox@
                    stress_x_direction[j] = read.ReadDouble();
                }


                switch (Counting_method)
                {
                    case "rain_flow": Sigma_equiv = Rain_flow_method(Oneaxial_stress_cycle(cycl_matrix, stress_x_direction), Cof_m); break;
                    case "full_cycle": Sigma_equiv = Full_cycle_method(Oneaxial_stress_cycle(cycl_matrix, stress_x_direction), Cof_m); break;
                    default: MessageBox.Show("Select Cycle Counting Method"); return;
                }


                outtext = Element_id + Environment.NewLine + Exponent_String_Format(Sigma_equiv);

                equivalent_stress_file.WriteLine(outtext);

                if (Console_Show_Or_No)
                    Console.WriteLine(outtext);

                Console.Title = "Completed Percent " + (100 * i / Element_Count) + "%";

            }


            equivalent_stress_file.Close();
            read.Close();
            File.Delete(Temporary_File_Name);
            Console.Title = "Completed Percent " + 100 + "%";
            Console.WriteLine("\nEnd Solving.\nElapsed Time " + (DateTime.Now - time));

        }
        public void Engine_for_2D()
        {


            #region Reading Cyclograms File !!!
            Console.WriteLine("Reading Cyclograms File !!!");

            int Cycle_Line_count = 0;
            int Cycle_Column_count = 0;

            double[,] cycl_matrix = returne_cyclograms(ref Cycle_Line_count, ref Cycle_Column_count);

            if (cycl_matrix == null)
            {
                MessageBox.Show("Cyclograms File Is Incorrect..." + new string(' ', 50), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Console.WriteLine("Cyclograms File Read Successfully !!!");
            #endregion

            #region Reading Patran Report File !!!
            Console.WriteLine("Reading Patran Report File !!!");

            int Load_Case_Count = 0;
            int Element_Count = 0;
            int tox_byate_lengt = 0;


            if (!Create_Temporary_Result_File(ref Load_Case_Count, ref Element_Count, ref tox_byate_lengt, 4))
            {
                MessageBox.Show("Report File is incorrect..." + new string(' ', 50), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Main_Form.progressBar1.Maximum = Element_Count;

            Console.WriteLine("Patran Report File Read Successfully !!!");
            #endregion

            #region IF Cycle_Column_count == Load_Case_Count
            if (Cycle_Column_count != Load_Case_Count)
            {
                MessageBox.Show("The Number of Load Cases is Not Equal to The Number of Cyclograms!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            #endregion

            #region Element_Property_Check
            Console.WriteLine("Property check started !!!");

            BinaryReader element_id_from_report = new BinaryReader(File.OpenRead(Temporary_File_Name));
            List<int> error_list = new List<int>();

            int id;
            for (int i = 0; i < 32 * Element_Count; i += 32)
            {

                element_id_from_report.BaseStream.Position = i;
                id = (int)element_id_from_report.ReadInt64();

                if (!elm_prop.ContainsKey(id))
                    error_list.Add(id);
            }
            element_id_from_report.Close();


            if (error_list.Count != 0)
            {
                string path = Temporary_File_Name.Substring(0, Temporary_File_Name.IndexOf(".txt")) + "_Error_List.txt";
                StreamWriter Error_List_Stream = new StreamWriter(path);

                foreach (int elmid in error_list)
                    Error_List_Stream.Write(elmid + " ");

                Error_List_Stream.Close();

                MessageBox.Show("Elements in Error_List.txt" + "Has No Properties", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Console.WriteLine("Property check ended !!!");
            #endregion

            MessageBox.Show(string.Format("The Number Of Point In Ciclograms = {0}\nThe Number Of Load Cases = {1}\nThe Number Of Elements = {2}", Cycle_Line_count, Load_Case_Count, Element_Count));

            BinaryReader read = new BinaryReader(File.Open(Temporary_File_Name, FileMode.Open));

            StreamWriter equivalent_stress_file = new StreamWriter(Result_File_Name);


            Create_templit_2D(Templet_File_Name);

            equivalent_stress_file.WriteLine("Fatigue Stress:" + Environment.NewLine + "4" + Environment.NewLine +
                                                  "SUBTITLE 1" + Environment.NewLine + "SUBTITLE 2");
            Console.WriteLine("Start Solving....");

            string outtext = "";

            var time = DateTime.Now;
            


            int Element_id = 0;
            double temp1 = 0.0;
            double temp2 = 0.0;
            double[,] z0_stress = new double[Load_Case_Count, 3];
            double[,] z1_stress = new double[Load_Case_Count, 3];
            double[,] z2_stress = new double[Load_Case_Count, 3];

            double Sigma_equiv_z0, Sigma_equiv_z1, Sigma_equiv_z2;

            for (int i = 0; i < Element_Count; i++)
            {
                read.BaseStream.Position = i * 32;

                Element_id = (int)read.ReadInt64();

                Cof_m = elm_prop[Element_id][0];
                Cof_k = elm_prop[Element_id][1];

                for (int j = 0; j < Load_Case_Count; j++)
                {
                    for (int k = 1; k < 4; k++) //0-n elementi hamarn a, 0-X,1-Y,2-XY
                    {
                        read.BaseStream.Position = (2 * j * Element_Count + i) * 32 + k * 8; // (+8*k) araji long@ic heto ekox@
                        temp1 = read.ReadDouble();

                        read.BaseStream.Position = (2 * j * Element_Count + i + Element_Count) * 32 + k * 8;
                        temp2 = read.ReadDouble();

                        if (k == 1 || k == 2)
                        {
                            z0_stress[j, k - 1] = (temp1 + temp2) / 2;
                            z1_stress[j, k - 1] = z0_stress[j, k - 1] + Cof_k * (temp1 - temp2) / 2;
                            z2_stress[j, k - 1] = z0_stress[j, k - 1] + Cof_k * (temp2 - temp1) / 2;
                        }
                        else // es el qez zdvig@ axper jan
                        {
                            z0_stress[j, k - 1] = (temp1 + temp2) / 2;
                            z1_stress[j, k - 1] = z0_stress[j, k - 1] + (temp1 - temp2) / 2;
                            z2_stress[j, k - 1] = z0_stress[j, k - 1] + (temp2 - temp1) / 2;
                        }
                    }
                }

                Sigma_equiv_z0 = 0;
                Sigma_equiv_z1 = 0;
                Sigma_equiv_z2 = 0;

                switch (Counting_method)
                {
                    case "rain_flow":
                        {
                            Sigma_equiv_z1 = Rain_flow_method(Multiaxial_stress_cycle_2D(cycl_matrix, z1_stress), Cof_m);
                            Sigma_equiv_z0 = Rain_flow_method(Multiaxial_stress_cycle_2D(cycl_matrix, z0_stress), Cof_m);
                            Sigma_equiv_z2 = Rain_flow_method(Multiaxial_stress_cycle_2D(cycl_matrix, z2_stress), Cof_m);
                        }
                        break;


                    case "full_cycle":
                        {
                            Sigma_equiv_z1 = Full_cycle_method(Multiaxial_stress_cycle_2D(cycl_matrix, z1_stress), Cof_m);
                            Sigma_equiv_z0 = Full_cycle_method(Multiaxial_stress_cycle_2D(cycl_matrix, z0_stress), Cof_m);
                            Sigma_equiv_z2 = Full_cycle_method(Multiaxial_stress_cycle_2D(cycl_matrix, z2_stress), Cof_m);
                        }
                        break;

                    default: MessageBox.Show("Select Cycle Counting Method"); return;
                }

                outtext = Element_id + Environment.NewLine + Exponent_String_Format(Sigma_equiv_z1, Sigma_equiv_z0, Sigma_equiv_z2,
                                                                 Max_Valu(Sigma_equiv_z1, Sigma_equiv_z0, Sigma_equiv_z2));


                equivalent_stress_file.WriteLine(outtext);


                if (Console_Show_Or_No)
                    Console.WriteLine(outtext);


                Console.Title = "Completed Percent " + (100 * i / Element_Count) + "%";
               
            }

            equivalent_stress_file.Close();
            read.Close();
            File.Delete(Temporary_File_Name);
            Console.Title = "Completed Percent " + 100 + "%";
            Console.WriteLine("\nEnd Solving.\nElapsed Time " + (DateTime.Now - time));
        }
        public void Engine_for_2D_critical_plane()
        {

            if (Delta < 0.001)
            {
                MessageBox.Show("Minimum Angular Iteration Must Be Greater or Equal 0.001 Degree !", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                Delta *= Math.PI / 180;
            }

            #region Reading Cyclograms File !!!
            Console.WriteLine("Reading Cyclograms File !!!");

            int Cycle_Line_count = 0;
            int Cycle_Column_count = 0;

            double[,] cycl_matrix = returne_cyclograms(ref Cycle_Line_count, ref Cycle_Column_count);

            if (cycl_matrix == null)
            {
                MessageBox.Show("Cyclograms File Is Incorrect..." + new string(' ', 50), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Console.WriteLine("Cyclograms File Read Successfully !!!");
            #endregion

            #region Reading Patran Report File !!!
            Console.WriteLine("Reading Patran Report File !!!");

            int Load_Case_Count = 0;
            int Element_Count = 0;
            int tox_byate_lengt = 0;


            if (!Create_Temporary_Result_File(ref Load_Case_Count, ref Element_Count, ref tox_byate_lengt, 4))
            {
                MessageBox.Show("Report File is incorrect..." + new string(' ', 50), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            Console.WriteLine("Patran Report File Read Successfully !!!");
            #endregion

            #region IF Cycle_Column_count == Load_Case_Count
            if (Cycle_Column_count != Load_Case_Count)
            {
                MessageBox.Show("The Number of Load Cases is Not Equal to The Number of Cyclograms !", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            #endregion

            #region Element_Property_Check
            Console.WriteLine("Property check started !!!");

            BinaryReader element_id_from_report = new BinaryReader(File.OpenRead(Temporary_File_Name));
            List<int> error_list = new List<int>();

            int id;
            for (int i = 0; i < 32 * Element_Count; i += 32)
            {

                element_id_from_report.BaseStream.Position = i;
                id = (int)element_id_from_report.ReadInt64();

                if (!elm_prop.ContainsKey(id))
                    error_list.Add(id);
            }
            element_id_from_report.Close();


            if (error_list.Count != 0)
            {
                string path = Temporary_File_Name.Substring(0, Temporary_File_Name.IndexOf(".txt")) + "_Error_List.txt";
                StreamWriter Error_List_Stream = new StreamWriter(path);

                foreach (int elmid in error_list)
                    Error_List_Stream.Write(elmid + " ");

                Error_List_Stream.Close();

                MessageBox.Show("Elements in Error_List.txt" + "Has No Properties", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Console.WriteLine("Property check ended !!!");
            #endregion


            MessageBox.Show(string.Format("The Number Of Point In Ciclograms = {0}\nThe Number Of Load Cases = {1}\nThe Number Of Elements = {2}", Cycle_Line_count, Load_Case_Count, Element_Count));


            BinaryReader read = new BinaryReader(File.Open(Temporary_File_Name, FileMode.Open));

            StreamWriter equivalent_stress_file = new StreamWriter(Result_File_Name);

            Create_templit_2D_critical(Templet_File_Name);

            equivalent_stress_file.WriteLine("Fatigue Stress:" + Environment.NewLine + "12" + Environment.NewLine +
                                                  "SUBTITLE 1" + Environment.NewLine + "SUBTITLE 2");

            Console.WriteLine("Start Solving....");

            string outtext = "";

            var time = DateTime.Now;


            int Element_id = 0;
            double temp1 = 0.0;
            double temp2 = 0.0;
            double[,] z0_stress = new double[Load_Case_Count, 3];
            double[,] z1_stress = new double[Load_Case_Count, 3];
            double[,] z2_stress = new double[Load_Case_Count, 3];

            double[] stres_z0_alfa_plane;
            double[] stres_z1_alfa_plane;
            double[] stres_z2_alfa_plane;

            double[] stres_z0_plane_cycle;
            double[] stres_z1_plane_cycle;
            double[] stres_z2_plane_cycle;

            double Sigma_equiv_z0, Sigma_equiv_z1, Sigma_equiv_z2;


            for (int i = 0; i < Element_Count; i++)
            {
                read.BaseStream.Position = i * 32;

                Element_id = (int)read.ReadInt64();

                Cof_m = elm_prop[Element_id][0];
                Cof_k = elm_prop[Element_id][1];

                for (int j = 0; j < Load_Case_Count; j++)
                {
                    for (int k = 1; k < 4; k++) //0-n elementi hamarn a, 0-X,1-Y,2-XY
                    {
                        read.BaseStream.Position = (2 * j * Element_Count + i) * 32 + k * 8; // (+8*k) araji long@ic heto ekox@
                        temp1 = read.ReadDouble();

                        read.BaseStream.Position = (2 * j * Element_Count + i + Element_Count) * 32 + k * 8;
                        temp2 = read.ReadDouble();

                        z0_stress[j, k - 1] = (temp1 + temp2) / 2;

                        if (k == 1 || k == 2)
                        {
                            z1_stress[j, k - 1] = z0_stress[j, k - 1] + Cof_k * (temp1 - temp2) / 2;
                            z2_stress[j, k - 1] = z0_stress[j, k - 1] + Cof_k * (temp2 - temp1) / 2;
                        }
                        else // es el qez zgvig@ axper jan
                        {
                            z1_stress[j, k - 1] = z0_stress[j, k - 1] + (temp1 - temp2) / 2;
                            z2_stress[j, k - 1] = z0_stress[j, k - 1] + (temp2 - temp1) / 2;
                        }
                    }
                }


                Sigma_equiv_z0 = 0;
                Sigma_equiv_z1 = 0;
                Sigma_equiv_z2 = 0;

                double[] max_eqv = new double[3];      //syun1 = z1, z0, z2; syun2 = z1_alfa, z0_alf, z2_alfa
                double[] max_eqv_alfa = new double[3]; //syun1 = z1, z0, z2; syun2 = z1_alfa, z0_alf, z2_alfa


                for (double alfa = 0; alfa <= Math.PI; alfa += Delta)
                {
                    stres_z0_alfa_plane = Stress_in_alfa_plane_2D(z0_stress, alfa);
                    stres_z1_alfa_plane = Stress_in_alfa_plane_2D(z1_stress, alfa);
                    stres_z2_alfa_plane = Stress_in_alfa_plane_2D(z2_stress, alfa);

                    stres_z0_plane_cycle = Oneaxial_stress_cycle(cycl_matrix, stres_z0_alfa_plane);
                    stres_z1_plane_cycle = Oneaxial_stress_cycle(cycl_matrix, stres_z1_alfa_plane);
                    stres_z2_plane_cycle = Oneaxial_stress_cycle(cycl_matrix, stres_z2_alfa_plane);

                    Sigma_equiv_z0 = 0;
                    Sigma_equiv_z1 = 0;
                    Sigma_equiv_z2 = 0;

                    switch (Counting_method)
                    {
                        case "rain_flow":
                            {
                                Sigma_equiv_z1 = Rain_flow_method(stres_z1_plane_cycle, Cof_m);
                                Sigma_equiv_z0 = Rain_flow_method(stres_z0_plane_cycle, Cof_m);
                                Sigma_equiv_z2 = Rain_flow_method(stres_z2_plane_cycle, Cof_m);
                            }
                            break;
                        case "full_cycle":
                            {
                                Sigma_equiv_z1 = Rain_flow_method(stres_z1_plane_cycle, Cof_m);
                                Sigma_equiv_z0 = Rain_flow_method(stres_z0_plane_cycle, Cof_m);
                                Sigma_equiv_z2 = Rain_flow_method(stres_z2_plane_cycle, Cof_m);
                            }
                            break;
                        default: MessageBox.Show("Select Cycle Counting Method"); return;
                    }


                    if (Sigma_equiv_z1 > max_eqv[0])
                    {
                        max_eqv[0] = Sigma_equiv_z1;
                        max_eqv_alfa[0] = alfa;
                    }
                    if (Sigma_equiv_z0 > max_eqv[1])
                    {
                        max_eqv[1] = Sigma_equiv_z0;
                        max_eqv_alfa[1] = alfa;
                    }
                    if (Sigma_equiv_z2 > max_eqv[2])
                    {
                        max_eqv[2] = Sigma_equiv_z2;
                        max_eqv_alfa[2] = alfa;
                    }

                    Array.Clear(stres_z0_plane_cycle, 0, stres_z0_plane_cycle.Length);
                    Array.Clear(stres_z1_plane_cycle, 0, stres_z1_plane_cycle.Length);
                    Array.Clear(stres_z2_plane_cycle, 0, stres_z2_plane_cycle.Length);
                }

                double z1x = max_eqv[0] * Math.Cos(max_eqv_alfa[0]);
                double z1y = max_eqv[0] * Math.Sin(max_eqv_alfa[0]);

                double z0x = max_eqv[1] * Math.Cos(max_eqv_alfa[1]);
                double z0y = max_eqv[1] * Math.Sin(max_eqv_alfa[1]);

                double z2x = max_eqv[2] * Math.Cos(max_eqv_alfa[2]);
                double z2y = max_eqv[2] * Math.Sin(max_eqv_alfa[2]);


                double Max_Of_Layers = max_eqv.Max();
                double Max_Of_Layers_Alfa = max_eqv_alfa[Array.IndexOf(max_eqv, Max_Of_Layers)];

                double zmax_x = Max_Of_Layers * Math.Cos(Max_Of_Layers_Alfa);
                double zmax_y = Max_Of_Layers * Math.Sin(Max_Of_Layers_Alfa);


                outtext = Element_id + Environment.NewLine + Exponent_String_Format(z1x, z1y, 0.0, z0x, z0y, 0.0, z2x, z2y, 0.0, zmax_x, zmax_y, 0.0);

                equivalent_stress_file.WriteLine(outtext);


                if (Console_Show_Or_No)
                    Console.WriteLine(outtext);

                Console.Title = "Completed Percent " + (100 * i / Element_Count) + "%";

                //if (i == 1000)
                //{
                //    Console.WriteLine("\nEnd Solving.\nElapsed Time " + (DateTime.Now - time));
                //    Console.ReadKey();
                //}
            }


            equivalent_stress_file.Close();
            read.Close();
            File.Delete(Temporary_File_Name);
            Console.Title = "Completed Percent " + 100 + "%";
            Console.WriteLine("\nEnd Solving.\nElapsed Time " + (DateTime.Now - time));
        }

        
        static double[] Stress_in_alfa_plane_2D(double[,] b, double alfa)
        {
            int b_rows = b.GetLength(0);
            double[] stress_in_alfa_plane = new double[b_rows];

            for (int i = 0; i < b_rows; i++)
            {
                stress_in_alfa_plane[i] = Sigma_alfa_2D(b[i, 0], b[i, 1], b[i, 2], alfa);
            }
            return stress_in_alfa_plane;
        }
        static double[] Oneaxial_stress_cycle(double[,] a, double[] b)
        {
            double[] principal_stresses = new double[a.GetLength(0)];

            int a_rows = a.GetLength(0);
            int a_cols = a.GetLength(1);

            //for (int i = 0; i < a_rows; i++)
            //{
            //    for (int j = 0; j < a_cols; j++)
            //    {
            //        principal_stresses[i] += a[i, j] * b[j];
            //    }
            //}

            Parallel.For(0, a_rows, i =>
            {
                for (int j = 0; j < a_cols; j++)
                {
                    principal_stresses[i] += a[i, j] * b[j];
                }
            });

            return principal_stresses;
        }
        static double[] Multiaxial_stress_cycle_2D(double[,] a, double[,] b)
        {
            double[] stresses_cycle = new double[a.GetLength(0)];

            int a_rows = a.GetLength(0);
            int a_cols = a.GetLength(1);
            int b_cols = b.GetLength(1);

            //for (int i = 0; i < a_rows; i++)
            Parallel.For(0, a_rows, i =>
            {
                double[] stress_component = new double[b_cols];

                for (int j = 0; j < b_cols; j++)
                {
                    for (int k = 0; k < a_cols; k++)
                    {
                        stress_component[j] += a[i, k] * b[k, j];
                    }
                }

                switch (Multiaxial_stress)
                {
                    case "2D_1": stresses_cycle[i] = sign_equivalent_von_mises_stress_2D(stress_component[0], stress_component[1], stress_component[2]); break;
                    case "2D_2": stresses_cycle[i] = sign_maximum_shear_stress_2D(stress_component[0], stress_component[1], stress_component[2]); break;
                    case "2D_3": stresses_cycle[i] = max_principal(stress_component[0], stress_component[1], stress_component[2]); break;
                    case "2D_4": stresses_cycle[i] = equivalent_von_mises_stress(stress_component[0], stress_component[1], stress_component[2]); break;
                    default: MessageBox.Show("Select one of 2D methods"); break;
                }
                Array.Clear(stress_component, 0, stress_component.Length);
            });
            return stresses_cycle;
        }


        static double Rain_flow_method(double[] x, double kof_m)
        {

            double Sigma_equiv = 0;
            int k0 = x.Length;

            if (k0 < 3)
                return Sigma_equiv;

            int k, m, n, Nk, i1_cur;
            double x1_cur, x2_cur;
            bool k_log = false;

            Nk = k0 + 2;
            Array.Resize(ref x, Nk);
            for (int i = Nk - 2; i > 0; i--)
            {
                x[i] = x[i - 1];
            }
            x[0] = 0;

            double[] xx = new double[Nk];
            double[,] x_cyc = new double[Nk, 2];

            if (x[1] - x[k0] != 0)
            {
                x[k0 + 1] = x[1];
                k0++;
            }


            k = 0;
            for (int i = 1; i <= k0; i++)
            {
                k++;
                if (x[k + 1] != x[k])
                {
                    if (x[k + 1] > x[k])
                        k_log = true;
                    break;
                }
            }

            if (k == k0)
            {
                return Sigma_equiv;
            }
            else
            {
                for (int i = k; i <= k0 - 1; i++)
                {
                    xx[i - (k - 1)] = x[i];
                }

                for (int i = 1; i <= k - 1; i++)
                {
                    xx[i + (k0 - k)] = x[i];
                }
            }
            xx[k0] = xx[1];

            m = 1;
            x1_cur = xx[1];
            x[m] = x1_cur;

            for (int i = 2; i <= k0; i++)
            {
                x2_cur = xx[i];
                if (k_log)
                {
                    if (x2_cur >= x1_cur)
                    {
                        x1_cur = x2_cur;
                    }
                    else
                    {
                        m++;
                        x[m] = x1_cur;
                        k_log = !k_log;
                        x1_cur = x2_cur;
                    }
                }
                else if (x2_cur <= x1_cur)
                {
                    x1_cur = x2_cur;
                }
                else
                {
                    m++;
                    x[m] = x1_cur;

                    k_log = !k_log;
                    x1_cur = x2_cur;
                }
            }

            if ((x[2] - x[1]) * (x[1] - x[m]) > 0)
            {
                for (int i = 2; i <= m; i++)
                {
                    x[i - 1] = x[i];
                }
                m--;
            }

            i1_cur = 1;
            x1_cur = x[i1_cur];


            for (int i = 1; i <= m; i++)
            {
                if (x[i] > x1_cur)
                {
                    i1_cur = i;
                    x1_cur = x[i];
                }
            }


            for (int i = i1_cur; i <= m; i++)
            {
                xx[i - i1_cur + 1] = x[i];
            }
            for (int i = 1; i <= i1_cur - 1; i++)
            {
                xx[i + m - i1_cur + 1] = x[i];
            }
            xx[m + 1] = xx[1];




            int m_cur = 0;
            //---------------------------------------------------------------------------------
            k = 0;
            n = 0;

        stp1:
            n++;
            m_cur = m_cur + 1;
            x[n] = xx[m_cur];

        stp2:
            if (n < 3) { goto stp1; }

            x2_cur = Math.Abs(x[n] - x[n - 1]);
            x1_cur = Math.Abs(x[n - 1] - x[n - 2]);

            //stp3:
            if (x2_cur < x1_cur) { goto stp1; }

            //stp4: 
            k++;
            x_cyc[k, 0] = x[n - 2];
            x_cyc[k, 1] = x[n - 1];

            n -= 2;
            x[n] = x[n + 2];

            if (n == 1) { xx[m + 1] = x[1]; }

            if (m_cur == m + 1 && n == 1) { goto stp5; }

            goto stp2;

        stp5:

            //-------------------------------------------------------------------------------------

            double xm;
            for (int i = 1; i <= k; i++)
            {
                if (x_cyc[i, 1] > x_cyc[i, 0])
                {
                    xm = x_cyc[i, 0];
                    x_cyc[i, 0] = x_cyc[i, 1];
                    x_cyc[i, 1] = xm;
                }
            }


            double Sigma_equiv_i = 0;
            for (int i = 1; i <= k; i++)
            {
                switch (Stress_equation)
                {
                    case "cai": Sigma_equiv_i = Cai_Equation(x_cyc[i, 1], x_cyc[i, 0]); break;
                    case "walker": Sigma_equiv_i = Walker_Equation(x_cyc[i, 1], x_cyc[i, 0], Coef_a_walker, Coef_gama_walker); break;
                    case "goodman": Sigma_equiv_i = Goodman_Equation(x_cyc[i, 1], x_cyc[i, 0], Gudman_ult_stress); break;
                    case "soderberg": Sigma_equiv_i = Soderberg_Equation(x_cyc[i, 1], x_cyc[i, 0]); break;
                    case "morro": Sigma_equiv_i = Morro_Equation(x_cyc[i, 1], x_cyc[i, 0]); break;
                    case "gerber": Sigma_equiv_i = Gerber_Equation(x_cyc[i, 1], x_cyc[i, 0]); break;
                    case "asme": Sigma_equiv_i = ASME_Elliptic_Equation(x_cyc[i, 1], x_cyc[i, 0]); break;
                    case "swt": Sigma_equiv_i = Smith_Watson_Topper_Equation(x_cyc[i, 1], x_cyc[i, 0]); break;
                    case "stulen": Sigma_equiv_i = Stulen_Equation(x_cyc[i, 1], x_cyc[i, 0]); break;
                    case "topper": Sigma_equiv_i = Topper_Sandor_Equation(x_cyc[i, 1], x_cyc[i, 0]); break;
                    default: MessageBox.Show("Stress Equation not selected"); return 0;
                }

                Sigma_equiv += Math.Pow(Sigma_equiv_i, kof_m);
            }

            Sigma_equiv = Math.Pow(Sigma_equiv, 1 / kof_m);

            return Sigma_equiv;
        }
        static double Full_cycle_method(double[] x, double kof_m)
        {

            double Sigma_equiv = 0;
            int k0 = x.Length;

            if (k0 < 3)
                return Sigma_equiv;

            int k, m, Nk, i1_cur, i2_cur;
            double x1_cur, x2_cur, a_cur;
            bool k_log = false;

            Nk = k0 + 2;
            Array.Resize(ref x, Nk);
            for (int i = Nk - 2; i > 0; i--)
            {
                x[i] = x[i - 1];
            }
            x[0] = 0;

            double[] xx = new double[Nk];
            double[,] x_cyc = new double[Nk, 2];

            if (x[1] - x[k0] != 0)
            {
                x[k0 + 1] = x[1];
                k0++;
            }


            k = 0;
            for (int i = 1; i <= k0; i++)
            {
                k++;
                if (x[k + 1] != x[k])
                {
                    if (x[k + 1] > x[k])
                        k_log = true;
                    break;
                }
            }

            if (k == k0)
            {
                return Sigma_equiv;
            }
            else
            {
                for (int i = k; i <= k0 - 1; i++)
                {
                    xx[i - (k - 1)] = x[i];
                }

                for (int i = 1; i <= k - 1; i++)
                {
                    xx[i + (k0 - k)] = x[i];
                }
            }
            xx[k0] = xx[1];

            m = 1;
            x1_cur = xx[1];
            x[m] = x1_cur;

            for (int i = 2; i <= k0; i++)
            {
                x2_cur = xx[i];
                if (k_log)
                {
                    if (x2_cur >= x1_cur)
                    {
                        x1_cur = x2_cur;
                    }
                    else
                    {
                        m++;
                        x[m] = x1_cur;

                        k_log = !k_log;
                        x1_cur = x2_cur;
                    }
                }
                else if (x2_cur <= x1_cur)
                {
                    x1_cur = x2_cur;
                }
                else
                {
                    m++;
                    x[m] = x1_cur;

                    k_log = !k_log;
                    x1_cur = x2_cur;
                }
            }

            if ((x[2] - x[1]) * (x[1] - x[m]) > 0)
            {
                for (int i = 2; i <= m; i++)
                {
                    x[i - 1] = x[i];
                }
                m--;
            }

            i1_cur = 1;
            x1_cur = x[i1_cur];


            for (int i = 1; i <= m; i++)
            {
                if (x[i] > x1_cur)
                {
                    i1_cur = i;
                    x1_cur = x[i];
                }
            }


            for (int i = i1_cur; i <= m; i++)
            {
                xx[i - i1_cur + 1] = x[i];
            }
            for (int i = 1; i <= i1_cur - 1; i++)
            {
                xx[i + m - i1_cur + 1] = x[i];
            }
            xx[m + 1] = xx[1];



            int m_cur = 0;

            /*Rem full cycle method*/
            //---------------------------------------------------------------------------------  

            k = 0;
            m_cur = m;
            do
            {
                i1_cur = 1;
                i2_cur = i1_cur + 1;
                a_cur = Math.Abs(xx[i2_cur] - xx[i1_cur]);
                for (int i = 1; i <= m_cur; i++)
                {

                    if (Math.Abs(xx[i + 1] - xx[i]) < a_cur)
                    {
                        i1_cur = i;
                        i2_cur = i + 1;
                        a_cur = Math.Abs(xx[i + 1] - xx[i]);
                    }
                }
                k = k + 1;
                x_cyc[k, 0] = xx[i1_cur];
                x_cyc[k, 1] = xx[i2_cur];

                for (int i = i1_cur; i <= (m_cur - 1); i++)
                    xx[i] = xx[i + 2];

                if (i1_cur == 1)
                    xx[m_cur - 1] = xx[3];

                if (i2_cur == m_cur)
                    xx[1] = xx[m_cur + 1];

                m_cur -= 2;
            }
            while (m_cur > 2);

            k = k + 1;
            x_cyc[k, 0] = xx[1];
            x_cyc[k, 1] = xx[2];

            //---------------------------------------------------------------------------------------
            double xm;
            for (int i = 1; i <= k; i++)
            {
                if (x_cyc[i, 1] > x_cyc[i, 0])
                {
                    xm = x_cyc[i, 0];
                    x_cyc[i, 0] = x_cyc[i, 1];
                    x_cyc[i, 1] = xm;
                }
            }


            double Sigma_equiv_i = 0;
            for (int i = 1; i <= k; i++)
            {
                switch (Stress_equation)
                {
                    case "cai": Sigma_equiv_i = Cai_Equation(x_cyc[i, 1], x_cyc[i, 0]); break;
                    case "walker": Sigma_equiv_i = Walker_Equation(x_cyc[i, 1], x_cyc[i, 0], Coef_a_walker, Coef_gama_walker); break;
                    case "goodman": Sigma_equiv_i = Goodman_Equation(x_cyc[i, 1], x_cyc[i, 0], Gudman_ult_stress); break;
                    case "soderberg": Sigma_equiv_i = Soderberg_Equation(x_cyc[i, 1], x_cyc[i, 0]); break;
                    case "morro": Sigma_equiv_i = Morro_Equation(x_cyc[i, 1], x_cyc[i, 0]); break;
                    case "gerber": Sigma_equiv_i = Gerber_Equation(x_cyc[i, 1], x_cyc[i, 0]); break;
                    case "asme": Sigma_equiv_i = ASME_Elliptic_Equation(x_cyc[i, 1], x_cyc[i, 0]); break;
                    case "swt": Sigma_equiv_i = Smith_Watson_Topper_Equation(x_cyc[i, 1], x_cyc[i, 0]); break;
                    case "stulen": Sigma_equiv_i = Stulen_Equation(x_cyc[i, 1], x_cyc[i, 0]); break;
                    case "topper": Sigma_equiv_i = Topper_Sandor_Equation(x_cyc[i, 1], x_cyc[i, 0]); break;
                    default: MessageBox.Show("Stress Equation not selected"); return 0;
                }
                Sigma_equiv += Math.Pow(Sigma_equiv_i, kof_m);
            }

            Sigma_equiv = Math.Pow(Sigma_equiv, 1 / kof_m);

            return Sigma_equiv;
        }



        static double Cai_Equation(double min, double max)
        {
            double Sig_a = (max - min) / 2;
            double Sig_m = (max + min) / 2;

            if (Sig_m >= 0)
            {
                return Math.Sqrt(2 * max * Sig_a);
            }
            else if (max > 0)
            {
                return (Math.Sqrt(2) * (Sig_a + 0.2 * Sig_m));
            }
            else
            {
                return 0;
            }
        }
        static double Walker_Equation(double min, double max, double A, double gama)
        {
            double Sig_a = (max - min) / 2;
            double Sig_m = (max + min) / 2;

            if (max <= 0)
                return 0;
            else
                return A * Math.Pow(max, (1 - gama)) * Math.Pow(Sig_a, gama);
        }
        static double Goodman_Equation(double min, double max, double ult_stress)
        {
            double Sig_a = (max - min) / 2;
            double Sig_m = (max + min) / 2;

            if (Sig_m <= -(ult_stress))
            {
                return 0;
            }
            else if (Sig_m > -(ult_stress) && Sig_m <= 0)
            {
                return Sig_a;
            }
            else if (Sig_m > 0 && Sig_m < ult_stress)
            {
                return Sig_a / (1 - Sig_m / ult_stress);
            }
            else
            {
                return -1;
            }
        }
        static double Soderberg_Equation(double min, double max)
        {
            return 0;
        }
        static double Morro_Equation(double min, double max)
        {
            return 0;
        }
        static double Gerber_Equation(double min, double max)
        {
            return 0;
        }
        static double ASME_Elliptic_Equation(double min, double max)
        {
            return 0;
        }
        static double Smith_Watson_Topper_Equation(double min, double max)
        {
            return 0;
        }
        static double Stulen_Equation(double min, double max)
        {
            return 0;
        }
        static double Topper_Sandor_Equation(double min, double max)
        {
            return 0;
        }


        static double Sigma_alfa_2D(double x, double y, double xy, double alfa)
        {
            return 0.5 * (x + y) + 0.5 * (x - y) * Math.Cos(2.0 * alfa) + xy * Math.Sin(2.0 * alfa);
        }
        static double Max_Valu(params double[] a)
        {
            return a.Max();
        }
        static string Exponent_String_Format(params double[] a)
        {
            StringBuilder return_str = new StringBuilder();
            for (int i = 0; i < a.Length; i++)
            {
                if (i % 6 == 0 && i != 0)
                    return_str.AppendLine();

                if (a[i] < 0)
                    return_str.AppendFormat("{0:.0000000E+00}", a[i]);
                else
                    return_str.AppendFormat("{0: .0000000E+00}", a[i]);

            }
            return return_str.ToString();
        }


        static double[,] returne_cyclograms(ref int Cycle_Line_count, ref int Cycle_Column_count)
        {

            string[] Line_Cicl = File.ReadAllLines(Cyclogramm_File_Name); // stringvi masiv en haytararum eu fayli sax toger@ darcnum arandzin andamner                      

            Cycle_Line_count = Line_Cicl.Length;
            Cycle_Column_count = Line_Cicl[0].Split(symbols, StringSplitOptions.RemoveEmptyEntries).Length;
            double[,] cycl_matrix = new double[Cycle_Line_count, Cycle_Column_count];  // erkchap flot masiv , toxer@ ciklagrammai keteri tivn a, syuneri tiv@ sluchaneri tivn a


            for (int i = 0; i < Cycle_Line_count; i++)
            {
                string[] tox = Line_Cicl[i].Split(symbols, StringSplitOptions.RemoveEmptyEntries);
                int Cycle_Column_count_temp = tox.Length;

                if (Cycle_Column_count != Cycle_Column_count_temp)
                    return null;

                for (int j = 0; j < Cycle_Column_count; j++)
                {
                    if (!double.TryParse(tox[j], NumberStyles.Any, CultureInfo.InvariantCulture, out cycl_matrix[i, j]))
                        return null;
                }
            }
            return cycl_matrix;
        }

        static bool Create_Temporary_Result_File(ref int Load_Case_Count, ref int Element_Count, ref int tox_byate_lengt, int count)
        {

            string Report_line = "";
            double[] ecomponents = new double[count - 1];
            long elm_id = 0;


            StreamReader Report_R = File.OpenText(Report_File_Name);
            BinaryWriter Report_W = new BinaryWriter(File.Create(Temporary_File_Name));

            while ((Report_line = Report_R.ReadLine()) != null)
            {
                if (If_numeric_string(Report_line, count, ref elm_id, ref ecomponents))
                {
                    tox_byate_lengt = Report_line.Length;
                    break;
                }
            }
            Report_R.Close();

            Report_R = File.OpenText(Report_File_Name);


            while ((Report_line = Report_R.ReadLine()) != null)
            {

                if (If_numeric_string(Report_line, count, ref elm_id, ref ecomponents))
                {
                    if (Report_line.Length != tox_byate_lengt)
                    {
                        Report_R.Close(); Report_W.Close();
                        return false;
                    }

                    Report_W.Write(elm_id);

                    for (int i = 0; i < count - 1; i++)
                        Report_W.Write(ecomponents[i]);

                    Element_Count++;
                }

                if (Report_line.Contains("Load Case:"))
                    Load_Case_Count++;

            }

            if (Load_Case_Count == 0 || Element_Count % Load_Case_Count != 0)
            {
                Report_R.Close(); Report_W.Close();
                return false;
            }

            Element_Count = Element_Count / Load_Case_Count;

            if (count == 4)
            {
                if (Load_Case_Count % 2 != 0)
                {
                    Report_R.Close(); Report_W.Close();
                    return false;
                }
                else
                {
                    Load_Case_Count /= 2;
                }
            }

            Report_R.Close();
            Report_W.Close();
            return true;
        }

        static bool If_numeric_string(string a, int colum_count, ref long elm_id, ref double[] componens)
        {
            string[] mas = a.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

            if (mas.Length < colum_count)
                return false;

            if (!long.TryParse(mas[0], out elm_id))
                return false;


            for (int i = 1; i < colum_count; i++)
            {
                if (!double.TryParse(mas[i], NumberStyles.Any, CultureInfo.InvariantCulture, out componens[i - 1]))
                    return false;
            }
            return true;
        }


        static void Create_templit_1D(string Templet_File_Name)
        {
            StreamWriter file = new StreamWriter(Templet_File_Name);

            file.Write("/* edvin_hakobyan.res_tmpl */" + Environment.NewLine +

                       "KEYLOC = 0" + Environment.NewLine +
                       "" + Environment.NewLine +
                       "TYPE = scalar" + Environment.NewLine +
                       "COLUMN = 1" + Environment.NewLine +
                       "PRI = Equivalent Stress" + Environment.NewLine +
                       "SEC = Centroid" + Environment.NewLine);

            file.Close();
        }
        static void Create_templit_2D(string Templet_File_Name)
        {
            StreamWriter file = new StreamWriter(Templet_File_Name);

            file.Write("/* edvin_hakobyan.res_tmpl */" + Environment.NewLine +

                       "KEYLOC = 0" + Environment.NewLine +
                       "" + Environment.NewLine +
                       "TYPE = scalar" + Environment.NewLine +
                       "COLUMN = 1" + Environment.NewLine +
                       "PRI = Equivalent Stress" + Environment.NewLine +
                       "SEC = Layer_Z1" + Environment.NewLine +
                       "" + Environment.NewLine +
                       "TYPE = scalar" + Environment.NewLine +
                       "COLUMN = 2" + Environment.NewLine +
                       "PRI = Equivalent Stress" + Environment.NewLine +
                       "SEC = Layer_N" + Environment.NewLine +
                       "" + Environment.NewLine +
                       "TYPE = scalar" + Environment.NewLine +
                       "COLUMN = 3" + Environment.NewLine +
                       "PRI = Equivalent Stress" + Environment.NewLine +
                       "SEC = Layer_Z2" + Environment.NewLine +
                       "" + Environment.NewLine +
                       "TYPE = scalar" + Environment.NewLine +
                       "COLUMN = 4" + Environment.NewLine +
                       "PRI = Equivalent Stress" + Environment.NewLine +
                       "SEC = Max_Layers" + Environment.NewLine);

            file.Close();
        }
        static void Create_templit_2D_critical(string Templet_File_Name)
        {

            StreamWriter file = new StreamWriter(Templet_File_Name);

            file.Write("/* edvin_hakobyan.res_tmpl */" + Environment.NewLine +
                       "KEYLOC = 0" + Environment.NewLine +
                       "" + Environment.NewLine +

                       "TYPE = vector" + Environment.NewLine +
                       "COLUMN = 1,2,3" + Environment.NewLine +
                       "PRI = Equivalent Stress" + Environment.NewLine +
                       "SEC = Layer-Z1" + Environment.NewLine +
                       "CTYPE = ELEM" + Environment.NewLine +
                       "" + Environment.NewLine +

                       "TYPE = vector" + Environment.NewLine +
                       "COLUMN = 4,5,6" + Environment.NewLine +
                       "PRI = Equivalent Stress" + Environment.NewLine +
                       "SEC = Layer-Netral" + Environment.NewLine +
                       "CTYPE = ELEM" + Environment.NewLine +
                       "" + Environment.NewLine +

                       "TYPE = vector" + Environment.NewLine +
                       "COLUMN = 7,8,9" + Environment.NewLine +
                       "PRI = Equivalent Stress" + Environment.NewLine +
                       "SEC = Layer-Z2" + Environment.NewLine +
                       "CTYPE = ELEM" + Environment.NewLine +
                       "" + Environment.NewLine +

                       "TYPE = vector" + Environment.NewLine +
                       "COLUMN = 10,11,12" + Environment.NewLine +
                       "PRI = Equivalent Stress" + Environment.NewLine +
                       "SEC = Layers-max" + Environment.NewLine +
                       "CTYPE = ELEM" + Environment.NewLine +
                       "" + Environment.NewLine);
            file.Close();


            //TYPE = TENSOR
            //COLUMN = 25, 26, 27, 28, 29, 30
            //PRI = Stress
            //SEC = Components
            //CTYPE = ELEM
            //TYPE = END

        }


        //for 2D
        static double sign_equivalent_von_mises_stress_2D(double x, double y, double xy)
        {
            double Max = max_principal(x, y, xy);
            double Min = min_principal(x, y, xy);

            if (Math.Abs(Max) > Math.Abs(Min))
                return Math.Sign(Max) * Math.Sqrt(2 * (Max * Max + Min * Min - Max * Min));
            else
                return Math.Sign(Min) * Math.Sqrt(2 * (Max * Max + Min * Min - Max * Min));
        }
        static double sign_maximum_shear_stress_2D(double x, double y, double xy)
        {
            double Max = max_principal(x, y, xy);
            double Min = min_principal(x, y, xy);

            if (Math.Abs(Max) > Math.Abs(Min))
                return Math.Sign(Max) * (Max - Min) / 2.0;
            else
                return Math.Sign(Min) * (Max - Min) / 2.0;
        }
        static double max_principal(double x, double y, double xy)
        {
            double sum = (x + y) / 2;
            double sub = (x - y) / 2;
            return sum + Math.Pow(((sub * sub) + xy * xy), 0.5);
        }
        static double min_principal(double x, double y, double xy)
        {
            double sum = (x + y) / 2;
            double sub = (x - y) / 2;
            return sum - Math.Pow(((sub * sub) + xy * xy), 0.5);
        }
        static double equivalent_von_mises_stress(double x, double y, double xy)
        {
            return Math.Sqrt(x * x + y * y + 3 * xy * xy - x * y);
        }


    }
}
