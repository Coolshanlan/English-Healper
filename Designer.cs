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
    public partial class Designer : Form
    {
        public Designer()
        {
            InitializeComponent();
            label4.Text = "事跡：跟某胖胖打賭如果英文考"+"\r\n"+ "　　　80分以上的話，他就要在" + "\r\n" + "　　　班上宣傳我的英文好棒棒" + "\r\n" + "　　　式的有用且有意義的超棒" + "\r\n" + "　　　程式";
            label6.Text = "作品：酷炫猜猜拳"+"\r\n"+"　　　英文好棒棒幫手"+ "\r\n　　　射擊小飛機\r\n　　　類雞雞網路\r\n　　　酷炫搶包包\r\n　　　公假單產生器";
        }

        private void Designer_MouseUp(object sender, MouseEventArgs e)
        {
            this.Close();
        }

        private void Designer_KeyDown(object sender, KeyEventArgs e)
        {
            this.Close();
        }

        private void Designer_Load(object sender, EventArgs e)
        {

        }

        private void Designer_MouseClick(object sender, MouseEventArgs e)
        {
            this.Close();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
