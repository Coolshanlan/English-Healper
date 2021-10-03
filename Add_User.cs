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
    public partial class Add_User : Form
    {
        public Add_User()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("Error");
                return;
            }
            if(Sqlclass.totable("select count(*) from E2_User where Name ='"+textBox1.Text+"'").Rows[0][0].ToString() == "0")
            {
                Sqlclass.totable("Insert into E2_User (Name,Date,Password) Values('"+textBox1.Text+"','"+DateTime.Now.Year.ToString("0000")+DateTime.Now.Month.ToString("00")+DateTime.Now.Day.ToString("00")+"','"+textBox2.Text+"')");
                MessageBox.Show("Add successfully.");
                this.Close();
            }
            else
            {
                MessageBox.Show("The User Name already exist.");
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                button1.PerformClick();
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.PerformClick();
            }
        }
    }
}
