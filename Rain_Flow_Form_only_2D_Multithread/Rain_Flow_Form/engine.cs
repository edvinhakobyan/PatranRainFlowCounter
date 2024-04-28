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
    public class Engine
    {
        static double Cof_k;
        static double Cof_m;
        static double Sig_02;
        static double Ktg;
        static double Gudman_ult_stress = 1; //+
        static char[] symbols = { ' ', '\t' };


        public static string Report_File_Name { get; set; }
        public static string Ciclogramm_File_Name { get; set; }
        public static string Result_File_Name { get; set; }
        public static string Templet_File_Name { get; set; }
        public static string Temporary_File_Name { get; set; }
        public static double Delta { get; set; }
        public static double Coef_a_walker { get; set; }
        public static double Coef_gama_walker { get; set; }
        public static string Multiaxial_stress { get; set; }
        public static string Stress_equation { get; set; }
        public static string Cycle_method { get; set; } = "rain_flow";
        public static bool Console_Show_Or_No { get; set; }
        public static Dictionary<int, double[]> Elm_prop { get; set; } = new Dictionary<int, double[]>();


        public void Engine_for_1D()
        {

            #region Reading Cyclograms File !!!
            Console.WriteLine("Reading Cyclograms File !!!");

            int Cycle_Line_count = 0;
            int Cycle_Column_count = 0;

            double[,] cycl_matrix = Read_cyclograms(ref Cycle_Line_count, ref Cycle_Column_count);

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

                if (!Elm_prop.ContainsKey(id))
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


            MessageBox.Show(string.Format("Number Of Point In Ciclograms = {0}\nNumber Of Load Cases = {1}\nNumber Of Elements = {2}", Cycle_Line_count, Load_Case_Count, Element_Count));

            BinaryReader read = new BinaryReader(File.Open(Temporary_File_Name, FileMode.Open));

            StreamWriter equivalent_stress_file = new StreamWriter(Result_File_Name);

            Create_templit_1D(Templet_File_Name);

            equivalent_stress_file.WriteLine("Fatigue Stress:" + Environment.NewLine + "1" + Environment.NewLine +
                                                  "SUBTITLE 1" + Environment.NewLine + "SUBTITLE 2");

            Console.WriteLine("Start Solving....");
            var time = DateTime.Now;
            double[] stress_x_direction = new double[Load_Case_Count];

            for (int i = 0; i < Element_Count; i++)
            {
                read.BaseStream.Position = i * 16;

                int Element_id = (int)read.ReadInt64();
                Cof_m = Elm_prop[Element_id][0];

                for (int j = 0; j < Load_Case_Count; j++)
                {
                    read.BaseStream.Position = (j * Element_Count + i) * 16 + 8; // (+8) araji long@ic heto ekox@
                    stress_x_direction[j] = read.ReadDouble();
                }


                double Sigma_equiv;
                switch (Cycle_method)
                {
                    case "rain_flow": Sigma_equiv = Rain_flow_method(Oneaxial_stress_cycle(cycl_matrix, stress_x_direction), Cof_m); break;
                    case "full_cycle": Sigma_equiv = Full_cycle_method(Oneaxial_stress_cycle(cycl_matrix, stress_x_direction), Cof_m); break;
                    default: MessageBox.Show("Select Cycle Counting Method"); return;
                }


                string outtext = Element_id + Environment.NewLine + Exponent_String_Format(Sigma_equiv);

                equivalent_stress_file.WriteLine(outtext);

                if (Console_Show_Or_No)
                    Console.WriteLine(outtext);

                Console.Title = "Completed " + (100 * i / Element_Count) + "%";

            }


            equivalent_stress_file.Close();
            read.Close();
            File.Delete(Temporary_File_Name);
            Console.Title = "Completed " + 100 + "%";
            Console.WriteLine("\nEnd Solving.\nElapsed Time " + (DateTime.Now - time));
        }
        public void Engine_for_2D()
        {

            #region Reading Cyclograms File !!!
            Console.WriteLine("Reading Cyclograms File !!!");

            int Cycle_Line_count = 0;
            int Cycle_Column_count = 0;

            double[,] cycl_matrix = Read_cyclograms(ref Cycle_Line_count, ref Cycle_Column_count);

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

                if (!Elm_prop.ContainsKey(id))
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

            MessageBox.Show(string.Format("Number Of Point In Ciclograms = {0}\nNumber Of Load Cases = {1}\nNumber Of Elements = {2}", Cycle_Line_count, Load_Case_Count, Element_Count));

            BinaryReader read = new BinaryReader(File.Open(Temporary_File_Name, FileMode.Open));

            StreamWriter equivalent_stress_file = new StreamWriter(Result_File_Name);


            Create_templit_2D(Templet_File_Name);

            equivalent_stress_file.WriteLine("Fatigue Stress:" + Environment.NewLine + "4" + Environment.NewLine +
                                                  "SUBTITLE 1" + Environment.NewLine + "SUBTITLE 2");
            Console.WriteLine("Start Solving....");
            var time = DateTime.Now;
            double[,] z0_stress = new double[Load_Case_Count, 3];
            double[,] z1_stress = new double[Load_Case_Count, 3];
            double[,] z2_stress = new double[Load_Case_Count, 3];

            double Sigma_equiv_z0, Sigma_equiv_z1, Sigma_equiv_z2;


            for (int i = 0; i < Element_Count; i++)
            {
                read.BaseStream.Position = i * 32;

                int Element_id = (int)read.ReadInt64();

                Cof_m = Elm_prop[Element_id][0];
                Cof_k = Elm_prop[Element_id][1];
                Sig_02 = Elm_prop[Element_id][2];
                Ktg = Elm_prop[Element_id][3];

                for (int j = 0; j < Load_Case_Count; j++)
                {
                    for (int k = 1; k < 4; k++) //0-n elementi hamarn a, 0-X,1-Y,2-XY
                    {
                        read.BaseStream.Position = (2 * j * Element_Count + i) * 32 + k * 8; // (+8*k) araji long@ic heto ekox@
                        double temp1 = read.ReadDouble();

                        read.BaseStream.Position = (2 * j * Element_Count + i + Element_Count) * 32 + k * 8;
                        double temp2 = read.ReadDouble();

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


                switch (Cycle_method)
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

                string outtext = Element_id + Environment.NewLine + Exponent_String_Format(Sigma_equiv_z1, Sigma_equiv_z0, Sigma_equiv_z2, Max_Valu(Sigma_equiv_z1, Sigma_equiv_z0, Sigma_equiv_z2));
                equivalent_stress_file.WriteLine(outtext);


                if (Console_Show_Or_No)
                    Console.WriteLine(outtext);


                Console.Title = "Completed " + (100 * i / Element_Count) + "%";

            }

            equivalent_stress_file.Close();
            read.Close();
            File.Delete(Temporary_File_Name);
            Console.Title = "Completed " + 100 + "%";
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

            double[,] cycl_matrix = Read_cyclograms(ref Cycle_Line_count, ref Cycle_Column_count);

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

                if (!Elm_prop.ContainsKey(id))
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

                Cof_m = Elm_prop[Element_id][0];
                Cof_k = Elm_prop[Element_id][1];
                Sig_02 = Elm_prop[Element_id][2];
                Ktg = Elm_prop[Element_id][3];

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

                    switch (Cycle_method)
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

                Console.Title = "Completed " + (100 * i / Element_Count) + "%";

                //if (i == 1000)
                //{
                //    Console.WriteLine("\nEnd Solving.\nElapsed Time " + (DateTime.Now - time));
                //    Console.ReadKey();
                //}
            }


            equivalent_stress_file.Close();
            read.Close();
            File.Delete(Temporary_File_Name);
            Console.Title = "Completed " + 100 + "%";
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
                    case "2D_1": stresses_cycle[i] = Sign_equivalent_von_mises_stress_2D(stress_component[0], stress_component[1], stress_component[2]); break;
                    case "2D_2": stresses_cycle[i] = Sign_maximum_shear_stress_2D(stress_component[0], stress_component[1], stress_component[2]); break;
                    case "2D_3": stresses_cycle[i] = Max_principal(stress_component[0], stress_component[1], stress_component[2]); break;
                    case "2D_4": stresses_cycle[i] = Equivalent_von_mises_stress(stress_component[0], stress_component[1], stress_component[2]); break;
                    default: MessageBox.Show("Select one of 2D methods"); break;
                }
            });
            return stresses_cycle;
        }


        static double Rain_flow_method(double[] x, double kof_m)
        {
            var result = CalculateRainFlow(x);

            var equiv = CalculateEquvalentStress(result, kof_m);

            return equiv;
        }
        static double Full_cycle_method(double[] x, double kof_m)
        {
            var result = CalculateFullCycle(x);

            var equiv = CalculateEquvalentStress(result, kof_m);

            return equiv;
        }
        static List<double[]> CalculateRainFlow(double[] rgn)
        {
            int k0 = rgn.Length;
            int k_tol = 8;

            // Initialize arrays
            var buff = new double[k0 + 1];
            var xbuff = new double[k0 + 1];
            var x_cyc = new List<double[]>();

            // Initialize variables
            double xa, xm, sig;
            int kMaxVal = 0, s = 0;
            bool k_log = false;

            // Fill xbuff array
            for (int i = 0; i < k0; i++)
            {
                xbuff[i] = Math.Round(rgn[i], k_tol);
            }
            xbuff[k0] = xbuff[0];

            // If less than 3 data points, return 0
            if (k0 < 3)
            {
                goto stp5;
            }

            double maxVal = xbuff[0];

            // Find maximum value and its index
            for (int i = 1; i <= k0; i++)
            {
                if (xbuff[k0 - i + 1] != xbuff[k0 - i])
                {
                    k_log = xbuff[k0 - i + 1] > xbuff[k0 - i];
                    s = 1;
                    break;
                }
            }

            // If all data points are same, return 0
            if (s == 0)
            {
                goto stp5;
            }

            // Process data points to find cycles
            int j = -1;
            for (int i = 0; i < k0; i++)
            {
                if (k_log && xbuff[i + 1] < xbuff[i])
                {
                    j++;
                    k_log = !k_log;
                    buff[j] = xbuff[i];

                    // Update maximum value and its index
                    if (buff[j] >= maxVal)
                    {
                        maxVal = buff[j];
                        kMaxVal = j;
                    }
                }
                else if (!k_log && xbuff[i + 1] > xbuff[i])
                {
                    j++;
                    k_log = !k_log;
                    buff[j] = xbuff[i];
                }
            }

            // Rearrange data points to create cycles
            int m = -1;
            for (int i = kMaxVal; i <= j; i++)
            {
                m++;
                xbuff[m] = buff[i];
            }

            for (int i = 0; i < kMaxVal; i++) //TODO
            {
                m++;
                xbuff[m] = buff[i];
            }
            xbuff[m + 1] = xbuff[0];

            // Calculate rainflow
            int k = -1, m_cur = -1, n = -1;
        stp1:
            n++; m_cur++;
            buff[n] = xbuff[m_cur];
        stp2:
            if (n < 2) goto stp1;
            double x2_cur = Math.Abs(buff[n] - buff[n - 1]);
            double x1_cur = Math.Abs(buff[n - 1] - buff[n - 2]);
        stp3:
            if (x2_cur < x1_cur) goto stp1;
            stp4:
            x_cyc.Add(new double[] { buff[n - 2], buff[n - 1] });

            n -= 2;
            buff[n] = buff[n + 2];
            if (n == 0) xbuff[m + 1] = buff[0];
            if (m_cur == m + 1 && n == 0) goto stp5;
            goto stp2;
        stp5:


            for (int i = 0; i < x_cyc.Count; i++)
            {
                if (x_cyc[i][0] > x_cyc[i][1])
                {
                    var temp = x_cyc[i][1];
                    x_cyc[i][1] = x_cyc[i][0];
                    x_cyc[i][0] = temp;
                }
            }

            return x_cyc;
        }
        static List<double[]> CalculateFullCycle(double[] rgn)
        {
            int k0 = rgn.Length;
            int k_tol = 8;

            // Initialize arrays
            var buff = new double[k0 + 1];
            var xbuff = new double[k0 + 1];
            var x_cyc = new List<double[]>();

            // Initialize variables
            double xa, xm, sig;
            int kMaxVal = 0, s = 0;
            bool k_log = false;

            // Fill xbuff array
            for (int i = 0; i < k0; i++)
            {
                xbuff[i] = Math.Round(rgn[i], k_tol);
            }
            xbuff[k0] = xbuff[0];

            // If less than 3 data points, return 0
            if (k0 < 3)
            {
                return x_cyc;
            }

            double maxVal = xbuff[0];

            // Find maximum value and its index
            for (int i = 1; i <= k0; i++)
            {
                if (xbuff[k0 - i + 1] != xbuff[k0 - i])
                {
                    k_log = xbuff[k0 - i + 1] > xbuff[k0 - i];
                    s = 1;
                    break;
                }
            }

            // If all data points are same, return 0
            if (s == 0)
            {
                return x_cyc;
            }

            // Process data points to find cycles
            int j = -1;
            for (int i = 0; i < k0; i++)
            {
                if (k_log && xbuff[i + 1] < xbuff[i])
                {
                    j++;
                    k_log = !k_log;
                    buff[j] = xbuff[i];

                    // Update maximum value and its index
                    if (buff[j] >= maxVal)
                    {
                        maxVal = buff[j];
                        kMaxVal = j;
                    }
                }
                else if (!k_log && xbuff[i + 1] > xbuff[i])
                {
                    j++;
                    k_log = !k_log;
                    buff[j] = xbuff[i];
                }
            }

            // Rearrange data points to create cycles
            int m = -1;
            for (int i = kMaxVal; i <= j; i++)
            {
                m++;
                xbuff[m] = buff[i];
            }

            for (int i = 0; i < kMaxVal; i++) //TODO
            {
                m++;
                xbuff[m] = buff[i];
            }
            xbuff[m + 1] = xbuff[0];


            /*Rem full cycle method*/
            //---------------------------------------------------------------------------------  

            //TODO
            var m_cur = m;
            do
            {
                var i1_cur = 1;
                var i2_cur = i1_cur + 1;
                var a_cur = Math.Abs(xbuff[i2_cur] - xbuff[i1_cur]);
                for (int i = 1; i <= m_cur; i++)
                {

                    if (Math.Abs(xbuff[i + 1] - xbuff[i]) < a_cur)
                    {
                        i1_cur = i;
                        i2_cur = i + 1;
                        a_cur = Math.Abs(xbuff[i + 1] - xbuff[i]);
                    }
                }
                x_cyc.Add(new double[] { xbuff[i1_cur], xbuff[i2_cur] });

                for (int i = i1_cur; i <= (m_cur - 1); i++)
                    xbuff[i] = xbuff[i + 2];

                if (i1_cur == 1)
                    xbuff[m_cur - 1] = xbuff[3];

                if (i2_cur == m_cur)
                    xbuff[1] = xbuff[m_cur + 1];

                m_cur -= 2;
            }
            while (m_cur > 2);

            x_cyc.Add(new double[] { xbuff[0], xbuff[1] });


            for (int i = 0; i < x_cyc.Count; i++)
            {
                if (x_cyc[i][0] > x_cyc[i][1])
                {
                    var temp = x_cyc[i][1];
                    x_cyc[i][1] = x_cyc[i][0];
                    x_cyc[i][0] = temp;
                }
            }

            return x_cyc;
        }
        static double CalculateEquvalentStress(List<double[]> x_cyc, double kof_m)
        {
            // Calculate equivalent stress for each cycle
            var Sigma_equiv = 0.0;
            foreach (var cyc in x_cyc)
            {
                double Sigma_equiv_i = 0;
                switch (Stress_equation)
                {
                    case "cai": Sigma_equiv_i = Cai_Equation(cyc[0], cyc[1]); break;
                    case "cai_new": Sigma_equiv_i = Cai_New_Equation(cyc[0], cyc[1], Sig_02, Ktg); break;
                    case "walker": Sigma_equiv_i = Walker_Equation(cyc[0], cyc[1], Coef_a_walker, Coef_gama_walker); break;
                    case "goodman": Sigma_equiv_i = Goodman_Equation(cyc[0], cyc[1], Gudman_ult_stress); break;
                    case "soderberg": Sigma_equiv_i = Soderberg_Equation(cyc[0], cyc[1]); break;
                    case "morro": Sigma_equiv_i = Morro_Equation(cyc[0], cyc[1]); break;
                    case "gerber": Sigma_equiv_i = Gerber_Equation(cyc[0], cyc[1]); break;
                    case "asme": Sigma_equiv_i = ASME_Elliptic_Equation(cyc[0], cyc[1]); break;
                    case "swt": Sigma_equiv_i = Smith_Watson_Topper_Equation(cyc[0], cyc[1]); break;
                    case "stulen": Sigma_equiv_i = Stulen_Equation(cyc[0], cyc[1]); break;
                    case "topper": Sigma_equiv_i = Topper_Sandor_Equation(cyc[0], cyc[1]); break;
                    default: MessageBox.Show("Stress Equation not selected"); return 0;
                }

                Sigma_equiv += Math.Pow(Sigma_equiv_i, kof_m);
            }

            Sigma_equiv = Math.Pow(Sigma_equiv, 1 / kof_m); // Calculate final equivalent stress

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
        static double Cai_New_Equation(double min, double max, double sig02, double Ktg)
        {
            var sig_a = (max - min) / 2;
            var sig_m = (max + min) / 2;
            var s = sig02 / Ktg;

            if (sig_m >= 0)
            {
                return Math.Sqrt(2 * max * sig_a);
            }

            if (max > 0 && Math.Abs(min) < s)
            {
                return (Math.Sqrt(2) * (sig_a + 0.2 * sig_m));
            }

            if (max > 0 && Math.Abs(min) >= s)
            {
                return Math.Sqrt(2.0) * (max + s + 0.2 * (max - s)) / 2.0;
            }

            if (max <= 0)
            {
                return 0;
            }

            throw new Exception($"Save this message: min={min} max={max} sig02={sig02} Ktg={Ktg}");
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


        static double[,] Read_cyclograms(ref int Cycle_Line_count, ref int Cycle_Column_count)
        {

            string[] Line_Cicl = File.ReadAllLines(Ciclogramm_File_Name); // stringvi masiv en haytararum eu fayli sax toger@ darcnum arandzin andamner                      

            Cycle_Line_count = Line_Cicl.Length;
            Cycle_Column_count = Line_Cicl[0].Split(symbols, StringSplitOptions.RemoveEmptyEntries).Length;
            double[,] cycl_matrix = new double[Cycle_Line_count, Cycle_Column_count];  // erkchap double masiv , toxer@ ciklagrammai keteri tivn a, syuneri tiv@ sluchaneri tivn a


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
        static double Sign_equivalent_von_mises_stress_2D(double x, double y, double xy)
        {
            double Max = Max_principal(x, y, xy);
            double Min = Min_principal(x, y, xy);

            if (Math.Abs(Max) > Math.Abs(Min))
                return Math.Sign(Max) * Math.Sqrt(2 * (Max * Max + Min * Min - Max * Min));
            else
                return Math.Sign(Min) * Math.Sqrt(2 * (Max * Max + Min * Min - Max * Min));
        }
        static double Sign_maximum_shear_stress_2D(double x, double y, double xy)
        {
            double Max = Max_principal(x, y, xy);
            double Min = Min_principal(x, y, xy);

            if (Math.Abs(Max) > Math.Abs(Min))
                return Math.Sign(Max) * (Max - Min) / 2.0;
            else
                return Math.Sign(Min) * (Max - Min) / 2.0;
        }
        static double Max_principal(double x, double y, double xy)
        {
            double sum = (x + y) / 2;
            double sub = (x - y) / 2;
            return sum + Math.Pow(((sub * sub) + xy * xy), 0.5);
        }
        static double Min_principal(double x, double y, double xy)
        {
            double sum = (x + y) / 2;
            double sub = (x - y) / 2;
            return sum - Math.Pow(((sub * sub) + xy * xy), 0.5);
        }
        static double Equivalent_von_mises_stress(double x, double y, double xy)
        {
            return Math.Sqrt(x * x + y * y + 3 * xy * xy - x * y);
        }
    }
}
