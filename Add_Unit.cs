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
    public partial class Add_Unit : Form
    {
        DataTable User = new DataTable();
        int user_index = -1;
        public Add_Unit(DataTable U,int si)
        {
            InitializeComponent();
            User = U;
            user_index = si;
        }

        private void Add_Unit_Load(object sender, EventArgs e)
        {
            comboBox1.Items.AddRange(User.Rows.OfType<DataRow>().Select(x=>x["Name"]).ToArray());
            if (comboBox1.Items.Count != 0) comboBox1.SelectedIndex = user_index;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int indexu = int.Parse(Sqlclass.totable("select count(*) from E2_Unit").Rows[0][0].ToString());
            if(Sqlclass.totable("select * from E2_Unit where Name = '"+textBox1.Text+"' and User_ID = '"+User.Rows[comboBox1.SelectedIndex]["ID"]+"'").Rows.Count != 0)
            {
                MessageBox.Show("Name already exists ");
            }
            else
            {
                Sqlclass.totable("Insert into E2_Unit (ID,User_ID,Name) Values('"+indexu+"','"+ User.Rows[comboBox1.SelectedIndex]["ID"] + "','"+textBox1.Text+"')");
                MessageBox.Show("Add successfully.");
                this.Close();
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.PerformClick();
            }
        }
    }
}
