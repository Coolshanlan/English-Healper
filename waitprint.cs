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
    public partial class waitprint : Form
    {
        public waitprint()
        {
            InitializeComponent();
        }
        int a = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            a++;
            if(a == 5)
            {
                a = 0;
                label1.Text = "Please wait and do not touch";
            }
            else
            {
                label1.Text += " . ";
            }
            label1.Location = new Point(this.Size.Width/2-label1.Size.Width/2,label1.Location.Y);
        }
    }
}
