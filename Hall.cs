using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace WindowsFormsApplication2
{
    public partial class Hall : Form
    {
        public Hall()
        {
            InitializeComponent();
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void aboutMeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Designer().Show();
        }

        private void aboutWToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new About_Me().Show();
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (label1.Location.X < -210)
            {
                label1.Location = new Point(400, 45);
            }
            else
            {
                label1.Location = new Point(label1.Location.X - 1, 45);
            }
        }
        DataTable User = new DataTable();
        DataTable Unit = new DataTable();

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            User = Sqlclass.totable("select * from E2_User");
            comboBox2.Items.AddRange(User.Rows.OfType<DataRow>().ToList().Select(x => x["Name"]).ToArray());
            //if(comboBox2.Items.Count!=0)
            //comboBox2.SelectedIndex = 0;
        }
        string user_id = "";
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex == -1)
            {
                button1.Enabled = false;
                button2.Enabled = false;
                button3.Enabled = false;

                return;
            }
            else
            {
                button1.Enabled =true;
                button2.Enabled =true;
                button3.Enabled =true;

            }
            if (Interaction.InputBox("Password") != User.Rows[comboBox2.SelectedIndex]["Password"].ToString())
            {
                comboBox2.SelectedIndex = -1;
                comboBox1.Items.Clear();
                MessageBox.Show("Error");
                return;
            }

            comboBox1.Items.Clear();
            DataTable All_unit = new DataTable();
            All_unit = Sqlclass.totable("select * from E2_Unit");
            Unit = new DataTable();
            foreach (DataColumn dc in All_unit.Columns)
            {
                Unit.Columns.Add(dc.ColumnName);
            }
            foreach(DataRow dr in All_unit.Rows)
            {
                if(dr["User_ID"].ToString().Split('、').ToList().FindIndex(x=>x == User.Rows[comboBox2.SelectedIndex]["ID"].ToString())!= -1)
                {
                    Unit.ImportRow(dr);
                }
            }
           // Unit = Sqlclass.totable("select * from E2_Unit where User_ID = '" + User.Rows[comboBox2.SelectedIndex]["ID"] + "'");
            comboBox1.Items.Add("All Word");
            comboBox1.Items.AddRange(Unit.Rows.OfType<DataRow>().ToList().Select(x => x["Name"]).ToArray());
            comboBox1.SelectedIndex = 0;
            user_id = User.Rows[comboBox2.SelectedIndex]["ID"].ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Add_Unit adu = new Add_Unit(User,comboBox2.SelectedIndex);
            adu.ShowDialog();
            comboBox2_SelectedIndexChanged(null,null);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Add_User adus = new Add_User();
            adus.ShowDialog();
            Form1_Load(null, null);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex == -1) return;
            if(comboBox1.Text =="All Word")
            {
                string unit_S = "";
                if(Unit.Rows.Count == 0)
                {
                    MessageBox.Show("You do not have any Unit.");
                    return;
                }
                foreach(DataRow dr in Unit.Rows)
                {
                    unit_S += dr[0].ToString()+"," ;
                }
                unit_S = unit_S.Substring(0,unit_S.Length-1);
                Unit_Hall un = new Unit_Hall(open_unit_interface(unit_S, user_id), unit_S, comboBox1.Text,user_id);
                this.Hide();
                un.ShowDialog();
                this.Show();
            }
            else
            {
                string unit_string = Unit.Rows[(comboBox1.SelectedIndex - 1)]["ID"].ToString();
                Unit_Hall un = new Unit_Hall(open_unit_interface(unit_string, user_id), unit_string, comboBox1.Text, user_id);
                this.Hide();
                timer1.Enabled = false; 
                un.ShowDialog();
                timer1.Enabled = true;
                this.Show();
            }
        }

        public static DataTable open_unit_interface(string unitstring,string user_id)
        {
            DataTable All_Word = Sqlclass.totable("select * from E2_Word");
            List<string> unitindex = unitstring.Split(',').ToList();
            DataTable Word = new DataTable();
            foreach(DataColumn dc in All_Word.Columns)
            {
                Word.Columns.Add(dc.ColumnName);
            }
            foreach(DataRow dr in All_Word.Rows)
            {
                if(unitindex.Sum(x=> dr["Unit_ID"].ToString().Split(',').ToList().Count(z=>z==x)) != 0)
                {
                    Word.ImportRow(dr);
                }
            }
            Word.Columns.Remove("Unit_ID");
            return Word;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Mix_Unit mu = new Mix_Unit(Unit, user_id);
            timer1.Enabled = false;
            mu.ShowDialog();
            timer1.Enabled = true;
        }

        private void dictionarySearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                DictionarySearch ds = new DictionarySearch();
                ds.ShowDialog();
            });
        }
    }
}
