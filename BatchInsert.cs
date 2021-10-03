using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class BatchInsert : Form
    {
        public BatchInsert(string U_D)
        {
            InitializeComponent();
            Unit_ID = U_D;
        }
        string Unit_ID = "";
        private void cmdBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.CheckFileExists = true;
                ofd.Filter = "excel2003|*.xls|excel|*.xlsx";

                if (ofd.ShowDialog() == DialogResult.OK)
                    this.txtFilePath.Text = ofd.FileName;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string dataSource = this.txtFilePath.Text;

            if (string.IsNullOrEmpty(dataSource))
            {
                MessageBox.Show("未選擇資料來源檔案。");
                return;
            }
            DataTable dt = new DataTable();
            string dbcontxt = @"Data Source =" + dataSource + ";Provider=Microsoft.Jet.OLEDB.4.0;Extended Properties='Excel 8.0;HDR=Yes;IMDS=2;'";
            OleDbConnection con = new OleDbConnection(dbcontxt);
            con.Open();
            DataTable sheettb = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            comboBox1.Items.AddRange(sheettb.Rows.OfType<DataRow>().Select(x => x.ItemArray[2].ToString().Substring(0, x.ItemArray[2].ToString().Length - 1)).ToArray());
            con.Close();
        }

        private void BatchInsert_Load(object sender, EventArgs e)
        {
            label3.Text += Sqlclass.totable("select Name from E2_Unit where ID = '"+Unit_ID+"'").Rows[0][0].ToString();
            CheckForIllegalCrossThreadCalls = false;
            label4.Location = new Point(label3.Location.X+label3.Size.Width,label4.Location.Y);
            dgvDataList.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvDataList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            Task.Run(() =>
            {
                int allnum = dgvDataList.Rows.Count;
                label4.ForeColor = Color.Red;
                for (int i = 0; i < allnum; i++)
                {
                    if  (dgvDataList.Rows[i].Cells["English"].Value ==null || dgvDataList.Rows[i].Cells["English"].Value.ToString().Trim() == "")
                    {
                        label4.Text = "" + (i + 1) + "/" + (allnum + 1) + " data has been completed";
                        continue;
                    }
                    DataTable Word = Sqlclass.totable("select * from E2_Word where English ='" + dgvDataList.Rows[i].Cells["English"].Value + "' and PartOfSpeech ='" + dgvDataList.Rows[i].Cells["Part of speech"].Value + "'");
                    if (Word.Rows.Count == 0)
                    {
                        Sqlclass.totable("Insert into E2_Word (Unit_ID,English,Chinese,PartOfSpeech,Wrong) Values('" + Unit_ID + "','" + dgvDataList.Rows[i].Cells["English"].Value + "','" + dgvDataList.Rows[i].Cells["Chinese"].Value + "','" + dgvDataList.Rows[i].Cells["Part of speech"].Value + "','0')");
                    }
                    else
                    {
                        if (Word.Rows[0]["Unit_ID"].ToString().Split(',').ToList().FindIndex(x => x == Unit_ID) != -1)
                        {
                            label4.Text = "" + (i + 1) + "/" + (allnum + 1) + " data has been completed";
                            continue;
                        }
                        List<string> chinese = Word.Rows[0]["Chinese"].ToString().Split('、').ToList();
                        List<string> new_c = dgvDataList.Rows[i].Cells["Chinese"].Value.ToString().Split('、').ToList();
                        string updatechinese = "";
                        foreach (var a in new_c)
                        {
                            if (chinese.Count(x => x == a) == 0) chinese.Add(a);
                        }

                        if (chinese.Count == 0) updatechinese = chinese[0];
                        else
                        {
                            foreach (var a in chinese)
                            {
                                updatechinese += a + "、";
                            }
                            updatechinese = updatechinese.Substring(0, updatechinese.Length - 1);
                        }
                        Sqlclass.totable("Update E2_Word set Unit_ID = '" + Word.Rows[0]["Unit_ID"].ToString() + "," + Unit_ID + "', Chinese = '" + updatechinese + "' where English='" + dgvDataList.Rows[i].Cells["English"].Value + "'");
                    }
                    label4.Text = "" + (i + 1) + "/" + (allnum + 1) + " data has been completed";
                }
                button2.Enabled = true;
                label4.ForeColor = Color.Green;
            });
           
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string dataSource = this.txtFilePath.Text;
            string dbcontxt = @"Data Source =" + dataSource + ";Provider=Microsoft.Jet.OLEDB.4.0;Extended Properties='Excel 8.0;HDR=Yes;IMDS=2;'";
            OleDbConnection con = new OleDbConnection(dbcontxt);
            DataTable dt = new DataTable();
            OleDbDataAdapter dba = new OleDbDataAdapter("select * from [" + comboBox1.Text + "$]", con);
            con.Open();
            dba.Fill(dt);
            con.Close();
            dgvDataList.DataSource = dt;
        }
    }
}
