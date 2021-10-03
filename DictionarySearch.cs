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

namespace WindowsFormsApplication2
{
    public partial class DictionarySearch : Form
    {
        public DictionarySearch()
        {
            InitializeComponent();
        }

        private void DictionarySearch_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            this.TopMost = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                string ss = textBox1.Text.Replace("'", "-");
                ss = ss.Replace(" ", "-");
                if(ss.Split('-').Count() > 1)
                {
                    Process.Start("https://dictionary.cambridge.org/zht/%E8%A9%9E%E5%85%B8/%E8%8B%B1%E8%AA%9E-%E6%BC%A2%E8%AA%9E-%E7%B9%81%E9%AB%94/" + ss+ "");
                }
                else
                Process.Start("https://dictionary.cambridge.org/zht/%E8%A9%9E%E5%85%B8/%E8%8B%B1%E8%AA%9E-%E6%BC%A2%E8%AA%9E-%E7%B9%81%E9%AB%94/" + textBox1.Text + "");
            }
            else if (comboBox1.SelectedIndex == 1) Process.Start("https://translate.google.com.tw/?hl=zh-TW#en/zh-TW/" + textBox1.Text + "");
            else if (comboBox1.SelectedIndex == 2)
            {
                string ss = textBox1.Text.Replace(" ", "+");
                Process.Start("https://hk.dictionary.yahoo.com/dictionary?p=" + ss + "");
            }
            else if (comboBox1.SelectedIndex == 3) Process.Start("https://tw.voicetube.com/definition/" + textBox1.Text + "");
            else Process.Start("http://cdict.info/query/" + textBox1.Text + "");
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                button1.PerformClick();
            }
        }
    }
}
