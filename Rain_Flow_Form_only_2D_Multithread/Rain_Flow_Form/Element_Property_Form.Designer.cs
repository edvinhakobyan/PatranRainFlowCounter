namespace Fatige_Stress_Counting_Tool
{
    partial class Element_Property_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Element_Property_Form));
            this.OK = new System.Windows.Forms.Button();
            this.Add_Tab = new System.Windows.Forms.Button();
            this.Remuv_Tab = new System.Windows.Forms.Button();
            this.Remuv_All_Tab = new System.Windows.Forms.Button();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // OK
            // 
            this.OK.Location = new System.Drawing.Point(392, 265);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(80, 25);
            this.OK.TabIndex = 4;
            this.OK.Text = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // Add_Tab
            // 
            this.Add_Tab.Location = new System.Drawing.Point(12, 4);
            this.Add_Tab.Name = "Add_Tab";
            this.Add_Tab.Size = new System.Drawing.Size(75, 23);
            this.Add_Tab.TabIndex = 4;
            this.Add_Tab.Text = "Add Tab ->";
            this.Add_Tab.UseVisualStyleBackColor = true;
            this.Add_Tab.Click += new System.EventHandler(this.Add_Tab_Click);
            // 
            // Remuv_Tab
            // 
            this.Remuv_Tab.Location = new System.Drawing.Point(99, 4);
            this.Remuv_Tab.Name = "Remuv_Tab";
            this.Remuv_Tab.Size = new System.Drawing.Size(75, 23);
            this.Remuv_Tab.TabIndex = 5;
            this.Remuv_Tab.Text = "Remuv Tab";
            this.Remuv_Tab.UseVisualStyleBackColor = true;
            this.Remuv_Tab.Click += new System.EventHandler(this.Remuv_Tab_Click);
            // 
            // Remuv_All_Tab
            // 
            this.Remuv_All_Tab.Location = new System.Drawing.Point(190, 4);
            this.Remuv_All_Tab.Name = "Remuv_All_Tab";
            this.Remuv_All_Tab.Size = new System.Drawing.Size(88, 23);
            this.Remuv_All_Tab.TabIndex = 6;
            this.Remuv_All_Tab.Text = "Remuv All Tab";
            this.Remuv_All_Tab.UseVisualStyleBackColor = true;
            this.Remuv_All_Tab.Click += new System.EventHandler(this.Remuv_All_Tab_Click);
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tabControl.Location = new System.Drawing.Point(12, 33);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(460, 186);
            this.tabControl.TabIndex = 1;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 242);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(460, 11);
            this.progressBar1.TabIndex = 14;
            // 
            // Element_Property_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 298);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.Remuv_All_Tab);
            this.Controls.Add(this.Remuv_Tab);
            this.Controls.Add(this.Add_Tab);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.tabControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1000, 1000);
            this.MinimumSize = new System.Drawing.Size(400, 300);
            this.Name = "Element_Property_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Element Property";
            this.Load += new System.EventHandler(this.Element_Property_Form_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Button Add_Tab;
        private System.Windows.Forms.Button Remuv_Tab;
        private System.Windows.Forms.Button Remuv_All_Tab;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}

