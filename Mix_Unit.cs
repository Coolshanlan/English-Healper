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
    public partial class Mix_Unit : Form
    {
        string user_id = ""; 
        public Mix_Unit(DataTable ut,string u_id)
        {
            InitializeComponent();
            Unit = ut;
            user_id = u_id;
            var a = Unit.Rows.OfType<DataRow>().Select(x => x.ItemArray.ToList()).ToList() ;
            foreach(var aa in a)
            {
                unit_dic.Add(aa[2].ToString(), aa[0].ToString());
            }
        }
        Dictionary<string, string> unit_dic = new Dictionary<string, string>();
        DataTable Unit = new DataTable();
        private void Mix_Unit_Load(object sender, EventArgs e)
        {
            for(int i = 0; i < Unit.Rows.Count; i++)
            {
                CheckBox cb = new CheckBox();
                cb.Location = new Point(12,12+30*i);
                cb.Text = Unit.Rows[i]["Name"].ToString();
                cb.AutoSize = true;
                cb.Checked = false;
                cb.Font = new System.Drawing.Font("Georgia", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.Controls.Add(cb);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string unit_string = "";
            string unit_title = "(";
            foreach(Control c in this.Controls)
            {
                if(c.GetType() == typeof(CheckBox) && ((CheckBox)c).Checked == true)
                {
                    unit_string += unit_dic[c.Text] + ",";
                    unit_title += c.Text+",";
                }
            }
            if (unit_string == "")
            {
                button2.PerformClick();
                return;
            }
            unit_string = unit_string.Substring(0,unit_string.Length-1);
            unit_title = unit_title.Substring(0, unit_title.Length - 1);
            unit_title += ")";
            Unit_Hall un = new Unit_Hall(Hall.open_unit_interface(unit_string,user_id), unit_string, unit_title, user_id);
            this.Hide();
            un.ShowDialog();
            this.Close();
        }
    }
}
