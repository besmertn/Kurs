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
        ChecksControl checkControl = new ChecksControl();
        int cashRegisterNumber;
        double summ = 0;

        private void changeSumm()
        {
            summ = 0;
            int rowCounter = dataGridView1.Rows.Count;
            foreach(DataGridViewRow Row in dataGridView1.Rows){
                try
                {
                    summ += Convert.ToDouble(Row.Cells[3].Value) * Convert.ToDouble(Row.Cells[4].Value);
                }
                catch { }
            }
            summTextBox.Text = summ.ToString();
            if (summ != 0)
                buyButton.Enabled = true;
            else
                buyButton.Enabled = false;
            summLabel.Text = "Summ: " + summ.ToString();
        }
        public MainForm(Authorize parent)
        {

            Authorize parentForm = parent;
            cashRegisterNumber =  parent.cashRegisterNumber;
            this.Text = "Касса№ " + cashRegisterNumber;
            InitializeComponent();
            defaultCheckText();
            goodsBase.createGoodsList();
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
           changeSumm();
        }

        private void search_Click(object sender, EventArgs e)
        {
            Goods data = goodsBase.searchGoods(barcodeSearchText.Text);
            foreach (DataGridViewRow Row in dataGridView1.Rows) {
                if (Row.Cells[0].Value.Equals(data.Barcode)) {
                    Row.Cells[3].Value = Convert.ToInt32(Row.Cells[3].Value) + 1;
                    return;
                }
            }
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
            if (checkBox.Checked == true)
            {
                string[] topText = topCheckRichTextBox.Lines.ToArray();
                string[] bottomText = bottomCheckRichTextBox.Lines.ToArray();

                checkControl.createCheck(topText
                    , checkControl.generateMainCheckText(purchase, summ)
                    , bottomText
                    , cashRegisterNumber);
                topText[0] = "Касса№ " + cashRegisterNumber + "\t Чек№ " + goodsBase.getCheckCounter(cashRegisterNumber);
                topCheckRichTextBox.Lines = topText;
            }
            dataGridView1.Rows.Clear();
            changeSumm();
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

        private void defaultCheckText() {
            topCheckRichTextBox.Clear();
            topCheckRichTextBox.AppendText("Касса№ " + cashRegisterNumber+ "\t");
            topCheckRichTextBox.AppendText("Чек№ " + goodsBase.getCheckCounter(cashRegisterNumber) +"\n");
            topCheckRichTextBox.AppendText("------------------------------------------------------------------------------------\n");
            bottomCheckRichTextBox.AppendText("------------------------------------------------------------------------------------\n");
        }

        private void payTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                shortChangeTextBox.Text = (Convert.ToDouble(payTextBox.Text) - summ).ToString();
            }
            catch (FormatException) {
                shortChangeTextBox.Text = "0";
            }
        }
    }
    
}

