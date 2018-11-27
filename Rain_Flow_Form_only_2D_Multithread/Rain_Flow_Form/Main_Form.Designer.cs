namespace Fatige_Stress_Counting_Tool
{
    partial class Main_Form
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main_Form));
            this.menu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Atach_1D_Report_File = new System.Windows.Forms.ToolStripMenuItem();
            this.Atach_2D_Report_File = new System.Windows.Forms.ToolStripMenuItem();
            this.Atach_3D_Report_File = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.Atach_Cyclogram_File = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.Save_Result_As = new System.Windows.Forms.ToolStripMenuItem();
            this.vewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Show_Consol = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Counting_Methods = new System.Windows.Forms.ToolStripMenuItem();
            this.mean_stress_correction = new System.Windows.Forms.ToolStripMenuItem();
            this.Stress_Combination = new System.Windows.Forms.ToolStripMenuItem();
            this.Element_Property = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inputFileFormatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Uniaxial = new System.Windows.Forms.ToolStripMenuItem();
            this.Biaxial = new System.Windows.Forms.ToolStripMenuItem();
            this.Volumetric = new System.Windows.Forms.ToolStripMenuItem();
            this.Cycl_File_Format = new System.Windows.Forms.ToolStripMenuItem();
            this.Run = new System.Windows.Forms.Button();
            progressBar1 = new System.Windows.Forms.ProgressBar();
            this.menu.SuspendLayout();
            this.SuspendLayout();
            // 
            // menu
            // 
            this.menu.BackColor = System.Drawing.SystemColors.ControlLight;
            this.menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.vewToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menu.Location = new System.Drawing.Point(0, 0);
            this.menu.Name = "menu";
            this.menu.Size = new System.Drawing.Size(425, 24);
            this.menu.TabIndex = 0;
            this.menu.Text = "menu";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Atach_1D_Report_File,
            this.Atach_2D_Report_File,
            this.Atach_3D_Report_File,
            this.toolStripSeparator1,
            this.Atach_Cyclogram_File,
            this.toolStripSeparator2,
            this.Save_Result_As});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // Atach_1D_Report_File
            // 
            this.Atach_1D_Report_File.Name = "Atach_1D_Report_File";
            this.Atach_1D_Report_File.Size = new System.Drawing.Size(187, 22);
            this.Atach_1D_Report_File.Text = "Atach 1D Report File";
            this.Atach_1D_Report_File.Click += new System.EventHandler(this.Atach_1D_Report_File_Click);
            // 
            // Atach_2D_Report_File
            // 
            this.Atach_2D_Report_File.Name = "Atach_2D_Report_File";
            this.Atach_2D_Report_File.Size = new System.Drawing.Size(187, 22);
            this.Atach_2D_Report_File.Text = "Atach 2D Report File";
            this.Atach_2D_Report_File.Click += new System.EventHandler(this.Atach_2D_Report_File_Click);
            // 
            // Atach_3D_Report_File
            // 
            this.Atach_3D_Report_File.Enabled = false;
            this.Atach_3D_Report_File.Name = "Atach_3D_Report_File";
            this.Atach_3D_Report_File.Size = new System.Drawing.Size(187, 22);
            this.Atach_3D_Report_File.Text = "Atach 3D Report File";
            this.Atach_3D_Report_File.Click += new System.EventHandler(this.Atach_3D_Report_File_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(184, 6);
            // 
            // Atach_Cyclogram_File
            // 
            this.Atach_Cyclogram_File.Name = "Atach_Cyclogram_File";
            this.Atach_Cyclogram_File.Size = new System.Drawing.Size(187, 22);
            this.Atach_Cyclogram_File.Text = "Atach Cyclogram File";
            this.Atach_Cyclogram_File.Click += new System.EventHandler(this.Atach_Cyclogram_File_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(184, 6);
            // 
            // Save_Result_As
            // 
            this.Save_Result_As.Name = "Save_Result_As";
            this.Save_Result_As.Size = new System.Drawing.Size(187, 22);
            this.Save_Result_As.Text = "Save Result As";
            this.Save_Result_As.Click += new System.EventHandler(this.Save_Result_As_Click);
            // 
            // vewToolStripMenuItem
            // 
            this.vewToolStripMenuItem.BackColor = System.Drawing.SystemColors.ControlLight;
            this.vewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Show_Consol});
            this.vewToolStripMenuItem.Name = "vewToolStripMenuItem";
            this.vewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.vewToolStripMenuItem.Text = "View";
            // 
            // Show_Consol
            // 
            this.Show_Consol.Checked = true;
            this.Show_Consol.CheckOnClick = true;
            this.Show_Consol.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Show_Consol.Name = "Show_Consol";
            this.Show_Consol.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.C)));
            this.Show_Consol.Size = new System.Drawing.Size(181, 22);
            this.Show_Consol.Text = "Show Consol";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Counting_Methods,
            this.mean_stress_correction,
            this.Stress_Combination,
            this.Element_Property});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // Counting_Methods
            // 
            this.Counting_Methods.Name = "Counting_Methods";
            this.Counting_Methods.Size = new System.Drawing.Size(196, 22);
            this.Counting_Methods.Text = "Counting Methods";
            this.Counting_Methods.Click += new System.EventHandler(this.Counting_Methods_Click);
            // 
            // mean_stress_correction
            // 
            this.mean_stress_correction.Name = "mean_stress_correction";
            this.mean_stress_correction.Size = new System.Drawing.Size(196, 22);
            this.mean_stress_correction.Text = "Mean Stress Correction";
            this.mean_stress_correction.Click += new System.EventHandler(this.Mean_Stress_Correction_Click);
            // 
            // Stress_Combination
            // 
            this.Stress_Combination.Name = "Stress_Combination";
            this.Stress_Combination.Size = new System.Drawing.Size(196, 22);
            this.Stress_Combination.Text = "Stress Combination";
            this.Stress_Combination.Click += new System.EventHandler(this.Stress_Combination_Click);
            // 
            // Element_Property
            // 
            this.Element_Property.Name = "Element_Property";
            this.Element_Property.Size = new System.Drawing.Size(196, 22);
            this.Element_Property.Text = "Element Property";
            this.Element_Property.Click += new System.EventHandler(this.Element_Property_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.inputFileFormatToolStripMenuItem,
            this.Cycl_File_Format});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // inputFileFormatToolStripMenuItem
            // 
            this.inputFileFormatToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Uniaxial,
            this.Biaxial,
            this.Volumetric});
            this.inputFileFormatToolStripMenuItem.Name = "inputFileFormatToolStripMenuItem";
            this.inputFileFormatToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.inputFileFormatToolStripMenuItem.Text = "Input File Format";
            // 
            // Uniaxial
            // 
            this.Uniaxial.Name = "Uniaxial";
            this.Uniaxial.Size = new System.Drawing.Size(132, 22);
            this.Uniaxial.Text = "Uniaxial";
            this.Uniaxial.Click += new System.EventHandler(this.Uniaxial_Click);
            // 
            // Biaxial
            // 
            this.Biaxial.Name = "Biaxial";
            this.Biaxial.Size = new System.Drawing.Size(132, 22);
            this.Biaxial.Text = "Biaxial";
            this.Biaxial.Click += new System.EventHandler(this.Biaxial_Click);
            // 
            // Volumetric
            // 
            this.Volumetric.Name = "Volumetric";
            this.Volumetric.Size = new System.Drawing.Size(132, 22);
            this.Volumetric.Text = "Volumetric";
            this.Volumetric.Click += new System.EventHandler(this.Volumetric_Click);
            // 
            // Cycl_File_Format
            // 
            this.Cycl_File_Format.Name = "Cycl_File_Format";
            this.Cycl_File_Format.Size = new System.Drawing.Size(194, 22);
            this.Cycl_File_Format.Text = "Cyclogram File Format";
            this.Cycl_File_Format.Click += new System.EventHandler(this.Cycl_File_Format_Click);
            // 
            // Run
            // 
            this.Run.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Run.Location = new System.Drawing.Point(10, 40);
            this.Run.Name = "Run";
            this.Run.Size = new System.Drawing.Size(74, 27);
            this.Run.TabIndex = 1;
            this.Run.Text = "Run";
            this.Run.UseVisualStyleBackColor = true;
            this.Run.Click += new System.EventHandler(this.Run_Click);
            // 
            // progressBar1
            // 
            progressBar1.Location = new System.Drawing.Point(5, 572);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new System.Drawing.Size(413, 23);
            progressBar1.TabIndex = 2;
            // 
            // Main_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(425, 597);
            this.Controls.Add(progressBar1);
            this.Controls.Add(this.Run);
            this.Controls.Add(this.menu);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menu;
            this.MaximizeBox = false;
            this.Name = "Main_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "";
            this.Text = "Equivalent Stress Counting";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menu.ResumeLayout(false);
            this.menu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem vewToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menu;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.Button Run;
        private System.Windows.Forms.ToolStripMenuItem Counting_Methods;
        private System.Windows.Forms.ToolStripMenuItem Show_Consol;
        private System.Windows.Forms.ToolStripMenuItem mean_stress_correction;
        private System.Windows.Forms.ToolStripMenuItem Stress_Combination;
        private System.Windows.Forms.ToolStripMenuItem inputFileFormatToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem Uniaxial;
        private System.Windows.Forms.ToolStripMenuItem Biaxial;
        private System.Windows.Forms.ToolStripMenuItem Volumetric;
        private System.Windows.Forms.ToolStripMenuItem Atach_1D_Report_File;
        private System.Windows.Forms.ToolStripMenuItem Atach_2D_Report_File;
        private System.Windows.Forms.ToolStripMenuItem Atach_3D_Report_File;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem Atach_Cyclogram_File;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem Save_Result_As;
        private System.Windows.Forms.ToolStripMenuItem Element_Property;
        private System.Windows.Forms.ToolStripMenuItem Cycl_File_Format;
        public static System.Windows.Forms.ProgressBar progressBar1;
    }
}

