using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Dianli
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        List<Satellite> satellites = new List<Satellite>();//所有卫星的集合

        private void toolOpen_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.Rows.Clear();
                satellites.Clear();
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Filter = "|*.txt";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    var reader = new StreamReader(openFileDialog1.FileName);
                    var time = new Time(reader.ReadLine());//第一行是时间
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if(line.Length > 0)
                        {
                            var satellite = new Satellite(line,time);
                            satellites.Add(satellite);
                        }
                    }
                    reader.Close();
                    //显示数据
                    for(int i = 0;i < satellites.Count;i++)
                    {
                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[i].Cells[0].Value = satellites[i].name;
                        dataGridView1.Rows[i].Cells[1].Value = satellites[i].X;
                        dataGridView1.Rows[i].Cells[2].Value = satellites[i].Y;
                        dataGridView1.Rows[i].Cells[3].Value = satellites[i].Z;
                    }
                    MessageBox.Show("导入数据成功");
                    toolStripStatusLabel1.Text = "状态：导入数据成功";
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private void toolCal_Click(object sender, EventArgs e)
        {
            if(satellites.Count != 0)
            {
                string res = "卫星编号：  高度角：  方位角：  电离层延迟距离：\n";
                foreach (Satellite d in satellites)
                {
                    res += $"{d.name}  {(d.E * 180 / Math.PI):F3}  {(d.A * 180 / Math.PI):F3}  {d.IPP_s:F3}\n";
                }
                richTextBox1.Text = res;
                toolStripStatusLabel1.Text = "状态：计算成功";
                MessageBox.Show("计算成功");
                tabControl1.SelectedIndex = 1;
            }
            else
            {
                MessageBox.Show("未导入数据！");
            }
        }

        private void toolSave_Click(object sender, EventArgs e)
        {
            try
            {
                if(richTextBox1.Text != "")
                {
                    SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                    saveFileDialog1.Filter = "|*.txt";
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        var writer = new StreamWriter(saveFileDialog1.FileName);
                        writer.Write(richTextBox1.Text);
                        writer.Close();
                    }
                    MessageBox.Show("保存成功");
                }
                else
                {
                    MessageBox.Show("未计算或者导入数据");
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
