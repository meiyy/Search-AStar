using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Search_Algorithm
{

    public delegate void DFlash(Node tmp);
    public partial class Form1 : Form
    {
        int width = 17, height = 10;
        int[,] stat;
        int[,] f;
        int[,] g;
        int[,] h;
        bool[,] close;
        bool[,] open;
        Node start, end;
        Node[,] father;
        Button[,] btn;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Width = width * 51 + 96;
            Height = height * 51 + 46;
            button1.Location = new Point(width * 51 + 20, Height/3+10);
            btn=new Button[width, height];
            stat = new int[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    int x = i * 51 + 3;
                    int y = j * 51 + 3;
                    btn[i, j] = new Button();
                    btn[i, j].Height = btn[i, j].Width = 50;
                    btn[i, j].Location = new Point(x, y);
                    Controls.Add(btn[i, j]);
                    btn[i, j].MouseDown += BtnOnClick;
                    btn[i, j].BackColor = Color.LightGray;
                }
            }
        }

        private void BtnOnClick(object sender, EventArgs e)
        {
            Button me = (Button)sender;
            int x = me.Location.X / 51;
            int y = me.Location.Y / 51;
            if(stat[x, y]==0)
            {
                stat[x, y] = 1;
                me.BackColor = Color.Black;
            }
            else if (stat[x, y] == 1)
            {
                stat[x, y] = 2;
                me.Text = "S";
                me.BackColor = Color.Aqua;
                start = new Node(x, y, 0);
            }
            else if (stat[x, y] == 2)
            {
                stat[x, y] = 3;
                me.Text = "E";
                end = new Node(x, y, 0);
            }
            else if (stat[x, y] == 3)
            {
                stat[x, y] = 0;
                me.Text = "";
                me.BackColor=Color.LightGray;
            }
        }

        public void Flash(Node tmp)
        {
            for (int i = 0; i < 0; i++)
            {
                btn[tmp.x, tmp.y].BackColor = Color.Red;
                Update();
                System.Threading.Thread.Sleep(50);
                btn[tmp.x, tmp.y].BackColor = Color.LightGray;
                Update();
                System.Threading.Thread.Sleep(50);
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if(btn[i,j].Text=="O")
                    {
                        btn[i, j].Text = "";
                    }
                    if(stat[i,j] == 3)
                    {
                        end = new Node(i, j,0);
                    }
                    if (stat[i, j] == 2)
                    {
                        start = new Node(i, j,0);
                    }
                }
            }
            Init();
            int ans = Solve(out List<Node> path, Flash);
            foreach(var i in path)
            {
                btn[i.x, i.y].ForeColor = Color.Red;
                btn[i.x, i.y].Text = "O";
            }
            MessageBox.Show(ans.ToString());
        }

        public void Init()
        {
            f = new int[width, height];
            g = new int[width, height];
            h = new int[width, height];
            open = new bool[width, height];
            close = new bool[width, height];
            father = new Node[width, height];
        }

        bool CanGo(int xx, int yy)
        {
            return xx >= 0 && yy >= 0 && xx < width && yy < height;
        }

        int GetH(int x, int y)
        {
            return Math.Abs(x - end.x) + Math.Abs(y - end.y);
        }

        public int Solve(out List<Node> path, DFlash Flash)
        {
            path = new List<Node>();
            NodeCmp cmp = new NodeCmp();
            SortedSet<Node> set = new SortedSet<Node>(cmp) { start };
            int[] xTo = { 1, -1, 0, 0 };
            int[] yTo = { 0, 0, 1, -1 };
            while (set.Count != 0)
            {
                Node tmp = set.First();
                set.Remove(tmp);
                Flash(tmp);
                close[tmp.x, tmp.y] = true;
                open[tmp.x, tmp.y] = false;
                if (tmp.x == end.x && tmp.y == end.y)
                {
                    Node it = tmp;
                    it = father[it.x, it.y];
                    while (it.x != start.x || it.y != start.y)
                    {
                        path.Insert(0, it);
                        it = father[it.x, it.y];
                    }
                    return g[tmp.x, tmp.y];
                }
                for (int i = 0; i < 4; i++)
                {
                    int tx = tmp.x + xTo[i];
                    int ty = tmp.y + yTo[i];
                    if (CanGo(tx, ty) && stat[tx, ty] != 1 && !close[tx, ty])
                    {
                        if (!open[tx, ty])
                        {
                            father[tx, ty] = tmp;
                            g[tx, ty] = g[tmp.x, tmp.y] + 1;
                            h[tx, ty] = GetH(tx, ty);
                            f[tx, ty] = g[tx, ty] + h[tx, ty];
                            set.Add(new Node(tx, ty, f[tx, ty]));
                            open[tmp.x, tmp.y] = true;
                        }
                        else if (g[tx, ty] > g[tmp.x, tmp.y] + 1)
                        {
                            father[tx, ty] = tmp;
                            set.Remove(new Node(tx, ty, f[tx, ty]));
                            g[tx, ty] = g[tmp.x, tmp.y] + 1;
                            h[tx, ty] = GetH(tx, ty);
                            f[tx, ty] = g[tx, ty] + h[tx, ty];
                            set.Add(new Node(tx, ty, f[tx, ty]));
                        }
                    }
                }
            }
            return -1;
        }
    }
}

