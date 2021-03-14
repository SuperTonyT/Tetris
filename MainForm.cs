using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    public partial class MainForm : Form
    {
        private Game game;
        private int score = 0;
        private bool speedUpFlag = false;   //true表示可以加快速度

        private SqlConnection conn;
        public MainForm()
        {
            InitializeComponent();
            string ConnStr = @"server = DESKTOP-PD09L5G\SQLEXPRESS;database = Tetris;uid = sa;pwd = xxx";
            conn = new SqlConnection(ConnStr);
        }

        private void controlButton_Click(object sender, EventArgs e)
        {
            if (timerMain.Enabled)
            {
                timerMain.Stop();
                timer.Stop();
                AddRecord();    //添加记录
                MessageBox.Show(scoreLabel.Text, "游戏结束", MessageBoxButtons.OK);
                score = 0;
                scoreLabel.Text = "分数：0";
                timerMain.Interval = 1000;
                picField.Refresh();
                picNext.Refresh();
                controlButton.Text = "开始";
            }
            else
            {
                timerMain.Start();
                timer.Start();
                game = new Game(picField.Handle, picNext.Handle);
                game.Start();
                controlButton.Text = "结束";
            }
        }

        private void Form1_Activated(object sender, EventArgs e)    //直接在此事件中重绘大概率无效，可能是此事件在窗口实际显示之前发生
        {
            timerRedraw.Start();    //间隔一定时间后重绘
        }

        private void timerMain_Tick(object sender, EventArgs e)
        {
            if(speedUpFlag) //可以加速
            {
                if (timerMain.Interval > 200)
                    timerMain.Interval -= 50;
                else
                    timerMain.Interval = (int)((float)timerMain.Interval / 1.05);
                speedUpFlag = false;
            }
            bool flag = game.Next();
            score = game.GetScore();
            scoreLabel.Text = "分数：" + score;
            if(!flag)    //无法执行下一步表示游戏结束
            {
                timerMain.Stop();
                timer.Stop();
                AddRecord();    //添加记录
                MessageBox.Show(scoreLabel.Text, "游戏结束", MessageBoxButtons.OK);
                score = 0;
                scoreLabel.Text = "分数：0";
                timerMain.Interval = 1000;
                picField.Refresh();
                picNext.Refresh();
                controlButton.Text = "开始";
            }

        }

        private void timerRedraw_Tick(object sender, EventArgs e)
        {
            if(timerMain.Enabled)
                game.ReDraw();
            timerRedraw.Stop();
        }

        protected override bool ProcessDialogKey(Keys keyData)  //消息预处理，防止按键被控件捕获
        {
            if(timerMain.Enabled)   //在游戏开始的情况下，否则可能出现game未实例化
            {
                switch (keyData)
                {
                    case Keys.Left:
                        game.Left();
                        return true;
                    case Keys.Right:
                        game.Right();
                        return true;
                    case Keys.Up:
                        game.RotateLeft();
                        return true;
                    case Keys.Down:
                        game.RotateRight();
                        return true;
                    case Keys.Space:
                        game.Down();
                        return true;
                }
            }
            return base.ProcessDialogKey(keyData);
        }

        private void timer_Tick(object sender, EventArgs e) //每10秒增加一次速度,为了消除计时器间隔变化时的卡顿，速度变换在timerMain中执行
        {
            speedUpFlag = true; //表示可以加速
        }

        private void AddRecord()    //向数据库中添加记录
        {
            string str="insert into Record values('"+DateTime.Now+"',"+score+")";
            SqlCommand com = new SqlCommand(str, conn);
            conn.Open();
            com.ExecuteNonQuery();
            conn.Close();
        }

        private void recordButton_Click(object sender, EventArgs e)
        {
            RecordForm recordForm = new RecordForm();
            recordForm.ShowDialog();
        }
    }
}
