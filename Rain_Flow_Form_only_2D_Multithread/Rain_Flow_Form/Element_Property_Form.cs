using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;


namespace Fatige_Stress_Counting_Tool
{
    public partial class Element_Property_Form : Form
    {

        static bool Elm_list_enable = false;
        static bool Koef_m_enable = false;
        static bool Koef_k_enable = false;
        static bool delta_enable = false;

        List<int> list_;

        public static bool Elm_list_enable_Pr
        {
            get { return Elm_list_enable; }
            set { Elm_list_enable = value; }
        }
        public static bool Koef_m_enable_Pr
        {
            get { return Koef_m_enable; }
            set { Koef_m_enable = value; }
        }
        public static bool Koef_k_enable_Pr
        {
            get { return Koef_k_enable; }
            set { Koef_k_enable = value; }
        }
        public static bool delta_enable_Pr
        {
            get { return delta_enable; }
            set { delta_enable = value; }
        }

        public Element_Property_Form()
        {
            InitializeComponent();
            list_ = new List<int>();
            Add_Tab_Page();
        }

        private void Element_Property_Form_Load(object sender, EventArgs e)
        {
            Delta_angle.Enabled = delta_enable;
            foreach (TabPage a in tabControl.TabPages)
            {
                if (!(a.Controls["Elm_list"].Enabled = Elm_list_enable))
                    ((TextBox)a.Controls["Elm_list"]).Clear();

                if(!(a.Controls["Kof_m"].Enabled = Koef_m_enable))
                    ((TextBox)a.Controls["Kof_m"]).Clear();

                if(!(a.Controls["Kof_k"].Enabled = Koef_k_enable))
                    ((TextBox)a.Controls["Kof_k"]).Clear();
            }
        }

        private void Add_Tab_Click(object sender, EventArgs e)
        {
            // Add tab:
            Add_Tab_Page();
        }

        private void Remuv_Tab_Click(object sender, EventArgs e)
        {
            // Removes the selected tab:
            if (tabControl.TabCount > 0)
            {
                int a = int.Parse(tabControl.SelectedTab.Text.Split(new char[] { ' ' }, StringSplitOptions.None)[2]);
                list_.Remove(a);
                tabControl.TabPages.Remove(tabControl.SelectedTab);
            }
        }

        private void Remuv_All_Tab_Click(object sender, EventArgs e)
        {
            // Removes all the tabs:
            list_.Clear();
            tabControl.TabPages.Clear();
        }

        public void Add_Tab_Page()
        {
            TabPage tabPage = new TabPage();

            Label label_1 = new Label();
            Label label_2 = new Label();
            Label label_3 = new Label();
            Label label_4 = new Label();
            Label label_5 = new Label();
            Label label_6 = new Label();
            Label label_7 = new Label();

            TextBox textBox_1 = new TextBox();
            TextBox textBox_2 = new TextBox();
            TextBox textBox_3 = new TextBox();
            // 
            // label1
            // 
            label_1.Location = new System.Drawing.Point(5, 15);
            label_1.Name = "label1";
            label_1.Size = new System.Drawing.Size(67, 16);
            label_1.Text = "Elements:";
            // 
            // label2
            // 
            label_2.Location = new System.Drawing.Point(5, 40);
            label_2.Name = "label2";
            label_2.Size = new System.Drawing.Size(87, 16);
            label_2.Text = "Coefficient m:";
            // 
            // label3
            // 
            label_3.Location = new System.Drawing.Point(5, 65);
            label_3.Name = "label3";
            label_3.Size = new System.Drawing.Size(84, 16);
            label_3.Text = "Coefficient K:";
            // label4
            // 
            label_4.Location = new System.Drawing.Point(320, 45);
            label_4.Name = "label4";
            label_4.Size = new System.Drawing.Size(100, 15);
            label_4.Text = "0";
            // label5
            // 
            label_5.Location = new System.Drawing.Point(180, 45);
            label_5.Name = "label5";
            label_5.Size = new System.Drawing.Size(180, 16);
            label_5.Text = "Line Character Count :";
            // label6
            // 
            label_6.Location = new System.Drawing.Point(320, 65);
            label_6.Name = "label4";
            label_6.Size = new System.Drawing.Size(100, 15);
            label_6.Text = "0";
            // labelt
            // 
            label_7.Location = new System.Drawing.Point(180, 65);
            label_7.Name = "label5";
            label_7.Size = new System.Drawing.Size(100, 16);
            label_7.Text = "Element Count :";
            // 
            // textBox1
            // 
            textBox_1.Location = new System.Drawing.Point(105, 15);
            textBox_1.Name = "Elm_list";
            textBox_1.Size = new System.Drawing.Size(320, 22);
            textBox_1.MaxLength = 1000000;
            textBox_1.Multiline = true;
            textBox_1.TextChanged += new System.EventHandler(textBox1_TextChanged);
            textBox_1.Enabled = Elm_list_enable;
            // 
            // textBox2
            // 
            textBox_2.Location = new System.Drawing.Point(105, 40);
            textBox_2.Name = "Kof_m";
            textBox_2.Size = new System.Drawing.Size(64, 22);
            textBox_2.TextChanged += new System.EventHandler(textBox_TextChanged);
            textBox_2.Enabled = Koef_m_enable;
            // 
            // textBox3
            // 
            textBox_3.Location = new System.Drawing.Point(105, 65);
            textBox_3.Name = "Kof_k";
            textBox_3.Size = new System.Drawing.Size(64, 22);
            textBox_3.TextChanged += new System.EventHandler(textBox_TextChanged);
            textBox_3.Enabled = Koef_k_enable;
            // 
            // tabPage
            // 
            tabPage.Controls.Add(textBox_1);
            tabPage.Controls.Add(textBox_2);
            tabPage.Controls.Add(textBox_3);
            tabPage.Controls.Add(label_1);
            tabPage.Controls.Add(label_2);
            tabPage.Controls.Add(label_3);
            tabPage.Controls.Add(label_4);
            tabPage.Controls.Add(label_5);
            tabPage.Controls.Add(label_6);
            tabPage.Controls.Add(label_7);
            tabPage.BorderStyle = BorderStyle.FixedSingle;
            tabPage.Location = new System.Drawing.Point(5, 25);
            tabPage.Name = "tabPage";
            tabPage.Padding = new System.Windows.Forms.Padding(3);
            tabPage.Size = new System.Drawing.Size(310, 100);
            tabPage.TabIndex = tabControl.TabPages.Count;
            tabPage.Text = "Elm Grup " + dic_list();
            tabPage.UseVisualStyleBackColor = true;

            tabControl.Controls.Add(tabPage);
        }

        private int dic_list()
        {
            if (list_.Count == 0)
            {
                list_.Add(0);
                return 0;
            }

            for (int i = 0; i <= list_.Max(); i++)
            {
                if (!list_.Contains(i))
                {
                    list_.Add(i);
                    return i;
                }
            }
            list_.Add(list_.Count);
            return list_.Count - 1;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            List<int> a = null;
            TextBox tb = sender as TextBox;
            Label lb1 = tb.Parent.Controls[6] as Label;
            Label lb2 = tb.Parent.Controls[8] as Label;
            if (tb.Text != "")
            {
                try
                {
                    a = elem_id_list(tb.Text);
                }
                catch
                {
                    MessageBox.Show("Invalid Format" + new string(' ', 50), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    tb.Clear();
                    return;
                }
            }
            else
            {
                return;
            }
            lb1.Text = tb.Text.Length.ToString();
            lb2.Text = a.Count.ToString();
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            double a;
            if (!double.TryParse(tb.Text, out a) && tb.Text != "")
            {
                tb.Clear();
            }
        }

        private void OK_Click(object sender, EventArgs e)
        {

            Dictionary<int, double[]> dic = new Dictionary<int, double[]>();

            for (int i = 0; i < tabControl.Controls.Count; i++)
            {
                List<int> elm_list = new List<int>();
                double m = 0, k = 0;

                if (tabControl.Controls[i].Controls[0].Enabled)
                {
                    if (tabControl.Controls[i].Controls[0].Text == "")
                    {
                        MessageBox.Show("Element List in <" + ((TextBox)(tabControl.Controls[i].Controls[0])).Parent.Text + "> is Empty");
                        return;
                    }
                    else
                    {
                        elm_list = elem_id_list(((TextBox)(tabControl.Controls[i].Controls[0])).Text);
                    }
                }


                if (tabControl.Controls[i].Controls[1].Enabled)
                {
                    if (tabControl.Controls[i].Controls[1].Text == "")
                    {
                        MessageBox.Show("Coefficient m in <" + ((TextBox)(tabControl.Controls[i].Controls[1])).Parent.Text + "> is Empty");
                        return;
                    }
                    else
                    {
                        m = double.Parse(((TextBox)(tabControl.Controls[i].Controls[1])).Text);
                    }
                }


                if (tabControl.Controls[i].Controls[2].Enabled)
                {
                    if (tabControl.Controls[i].Controls[2].Text == "")
                    {
                        MessageBox.Show("Coefficient k in <" + ((TextBox)(tabControl.Controls[i].Controls[2])).Parent.Text + "> is Empty");
                        return;
                    }
                    else
                    {
                        k = double.Parse(((TextBox)(tabControl.Controls[i].Controls[2])).Text);
                    }
                }

                for (int j = 0; j < elm_list.Count; j++)
                {

                    // The Add method throws an exception if the new key is 
                    // already in the dictionary.
                    try
                    {
                        dic.Add(elm_list[j], new double[] { m, k });
                    }
                    catch (ArgumentException)
                    {
                        MessageBox.Show("An element with ID =" + elm_list[j] + " olredy exist");
                        return;
                    }
                }
            }


            if (Delta_angle.Enabled)
            {
                if (Delta_angle.Text == "")
                {
                    MessageBox.Show("Angular iteration step is empty");
                    return;
                }
                else
                {
                    engine.Delta_Pr = double.Parse(Delta_angle.Text);
                }
            }


            //dic = dic.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);

            engine.elm_prop_Pr = dic;

            if(Delta_angle.Enabled)
            engine.Delta_Pr = double.Parse(Delta_angle.Text);

            Close();
        }

        static List<int> elem_id_list(string input)
        {
            List<int> elem_id = new List<int>();

            char[] split_1 = new char[] { ' ', ',' };
            char[] split_2 = new char[] { ':' };

            string[] a = input.Split(split_1, StringSplitOptions.RemoveEmptyEntries);

            if (!(a[0] == "e" || a[0] == "E" || a[0] == "Elm" || a[0] == "Element"))
                throw new Exception();


            for (int i = 1; i < a.Length; i++)
            {
                int char_count = a[i].Count(f => f == ':');

                if (char_count == 0)
                {
                    elem_id.Add(int.Parse(a[i]));
                }
                else if (char_count == 1)
                {
                    int fairst_id = int.Parse(a[i].Split(split_2)[0]);
                    int last_id = int.Parse(a[i].Split(split_2)[1]);

                    for (int j = fairst_id; j <= last_id; j++)
                    {
                        elem_id.Add(j);
                    }
                }
                else if (char_count == 2)
                {
                    int first_id = int.Parse(a[i].Split(split_2)[0]);
                    int last_id = int.Parse(a[i].Split(split_2)[1]);
                    int increment = int.Parse(a[i].Split(split_2)[2]);

                    if (first_id < last_id)

                        for (int j = first_id; j <= last_id; j += increment)
                            elem_id.Add(j);

                    if (first_id > last_id)

                        for (int j = first_id; j >= last_id; j += increment)
                            elem_id.Add(j);
                }
                char_count = 0;
            }
            return elem_id;
        }

        private void Delta_angle_TextChanged(object sender, EventArgs e)
        {
            double a;
            if (!double.TryParse(Delta_angle.Text, out a) && Delta_angle.Text != "")
            {
                Delta_angle.Clear();
            }
        }





















    }
}
