namespace MyCar
{
    partial class Form2
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
            this.mileageTextBox = new System.Windows.Forms.TextBox();
            this.priceBlrTextBox = new System.Windows.Forms.TextBox();
            this.descriptionTextBox = new System.Windows.Forms.TextBox();
            this.priceUsdTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.saveBtn = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.categoryTextBox = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // mileageTextBox
            // 
            this.mileageTextBox.Location = new System.Drawing.Point(77, 15);
            this.mileageTextBox.Name = "mileageTextBox";
            this.mileageTextBox.Size = new System.Drawing.Size(711, 20);
            this.mileageTextBox.TabIndex = 0;
            // 
            // priceBlrTextBox
            // 
            this.priceBlrTextBox.Location = new System.Drawing.Point(77, 93);
            this.priceBlrTextBox.Name = "priceBlrTextBox";
            this.priceBlrTextBox.Size = new System.Drawing.Size(711, 20);
            this.priceBlrTextBox.TabIndex = 3;
            this.priceBlrTextBox.TextChanged += new System.EventHandler(this.priceBlrTextBox_TextChanged);
            // 
            // descriptionTextBox
            // 
            this.descriptionTextBox.Location = new System.Drawing.Point(77, 67);
            this.descriptionTextBox.Name = "descriptionTextBox";
            this.descriptionTextBox.Size = new System.Drawing.Size(711, 20);
            this.descriptionTextBox.TabIndex = 2;
            // 
            // priceUsdTextBox
            // 
            this.priceUsdTextBox.Location = new System.Drawing.Point(77, 119);
            this.priceUsdTextBox.Name = "priceUsdTextBox";
            this.priceUsdTextBox.Size = new System.Drawing.Size(533, 20);
            this.priceUsdTextBox.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Пробег";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 96);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Цена BYN";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 122);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Цена USD";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 70);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Описание";
            // 
            // saveBtn
            // 
            this.saveBtn.Location = new System.Drawing.Point(16, 145);
            this.saveBtn.Name = "saveBtn";
            this.saveBtn.Size = new System.Drawing.Size(772, 23);
            this.saveBtn.TabIndex = 5;
            this.saveBtn.Text = "Сохранить";
            this.saveBtn.UseVisualStyleBackColor = true;
            this.saveBtn.Click += new System.EventHandler(this.saveBtn_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 44);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Категория";
            // 
            // categoryTextBox
            // 
            this.categoryTextBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.categoryTextBox.FormattingEnabled = true;
            this.categoryTextBox.Items.AddRange(new object[] {
            "Двигатель",
            "Подвеска",
            "Тормозная система",
            "Свет",
            "Топливная система",
            "Рулевое управление",
            "Электрика",
            "Трансмиссия",
            "Система охлаждения",
            "Кузов",
            "Кондиционер",
            "Плановое обслуживание",
            "Прочие расходы"});
            this.categoryTextBox.Location = new System.Drawing.Point(77, 40);
            this.categoryTextBox.Name = "categoryTextBox";
            this.categoryTextBox.Size = new System.Drawing.Size(711, 21);
            this.categoryTextBox.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(616, 122);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(81, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Текущий курс:";
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(800, 180);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.categoryTextBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.saveBtn);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.priceUsdTextBox);
            this.Controls.Add(this.descriptionTextBox);
            this.Controls.Add(this.priceBlrTextBox);
            this.Controls.Add(this.mileageTextBox);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form2";
            this.Text = "Новая запись";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox mileageTextBox;
        private System.Windows.Forms.TextBox priceBlrTextBox;
        private System.Windows.Forms.TextBox descriptionTextBox;
        private System.Windows.Forms.TextBox priceUsdTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button saveBtn;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox categoryTextBox;
        private System.Windows.Forms.Label label6;
    }
}