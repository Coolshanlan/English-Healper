using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class Exercise : Form
    {
        List<Label> l_list = new List<Label>();
        List<Label> l_list2 = new List<Label>();
        List<TextBox> t_list = new List<TextBox>();
        List<WordDi> Exercise_Word = new List<WordDi>();
        public Exercise(List<WordDi> wd)
        {
            InitializeComponent();
            Exercise_Word = wd;
        }
        void create_controls(int number)
        {
            for (int i = 0; i < number; i++)
            {
                TextBox t = new TextBox();
                t.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
                t.Location = new System.Drawing.Point(150, 1);
                t.Name = "textBox1";
                t.Size = new System.Drawing.Size(89, 27);
                t.TabIndex = 3;

                Label l = new Label();
                l.Text = Exercise_Word[i].Word+"("+ Exercise_Word[i].PartOfSpeech + ")";
                l.AutoSize = true;
                l.Font = new System.Drawing.Font("Georgia", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                l.Name = "label1";
                l.RightToLeft = System.Windows.Forms.RightToLeft.No;
                l.TabIndex = 2;
                l.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

                Label l2 = new Label();
                l2.AutoSize = true;
                l2.Font = new System.Drawing.Font("Georgia", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                l2.Name = "";
                l2.RightToLeft = System.Windows.Forms.RightToLeft.No;
                l2.TabIndex = 2;
                l2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

                Panel p = new Panel();
                p.AutoScroll = true;
                p.Controls.Add(t);
                p.Controls.Add(l);
                p.Controls.Add(l2);
                p.Location = new System.Drawing.Point(2 + (i % 5) * 264, 57 + (i / 5) * 80);
                p.Name = "panel3";
                p.Size = new System.Drawing.Size(244, 70);
                p.TabIndex = 4;

                this.Controls.Add(p);
                l.Location = new System.Drawing.Point(t.Left - l.Size.Width - 5, 3);
                l2.Location = new System.Drawing.Point(t.Left - l.Size.Width - 5, 25);
                if ((l.Size.Width) + 5 > t.Location.X )
                {
                    if(( l.Size.Width) + 5 + t.Location.X+t.Size.Width < 400) p.AutoSize = true;
                    l.Location = new System.Drawing.Point(5, 3);
                    l2.Location = new System.Drawing.Point(5, 25);
                    t.Location = new Point(l.Location.X + l.Size.Width + 5, l.Location.Y);
                }
                l_list.Add(l);
                l_list2.Add(l2);
                t_list.Add(t);
                if (this.HScroll == true) this.Size = new Size(this.HorizontalScroll.Maximum + 50, this.Size.Height);
            }

        }
        private void Exercise_Load(object sender, EventArgs e)
        {
            this.ControlBox = false;
            create_controls(Exercise_Word.Count);
            Wordnum = Exercise_Word.Count;
            t_list[0].Focus();
        }
        int Wordnum = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            if (label1.Text == "")
            {
                List<int> Wrong_Word = check();//index
                label1.Text = Wordnum - Wrong_Word.Count + "/" + Wordnum + "(Correct rate:" + ((((double)Wordnum - Wrong_Word.Count) / (double)Wordnum) * 100).ToString("0.00") + "%)";
                for (int i = 0; i < Wordnum; i++)
                {
                    if (Wrong_Word.FindIndex(x => x == i) != -1)
                    {
                        Sqlclass.totable("Update E2_Word set Wrong='" + (Exercise_Word[i].Wrong + 1) + "' where ID ='" + Exercise_Word[i].ID + "'");
                    }
                    else
                    {
                        if (Exercise_Word[i].Wrong != 0)
                        {
                            Sqlclass.totable("Update E2_Word set Wrong='" + (Exercise_Word[i].Wrong - 1) + "' where ID ='" + Exercise_Word[i].ID + "'");
                        }
                    }
                }
                if (Wrong_Word.Count != 0)
                    MessageBox.Show("Please recise answer.");
                else button2.Enabled = true;
            }
            else
            {
                List<int> Wrong_Word = check();
                if (Wrong_Word.Count != 0)
                {
                    MessageBox.Show("Please recise answer.");
                }
                else button2.Enabled = true;
            }
            List<int> check()
            {
                List<int> Wrong_Word = new List<int>();
                for (int i = 0; i < Wordnum; i++)
                {
                    List<string> ans = Exercise_Word[i].Ans.Split('、').ToList();
                    List<string> reply = t_list[i].Text.Split('、').ToList();
                    bool correct = false;
                    bool finalcorrect = true;
                    foreach(var a in reply)
                    {
                        correct = false;
                        foreach(var aa in ans)
                        {
                            if (a == aa) correct = true;
                        }
                        if (finalcorrect && !correct)
                        {
                            finalcorrect = false;
                        }
                    }
                    if (!finalcorrect)
                    {
                        l_list[i].BackColor = Color.LightPink;
                        Wrong_Word.Add(i);
                    }
                    else
                    {
                        l_list[i].BackColor = Color.LightGreen;
                    }
                    l_list2[i].Text = Exercise_Word[i].Ans;


                }
                return Wrong_Word;
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
