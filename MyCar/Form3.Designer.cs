﻿namespace MyCar
{
    partial class Form3
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
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.apiCarIdTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.apiServerTextBox = new System.Windows.Forms.TextBox();
            this.addButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(15, 63);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(527, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Проверить подключение";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Api Car ID:";
            // 
            // apiCarIdTextBox
            // 
            this.apiCarIdTextBox.Location = new System.Drawing.Point(77, 6);
            this.apiCarIdTextBox.Name = "apiCarIdTextBox";
            this.apiCarIdTextBox.Size = new System.Drawing.Size(465, 20);
            this.apiCarIdTextBox.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Api Server:";
            // 
            // apiServerTextBox
            // 
            this.apiServerTextBox.Location = new System.Drawing.Point(77, 37);
            this.apiServerTextBox.Name = "apiServerTextBox";
            this.apiServerTextBox.Size = new System.Drawing.Size(465, 20);
            this.apiServerTextBox.TabIndex = 4;
            // 
            // addButton
            // 
            this.addButton.Enabled = false;
            this.addButton.Location = new System.Drawing.Point(15, 92);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(527, 23);
            this.addButton.TabIndex = 5;
            this.addButton.Text = "Добавить новую машину";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(554, 127);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.apiServerTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.apiCarIdTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form3";
            this.Text = "Настройка";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox apiCarIdTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox apiServerTextBox;
        private System.Windows.Forms.Button addButton;
    }
}