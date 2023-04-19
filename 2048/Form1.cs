using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace _2048
{
    public partial class Form1 : Form
    {
        //得分
        int score;

        public Form1()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            MaximizeBox = false;
            Text = "2048";
            KeyDown += Form1_KeyDown;
        }

        // 在窗体加载时初始化网格和数据
        private void Form1_Load(object sender, EventArgs e)
        {
            InitGrid();
            InitData();
        }

        // 定义一个4x4的标签数组
        private Label[,] labels = new Label[4, 4];
        //标签尺寸
        int lableSize = 70;
        private void InitGrid()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {

                    Label label = new Label();
                    label.BorderStyle = BorderStyle.FixedSingle;
                    label.Font = new Font("微软雅黑", 11, FontStyle.Bold);
                    label.Size = new Size(lableSize, lableSize);
                    label.TextAlign = ContentAlignment.MiddleCenter;
                    label.Left = i * lableSize;
                    label.Top = j * lableSize;
                    labels[i, j] = label;
                    panel1.Controls.Add(label);
                }
            }
        }

        // 定义一个4x4的数组存储游戏数据
        private int[,] data = new int[4, 4];
        // 初始化游戏数据，随机添加两个2或4
        private void InitData()
        {
            Random random = new Random();
            for (int i = 0; i < 2; i++)
            {
                int x = random.Next(4);
                int y = random.Next(4);
                int value = random.Next(2) == 0 ? 2 : 4;
                data[x, y] = value;
                labels[x, y].Text = value.ToString();
            }
        }

        // 监听键盘按键事件
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            bool moved = false;
            switch (e.KeyCode)
            {
                case Keys.Up:
                    moved = MoveUp();
                    break;
                case Keys.Down:
                    moved = MoveDown();
                    break;
                case Keys.Left:
                    moved = MoveLeft();
                    break;
                case Keys.Right:
                    moved = MoveRight();
                    break;
            }
            if (moved)
            {
                GenerateNumber();
                UpdateLabel();
                var flg = CheckGameOver();
                if (flg)
                {
                    MessageBox.Show("游戏结束！");
                }
            }
        }

        // 向上移动
        private bool MoveUp()
        {
            bool moved = false;
            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < 4; i++)
                {
                    for (int k = i+1; k < 4; k++)
                    {
                        if (data[j, k] != 0)
                        {
                            if (data[j, i] == 0)
                            {
                                data[j, i] = data[j, k];
                                data[j, k] = 0;
                                moved = true;
                            }
                            else if (data[j, i] == data[j, k])
                            {
                                data[j, i] *= 2;
                                data[j, k] = 0;
                                moved = true;
                                // 更新得分
                                score += data[j, i];
                            }
                        }
                    }
                }
            }
            return moved;
        }

        //向下移动
        private bool MoveDown()
        {
            bool moved = false;
            for (int j = 0; j < 4; j++)
            {
                for (int i = 3; i >= 0; i--)
                {
                    for (int k = i - 1; k >= 0; k--)
                    {
                        if (data[j, k] != 0)
                        {
                            if (data[j, i] == 0)
                            {
                                data[j, i] = data[j, k];
                                data[j, k] = 0;
                                moved = true;
                            }
                            else if (data[j, i] == data[j, k])
                            {
                                data[j, i] *= 2;
                                data[j, k] = 0;
                                moved = true;
                                // 更新得分
                                score += data[j, i];
                            }
                        }
                    }
                }
            }
            return moved;
        }

        //向左移动
        private bool MoveLeft()
        {
            bool moved = false;
            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < 4; i++)
                {
                    for (int k = i + 1; k < 4; k++)
                    {
                        if (data[k, j] != 0)
                        {
                            if (data[i, j] == 0)
                            {
                                data[i, j] = data[k, j];
                                data[k, j] = 0;
                                moved = true;
                            }
                            else if (data[i, j] == data[k, j])
                            {
                                data[i, j] *= 2;
                                data[k, j] = 0;
                                moved = true;
                                // 更新得分
                                score += data[i, j];
                            }
                        }
                    }
                }
            }
            return moved;
        }

        //向右移动
        private bool MoveRight()
        {
            bool moved = false;
            for (int j = 0; j < 4; j++)
            {
                for (int i = 3; i >= 0; i--)
                {
                    for (int k = i - 1; k >= 0; k--)
                    {
                        if (data[k, j] != 0)
                        {
                            if (data[i, j] == 0)
                            {
                                data[i, j] = data[k, j];
                                data[k, j] = 0;
                                moved = true;
                            }
                            else if (data[i, j] == data[k, j])
                            {
                                data[i, j] *= 2;
                                data[k, j] = 0;
                                moved = true;
                                // 更新得分
                                score += data[i, j];
                            }
                        }
                    }
                }
            }
            return moved;
        }

        // 随机生成新的数字方块
        private void GenerateNumber()
        {
            // 记录所有空白格子的位置
            List<int[]> emptyCells = new List<int[]>();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (data[i, j] == 0)
                    {
                        emptyCells.Add(new int[] { i, j });
                    }
                }
            }
            // 如果没有空白格子，则返回
            if (emptyCells.Count == 0)
            {
                return;
            }
            Random random = new Random();
            // 随机选择一个空白格子，并在其中放置一个新的数字方块
            int[] cell = emptyCells[random.Next(0, emptyCells.Count)];
            data[cell[0], cell[1]] = random.NextDouble() < 0.9 ? 2 : 4;
        }

        // 更新游戏界面上的标签
        private void UpdateLabel()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (data[i, j] == 0)
                    {
                        labels[i, j].Text = "";
                    }
                    else
                    {
                        labels[i, j].Text = data[i, j].ToString();
                    }
                }
            }
            // 更新得分标签
            labelScore.Text = "得分：" + score.ToString();
        }

        // 检查游戏是否结束
        private bool CheckGameOver()
        {
            // 检查是否还存在空格子
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (data[i, j] == 0)
                    {
                        return false;
                    }
                }
            }
            // 检查是否存在相邻格子数值相同
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (i > 0 && data[i, j] == data[i - 1, j])
                    {
                        return false;
                    }
                    if (i < 3 && data[i, j] == data[i + 1, j])
                    {
                        return false;
                    }
                    if (j > 0 && data[i, j] == data[i, j - 1])
                    {
                        return false;
                    }
                    if (j < 3 && data[i, j] == data[i, j + 1])
                    {
                        return false;
                    }
                }
            }
            // 所有格子都被占满且不能再移动，游戏结束
            return true;
        }
    }
}
