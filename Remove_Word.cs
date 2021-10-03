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
    public partial class Remove_Word : Form
    {
        public Remove_Word(DataTable U_W,string u_i)
        {
            InitializeComponent();
            Unit_Word = U_W;
            unit_id = u_i;
        }
        string unit_id = "";
        DataTable Unit_Word = new DataTable();
        bool thisword = false;
        int index = -1;
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
            index = int.Parse(textBox1.Text)-1;
            if(index >=0 && index < Unit_Word.Rows.Count)
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
                return;
            }
            //foreach(DataRow dr in Unit_Word.Rows)
            //{
            //    index++;
            //    if (dr["ID"].ToString() == textBox1.Text)
            //    {
            //        textBox2.Text = dr["English"].ToString();
            //        textBox3.Text = dr["Chinese"].ToString();
            //        textBox4.Text = dr["PartOfSpeech"].ToString();
            //        thisword = true;
            //        return;
            //    }
            //}
            //index = -1;
            //thisword = false;
            //textBox2.Text = "";
            //textBox3.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (thisword)
            {
                if(MessageBox.Show("Check\r\n\r\nEnglish："+ Unit_Word.Rows[index]["English"].ToString() + "\r\n\r\nPart of speech：" + Unit_Word.Rows[index]["PartOfSpeech"].ToString() +"\r\n\r\nChinese："+ Unit_Word.Rows[index]["Chinese"].ToString() + "","", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    DataTable dt = Sqlclass.totable("select * from E2_Word where ID = '"+ Unit_Word.Rows[index]["ID"].ToString() + "'");
                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("Word is not exists");
                        return;
                    }
                    List<string> unitid_list = dt.Rows[0]["Unit_ID"].ToString().Split(',').ToList();

                    if(unitid_list.Count == 1)
                    {
                        Sqlclass.totable("Delete from E2_Word where ID = '" + Unit_Word.Rows[index]["ID"].ToString() + "'");
                    }
                    else
                    {
                        string unitstring = "";
                        foreach(var a in unitid_list)
                        {
                            if (a != unit_id) unitstring += a+",";
                        }
                        unitstring = unitstring.Substring(0,unitstring.Length-1);
                        Sqlclass.totable("Update E2_Word set Unit_ID='"+ unitstring + "' where ID='"+ Unit_Word.Rows[index]["ID"].ToString() + "'");
                    }
                    MessageBox.Show("Remove sccessful.");
                    this.Close();
                }
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) button1.PerformClick();
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void Remove_Word_Load(object sender, EventArgs e)
        {

        }
    }
}
