using System;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Fatige_Stress_Counting_Tool
{
    public partial class Element_Property_Form : Form
    {

        static bool sigma_02_enable = false;
        static bool elm_list_enable = false;
        static bool koef_m_enable = false;
        static bool koef_k_enable = false;
        static bool delta_enable = false;
        static bool ktg_enable = false;
        readonly List<int> list_;

        public static bool Elm_list_enable_Pr
        {
            get { return elm_list_enable; }
            set { elm_list_enable = value; }
        }
        public static bool Koef_m_enable_Pr
        {
            get { return koef_m_enable; }
            set { koef_m_enable = value; }
        }
        public static bool Koef_k_enable_Pr
        {
            get { return koef_k_enable; }
            set { koef_k_enable = value; }
        }
        public static bool Delta_enable_Pr
        {
            get { return delta_enable; }
            set { delta_enable = value; }
        }

        public static bool Sigma_02_enable
        {
            get { return sigma_02_enable; }
            set { sigma_02_enable = value; }
        }
        public static bool Ktg_enable
        {
            get { return ktg_enable; }
            set { ktg_enable = value; }
        }
        public Element_Property_Form()
        {
            InitializeComponent();
            list_ = new List<int>();
            Add_Tab_Page();
        }

        private void Element_Property_Form_Load(object sender, EventArgs e)
        {
            foreach (TabPage a in tabControl.TabPages)
            {
                if (!(a.Controls["Elm_list"].Enabled = elm_list_enable))
                    ((TextBox)a.Controls["Elm_list"]).Clear();

                if(!(a.Controls["Kof_m"].Enabled = koef_m_enable))
                    ((TextBox)a.Controls["Kof_m"]).Clear();

                if(!(a.Controls["Kof_k"].Enabled = koef_k_enable))
                    ((TextBox)a.Controls["Kof_k"]).Clear();

                if(!(a.Controls["Sigma_02"].Enabled = Sigma_02_enable))
                    ((TextBox)a.Controls["Sigma_02"]).Clear();

                if(!(a.Controls["Ktg"].Enabled = Ktg_enable))
                    ((TextBox)a.Controls["Ktg"]).Clear();
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
            Label label_8 = new Label();
            Label label_9 = new Label();

            TextBox textBox_1 = new TextBox();
            TextBox textBox_2 = new TextBox();
            TextBox textBox_3 = new TextBox();
            TextBox textBox_4 = new TextBox();
            TextBox textBox_5 = new TextBox();
            // 
            // label1
            // 
            label_1.Location = new Point(5, 15);
            label_1.Name = "label1";
            label_1.Size = new Size(67, 16);
            label_1.Text = "Elements:";
            // 
            // label2
            // 
            label_2.Location = new Point(5, 40);
            label_2.Name = "label2";
            label_2.Size = new Size(87, 16);
            label_2.Text = "Coefficient m:";
            // 
            // label3
            // 
            label_3.Location = new Point(5, 65);
            label_3.Name = "label3";
            label_3.Size = new Size(84, 16);
            label_3.Text = "Coefficient K:";
            // label4
            // 
            label_4.Location = new Point(320, 45);
            label_4.Name = "label4";
            label_4.Size = new Size(100, 15);
            label_4.Text = "0";
            // label5
            // 
            label_5.Location = new Point(180, 45);
            label_5.Name = "label5";
            label_5.Size = new Size(180, 16);
            label_5.Text = "Character Count :";
            // label6
            // 
            label_6.Location = new Point(320, 65);
            label_6.Name = "label6";
            label_6.Size = new Size(100, 15);
            label_6.Text = "0";
            // label7
            // 
            label_7.Location = new Point(180, 65);
            label_7.Name = "label7";
            label_7.Size = new Size(100, 16);
            label_7.Text = "Element Count :";
            // label8
            // 
            label_8.Location = new Point(5, 90);
            label_8.Name = "label8";
            label_8.Size = new Size(100, 16);
            label_8.Text = "σ_0.2";
            // label9
            // 
            label_9.Location = new Point(5, 115);
            label_9.Name = "label9";
            label_9.Size = new Size(100, 16);
            label_9.Text = "Ktg :";
            // 
            // textBox1
            // 
            textBox_1.Location = new Point(105, 15);
            textBox_1.Name = "Elm_list";
            textBox_1.Size = new Size(320, 22);
            textBox_1.MaxLength = 1000000;
            textBox_1.Multiline = true;
            textBox_1.TextChanged += new System.EventHandler(TextBox1_TextChanged);
            textBox_1.Enabled = elm_list_enable;
            // 
            // textBox2
            // 
            textBox_2.Location = new Point(105, 40);
            textBox_2.Name = "Kof_m";
            textBox_2.Size = new Size(64, 22);
            textBox_2.TextChanged += new System.EventHandler(TextBox_TextChanged);
            textBox_2.Enabled = koef_m_enable;
            // 
            // textBox3
            // 
            textBox_3.Location = new Point(105, 65);
            textBox_3.Name = "Kof_k";
            textBox_3.Size = new Size(64, 22);
            textBox_3.TextChanged += new System.EventHandler(TextBox_TextChanged);
            textBox_3.Enabled = koef_k_enable;
            // 
            // textBox4
            // 
            textBox_4.Location = new Point(105, 90);
            textBox_4.Name = "Sigma_02";
            textBox_4.Size = new Size(64, 22);
            textBox_4.TextChanged += new EventHandler(TextBox_TextChanged);
            textBox_4.Enabled = Sigma_02_enable;
            // 
            // textBox4
            // 
            textBox_5.Location = new Point(105, 115);
            textBox_5.Name = "Ktg";
            textBox_5.Size = new Size(64, 22);
            textBox_5.TextChanged += new EventHandler(TextBox_TextChanged);
            textBox_5.Enabled = Ktg_enable;
            // 
            // tabPage
            // 
            tabPage.Controls.Add(textBox_1);
            tabPage.Controls.Add(textBox_2);
            tabPage.Controls.Add(textBox_3);
            tabPage.Controls.Add(textBox_4);
            tabPage.Controls.Add(textBox_5);
            tabPage.Controls.Add(label_1);
            tabPage.Controls.Add(label_2);
            tabPage.Controls.Add(label_3);
            tabPage.Controls.Add(label_4);
            tabPage.Controls.Add(label_5);
            tabPage.Controls.Add(label_6);
            tabPage.Controls.Add(label_7);
            tabPage.Controls.Add(label_8);
            tabPage.Controls.Add(label_9);
            tabPage.BorderStyle = BorderStyle.FixedSingle;
            tabPage.Location = new Point(5, 25);
            tabPage.Name = "tabPage";
            tabPage.Padding = new System.Windows.Forms.Padding(3);
            tabPage.Size = new Size(310, 100);
            tabPage.TabIndex = tabControl.TabPages.Count;
            tabPage.Text = "Elm Grup " + Dic_list();
            tabPage.UseVisualStyleBackColor = true;

            tabControl.Controls.Add(tabPage);
        }

        private int Dic_list()
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

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            Label lb1 = tb.Parent.Controls[8] as Label;
            Label lb2 = tb.Parent.Controls[10] as Label;
            List<int> a;
            if (tb.Text != "")
            {
                try
                {
                    a = Elem_id_list(tb.Text);
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

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (!double.TryParse(tb.Text, out double a) && tb.Text != "")
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
                double m = 0, k = 0, sig02 = 0, ktg = 0;

                if (tabControl.Controls[i].Controls[0].Enabled)
                {
                    if (tabControl.Controls[i].Controls[0].Text == "")
                    {
                        MessageBox.Show("Element List in <" + ((TextBox)(tabControl.Controls[i].Controls[0])).Parent.Text + "> is Empty");
                        return;
                    }
                    else
                    {
                        elm_list = Elem_id_list(((TextBox)(tabControl.Controls[i].Controls[0])).Text);
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

                if (tabControl.Controls[i].Controls[3].Enabled)
                {
                    if (tabControl.Controls[i].Controls[3].Text == "")
                    {
                        MessageBox.Show("σ_0.2 in <" + ((TextBox)(tabControl.Controls[i].Controls[3])).Parent.Text + "> is Empty");
                        return;
                    }
                    else
                    {
                        sig02 = double.Parse(((TextBox)(tabControl.Controls[i].Controls[3])).Text);
                    }
                }

                if (tabControl.Controls[i].Controls[4].Enabled)
                {
                    if (tabControl.Controls[i].Controls[4].Text == "")
                    {
                        MessageBox.Show("Ktg in <" + ((TextBox)(tabControl.Controls[i].Controls[4])).Parent.Text + "> is Empty");
                        return;
                    }
                    else
                    {
                        ktg = double.Parse(((TextBox)(tabControl.Controls[i].Controls[4])).Text);
                    }
                }

                for (int j = 0; j < elm_list.Count; j++)
                {

                    // The Add method throws an exception if the new key is 
                    // already in the dictionary.
                    try
                    {
                        dic.Add(elm_list[j], new double[] { m, k, sig02, ktg });
                    }
                    catch (ArgumentException)
                    {
                        MessageBox.Show("An element with ID =" + elm_list[j] + " olredy exist");
                        return;
                    }
                }
            }

            Engine.Elm_prop = dic;

            Close();
        }

        static List<int> Elem_id_list(string input)
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
                    elem_id.Add(int.Parse(a[i]));
                else
                {
                    int first_id = int.Parse(a[i].Split(split_2)[0]);
                    int last_id = int.Parse(a[i].Split(split_2)[1]);
                    int increment = char_count == 2 ? int.Parse(a[i].Split(split_2)[2]) : 1;

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
    }
}
