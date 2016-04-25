using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace WindowsFormsApplication1
{

    public partial class Form1 : Form
    {
        GoodsDBControl goodsBase = new GoodsDBControl();
        double summ = 0;

        private void changeSumm()
        {
            summ = 0;
            int rowCounter = dataGridView1.Rows.Count;
            for (int i = 0; i < rowCounter - 1; i++)
            {
                summ += Convert.ToInt16(dataGridView1.Rows[i].Cells[3].Value) * Convert.ToDouble(dataGridView1.Rows[i].Cells[4].Value);
            }
            summLabel.Text = "Summ: " + summ.ToString();
        }
        public Form1(Authorize parent)
        {

            Authorize parentForm = parent;
            InitializeComponent(); 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*Form2 tempDialog = new Form2(this);
            Hide();
            tempDialog.ShowDialog();
            String query = "";
            for (int i = 0; i < dataGridView1.RowCount - 1; i++)
            {
                query = "UPDATE example SET value = '" + dataGridView1.Rows[i].Cells[1].Value + "' WHERE id=" + dataGridView1.Rows[i].Cells[0].Value + ";";
                
                a.Query(query);*/
            
            }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            changeSumm();
        }

        private void search_Click(object sender, EventArgs e)
        {
            string[] data = goodsBase.goodsSearch(barcodeSearchText.Text);
            int rowCounter = dataGridView1.Rows.Count;
            dataGridView1.Rows.Insert(0, 1);
            dataGridView1.Rows[0].SetValues(data[0], data[1], data[2],1,data[3]);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit(e);
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int currentRowIndex =  dataGridView1.CurrentRow.Index;
            dataGridView1.Rows.RemoveAt(currentRowIndex);
            changeSumm();
        }

        private void buyButton_Click(object sender, EventArgs e)
        {
            int rowCounter = dataGridView1.Rows.Count;
            string[,] data = new string[rowCounter,5];
            for (int i = 0; i < rowCounter - 1; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    data[i , j] = dataGridView1.Rows[i].Cells[j].Value.ToString();
                }
                
            }
            goodsBase.buyUpdate(data, rowCounter - 1);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string[] tmp = goodsBase.shelflifeControl();
            label1.Text = tmp[0];
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            this.Hide();
            form2.ShowDialog();
        }
     
    }
    
}

