
namespace chess
{
    partial class GameForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.boardPanel = new System.Windows.Forms.Panel();
            this.blackPlayerNameLabel = new System.Windows.Forms.Label();
            this.whitePlayerNameLabel = new System.Windows.Forms.Label();
            this.unDoButton = new System.Windows.Forms.Button();
            this.reDoButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // boardPanel
            // 
            this.boardPanel.Location = new System.Drawing.Point(195, 120);
            this.boardPanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.boardPanel.Name = "boardPanel";
            this.boardPanel.Size = new System.Drawing.Size(274, 256);
            this.boardPanel.TabIndex = 0;
            // 
            // blackPlayerNameLabel
            // 
            this.blackPlayerNameLabel.AutoSize = true;
            this.blackPlayerNameLabel.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.blackPlayerNameLabel.Location = new System.Drawing.Point(195, 89);
            this.blackPlayerNameLabel.Name = "blackPlayerNameLabel";
            this.blackPlayerNameLabel.Size = new System.Drawing.Size(65, 25);
            this.blackPlayerNameLabel.TabIndex = 1;
            this.blackPlayerNameLabel.Text = "label1";
            // 
            // whitePlayerNameLabel
            // 
            this.whitePlayerNameLabel.AutoSize = true;
            this.whitePlayerNameLabel.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.whitePlayerNameLabel.Location = new System.Drawing.Point(195, 411);
            this.whitePlayerNameLabel.Name = "whitePlayerNameLabel";
            this.whitePlayerNameLabel.Size = new System.Drawing.Size(65, 25);
            this.whitePlayerNameLabel.TabIndex = 2;
            this.whitePlayerNameLabel.Text = "label2";
            // 
            // unDoButton
            // 
            this.unDoButton.Location = new System.Drawing.Point(12, 12);
            this.unDoButton.Name = "unDoButton";
            this.unDoButton.Size = new System.Drawing.Size(75, 23);
            this.unDoButton.TabIndex = 5;
            this.unDoButton.Text = "undo";
            this.unDoButton.UseVisualStyleBackColor = true;
            this.unDoButton.Click += new System.EventHandler(this.unDoButton_Click);
            // 
            // reDoButton
            // 
            this.reDoButton.Location = new System.Drawing.Point(93, 12);
            this.reDoButton.Name = "reDoButton";
            this.reDoButton.Size = new System.Drawing.Size(75, 23);
            this.reDoButton.TabIndex = 6;
            this.reDoButton.Text = "redo";
            this.reDoButton.UseVisualStyleBackColor = true;
            this.reDoButton.Click += new System.EventHandler(this.reDoButton_Click);
            // 
            // GameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(719, 661);
            this.Controls.Add(this.reDoButton);
            this.Controls.Add(this.unDoButton);
            this.Controls.Add(this.whitePlayerNameLabel);
            this.Controls.Add(this.blackPlayerNameLabel);
            this.Controls.Add(this.boardPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GameForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Chess game";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GameForm_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel boardPanel;
        private System.Windows.Forms.Label blackPlayerNameLabel;
        private System.Windows.Forms.Label whitePlayerNameLabel;
        private System.Windows.Forms.Button unDoButton;
        private System.Windows.Forms.Button reDoButton;
    }
}

