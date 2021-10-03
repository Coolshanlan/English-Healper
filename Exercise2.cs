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
    public partial class Exercise2 : Form
    {
        List<List<Label>> lalist = new List<List<Label>>();
        List<List<Label>> laQuestion = new List<List<Label>>();//0:title 1:Word
        List<TextBox> txlist = new List<TextBox>();
        List<WordDi> Exercise_Word = new List<WordDi>();
        Dictionary<string, List<DataRow>> Word_Classify = new Dictionary<string, List<DataRow>>();
        List<int> Ans = new List<int>();
        DataTable All_Word = new DataTable();
        int Wordnum = 0;
        int questionnum = 0;
        string searchstring = "";
        DataTable search_Word = new DataTable();
        public Exercise2(List<WordDi> EW, int Qn, string ss, DataTable alw)
        {
            InitializeComponent();
            Exercise_Word = EW;
            Wordnum = Exercise_Word.Count;
            questionnum = Qn;
            searchstring = ss;
            All_Word = alw;
            Word_Classify.Add("phr", null); Word_Classify.Add("adj", null); Word_Classify.Add("n", null); Word_Classify.Add("conj", null); Word_Classify.Add("adv", null); Word_Classify.Add("", null); Word_Classify.Add("prep", null); Word_Classify.Add("v", null);
            List<string> sss = Word_Classify.Select(x => x.Key).ToList();
            foreach (var a in sss)
            {
                List<DataRow> ld = new List<DataRow>();
                for (int i = 0; i < All_Word.Rows.Count; i++)
                {
                    if (All_Word.Rows[i]["PartOfSpeech"].ToString() == a) ld.Add(All_Word.Rows[i]);
                }
                Word_Classify[a] = ld;
            }
        }
        Label labelset(string text, Point p, int fontsize, FontStyle fs)
        {
            Label l = new Label();
            l.AutoSize = true;
            l.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            l.Text = text;
            l.Location = new Point(p.X, p.Y);
            l.Font = new System.Drawing.Font("Georgia", fontsize, fs, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            return l;
        }
        void create_controls()
        {
            Point p = new Point(panel1.Location.X, panel1.Location.Y);
            Size s = new Size(panel1.Size.Width, panel1.Size.Height);
            for (int i = 0; i < Wordnum; i++)
            {
                int Ansnum = rd.Next(0, questionnum);
                Ans.Add(Ansnum);
                Panel pn = new Panel();
                pn.AutoSize = true;
                pn.Location = new Point(5, p.Y + s.Height + 5);
                TextBox t = new TextBox();
                Label ql1 = labelset((i + 1).ToString() + ".", new Point() { X = 5, Y = 8 }, 12, FontStyle.Bold);
                pn.Controls.Add(ql1);
                t.Location = new Point(ql1.Location.X + ql1.Size.Width, 10);
                t.Multiline = true;
                t.Size = new Size(50, 50);
                t.TextAlign = HorizontalAlignment.Center;
                t.Font = new System.Drawing.Font("Georgia", 24F, FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                pn.Controls.Add(t);
                Label ql2 = labelset(Exercise_Word[i].Word + (Exercise_Word[i].type == 1 ? (" (" + Exercise_Word[i].PartOfSpeech + ")") : ""), new Point() { X = t.Location.X + t.Size.Width + 20, Y = 5 }, 14, FontStyle.Bold);
                ql2.ForeColor = Color.Brown;
                pn.Controls.Add(ql2);
                laQuestion.Add(new List<Label>());
                lalist.Add(new List<Label>());
                laQuestion[i].Add(ql1);
                laQuestion[i].Add(ql2);
                txlist.Add(t);

                Point lp = new Point(ql2.Location.X, ql2.Location.Y + ql2.Size.Height + 10);
                Size ls = new Size(ql2.Size.Width, ql2.Size.Height);
                for (int j = 0; j < questionnum; j++)
                {
                    Wordag:
                    WordDi wd = RandomWord(Exercise_Word[i].type, Exercise_Word[i].PartOfSpeech);
                    while (!checkword(wd, Exercise_Word[i], Exercise_Word[i].type))
                    {
                        wd = RandomWord(Exercise_Word[i].type, Exercise_Word[i].PartOfSpeech);
                    }
                    for (int a = 0; a < lalist[i].Count; a++)
                        if (lalist[i][a].Text == (Ansnum == j ? ("(" + (a + 1) + ") " + Exercise_Word[i].Ans + "   ") : ("(" + (a + 1) + ") " + wd.Ans + "   ")))
                        {
                            goto Wordag;
                        }
                    Label ql = labelset(Ansnum == j ? ("(" + (j + 1) + ") " + Exercise_Word[i].Ans + "   ") : ("(" + (j + 1) + ") " + wd.Ans + "   "), new Point() { X = lp.X + (j == 0 ? 0 : (ls.Width + 5)), Y = lp.Y }, 12, FontStyle.Regular);
                    pn.Controls.Add(ql);
                    lalist[i].Add(ql);
                    lp = new Point(ql.Location.X, ql.Location.Y);
                    ls = new Size(ql.Size.Width, ql.Size.Height);
                }
                //if(questionnum /3 == 1)
                //{
                //    Point lp = new Point(ql2.Location.X,ql2.Location.Y+ql2.Size.Height+10);
                //    Size ls = new Size(ql2.Size.Width,ql2.Size.Height);
                //    for (int j = 0; j < 3; j++)
                //    {
                //        Label ql = labelset(Ansnum == j ? ("(" + (j + 1) + ") " + Exercise_Word[i].Ans) : ("(" + (j + 1) + ") " + RandomWord(Exercise_Word[i].type)), new Point() { X = lp.X + (j == 0 ? 0 : (ls.Width + 5)), Y = lp.Y }, 12, FontStyle.Regular);
                //        pn.Controls.Add(ql);
                //        lalist[i].Add(ql);
                //        lp = new Point(ql.Location.X, ql.Location.Y );
                //        ls = new Size(ql.Size.Width, ql.Size.Height);
                //    }
                //    lp = new Point(lalist[i][0].Location.X, lalist[i][0].Location.Y + lalist[i][0].Size.Height + 5);
                //    ls = new Size(lalist[i][0].Size.Width, lalist[i][0].Size.Height);
                //    for (int j = 0; j < questionnum-3; j++)
                //    {
                //        Label ql = labelset(Ansnum == j ? ("(" + (j + 4) + ") " + Exercise_Word[i].Ans) : ("(" + (j + 4) + ") " + RandomWord(Exercise_Word[i].type)), new Point() { X = lp.X + (j == 0 ? 0 : (ls.Width + 5)), Y = lp.Y }, 12, FontStyle.Regular);
                //        pn.Controls.Add(ql);
                //        lalist[i].Add(ql);
                //        lp = new Point(ql.Location.X, ql.Location.Y);
                //        ls = new Size(ql.Size.Width, ql.Size.Height);
                //    }
                //}
                //else
                //{
                //    Point lp = new Point(ql2.Location.X, ql2.Location.Y + ql2.Size.Height + 10);
                //    Size ls = new Size(ql2.Size.Width, ql2.Size.Height);
                //    for (int j = 0; j < questionnum; j++)
                //    {
                //        Label ql = labelset(Ansnum == j ? ("(" + (j + 1) + ") " + Exercise_Word[i].Ans) : ("(" + (j + 1) + ") " + RandomWord(Exercise_Word[i].type)), new Point() { X = lp.X + (j == 0 ? 0 : (ls.Width + 5)), Y = lp.Y }, 12, FontStyle.Regular);
                //        pn.Controls.Add(ql);
                //        lalist[i].Add(ql);
                //        lp = new Point(ql.Location.X, ql.Location.Y);
                //        ls = new Size(ql.Size.Width, ql.Size.Height);
                //    }
                //}
                p = new Point(pn.Location.X, pn.Location.Y);
                s = new Size(pn.Size.Width, pn.Size.Height);
                this.Controls.Add(pn);
                if (this.HScroll == true) this.Size = new Size(this.HorizontalScroll.Maximum + 50, this.Size.Height);
                // if (pn.Location.X + pn.Size.Width > this.Size.Width) this.Size = new Size(pn.Location.X + pn.Size.Width + 100, this.Size.Height);
            }
        }
        Random rd = new Random();
        WordDi RandomWord(int type, string pos)
        {
            WordDi wd = new WordDi();
            int rdn = rd.Next(0, Word_Classify[pos].Count);
            if (type == 0)
            {
                return wd = new WordDi() { ID = Word_Classify[pos][rdn]["ID"].ToString(), Ans = Word_Classify[pos][rdn]["English"].ToString(), PartOfSpeech = Word_Classify[pos][rdn]["PartOfSpeech"].ToString(), type = type, Word = Word_Classify[pos][rdn]["Chinese"].ToString() };
            }
            else
            {
                return wd = new WordDi() { ID = Word_Classify[pos][rdn]["ID"].ToString(), Ans = Word_Classify[pos][rdn]["Chinese"].ToString(), PartOfSpeech = Word_Classify[pos][rdn]["PartOfSpeech"].ToString(), type = type, Word = Word_Classify[pos][rdn]["English"].ToString() };
            }
        }
        bool checkword(WordDi down, WordDi up, int type)
        {
            bool che = true;
            if (Word_Classify[up.PartOfSpeech].Count < 6) return true;
            if (type == 0)
            {
                List<string> upstring = up.Word.Split('、').ToList();
                List<string> downstring = down.Word.Split('、').ToList();
                foreach (var a in upstring)
                {
                    foreach (var aa in downstring)
                    {
                        if (a == aa) che = false;
                    }
                }
                return che;
            }
            else
            {
                List<string> upstring = up.Ans.Split('、').ToList();
                List<string> downstring = down.Ans.Split('、').ToList();
                foreach (var a in upstring)
                {
                    foreach (var aa in downstring)
                    {
                        if (a == aa) che = false;
                    }
                }
                return che;
            }
        }
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
        }
        List<int> check()
        {
            List<int> Wronglist = new List<int>();
            for (int i = 0; i < Wordnum; i++)
            {
                if (txlist[i].Text != (Ans[i] + 1).ToString())
                {
                    Wronglist.Add(i);
                    lalist[i][Ans[i]].BackColor = Color.LightPink;
                    txlist[i].ForeColor = Color.Red;
                }
                else
                {
                    lalist[i][Ans[i]].BackColor = Color.LightGreen;
                    txlist[i].ForeColor = Color.Green;
                }
            }
            return Wronglist;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void Exercise2_Load(object sender, EventArgs e)
        {
            this.ControlBox = false;
            // All_Word = Sqlclass.totable("select * from E2_Word where Chinese like '"+searchstring+"%' or English like '"+searchstring+"%'");
            create_controls();
            txlist[0].Focus();
        }

        private void Exercise2_Paint(object sender, PaintEventArgs e)
        {
        }

        private void Exercise2_Move(object sender, EventArgs e)
        {
        }

        private void Exercise2_Scroll(object sender, ScrollEventArgs e)
        {
        }
    }
}
