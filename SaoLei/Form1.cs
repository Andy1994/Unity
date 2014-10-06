using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Minesweeper
{
    public partial class Form1 : Form
    {
        Button[,] bt;
        int[,] BoomNumber;//炸弹数量
        int[,] BoomFlag;//炸弹旗
        int isFirst = 0;//判断是否第一次点击
        int N = 0;//炸弹数量
        int ZX, ZY;//雷区长宽
        int GameOver = 0;//判断游戏是否结束
        int time = 0;//用时
        int restMines;//剩余炸弹
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            AddButton(15, 20);
        }

        private void AddButton(int x, int y)
        {
            //炸弹数量大概是25%
            N = Convert.ToInt32(x * y * 0.25);
            ZX = x;
            ZY = y;
            restMines = N;
            label4.Text = restMines.ToString() + " 个";
            //初始化BoomNumber（炸弹数量）数组
            BoomNumber = new int[x + 2, y + 2];
            for (int i = 0; i < x + 2; i++)
            {
                for (int j = 0; j < y + 2; j++)
                {
                    BoomNumber[i, j] = 0;
                }
            }
            //初始化炸弹旗数组
            BoomFlag = new int[x + 2, y + 2];
            for (int i = 0; i < x + 2; i++)
            {
                for (int j = 0; j < y + 2; j++)
                {
                    BoomFlag[i, j] = 0;
                }
            }
            //生成雷区
            bt = new Button[x + 2, y + 2];
            int k = 0;
            for (int i = 0; i < x + 1; i++)
            {
                for (int j = 0; j < y + 1; j++)
                {
                    bt[i, j] = new Button();
                    bt[i, j].Text = "";
                    bt[i, j].Location = new Point(30 * j, 30 * i);
                    bt[i, j].Size = new Size(30, 30);
                    bt[i, j].UseVisualStyleBackColor = true;
                    bt[i, j].MouseDown += new System.Windows.Forms.MouseEventHandler(Mouse_Down);
                    //bt[i, j].Text = i.ToString() + "," + j.ToString();
                    //bt[i, j].MouseUp += new System.Windows.Forms.MouseEventHandler(Mouse_Up);
                    //bt[i, j].MouseClick += new System.Windows.Forms.MouseEventHandler(Mouse_Click);
                    if (!(i == 0 || i == x + 2 || j == 0 || j == y + 2))
                        Controls.Add(bt[i, j]);
                    k++;
                }
            }
        }

        private void Mouse_Down(object sender, MouseEventArgs e)
        {
            int y = this.PointToClient(MousePosition).X / 30;
            int x = this.PointToClient(MousePosition).Y / 30;
            //生成炸弹位置，判断是不是第一次点击，第一次点击不会出现炸弹
            if (isFirst == 0)
            {
                //启动计时
                this.timer1.Interval = 1000; //设置间隔时间，为毫秒；
                this.timer1.Tick += new System.EventHandler(this.timer1_Tick);////设置每间隔3000毫秒（3秒）执行一次函数timer1_Tick
                this.timer1.Start();

                isFirst = 1;
                int NNumber = 0;
                Random ra = new Random();
                //生成N个炸弹
                while (true)
                {
                    for (int i = 1; ; i++)
                    {
                        int x2 = ra.Next(1, ZX);
                        int y2 = ra.Next(1, ZY);
                        if (BoomNumber[x2, y2] == 0 && !(x2 == x && y2 == y))
                        {
                            BoomNumber[x2, y2] = 10;
                            NNumber++;
                            if (NNumber == N) break;
                            //bt[x2, y2].BackgroundImageLayout = ImageLayout.Stretch;
                            //bt[x2, y2].BackgroundImage = Properties.Resources.boom2;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    if (NNumber == N) break;
                }
                //计算炸弹周围炸弹数量
                int CountBoom = 0;
                for (int i = 1; i <= ZX; i++)
                {
                    for (int j = 1; j <= ZY; j++)
                    {
                        if (BoomNumber[i, j] != 10)
                        {
                            if (BoomNumber[i - 1, j - 1] == 10) CountBoom = CountBoom + 1;
                            if (BoomNumber[i - 1, j] == 10) CountBoom = CountBoom + 1;
                            if (BoomNumber[i - 1, j + 1] == 10) CountBoom = CountBoom + 1;
                            if (BoomNumber[i, j - 1] == 10) CountBoom = CountBoom + 1;
                            if (BoomNumber[i, j + 1] == 10) CountBoom = CountBoom + 1;
                            if (BoomNumber[i + 1, j - 1] == 10) CountBoom = CountBoom + 1;
                            if (BoomNumber[i + 1, j] == 10) CountBoom = CountBoom + 1;
                            if (BoomNumber[i + 1, j + 1] == 10) CountBoom = CountBoom + 1;
                            BoomNumber[i, j] = CountBoom;
                            CountBoom = 0;
                        }
                        else continue;
                    }
                }
            }
            //单击左键
            if (e.Button == MouseButtons.Left)
            {
                if (BoomFlag[x, y] == 0)//之前没打开过或者没标记过的才能打开
                {
                    if (BoomNumber[x, y] == 10)//踩到炸弹，游戏结束，显示所有炸弹
                    {
                        GameOver = 1;
                        BoomFlag[x, y] = 3;//是炸弹
                    }
                    else if (BoomNumber[x, y] == 0)//打开的是0，打开附近的方格
                    {
                        OpenZero(x, y);
                    }
                    else//不是炸弹不是0，就打开方格
                    {
                        OpenButton(x, y);
                    }
                }
            }
            //单击右键
            if (e.Button == MouseButtons.Right)
            {
                if (BoomFlag[x, y] == 0)//标记炸弹
                {
                    BoomFlag[x, y] = 2;
                    restMines--;//剩余炸弹减去1
                    bt[x, y].BackgroundImageLayout = ImageLayout.Stretch;
                    bt[x, y].BackgroundImage = Properties.Resources.flag;
                }
                else if (BoomFlag[x, y] == 2)//之前标记过的取消标记
                {
                    BoomFlag[x, y] = 0;
                    restMines++;//剩余炸弹加上1
                    bt[x, y].BackgroundImageLayout = ImageLayout.Stretch;
                    bt[x, y].BackgroundImage = Properties.Resources.plane;
                }
            }
            //点击鼠标中键
            if (e.Button == MouseButtons.Middle)
            {
                int AroundBoomNumber = 0;
                if (BoomFlag[x, y] != 0 && BoomFlag[x, y] != 2)
                {
                    if (BoomFlag[x - 1, y - 1] == 2) AroundBoomNumber++;
                    if (BoomFlag[x - 1, y] == 2) AroundBoomNumber++;
                    if (BoomFlag[x - 1, y + 1] == 2) AroundBoomNumber++;
                    if (BoomFlag[x, y - 1] == 2) AroundBoomNumber++;
                    if (BoomFlag[x, y + 1] == 2) AroundBoomNumber++;
                    if (BoomFlag[x + 1, y - 1] == 2) AroundBoomNumber++;
                    if (BoomFlag[x + 1, y] == 2) AroundBoomNumber++;
                    if (BoomFlag[x + 1, y + 1] == 2) AroundBoomNumber++;
                    if (AroundBoomNumber == BoomNumber[x, y])//标记数到达方块上的数字时才能按中键
                    {
                        BiaoJi(x - 1, y - 1);
                        BiaoJi(x - 1, y);
                        BiaoJi(x - 1, y + 1);
                        BiaoJi(x, y - 1);
                        BiaoJi(x, y + 1);
                        BiaoJi(x + 1, y - 1);
                        BiaoJi(x + 1, y);
                        BiaoJi(x + 1, y + 1);
                    }
                }
            }
            label4.Text = restMines.ToString() + " 个";

            //判断炸弹是否全部找到,然后结束
            int sumBoom = 0;
            if (restMines == 0)
            {
                for (int i = 1; i <= ZX; i++)
                {
                    for (int j = 1; j <= ZY; j++)
                    {
                        if (BoomFlag[i, j] == 2 && BoomNumber[i, j] == 10)
                        {
                            sumBoom++;
                        }
                    }
                }
                if (sumBoom == N) GameOver = 3;
            }
            //踩到地雷游戏结束
            gameover(x, y);
        }

        //计时
        private void timer1_Tick(object sender, EventArgs e)
        {
            time++;
            label2.Text = time.ToString() + " 秒";
        }

        //游戏结束函数
        private void gameover(int x,int y)
        {
            //踩到地雷结束询问是否重开
            if (GameOver != 0)
            {
                this.timer1.Stop();
                if (GameOver == 1)
                {
                    GameOver = 0;
                    DisplayMines(x, y);
                    MessageBox.Show("踩到地雷了，要重新开始吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    //System.Diagnostics.Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location);
                }
                else if (GameOver == 3)
                {
                    MessageBox.Show("成功闯关！", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                }
                Form1 a = new Form1();
                a.Show();
                this.Hide();
            }
        }

        //按中键的时候判断标记是不是错误
        private void BiaoJi(int x, int y)
        {
            if (BoomNumber[x, y] != 10 && BoomFlag[x, y] == 2)
            {
                GameOver = 1;
                bt[x, y].BackgroundImageLayout = ImageLayout.Stretch;
                bt[x, y].BackgroundImage = Properties.Resources.flag2;
                return;
            }
            else if (BoomNumber[x, y] == 10 && BoomFlag[x, y] != 2 && BoomNumber[x, y] != 0)
            {
                GameOver = 1;
                bt[x, y].BackgroundImageLayout = ImageLayout.Stretch;
                bt[x, y].BackgroundImage = Properties.Resources.boom3;
                return;
            }
            else if (BoomNumber[x, y] != 10 && BoomFlag[x, y] != 2)
            {
                if (BoomNumber[x, y] == 0)
                {
                    OpenZero(x, y);
                }
                OpenButton(x, y);
            }
        }

        //打开0附近的方块
        private void OpenZero(int x, int y)
        {
            if (x != 0 && x != ZX + 1 && y != 0 && y != ZY + 1)
            {
                BoomFlag[x, y] = 4;
                OpenAround(x, y);
                if (BoomNumber[x - 1, y - 1] == 0 && BoomFlag[x - 1, y - 1] != 4)
                {
                    OpenZero(x - 1, y - 1);
                }
                if (BoomNumber[x - 1, y] == 0 && BoomFlag[x - 1, y] != 4)
                {
                    OpenZero(x - 1, y);
                }
                if (BoomNumber[x - 1, y + 1] == 0 && BoomFlag[x - 1, y + 1] != 4)
                {
                    OpenZero(x - 1, y + 1);
                }
                if (BoomNumber[x, y - 1] == 0 && BoomFlag[x, y - 1] != 4)
                {
                    OpenZero(x, y - 1);
                }
                if (BoomNumber[x, y + 1] == 0 && BoomFlag[x, y + 1] != 4)
                {
                    OpenZero(x, y + 1);
                }
                if (BoomNumber[x + 1, y - 1] == 0 && BoomFlag[x + 1, y - 1] != 4)
                {
                    OpenZero(x + 1, y - 1);
                }
                if (BoomNumber[x + 1, y] == 0 && BoomFlag[x + 1, y] != 4)
                {
                    OpenZero(x + 1, y);
                }
                if (BoomNumber[x + 1, y + 1] == 0 && BoomFlag[x + 1, y + 1] != 4)
                {
                    OpenZero(x + 1, y + 1);
                }
                return;
            }
        }

        //打开附近9个方格
        private void OpenAround(int x, int y)
        {
            OpenButton(x - 1, y - 1);
            OpenButton(x - 1, y);
            OpenButton(x - 1, y + 1);
            OpenButton(x, y - 1);
            OpenButton(x, y);
            OpenButton(x, y + 1);
            OpenButton(x + 1, y - 1);
            OpenButton(x + 1, y);
            OpenButton(x + 1, y + 1);
        }

        //显示全部Button.Text
        private void DisplayGod()
        {
            for (int i = 1; i <= ZX; i++)
            {
                for (int j = 1; j <= ZY; j++)
                {
                    OpenButton(i, j);
                }
            }
        }

        //显示所有地雷，点击的地雷变红色
        private void DisplayMines(int x, int y)
        {
            for (int i = 1; i <= ZX; i++)
            {
                for (int j = 1; j <= ZY; j++)
                {
                    if (BoomNumber[i, j] != 10 && BoomFlag[i, j] == 2)
                    {
                        bt[i, j].BackgroundImageLayout = ImageLayout.Stretch;
                        bt[i, j].BackgroundImage = Properties.Resources.flag2;
                    }
                    if (BoomNumber[i, j] == 10 && BoomFlag[i, j] != 2)
                    {
                        if (BoomFlag[i, j] == 3)
                        {
                            bt[i, j].BackgroundImageLayout = ImageLayout.Stretch;
                            bt[i, j].BackgroundImage = Properties.Resources.boom3;
                        }
                        else OpenButton(i, j);
                    }
                }
            }
        }

        //显示一个button.Text
        private void OpenButton(int x, int y)
        {
            if (x != 0 && x != ZX + 1 && y != 0 && y != ZY + 1)
            {
                if (BoomFlag[x, y] != 4 && BoomFlag[x, y] != 2) BoomFlag[x, y] = 1;
                if (BoomNumber[x, y] == 0)
                {
                    bt[x, y].ForeColor = Color.White;
                }
                if (BoomNumber[x, y] == 1)
                {
                    bt[x, y].ForeColor = Color.Blue;
                }
                if (BoomNumber[x, y] == 2)
                {
                    bt[x, y].ForeColor = Color.Green;
                }
                if (BoomNumber[x, y] == 3)
                {
                    bt[x, y].ForeColor = Color.Red;
                }
                if (BoomNumber[x, y] == 4)
                {
                    bt[x, y].ForeColor = Color.BlueViolet;
                }
                if (BoomNumber[x, y] == 5)
                {
                    bt[x, y].ForeColor = Color.Orange;
                }
                if (BoomNumber[x, y] == 6)
                {
                    bt[x, y].ForeColor = Color.Pink;
                }
                if (BoomNumber[x, y] == 7)
                {
                    bt[x, y].ForeColor = Color.Yellow;
                }
                if (BoomNumber[x, y] == 8)
                {
                    bt[x, y].ForeColor = Color.Black;
                }
                bt[x, y].Text = BoomNumber[x, y].ToString();
                if (BoomNumber[x, y] == 10)
                {
                    bt[x, y].BackgroundImageLayout = ImageLayout.Stretch;
                    bt[x, y].BackgroundImage = Properties.Resources.boom2;
                    bt[x, y].Text = "";
                }
            }
        }

        //生成的子窗口退出时程序退出
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        //按下重开按钮，游戏结束重开
        private void button_gameover_Click_1(object sender, EventArgs e)
        {
            GameOver = 2;
            gameover(0, 0);
        }
    }
}
