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
using System.Collections.Generic;

namespace WuchaTuoyuan
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        List<Point> points = new List<Point>();
        private void toolOpen_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Filter = "|*.txt";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    var reader = new StreamReader(openFileDialog1.FileName);
                    reader.ReadLine();
                    while(!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        Point point = new Point(line);
                        points.Add(point);
                    }
                    for (int i = 0; i < points.Count; i++)
                    {
                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[i].Cells[0].Value = i + 1;
                        dataGridView1.Rows[i].Cells[1].Value = points[i].x;
                        dataGridView1.Rows[i].Cells[2].Value = points[i].y;
                    }
                    toolStripStatusLabel1.Text = "状态：导入数据成功";
                    MessageBox.Show("导入成功");
                    reader.Close();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }

        private void toolCal_Click(object sender, EventArgs e)
        {
            if(dataGridView1.Rows[0].Cells[1].Value != null)
            {
                PointCenter pointCenter = new PointCenter(points);
                tabControl1.SelectedIndex = 1;
                richTextBox1.Text = $"长半轴：  短半轴：  \n{pointCenter.Su}  {pointCenter.Sv}";
                toolStripStatusLabel1.Text = "状态：计算成功";
                MessageBox.Show("计算成功");
            }
            else
            {
                MessageBox.Show("没有导入数据");
            }
        }

        private void toolSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "结果|*.txt";
            if(saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var writer = new StreamWriter(saveFileDialog1.FileName);
                writer.Write(richTextBox1.Text);
                writer.Close();
            }
        }
    }
}
