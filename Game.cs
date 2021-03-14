using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace Tetris
{
    class Game
    {
        private System.IntPtr fieldHandle;  //主界面句柄
        private System.IntPtr nextHandle;   //显示下一块的界面的句柄

        private int score = 0;

        private const int width = 10;   //场景宽度（块）
        private const int height = 20;  //场景高度（块）
        private const int squareSize = 30;  //方块边长（像素）

        private struct Position     //方块位置,横轴为X轴，纵轴为Y，从左上到右下递增
        {
            public int X;
            public int Y;
        }
        private Position[] block;   //当前块
        private enum BlockTypes     //7种形状
        {
            square = 1,
            line = 2,
            J = 3,
            L = 4,
            T = 5,
            Z = 6,
            S = 7
        };
        private BlockTypes currentBlockType;    //当前块形状
        private BlockTypes nextBlcokType;   //下一块形状

        private Color[,] background;  //背景各块的颜色
        private Color currentBlockColor;   //当前块颜色
        private Color nextBlockColor;   //下一块颜色

        public Game(System.IntPtr fieldHandle, System.IntPtr nextHandle)
        {
            this.fieldHandle = fieldHandle;
            this.nextHandle = nextHandle;
            block = new Position[4];
            background = new Color[height, width];
            for (int i = 0; i < height - 1; i++) //背景初始化，全为白色
                for (int j = 0; j < width; j++)
                    background[i, j] = Color.White;
            Random rand = new Random((int)DateTime.Now.Ticks);  //防止一开始生成的两块相同
            nextBlcokType = (BlockTypes)rand.Next(1, 8);    //随机数包含下限，不含上限
            nextBlockColor = SelectColor(nextBlcokType);
            NewBlock();
        }

        private Color SelectColor(BlockTypes type)   //根据方块类型选择颜色
        {
            switch(type)
            {
                case BlockTypes.square:
                    return Color.Yellow;
                case BlockTypes.line:
                    return Color.Cyan;
                case BlockTypes.J:
                    return Color.Blue;
                case BlockTypes.L:
                    return Color.Orange;
                case BlockTypes.T:
                    return Color.Purple;
                case BlockTypes.Z:
                    return Color.Red;
                case BlockTypes.S:
                    return Color.Green;
            }
            return Color.Red;
        }

        private bool NewBlock() //创建新块，无法创建返回0
        {
            currentBlockType = nextBlcokType;
            currentBlockColor = nextBlockColor;
            switch(currentBlockType)    //设定初始位置
            {
                /*
                ****01****
                ****23****
                */
                case BlockTypes.square:
                    block[0].Y = 0;
                    block[0].X = width / 2 - 1;
                    block[1].Y = 0;
                    block[1].X = width / 2;
                    block[2].Y = 1;
                    block[2].X = width / 2 - 1;
                    block[3].Y = 1;
                    block[3].X = width / 2;
                    break;
                /*
                ***0123***
                */
                case BlockTypes.line:
                    block[0].Y = 0;
                    block[0].X = width / 2 - 2;
                    block[1].Y = 0;
                    block[1].X = width / 2 - 1;
                    block[2].Y = 0;
                    block[2].X = width / 2;
                    block[3].Y = 0;
                    block[3].X = width / 2 + 1;
                    break;
                /*
                ****0*****
                ****123***
                */
                case BlockTypes.J:
                    block[0].Y = 0;
                    block[0].X = width / 2 - 1;
                    block[1].Y = 1;
                    block[1].X = width / 2 - 1;
                    block[2].Y = 1;
                    block[2].X = width / 2;
                    block[3].Y = 1;
                    block[3].X = width / 2 + 1;
                    break;
                /*
                *****0****
                ***321****
                */
                case BlockTypes.L:
                    block[0].Y = 0;
                    block[0].X = width / 2;
                    block[1].Y = 1;
                    block[1].X = width / 2;
                    block[2].Y = 1;
                    block[2].X = width / 2 - 1;
                    block[3].Y = 1;
                    block[3].X = width / 2 - 2;
                    break;
                /*
                *****2****
                ****013***
                */
                case BlockTypes.T:
                    block[0].Y = 1;
                    block[0].X = width / 2 - 1;
                    block[1].Y = 1;
                    block[1].X = width / 2;
                    block[2].Y = 0;
                    block[2].X = width / 2;
                    block[3].Y = 1;
                    block[3].X = width / 2 + 1;
                    break;
                /*
                ****01****
                *****23***
                */
                case BlockTypes.Z:
                    block[0].Y = 0;
                    block[0].X = width / 2 - 1;
                    block[1].Y = 0;
                    block[1].X = width / 2;
                    block[2].Y = 1;
                    block[2].X = width / 2;
                    block[3].Y = 1;
                    block[3].X = width / 2 + 1;
                    break;
                /*
                ****10****
                ***32*****
                */
                case BlockTypes.S:
                    block[0].Y = 0;
                    block[0].X = width / 2;
                    block[1].Y = 0;
                    block[1].X = width / 2 - 1;
                    block[2].Y = 1;
                    block[2].X = width / 2 - 1;
                    block[3].Y = 1;
                    block[3].X = width / 2 - 2;
                    break;
            }
            if (CheckConflict(block))
                return false;
            else
            {
                Random rand = new Random();
                nextBlcokType = (BlockTypes)rand.Next(1, 8);
                nextBlockColor = SelectColor(nextBlcokType);
                return true;
            }
        }

        private bool CheckConflict(Position[] pos)  //检查冲突和越界，有错返回1
        {
            bool flag = false;
            for(int i=0;i<4;i++)
            {
                if(pos[i].X>width-1 || pos[i].X<0 || pos[i].Y>height-1 || pos[i].Y<0 || background[pos[i].Y,pos[i].X]!=Color.White)
                {
                    flag = true;
                    break;
                }
            }
            return flag;
        }

        private void DrawCurrentBlock() //绘制当前块
        {
            Graphics g = Graphics.FromHwnd(fieldHandle);
            Brush brush = new SolidBrush(currentBlockColor);
            for(int i=0;i<4;i++)
                g.FillRectangle(brush, squareSize * block[i].X, squareSize * block[i].Y, squareSize, squareSize);
        }

        private void EraseCurrentBlock()    //消除当前块
        {
            Graphics g = Graphics.FromHwnd(fieldHandle);
            Brush brush = new SolidBrush(Color.White);
            for (int i = 0; i < 4; i++)
                g.FillRectangle(brush, squareSize * block[i].X, squareSize * block[i].Y, squareSize, squareSize);
        }

        private void DrawNextBlock()    //绘制下一块
        {
            Graphics g = Graphics.FromHwnd(nextHandle);
            Brush brush = new SolidBrush(nextBlockColor);
            switch(nextBlcokType)
            {
                case BlockTypes.square:
                    g.FillRectangle(brush, 45, 20, squareSize, squareSize);
                    g.FillRectangle(brush, 45 + squareSize, 20, squareSize, squareSize);
                    g.FillRectangle(brush, 45, 20 + squareSize, squareSize, squareSize);
                    g.FillRectangle(brush, 45 + squareSize, 20 + squareSize, squareSize, squareSize);
                    break;
                case BlockTypes.line:
                    g.FillRectangle(brush, 15, 35, squareSize, squareSize);
                    g.FillRectangle(brush, 15 + squareSize, 35, squareSize, squareSize);
                    g.FillRectangle(brush, 15 + 2*squareSize, 35, squareSize, squareSize);
                    g.FillRectangle(brush, 15 + 3*squareSize, 35, squareSize, squareSize);
                    break;
                case BlockTypes.J:
                    g.FillRectangle(brush, 30, 20, squareSize, squareSize);
                    g.FillRectangle(brush, 30, 20 + squareSize, squareSize, squareSize);
                    g.FillRectangle(brush, 30 + squareSize, 20 + squareSize, squareSize, squareSize);
                    g.FillRectangle(brush, 30 + 2*squareSize, 20 + squareSize, squareSize, squareSize);
                    break;
                case BlockTypes.L:
                    g.FillRectangle(brush, 30 + 2 * squareSize, 20, squareSize, squareSize);
                    g.FillRectangle(brush, 30, 20 + squareSize, squareSize, squareSize);
                    g.FillRectangle(brush, 30 + squareSize, 20 + squareSize, squareSize, squareSize);
                    g.FillRectangle(brush, 30 + 2 * squareSize, 20 + squareSize, squareSize, squareSize);
                    break;
                case BlockTypes.T:
                    g.FillRectangle(brush, 30 + squareSize, 20, squareSize, squareSize);
                    g.FillRectangle(brush, 30, 20 + squareSize, squareSize, squareSize);
                    g.FillRectangle(brush, 30 + squareSize, 20 + squareSize, squareSize, squareSize);
                    g.FillRectangle(brush, 30 + 2 * squareSize, 20 + squareSize, squareSize, squareSize);
                    break;
                case BlockTypes.Z:
                    g.FillRectangle(brush, 30, 20, squareSize, squareSize);
                    g.FillRectangle(brush, 30 + squareSize, 20, squareSize, squareSize);
                    g.FillRectangle(brush, 30 + squareSize, 20 + squareSize, squareSize, squareSize);
                    g.FillRectangle(brush, 30 + 2 * squareSize, 20 + squareSize, squareSize, squareSize);
                    break;
                case BlockTypes.S:
                    g.FillRectangle(brush, 30 + 2 * squareSize, 20, squareSize, squareSize);
                    g.FillRectangle(brush, 30, 20 + squareSize, squareSize, squareSize);
                    g.FillRectangle(brush, 30 + squareSize, 20 + squareSize, squareSize, squareSize);
                    g.FillRectangle(brush, 30 + squareSize, 20, squareSize, squareSize);
                    break;
            }
        }

        private void EraseNextBlock()   //消除下一块
        {
            Graphics g = Graphics.FromHwnd(nextHandle);
            Brush brush = new SolidBrush(Color.White);
            g.FillRectangle(brush, 0, 0, 150, 100);
        }

        private void DrawBackground()   //绘制背景,覆盖原背景
        {
            Graphics g = Graphics.FromHwnd(fieldHandle);
            Brush brush;
            for(int i=0;i<height;i++)
            {
                for(int j=0;j<width;j++)
                {
                    brush = new SolidBrush(background[i, j]);
                    g.FillRectangle(brush, j*squareSize, i*squareSize, squareSize, squareSize);
                }
            }
        }

        public void Start()
        {
            DrawCurrentBlock();
            DrawNextBlock();
        }

        public bool Down()  //下降一格，无法下降返回0
        {
            Position[] temp=new Position[4];
            for(int i=0;i<4;i++)
            {
                temp[i].X = block[i].X;
                temp[i].Y = block[i].Y + 1;
            }
            if (CheckConflict(temp))    //有冲突
                return false;
            else    //重绘当前块
            {
                EraseCurrentBlock();
                block = temp;
                DrawCurrentBlock();
                return true;
            }
        }

        public bool Left()  //左移一格
        {
            Position[] temp = new Position[4];
            for (int i = 0; i < 4; i++)
            {
                temp[i].X = block[i].X - 1;
                temp[i].Y = block[i].Y;
            }
            if (CheckConflict(temp))    //有冲突
                return false;
            else    //重绘当前块
            {
                EraseCurrentBlock();
                block = temp;
                DrawCurrentBlock();
                return true;
            }
        }

        public bool Right()  //右移一格
        {
            Position[] temp = new Position[4];
            for (int i = 0; i < 4; i++)
            {
                temp[i].X = block[i].X + 1;
                temp[i].Y = block[i].Y;
            }
            if (CheckConflict(temp))    //有冲突
                return false;
            else    //重绘当前块
            {
                EraseCurrentBlock();
                block = temp;
                DrawCurrentBlock();
                return true;
            }
        }

        /* 除了方块，其他形状全都以块1为中心旋转
        0123   0       0    2    01     10
               123   321   013    23   32    
        */
        public bool RotateLeft() //逆时针旋转
        {
            if (currentBlockType == BlockTypes.square)
                return false;
            Position[] temp = new Position[4];
            temp[1] = block[1];
            int X0 = block[1].X;
            int Y0 = block[1].Y;
            for(int i=0;i<4;i++)
            {
                if(i!=1)
                {
                    int X = block[i].X;
                    int Y = block[i].Y;
                    temp[i].X = X0 + (Y - Y0);
                    temp[i].Y = Y0 + (X0 - X);
                }
            }
            if (CheckConflict(temp))    //有冲突
                return false;
            else
            {
                EraseCurrentBlock();
                block = temp;
                DrawCurrentBlock();
                return true;
            }
        }

        public bool RotateRight()   //顺时针旋转
        {
            if (currentBlockType == BlockTypes.square)
                return false;
            Position[] temp = new Position[4];
            temp[1] = block[1];
            int X0 = block[1].X;
            int Y0 = block[1].Y;
            for (int i = 0; i < 4; i++)
            {
                if (i != 1)
                {
                    int X = block[i].X;
                    int Y = block[i].Y;
                    temp[i].X = X0 + (Y0 - Y);
                    temp[i].Y = Y0 + (X - X0);
                }
            }
            if (CheckConflict(temp))    //有冲突
                return false;
            else
            {
                EraseCurrentBlock();
                block = temp;
                DrawCurrentBlock();
                return true;
            }
        }

        private void Freeze()   //冻结当前块（加入背景中）
        {
            for (int i = 0; i < 4; i++)
                background[block[i].Y, block[i].X] = currentBlockColor;
        }
        
        private int CheckRows() //消除行
        {
            int n = 0;  //消除的行数
            for(int i=0;i<height;i++)
            {
                bool flag = true;  //判断当前行是否可以消除
                for(int j=0;j<width;j++)
                {
                    if(background[i,j]==Color.White)
                    {
                        flag = false;
                        break;
                    }    
                }
                if(flag)
                {
                    n++;
                    for(int j=i;j>0;j--)
                    {
                        for(int k=0;k<width;k++)
                        {
                            background[j, k] = background[j - 1, k];
                        }
                    }
                    for (int j = 0; j < width; j++)
                        background[0, j] = Color.White;
                }
            }
            return n;
        }

        public bool Next()  //下一步，返回0表示游戏结束
        {
            if(!Down()) //无法下降
            {
                Freeze(); // 冻结当前块
                int n = CheckRows();    //删除行数
                if (n != 0)
                {
                    DrawBackground();   //重绘背景
                    score = score + (int)Math.Pow(2, n - 1) * 100;  //删除n行(n>0)，分数加2^(n-1)*100
                }
                if (!NewBlock())    //无法创建新块表示游戏结束
                    return false;
                score += 10; //每新增一块加10分
                DrawCurrentBlock();
                EraseNextBlock();   //重绘下一块
                DrawNextBlock();
            }
            return true;
        }

        public void ReDraw()    //用于重新进入窗口后的重绘
        {
            DrawBackground();
            DrawCurrentBlock();
            DrawNextBlock();
        }

        public int GetScore()
        {
            return score;
        }
    }
}
