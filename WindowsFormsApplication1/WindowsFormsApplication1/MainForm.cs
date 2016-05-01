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

    public partial class MainForm : Form
    {
        GoodsDBControl goodsBase = new GoodsDBControl();
        double summ = 0;

        private void changeSumm()
        {
            summ = 0;
            int rowCounter = dataGridView1.Rows.Count;
            foreach(DataGridViewRow Row in dataGridView1.Rows){
                try
                {
                    summ += Convert.ToInt16(Row.Cells[3].Value) * Convert.ToInt16(Row.Cells[4].Value);
                }
                catch { }
            }            
            summLabel.Text = "Summ: " + summ.ToString();
        }
        public MainForm(Authorize parent)
        {

            Authorize parentForm = parent;
            InitializeComponent();
            goodsBase.createGoodsList();
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
           changeSumm();
        }

        private void search_Click(object sender, EventArgs e)
        {
            Goods data = goodsBase.searchGoods(barcodeSearchText.Text);
            dataGridView1.Rows.Insert(0, 1);
            dataGridView1.Rows[0].SetValues(data.Barcode, data.Name, data.Measure, 1, data.Price);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit(e);
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int currentRowIndex = dataGridView1.CurrentRow.Index;
            dataGridView1.Rows.RemoveAt(currentRowIndex);
            changeSumm();
        }

        private void buyButton_Click(object sender, EventArgs e)
        {
            List<Goods> purchase = new List<Goods>();
            foreach (DataGridViewRow Row in dataGridView1.Rows) {
                try
                {
                    purchase.Add(new Goods(Row.Cells[0].Value.ToString()
                    , Row.Cells[1].Value.ToString()
                    , Row.Cells[2].Value.ToString()
                    , Convert.ToInt32(Row.Cells[3].Value)
                    , Convert.ToDouble(Row.Cells[4].Value.ToString())));
                }
                catch (NullReferenceException) { }
            }                                        
            goodsBase.buyGoods(purchase);
        }     

        private void button2_Click(object sender, EventArgs e)
        {
            DeliveryForm form = new DeliveryForm();
            form.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StitchedGoodsForm form = new StitchedGoodsForm();
            form.ShowDialog();
        }    
    }
    
}

