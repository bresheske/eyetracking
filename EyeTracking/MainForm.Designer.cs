namespace EyeTracking.PreprocessingBuilder
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btntoggleframeprovider = new System.Windows.Forms.Button();
            this.comboboxframeprovider = new System.Windows.Forms.ComboBox();
            this.imagebox = new System.Windows.Forms.PictureBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.listboxallfilters = new System.Windows.Forms.ListBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.listboxactivefilters = new System.Windows.Forms.ListBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imagebox)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btntoggleframeprovider);
            this.groupBox1.Controls.Add(this.comboboxframeprovider);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(395, 96);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Frame Provider";
            // 
            // btntoggleframeprovider
            // 
            this.btntoggleframeprovider.Location = new System.Drawing.Point(257, 57);
            this.btntoggleframeprovider.Name = "btntoggleframeprovider";
            this.btntoggleframeprovider.Size = new System.Drawing.Size(132, 33);
            this.btntoggleframeprovider.TabIndex = 1;
            this.btntoggleframeprovider.Text = "Toggle Frame Provider";
            this.btntoggleframeprovider.UseVisualStyleBackColor = true;
            this.btntoggleframeprovider.Click += new System.EventHandler(this.btntoggleframeprovider_Click);
            // 
            // comboboxframeprovider
            // 
            this.comboboxframeprovider.FormattingEnabled = true;
            this.comboboxframeprovider.Location = new System.Drawing.Point(6, 19);
            this.comboboxframeprovider.Name = "comboboxframeprovider";
            this.comboboxframeprovider.Size = new System.Drawing.Size(383, 21);
            this.comboboxframeprovider.TabIndex = 0;
            this.comboboxframeprovider.SelectedIndexChanged += new System.EventHandler(this.comboboxframeprovider_SelectedIndexChanged);
            // 
            // imagebox
            // 
            this.imagebox.BackColor = System.Drawing.Color.Black;
            this.imagebox.Location = new System.Drawing.Point(413, 12);
            this.imagebox.Name = "imagebox";
            this.imagebox.Size = new System.Drawing.Size(689, 527);
            this.imagebox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imagebox.TabIndex = 1;
            this.imagebox.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.listboxallfilters);
            this.groupBox2.Location = new System.Drawing.Point(12, 114);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(179, 425);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Available Filters";
            // 
            // listboxallfilters
            // 
            this.listboxallfilters.FormattingEnabled = true;
            this.listboxallfilters.Location = new System.Drawing.Point(6, 19);
            this.listboxallfilters.Name = "listboxallfilters";
            this.listboxallfilters.Size = new System.Drawing.Size(167, 394);
            this.listboxallfilters.TabIndex = 0;
            this.listboxallfilters.DoubleClick += new System.EventHandler(this.listboxallfilters_DoubleClick);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.listboxactivefilters);
            this.groupBox3.Location = new System.Drawing.Point(197, 114);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(210, 425);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Active Filters";
            // 
            // listboxactivefilters
            // 
            this.listboxactivefilters.FormattingEnabled = true;
            this.listboxactivefilters.Location = new System.Drawing.Point(6, 19);
            this.listboxactivefilters.Name = "listboxactivefilters";
            this.listboxactivefilters.Size = new System.Drawing.Size(198, 394);
            this.listboxactivefilters.TabIndex = 1;
            this.listboxactivefilters.DoubleClick += new System.EventHandler(this.listboxactivefilters_DoubleClick);
            this.listboxactivefilters.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listboxactivefilters_KeyDown);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1114, 551);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.imagebox);
            this.Controls.Add(this.groupBox1);
            this.Name = "MainForm";
            this.Text = "Preprocessing Builder";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imagebox)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox imagebox;
        private System.Windows.Forms.Button btntoggleframeprovider;
        private System.Windows.Forms.ComboBox comboboxframeprovider;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ListBox listboxallfilters;
        private System.Windows.Forms.ListBox listboxactivefilters;
    }
}

