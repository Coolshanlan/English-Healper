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
    public partial class Revise_Word : Form
    {
        public Revise_Word(DataTable U_W, string u_i)
        {
            InitializeComponent();
            Unit_Word = U_W;
            Unit_ID = u_i;
        }
        string Unit_ID = "";
        DataTable Unit_Word = new DataTable();
        bool thisword = false;
        int index = -1;
        private void button1_Click(object sender, EventArgs e)
        {
            if (thisword)
            {
                if (MessageBox.Show("Check\r\n\r\nEnglish：" + Unit_Word.Rows[index]["English"].ToString() + " ---> "+textBox2.Text+ "\r\n\r\nPart of speech：" + Unit_Word.Rows[index]["PartOfSpeech"].ToString() + " ---> " + textBox4.Text + "\r\n\r\nChinese：" + Unit_Word.Rows[index]["Chinese"].ToString() + " ---> "+textBox3.Text+"", "", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {

                    DataTable Word = Sqlclass.totable("select * from E2_Word where English ='" + textBox2.Text + "' and PartOfSpeech ='" + textBox4.Text + "'");

                    if (Word.Rows.Count == 0)
                    {
                        try
                        {
                            Sqlclass.totable("Update E2_Word set English = '" + textBox2.Text + "',Chinese = '" + textBox3.Text + "',PartOfSpeech ='" + textBox4.Text + "' where ID = '" + Unit_Word.Rows[index]["ID"].ToString() + "'");
                            DataTable Wordlike = Sqlclass.totable("select * from E2_Word where English ='" + textBox2.Text + "'");
                            foreach (DataRow dr in Wordlike.Rows)
                            {
                                if (dr["Unit_ID"].ToString().Split(',').ToList().Count(x => x == Unit_ID.ToString()) == 0)
                                {
                                    Sqlclass.totable("Update E2_Word set Unit_ID = '" + dr["Unit_ID"].ToString() + "," + Unit_ID + "' where English='" + dr["English"].ToString() + "' and PartOfSpeech ='" + dr["PartOfSpeech"].ToString() + "'");
                                }
                            }
                        }
                        catch
                        {
                            Sqlclass.sqlcon.Close();
                            MessageBox.Show("Failed");
                            return;
                        }
                        MessageBox.Show("Revise sccessful.");
                        this.Close();
                        return;
                    }
                    else
                    {
                        if (Word.Rows[0]["Unit_ID"].ToString().Split(',').ToList().FindIndex(x => x == Unit_ID) != -1)
                        {
                            Sqlclass.totable("Update E2_Word set English = '" + textBox2.Text + "',Chinese = '" + textBox3.Text + "',PartOfSpeech ='" + textBox4.Text + "' where ID = '" + Unit_Word.Rows[index]["ID"].ToString() + "'");
                            MessageBox.Show("Revise sccessful");
                            this.Close();
                            return ;
                        }
                        List<string> chinese = Word.Rows[0]["Chinese"].ToString().Split('、').ToList();
                        List<string> new_c = textBox3.Text.Split('、').ToList();
                        string updatechinese = "";
                        foreach (var a in new_c)
                        {
                            if (chinese.Count(x => x == a) == 0) chinese.Add(a);
                        }

                        if (chinese.Count == 0) updatechinese = chinese[0];
                        else
                        {
                            foreach (var a in chinese)
                            {
                                updatechinese += a + "、";
                            }
                            updatechinese = updatechinese.Substring(0, updatechinese.Length - 1);
                        }

                        try
                        {
                            Sqlclass.totable("Update E2_Word set Unit_ID = '" + Word.Rows[0]["Unit_ID"].ToString() + "," + Unit_ID + "', Chinese = '" + updatechinese + "' where English='" + textBox2.Text + "'and PartOfSpeech ='" + textBox4.Text + "'");
                            DataTable Wordlike = Sqlclass.totable("select * from E2_Word where English ='" + textBox2.Text + "'");
                            foreach (DataRow dr in Wordlike.Rows)
                            {
                                if (dr["Unit_ID"].ToString().Split(',').ToList().Count(x => x == Unit_ID.ToString()) == 0)
                                {
                                    Sqlclass.totable("Update E2_Word set Unit_ID = '" + dr["Unit_ID"].ToString() + "," + Unit_ID + "' where English='" + dr["English"].ToString() + "' and PartOfSpeech ='" + dr["PartOfSpeech"].ToString() + "'");
                                }
                            }
                        }
                        catch
                        {
                            Sqlclass.sqlcon.Close();
                            MessageBox.Show("Failed");
                            return;
                        }
                        Sqlclass.totable("Delete E2_Word where ID = '" + Unit_Word.Rows[index]["ID"].ToString() + "'");
                        MessageBox.Show("Revise sccessful.");
                        this.Close();
                    }
                }
            }
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
           
           
        }

        private void textBox2_KeyUp(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    button1.PerformClick();
            //    return;
            //}
            //index = -1;
            //foreach (DataRow dr in Unit_Word.Rows)
            //{
            //    index++;
            //    if (dr["English"].ToString() == textBox2.Text)
            //    {
            //        textBox1.Text = dr["ID"].ToString();
            //        textBox3.Text = dr["Chinese"].ToString();
            //        thisword = true;
            //        return;
            //    }
            //}
            //index = -1;
            //thisword = false;
            //textBox1.Text = "";
            //textBox3.Text = "";
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.PerformClick();
                return;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            int aa;
            if (!int.TryParse(textBox1.Text, out aa))
            {
                index = -1;
                thisword = false;
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                return;
            }
            index = int.Parse(textBox1.Text) - 1;
            if (index >= 0 && index < Unit_Word.Rows.Count)
            {
                textBox2.Text = Unit_Word.Rows[index]["English"].ToString();
                textBox3.Text = Unit_Word.Rows[index]["Chinese"].ToString();
                textBox4.Text = Unit_Word.Rows[index]["PartOfSpeech"].ToString();
                thisword = true;
                return;
            }
            else
            {
                index = -1;
                thisword = false;
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                return;
            }
        }
    }
}
