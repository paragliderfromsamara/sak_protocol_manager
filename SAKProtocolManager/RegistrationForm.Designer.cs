namespace SAKProtocolManager
{
    partial class RegistrationForm
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
            this.freePeriodLbl = new System.Windows.Forms.Label();
            this.regKeyField = new System.Windows.Forms.TextBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.submitButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // freePeriodLbl
            // 
            this.freePeriodLbl.AutoSize = true;
            this.freePeriodLbl.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.freePeriodLbl.Location = new System.Drawing.Point(8, 23);
            this.freePeriodLbl.Name = "freePeriodLbl";
            this.freePeriodLbl.Size = new System.Drawing.Size(51, 23);
            this.freePeriodLbl.TabIndex = 0;
            this.freePeriodLbl.Text = "Срок";
            // 
            // regKeyField
            // 
            this.regKeyField.Location = new System.Drawing.Point(12, 120);
            this.regKeyField.Name = "regKeyField";
            this.regKeyField.Size = new System.Drawing.Size(309, 22);
            this.regKeyField.TabIndex = 1;
            this.regKeyField.TextChanged += new System.EventHandler(this.regKeyField_TextChanged);
            this.regKeyField.KeyDown += new System.Windows.Forms.KeyEventHandler(this.regKeyField_KeyDown);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(431, 119);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "Отмена";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // submitButton
            // 
            this.submitButton.Location = new System.Drawing.Point(332, 119);
            this.submitButton.Name = "submitButton";
            this.submitButton.Size = new System.Drawing.Size(93, 23);
            this.submitButton.TabIndex = 3;
            this.submitButton.Text = "Применить";
            this.submitButton.UseVisualStyleBackColor = true;
            this.submitButton.Click += new System.EventHandler(this.submitButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 103);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 14);
            this.label1.TabIndex = 4;
            this.label1.Text = "Ключ продукта";
            // 
            // RegistrationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(527, 186);
            this.ControlBox = false;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.submitButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.regKeyField);
            this.Controls.Add(this.freePeriodLbl);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "RegistrationForm";
            this.Text = "Регистрация";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label freePeriodLbl;
        private System.Windows.Forms.TextBox regKeyField;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button submitButton;
        private System.Windows.Forms.Label label1;
    }
}