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
    public partial class Add_Word : Form
    {
        public Add_Word(DataTable uw,string u_i)
        {
            InitializeComponent();
            Unit_Word = uw;
            Unit_ID = u_i;
        }
        DataTable Unit_Word = new DataTable();
        string Unit_ID = "-1";
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "") return;
            textBox1.Text = textBox1.Text.Replace("'", "''");
            DataTable Word = Sqlclass.totable("select * from E2_Word where English ='" + textBox1.Text + "' and PartOfSpeech ='"+textBox3.Text+"'");
          
            if (Word.Rows.Count == 0)
            {
                try
                {
                    Sqlclass.totable("Insert into E2_Word (Unit_ID,English,Chinese,PartOfSpeech,Wrong) Values('" + Unit_ID + "','" + textBox1.Text + "','" + textBox2.Text + "','"+textBox3.Text+"','2')");
                    DataTable Wordlike = Sqlclass.totable("select * from E2_Word where English ='" + textBox1.Text + "'");
                    foreach (DataRow dr in Wordlike.Rows)
                    {
                        if(dr["Unit_ID"].ToString().Split(',').ToList().Count(x=>x==Unit_ID.ToString()) == 0)
                        {
                            Sqlclass.totable("Update E2_Word set Unit_ID = '" + dr["Unit_ID"].ToString() + "," + Unit_ID  + "', Wrong ='2'  where English='" + dr["English"].ToString() + "' and PartOfSpeech ='" + dr["PartOfSpeech"].ToString() + "'");
                        }
                    }
                }
                catch
                {
                    Sqlclass.sqlcon.Close();
                    MessageBox.Show("Add failed");
                    return;
                }
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox1.Focus();
                MessageBox.Show("Add sccessful");
            }
            else
            {
                bool have = false;
                if (Word.Rows[0]["Unit_ID"].ToString().Split(',').ToList().FindIndex(x => x == Unit_ID) != -1)
                {
                   if( MessageBox.Show("Word already exists \r\n\r\n Do you want to revise these word information?\r\n\r\n\r\n English : "+ Word.Rows[0]["English"] + "\r\n\r\n Chinese :  " + Word.Rows[0]["Chinese"] + "", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        have = true;
                    }
                    else
                    {
                        MessageBox.Show("Add failed");
                        return;
                    }
                }
                List<string> chinese = Word.Rows[0]["Chinese"].ToString().Split('、').ToList();
                List<string> new_c = textBox2.Text.Split('、').ToList();
                string updatechinese = "";
                foreach(var a in new_c)
                {
                    if (chinese.Count(x => x == a) == 0) chinese.Add(a);
                }

                if (chinese.Count == 0) updatechinese = chinese[0];
                else
                {
                    foreach(var a in chinese)
                    {
                        updatechinese += a+"、";
                    }
                    updatechinese = updatechinese.Substring(0, updatechinese.Length-1);
                }

                try
                {
                    if(!have)
                    Sqlclass.totable("Update E2_Word set Unit_ID = '" + Word.Rows[0]["Unit_ID"].ToString() + "," + Unit_ID + "', Chinese = '" + updatechinese + "', Wrong ='2' where English='" + textBox1.Text+"'and PartOfSpeech ='"+textBox3.Text+"'");
                    else
                        Sqlclass.totable("Update E2_Word set  Chinese = '" + updatechinese + "' , Wrong ='2' where English='" + textBox1.Text + "'and PartOfSpeech ='" + textBox3.Text + "'");
                    DataTable Wordlike = Sqlclass.totable("select * from E2_Word where English ='" + textBox1.Text + "'");
                    foreach (DataRow dr in Wordlike.Rows)
                    {
                        if (dr["Unit_ID"].ToString().Split(',').ToList().Count(x => x == Unit_ID.ToString()) == 0)
                        {
                            Sqlclass.totable("Update E2_Word set Unit_ID = '" + dr["Unit_ID"].ToString() + "," + Unit_ID + "' , Wrong = '2' where English='" + dr["English"].ToString() + "' and PartOfSpeech ='" + dr["PartOfSpeech"].ToString() + "'");
                        }
                    }
                }
                catch
                {
                    Sqlclass.sqlcon.Close();
                    MessageBox.Show("Add failed");
                    return;
                }
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox1.Focus();
                MessageBox.Show("Add sccessful");
            }
            
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) button1.PerformClick();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
        }
    }
}
