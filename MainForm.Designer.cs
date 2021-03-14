namespace Tetris
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.picField = new System.Windows.Forms.PictureBox();
            this.picNext = new System.Windows.Forms.PictureBox();
            this.scoreLabel = new System.Windows.Forms.Label();
            this.controlButton = new System.Windows.Forms.Button();
            this.timerMain = new System.Windows.Forms.Timer(this.components);
            this.timerRedraw = new System.Windows.Forms.Timer(this.components);
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.recordButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picField)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picNext)).BeginInit();
            this.SuspendLayout();
            // 
            // picField
            // 
            this.picField.BackColor = System.Drawing.Color.White;
            this.picField.Location = new System.Drawing.Point(30, 30);
            this.picField.Name = "picField";
            this.picField.Size = new System.Drawing.Size(300, 600);
            this.picField.TabIndex = 0;
            this.picField.TabStop = false;
            // 
            // picNext
            // 
            this.picNext.BackColor = System.Drawing.Color.White;
            this.picNext.Location = new System.Drawing.Point(360, 30);
            this.picNext.Name = "picNext";
            this.picNext.Size = new System.Drawing.Size(150, 100);
            this.picNext.TabIndex = 1;
            this.picNext.TabStop = false;
            // 
            // scoreLabel
            // 
            this.scoreLabel.AutoSize = true;
            this.scoreLabel.Font = new System.Drawing.Font("宋体", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.scoreLabel.Location = new System.Drawing.Point(380, 180);
            this.scoreLabel.Name = "scoreLabel";
            this.scoreLabel.Size = new System.Drawing.Size(94, 24);
            this.scoreLabel.TabIndex = 2;
            this.scoreLabel.Text = "分数：0";
            // 
            // controlButton
            // 
            this.controlButton.Font = new System.Drawing.Font("宋体", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.controlButton.Location = new System.Drawing.Point(384, 315);
            this.controlButton.Name = "controlButton";
            this.controlButton.Size = new System.Drawing.Size(106, 38);
            this.controlButton.TabIndex = 3;
            this.controlButton.Text = "开始";
            this.controlButton.UseVisualStyleBackColor = true;
            this.controlButton.Click += new System.EventHandler(this.controlButton_Click);
            // 
            // timerMain
            // 
            this.timerMain.Interval = 1000;
            this.timerMain.Tick += new System.EventHandler(this.timerMain_Tick);
            // 
            // timerRedraw
            // 
            this.timerRedraw.Interval = 5;
            this.timerRedraw.Tick += new System.EventHandler(this.timerRedraw_Tick);
            // 
            // timer
            // 
            this.timer.Interval = 10000;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // recordButton
            // 
            this.recordButton.Font = new System.Drawing.Font("宋体", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.recordButton.Location = new System.Drawing.Point(384, 359);
            this.recordButton.Name = "recordButton";
            this.recordButton.Size = new System.Drawing.Size(106, 38);
            this.recordButton.TabIndex = 4;
            this.recordButton.Text = "记录";
            this.recordButton.UseVisualStyleBackColor = true;
            this.recordButton.Click += new System.EventHandler(this.recordButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(540, 660);
            this.Controls.Add(this.recordButton);
            this.Controls.Add(this.controlButton);
            this.Controls.Add(this.scoreLabel);
            this.Controls.Add(this.picNext);
            this.Controls.Add(this.picField);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "俄罗斯方块";
            this.Activated += new System.EventHandler(this.Form1_Activated);
            ((System.ComponentModel.ISupportInitialize)(this.picField)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picNext)).EndInit();
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.PictureBox picField;
        private System.Windows.Forms.PictureBox picNext;
        private System.Windows.Forms.Label scoreLabel;
        private System.Windows.Forms.Button controlButton;
        private System.Windows.Forms.Timer timerMain;
        private System.Windows.Forms.Timer timerRedraw;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Button recordButton;
    }
}

