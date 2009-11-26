namespace SampleClient
{
    partial class Main
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
            this.btnUnhandled = new System.Windows.Forms.Button();
            this.btnHandled = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnUnhandled
            // 
            this.btnUnhandled.Location = new System.Drawing.Point(12, 12);
            this.btnUnhandled.Name = "btnUnhandled";
            this.btnUnhandled.Size = new System.Drawing.Size(186, 23);
            this.btnUnhandled.TabIndex = 0;
            this.btnUnhandled.Text = "Throw Unhandled Exception";
            this.btnUnhandled.UseVisualStyleBackColor = true;
            this.btnUnhandled.Click += new System.EventHandler(this.btnUnhandled_Click);
            // 
            // btnHandled
            // 
            this.btnHandled.Location = new System.Drawing.Point(12, 41);
            this.btnHandled.Name = "btnHandled";
            this.btnHandled.Size = new System.Drawing.Size(186, 23);
            this.btnHandled.TabIndex = 0;
            this.btnHandled.Text = "Log Handled Exception";
            this.btnHandled.UseVisualStyleBackColor = true;
            this.btnHandled.Click += new System.EventHandler(this.btnHandled_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(210, 78);
            this.Controls.Add(this.btnHandled);
            this.Controls.Add(this.btnUnhandled);
            this.Name = "Main";
            this.Text = "Erroring Service Client";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnUnhandled;
        private System.Windows.Forms.Button btnHandled;
    }
}

