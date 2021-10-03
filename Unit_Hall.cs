using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using Aspose.Words;
using Microsoft.Office.Core;
using Microsoft.Office.Interop;
using System.Reflection;
using Microsoft.VisualBasic;
using System.Threading;

namespace WindowsFormsApplication2
{
    public partial class Unit_Hall : Form
    {
        DataTable Unit_Word = new DataTable();
        string Unit_String = "";
        string Unit_Title = "";
        string user_id = "";
        public Unit_Hall(DataTable w , string us,string ut,string u_id)
        {
            InitializeComponent();
            Unit_Word = w;
            searchdt = w;
            Unit_String = us;
            Unit_Title = ut;
            user_id = u_id;
           // Unit_Word.Columns.Add("Index");
            DataColumn dc = new DataColumn("Index");
            dc.DataType = typeof(int);
            Unit_Word.Columns.Add(dc);
            for (int i=1;i< Unit_Word.Rows.Count + 1; i++)
            {
                Unit_Word.Rows[i-1]["Index"] = i;
            }
        }

        private void aboutWToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new About_Me().Show();
        }
        
        private void aboutMeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Designer().Show();
        }

        private void googleTranslateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("翻譯功能需要網路" + "\r\n" + "" + "\r\n" + "有網路 -> 是" + "\r\n" + "" + "\r\n" + "沒網路 -> 取消", "Google Translate", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                new Google_Translate().Show();
            }
            else
            {
                return;
            }
        }

        private void Unit_Hall_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox5.SelectedIndex = 0;
            comboBox6.SelectedIndex = 0;
            if (Unit_String.Split(',').ToList().Count > 1 || Unit_Title == "All Word")
            {
                button1.Enabled = false;
                button2.Enabled = false;
                button3.Enabled = false;
                batchInsertToolStripMenuItem.Enabled = false;
                label6.Text = Unit_Title;
                label1.Text = "Mix Unit";
            }
            dataGridView1.DataSource = Unit_Word;
            if(label1.Text == "label1")
            label1.Text = Unit_Title;
            label1.Location = new Point((this.Size.Width-(panel1.Location.X+panel1.Size.Width))/2-(label1.Size.Width/2)+ (panel1.Location.X + panel1.Size.Width),label1.Location.Y);
            label6.Location = new Point((this.Size.Width - (panel1.Location.X + panel1.Size.Width)) / 2 - (label6.Size.Width / 2) + (panel1.Location.X + panel1.Size.Width), label6.Location.Y);
            textBox2.Select();
            dataGridView1.Columns[5].AutoSizeMode =  DataGridViewAutoSizeColumnMode.AllCells ;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[2].Width = 150;
           //dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
           //dataGridView1.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        }
        void Dvup()
        {
            if (dataGridView1.Columns[0].Name == "Index")
            {

            }
            else
            {
                dataGridView1.Columns["ID"].Visible = false;
                DataGridViewColumn dc = new DataGridViewColumn();
                dc.Name = "Index";
                dc.DataPropertyName = "Index";
                dc.CellTemplate = new DataGridViewTextBoxCell();
                dataGridView1.Columns.Insert(0, dc);
            }

            for(int i = 1; i < dataGridView1.Rows.Count+1; i++)
            {
                dataGridView1.Rows[i - 1].Cells[0].Value = i;
            }

        }
        Random rd = new Random();
        List<string> Word_Pool = new List<string>();
        List<string> Exercise_Word = new List<string>();
        List<string> Word_Ans = new List<string>();
        List<string> Test_ID = new List<string>();
        bool checkok(string t1,string t2)
        {
            int aa = 0;
             if (!int.TryParse(t2, out aa) || int.Parse(t2) > dataGridView1.Rows.Count || int.Parse(t2) <= 0) return false;
            if (t1 == "") return true;
                List<string> num = t1.Split('~').ToList();
                if (!int.TryParse(t2, out aa)) return false;
                foreach (var a in num)
                {
                    if (!int.TryParse(a.ToString(), out aa)) return false;
                }
                if (num.Count != 2 || int.Parse(num[0]) > int.Parse(num[1]) || int.Parse(t2) > (int.Parse(num[1])-int.Parse(num[0])+1)) return false;
            return true;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if (!checkok(textBox2.Text, textBox4.Text))
            {
                MessageBox.Show("Insert error");
                return;
            }
            if(comboBox5.SelectedIndex == 0)
            {
                this.Hide();
                Exercise es = new Exercise(Update_Test(comboBox1.SelectedIndex));
                es.ShowDialog();
                this.Show();
            }
            else
            {
                this.Hide();
                Exercise2 es = new Exercise2(Update_Test(comboBox1.SelectedIndex),int.Parse(numericUpDown1.Value.ToString()),textBox1.Text,checkBox3.Checked == true?Sqlclass.totable("select * from E2_Word"): testWord);
                es.ShowDialog();
                this.Show();
            }
            Update_Data();
        }
        DataTable testWord = new DataTable();
        List<WordDi> Update_Test(int type)
        {
            List<WordDi> WordList = new List<WordDi>();
            List<WordDi> Exercise_Word = new List<WordDi>();
            Word_Pool = new List<string>();
             testWord = new DataTable();
            int number = int.Parse(textBox4.Text);
                testWord = SearchResult();
            testWord = Range(textBox2.Text,testWord);

            for(int i = 0; i < testWord.Rows.Count; i++)
            {
                int wrongscroe = int.Parse(testWord.Rows[i]["Wrong"].ToString());
                for (int j=0;j< wrongscroe + 1; j++)
                {
                    Word_Pool.Add(i.ToString());
                }
            }


            for (int i = 0; i < number; i++)
            {
                int rdn = rd.Next(Word_Pool.Count);
                int wordindex = int.Parse(Word_Pool[rdn]);
                if (type == 0)
                {
                    Exercise_Word.Add(new WordDi() { ID = testWord.Rows[wordindex]["ID"].ToString(),Word = testWord.Rows[wordindex]["Chinese"].ToString(),Ans = testWord.Rows[wordindex]["English"].ToString(),PartOfSpeech = testWord.Rows[wordindex]["PartOfSpeech"].ToString(),Wrong= int.Parse(testWord.Rows[wordindex]["Wrong"].ToString()),type =type });
                }
                else if(type == 1)
                {
                    Exercise_Word.Add(new WordDi() { ID = testWord.Rows[wordindex]["ID"].ToString(), Word = testWord.Rows[wordindex]["English"].ToString(), Ans = testWord.Rows[wordindex]["Chinese"].ToString(), PartOfSpeech = testWord.Rows[wordindex]["PartOfSpeech"].ToString(), Wrong = int.Parse(testWord.Rows[wordindex]["Wrong"].ToString()), type = type });
                }
                else
                {
                    int rdn2 = rd.Next(0, 2);
                    if(rdn2 == 0) Exercise_Word.Add(new WordDi() { ID = testWord.Rows[wordindex]["ID"].ToString(), Word = testWord.Rows[wordindex]["Chinese"].ToString(), Ans = testWord.Rows[wordindex]["English"].ToString(), PartOfSpeech = testWord.Rows[wordindex]["PartOfSpeech"].ToString(), Wrong = int.Parse(testWord.Rows[wordindex]["Wrong"].ToString()), type = rdn2 });
                    else Exercise_Word.Add(new WordDi() { ID = testWord.Rows[wordindex]["ID"].ToString(), Word = testWord.Rows[wordindex]["English"].ToString(), Ans = testWord.Rows[wordindex]["Chinese"].ToString(), PartOfSpeech = testWord.Rows[wordindex]["PartOfSpeech"].ToString(), Wrong = int.Parse(testWord.Rows[wordindex]["Wrong"].ToString()), type = rdn2 });
                }
                Word_Pool.RemoveAll(x => x == Word_Pool[rdn]);
            }

            return Exercise_Word;


        }
        void Update_Data()
        {
            Unit_Word = Hall.open_unit_interface(Unit_String, user_id);
            DataColumn dc = new DataColumn("Index");
            dc.DataType = typeof(int);
            Unit_Word.Columns.Add(dc);
            for (int i = 1; i < Unit_Word.Rows.Count + 1; i++)
            {
                Unit_Word.Rows[i-1]["Index"] = i;
            }
            dataGridView1.DataSource = Unit_Word;
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                button4.PerformClick();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            (new Add_Word(Unit_Word, Unit_String)).ShowDialog();
            Update_Data();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            (new Remove_Word(Unit_Word,Unit_String)).ShowDialog();
            Update_Data();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            (new Revise_Word(Unit_Word, Unit_String)).ShowDialog();
            Update_Data();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Coming soon...");
        }

        private void button7_Click(object sender, EventArgs e)
        {
           
        }

        private void button5_Click(object sender, EventArgs e)
        {
            searchdt = Unit_Word;
            dataGridView1.DataSource = searchdt;
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {

        }
        DataTable searchdt =new DataTable();

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
             searchdt = new DataTable();
            foreach (DataColumn dc in Unit_Word.Columns)
            {
                searchdt.Columns.Add(dc.ColumnName);
            }
            foreach (DataRow dr in Unit_Word.Rows)
            {
                if (textBox1.Text.Length <= dr["English"].ToString().Length && (dr["English"].ToString().Substring(0, textBox1.Text.Length).ToUpper() == textBox1.Text.ToUpper() && ( dr["Chinese"].ToString().Contains(textBox5.Text))))
                {
                    searchdt.ImportRow(dr);
                }
            }

            dataGridView1.DataSource = searchdt;
        }

        private void howTOUseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new Howtouse()).Show();
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            textBox2.Text = Unit_Word.Rows.Count.ToString();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            label10.Text = (e.RowIndex + 1).ToString() + " / " + dataGridView1.Rows.Count;
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            if (e.ColumnIndex == 1)
            {
                //Process.Start("https://translate.google.com.tw/?hl=zh-TW#en/zh-TW/"+dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value+"");
                //Process.Start("http://cdict.info/query/"+dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                Process.Start("https://dictionary.cambridge.org/zht/%E8%A9%9E%E5%85%B8/%E8%8B%B1%E8%AA%9E-%E6%BC%A2%E8%AA%9E-%E7%B9%81%E9%AB%94/"+ dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value + "");
            }
        }
        DataTable SearchResult()
        {
            DataTable dt = new DataTable();
            foreach(DataGridViewColumn dc in dataGridView1.Columns)
            {
                dt.Columns.Add(dc.Name);
            }
            foreach (DataGridViewRow dr in dataGridView1.Rows)
            {
                DataRow drr = dt.NewRow();
                drr["ID"] = dr.Cells["ID"].Value;
                drr["Index"] = dr.Cells["Index"].Value;
                drr["English"] = dr.Cells["English"].Value;
                drr["PartOfSpeech"] = dr.Cells["PartOfSpeech"].Value;
                drr["Chinese"] = dr.Cells["Chinese"].Value;
                drr["Wrong"] = dr.Cells["Wrong"].Value;
                dt.Rows.Add(drr);
            }
            return dt;
        }
        DataTable Range(string rangestring , DataTable Source)
        {
            if (rangestring.Trim() == "") return Source;
            DataTable dt = new DataTable();
            int[] range;
            try
            {
                range = rangestring.Split('~').ToList().ConvertAll(Convert.ToInt32).ToArray();
            }
            catch
            {
                return null;
            }
            if (range[0] < 1 || range[1] < 1 || range[0] > Source.Rows.Count || range[1] > Source.Rows.Count) return null;
            foreach (DataGridViewColumn dc in dataGridView1.Columns)
            {
                dt.Columns.Add(dc.Name);
            }
            for (int i = range[0]; i < range[1] + 1; i++)
            {
                DataRow dr = dt.NewRow();
                dr["ID"] = Source.Rows[i-1]["ID"];
                dr["Index"] = Source.Rows[i - 1]["Index"];
                dr["English"] = Source.Rows[i - 1]["English"];
                dr["PartOfSpeech"] = Source.Rows[i - 1]["PartOfSpeech"];
                dr["Chinese"] = Source.Rows[i - 1]["Chinese"];
                dr["Wrong"] = Source.Rows[i - 1]["Wrong"];
                dt.Rows.Add(dr);
            }
            return dt;
        }
        private void button8_Click(object sender, EventArgs e)
        {

            DataTable protable = new DataTable();

                protable = new DataTable();
                foreach (DataGridViewColumn dc in dataGridView1.Columns)
                {
                    protable.Columns.Add(dc.HeaderText);
                }
                foreach (DataGridViewRow dr in dataGridView1.Rows)
                {
                    DataRow drr = protable.NewRow();
                    drr["ID"] = dr.Cells["ID"].Value.ToString();
                    drr["English"] = dr.Cells["English"].Value.ToString();
                    drr["Chinese"] = dr.Cells["Chinese"].Value.ToString();
                    drr["PartOfSpeech"] = dr.Cells["PartOfSpeech"].Value.ToString();
                    drr["Index"] = dr.Cells["Index"].Value.ToString();
                    protable.Rows.Add(drr);
                }
            

            protable = Range(textBox3.Text, protable);

            doneprint = false;
            wait = new waitprint();
            wait.Show();
            Task.Run(() =>
            {
                while (!doneprint) Thread.Sleep(100);
                wait.Close();
            });
            Task.Run(() =>
            { 

                Word.Application app = new Word.Application();

                File.Delete(System.Windows.Forms.Application.StartupPath + "\\WordModel\\bookmodel.doc");
                if(comboBox6.SelectedIndex == 1)
                File.Copy(System.Windows.Forms.Application.StartupPath + "\\WordModel\\bookmodel3.doc", System.Windows.Forms.Application.StartupPath + "\\WordModel\\bookmodel.doc");
                else
                File.Copy(System.Windows.Forms.Application.StartupPath + "\\WordModel\\bookmodel2.doc", System.Windows.Forms.Application.StartupPath + "\\WordModel\\bookmodel.doc");
                Word.Document document = app.Documents.Open(Path.GetFullPath(System.Windows.Forms.Application.StartupPath + "\\WordModel\\bookmodel.doc"));

                document.Select();

                document.Paragraphs.Last.Range.Select();
                if (comboBox6.SelectedIndex == 1)
                {
                    app.Selection.InsertFile(Path.GetFullPath(System.Windows.Forms.Application.StartupPath + "\\WordModel\\model3.doc"));
                    for (int i = 0; i < protable.Rows.Count; i += document.Content.Tables[1].Columns.Count)
                    {
                        for (int j = 1; j < document.Content.Tables[1].Columns.Count + 1; j++)
                        {
                            if (protable.Rows.Count > i + j - 1)
                            {
                                document.Tables[1].Cell(i + 1, j).Range.Text = protable.Rows[i + j - 1]["English"].ToString() + "(" + protable.Rows[i + j - 1]["PartOfSpeech"].ToString() + ")";
                            }
                            if (protable.Rows.Count <= i + j)
                            {
                                i = -1;
                                break;
                            }
                        }
                        if (i != -1)
                            document.Content.Tables[1].Rows.Add();
                        else
                            break;
                    }
                }
                else
                {
                    app.Selection.InsertFile(Path.GetFullPath(System.Windows.Forms.Application.StartupPath + "\\WordModel\\model2.doc"));
                    for (int i = 0; i < protable.Rows.Count; i += document.Content.Tables[1].Columns.Count/2)
                    {
                        for (int j = 1; j < document.Content.Tables[1].Columns.Count/2 + 1; j++)
                        {
                            if (protable.Rows.Count > i + j-1)
                            {
                                document.Tables[1].Cell(i + 1, j).Range.Text = protable.Rows[i + j - 1]["English"].ToString() + "(" + protable.Rows[i + j - 1]["PartOfSpeech"].ToString() + ")";
                                document.Tables[1].Cell(i + 1, j+ document.Content.Tables[1].Columns.Count / 2).Range.Text = protable.Rows[i + j - 1]["Chinese"].ToString();
                            }
                            if (protable.Rows.Count <= i + j)
                            {
                                i = -1;
                                break;
                            }
                        }
                        if (i != -1)
                            document.Content.Tables[1].Rows.Add();
                        else
                            break;
                    }
                }

                string bookPath = @"";
                string booktitle = Unit_Title.Replace(',', '_');
                if (comboBox2.SelectedIndex == 0)
                {
                    bookPath = Path.GetFullPath(System.Windows.Forms.Application.StartupPath + "\\WordBook\\" + booktitle + ".pdf");
                    document.SaveAs(Path.GetFullPath(System.Windows.Forms.Application.StartupPath + "\\WordBook\\" + booktitle + ".pdf"), (Word.WdSaveFormat.wdFormatPDF));
                }
                else
                {
                    bookPath = Path.GetFullPath(System.Windows.Forms.Application.StartupPath + "\\WordBook\\" + booktitle + ".doc");
                    document.SaveAs2(Path.GetFullPath(System.Windows.Forms.Application.StartupPath + "\\WordBook\\" + booktitle + ".doc"), Word.WdSaveFormat.wdFormatDocument97);
                }
                if (checkBox1.Checked)
                    document.PrintOut();
                document.Close();
                app.Quit();
                doneprint = true;
                MessageBox.Show(bookPath);
                Process.Start(bookPath);
            });

        }

        private void dataGridView1_DataSourceChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dr in dataGridView1.Rows)
            {
                if (int.Parse(dr.Cells["Wrong"].Value.ToString()) > 1)
                {
                    if (int.Parse(dr.Cells["Wrong"].Value.ToString()) > 2)
                    {
                        dr.DefaultCellStyle.BackColor = Color.LightPink;
                    }
                    else
                    {
                        dr.DefaultCellStyle.BackColor = Color.FromArgb(255,255,255,120);
                    }
                }
            }
            dataGridView1.Columns["ID"].Visible = false;
            dataGridView1.Columns["Index"].DisplayIndex = 0;
            label10.Text = 1 + " / " + dataGridView1.Rows.Count;
            //Dvup();
        }

        private void dataGridView1_Sorted(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dr in dataGridView1.Rows)
            {
                if (int.Parse(dr.Cells["Wrong"].Value.ToString()) > 1)
                {
                    if (int.Parse(dr.Cells["Wrong"].Value.ToString()) > 2)
                    {
                        dr.DefaultCellStyle.BackColor = Color.LightPink;
                    }
                    else
                    {
                        dr.DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255, 120);
                    }
                }
                //Dvup();
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (comboBox3.SelectedIndex == 2)
            //{
            //    textBox3.Enabled = true;
            //    label2.Enabled = true;
            //}
            //else
            //{
            //    textBox3.Enabled = false;
            //    label2.Enabled = false;
            //}
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox5.SelectedIndex == 1)
            {
                label3.Enabled = true;
                numericUpDown1.Enabled = true;
                checkBox3.Enabled = true;
            }
            else
            {
                label3.Enabled = false;
                numericUpDown1.Enabled = false;
                checkBox3.Enabled = false;
            }
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (comboBox4.SelectedIndex == 2)
            //{
            //    label4.Visible = true;
            //    button7.Enabled = true;
            //    textBox2.Enabled = true;
            //    button11.Enabled = false;
            //    textBox4.Text = "";
            //}
            //else
            //{
            //    button7.Enabled = false;
            //    textBox2.Enabled = false;
            //    label4.Visible = false;
            //    button11.Enabled = true;
            //}
        }

        private void button10_Click(object sender, EventArgs e)
        {
            textBox3.Text = Unit_Word.Rows.Count.ToString();
        }

        private void button11_Click(object sender, EventArgs e)
        {

                textBox4.Text = dataGridView1.Rows.Count.ToString();
        }

        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) button4.PerformClick();
        }

        private void batchInsertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new BatchInsert(Unit_String)).ShowDialog();
            Update_Data();
        }

        private void sqlToolStripMenuItem_Click(object sender, EventArgs e)
        {

            string sqlcomment = "" ;
            sqlcomment = "select ID,English,Chinese,PartOfSpeech,Wrong from E2_Word where "+ Interaction.InputBox("Comment");
            try
            {
                searchdt = Sqlclass.totable(sqlcomment);
                DataColumn dc = new DataColumn("Index");
                dc.DataType = typeof(int);
                searchdt.Columns.Add(dc);
                for (int i = 1; i < searchdt.Rows.Count + 1; i++)
                {
                    searchdt.Rows[i - 1]["Index"] = i;
                }
                dataGridView1.DataSource = searchdt;
            }
            catch
            {
                Sqlclass.sqlcon.Close();
                MessageBox.Show("Error");
            }
        }
        bool doneprint = false;
        waitprint wait = new waitprint();
        private void specialPrintToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void dictionarySeacchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                DictionarySearch ds = new DictionarySearch();
                ds.ShowDialog();
            });
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
 
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            
        }
    }
}
public class WordDi
{
    public string ID;
    public string Word;
    public string Ans;
    public int Wrong;
    public string PartOfSpeech;
    public int type;
}
