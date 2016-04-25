using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form2 : Form
    {
        GoodsDBControl goodsBase = new GoodsDBControl();
        public Form2()
        {
            InitializeComponent();
            CalendarColumn col = new CalendarColumn();
            dataGridView1.Columns.Add(col);
            dataGridView1.Columns[5].Name = "Срок хранения";

        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit(e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int rowCounter = dataGridView1.Rows.Count;
            string[,] data = new string[rowCounter-1, 6];
            for (int i = 0; i < rowCounter - 1; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    data[i, j] = dataGridView1.Rows[i].Cells[j].Value.ToString();
                     
                }
                
            }
            goodsBase.newDelivery(data, rowCounter - 1);
        }
    }
}
