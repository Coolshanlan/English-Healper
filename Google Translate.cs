using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;

namespace WindowsFormsApplication2
{
    public partial class Google_Translate : Form
    {
        public Google_Translate()
        {
            InitializeComponent();
            MessageBox.Show("目前只中翻英。");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string language;
            if (comboBox1.Text == "中文") language = "zh-TW | en";
            else language = "en | zh-TW";
            textBox2.Text = TranslateText(textBox1.Text, language);
        }




        /// <summary>

        /// Translate Text using Google Translate API’s

        /// Google URL – http://www.google.com/translate_t?hl=en&ie=UTF8&text={0}&langpair={1}

        /// </summary>

        /// <param name=”input”>Input string</param>

        /// <param name=”languagePair”>2 letter Language Pair, delimited by “|”.

        /// E.g. “ar|en” language pair means to translate from Arabic to English</param>

        /// <returns>Translated to String</returns>

        public string TranslateText(string input, string languagePair)
        {
            try
            {
                string url = "http://www.google.com/translate_t?hl=en&ie=UTF8&text=" + input + "&langpair=" + languagePair + "";
                WebClient webClient = new WebClient();
                webClient.Encoding = System.Text.Encoding.UTF8;
                string result = webClient.DownloadString(url);
                //MessageBox.Show(result.IndexOf("TRANSLATED_TEXT='") + "");
                result = result.Substring(result.IndexOf("TRANSLATED_TEXT='") + 17, 500);
                result = result.Substring(0, result.IndexOf("'"));
                return result;
            }
            catch
            {
                MessageBox.Show("輸入錯誤");
                return "";
            } 
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                button1.PerformClick();
                SendKeys.SendWait("{BS}");
                textBox1.SelectAll();
                return;
            }
        }

        private void Google_Translate_Load(object sender, EventArgs e)
        {

        }
    }
}
