using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using WindowsFormsApplication1.Controller;
using NLog;

namespace WindowsFormsApplication1.View
{
    public partial class MainForm : Form
    {
        readonly GoodsDbControl _goodsBase = new GoodsDbControl();
        readonly ChecksControl _checkControl = new ChecksControl();
        readonly SalesDbControl _salesControl = new SalesDbControl();
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly Authorize _parentForm;
        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        private int _cashRegisterNumber;
        private double _summ;

        private void ChangeSumm()
        {
            _summ = 0;
            foreach (DataGridViewRow row in PurchaseGridView.Rows)
            {
                try
                {
                    if (!Regex.IsMatch(row.Cells[3].Value.ToString(), @"[0-9]"))
                        throw new FormatException("Incorrect input data in row №" + (row.Index + 1));
                    _summ += Convert.ToDouble(row.Cells[3].Value)*Convert.ToDouble(row.Cells[4].Value);
                }
                catch (FormatException ex)
                {
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    MessageBoxIcon icon = MessageBoxIcon.Warning;
                    MessageBox.Show(ex.Message, @"Error Detected in Input", buttons, icon);
                }
            }
            summTextBox.Text = _summ.ToString("F");
            buyButton.Enabled = _summ > 0;
            summLabel.Text = @"Summ: " + _summ.ToString("F");
        }

        public MainForm(Authorize parentForm)
        {
            _parentForm = parentForm;
            _cashRegisterNumber = parentForm.CashRegisterNumber;
            Text = @"Касса№ " + _cashRegisterNumber;
            _logger.Debug("Касса№ " + _cashRegisterNumber + " начала работу");
            InitializeComponent();
            DefaultCheckText();
            _goodsBase.ShelfLifeControl();
            _goodsBase.ClearList();
            _goodsBase.FillList();
        }

        public sealed override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            ChangeSumm();
        }

        private void search_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Regex.IsMatch(barcodeSearchText.Text, @"[0-9]{1,9}")) throw new FormatException();
                Goods data = _goodsBase.SearchGoods(barcodeSearchText.Text);
                foreach (DataGridViewRow row in PurchaseGridView.Rows)
                {
                    if (!row.Cells[0].Value.Equals(data.Barcode)) continue;
                    row.Cells[3].Value = Convert.ToInt32(row.Cells[3].Value) + 1;
                    return;
                }
                PurchaseGridView.Rows.Add(data.Barcode, data.Name, data.Measure, 1,
                    (data.Price*_salesControl.GetDiscount(data.Barcode)).ToString("F"));
                ChangeSumm();
                _logger.Info("The goods added to the cart");
            }
            catch (FormatException)
            {
                MessageBox.Show(@"Error in input!", @"Error in input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                barcodeSearchText.Clear();
            }
            catch (NullReferenceException)
            {
                MessageBox.Show(@"Such goods doesn't exist!", @"Error in input", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                barcodeSearchText.Clear();
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _logger.Debug("Касса№ " + _cashRegisterNumber + " завершила работу");
            Hide();
            _parentForm.Show();
        }

        private void buyButton_Click(object sender, EventArgs e)
        {
            List<Goods> purchase = new List<Goods>();
            _logger.Debug("Purchase...");
            foreach (DataGridViewRow row in PurchaseGridView.Rows)
            {
                try
                {
                    purchase.Add(new Goods(row.Cells[0].Value.ToString()
                        , row.Cells[1].Value.ToString()
                        , row.Cells[2].Value.ToString()
                        , Convert.ToInt32(row.Cells[3].Value)
                        , Convert.ToDouble(row.Cells[4].Value.ToString())));
                }
                catch (NullReferenceException)
                {
                }
            }
            _goodsBase.BuyGoods(purchase);
            if (checkBox.Checked)
            {
                _logger.Info("Check printing");
                var topText = topCheckRichTextBox.Lines.ToArray();
                var bottomText = bottomCheckRichTextBox.Lines.ToArray();
                string checkPath = _checkControl.CreateCheck(topText
                    , _checkControl.GenerateMainCheckText(purchase, _summ)
                    , bottomText
                    , _cashRegisterNumber);
                Process myProcess = new Process
                {
                    StartInfo =
                    {
                        UseShellExecute = false,
                        FileName = "notepad.exe",
                        Arguments = checkPath,
                        CreateNoWindow = true
                    }
                };
                myProcess.Start();

                topText[0] = "Касса№ " + _cashRegisterNumber + "\t Чек№ " +
                             _goodsBase.GetCheckCounter(_cashRegisterNumber);
                topCheckRichTextBox.Lines = topText;
            }
            PurchaseGridView.Rows.Clear();
            barcodeSearchText.Clear();
            ChangeSumm();
            _logger.Debug("You've purchased");
        }

        private void NewDeliveryButton_Click(object sender, EventArgs e)
        {
            _logger.Debug("Opening of the `NewDelivery` form");
            var form = new DeliveryForm();
            form.ShowDialog();
        }

        private void StitchedGoodsFormButton_Click(object sender, EventArgs e)
        {
            _logger.Debug("Opening of the `StitchedGoodsForm` form");
            var form = new StitchedGoodsForm();
            form.ShowDialog();
        }

        private void DefaultCheckText()
        {
            topCheckRichTextBox.Clear();
            topCheckRichTextBox.AppendText("Касса№ " + _cashRegisterNumber + "\t");
            topCheckRichTextBox.AppendText("Чек№ " + _goodsBase.GetCheckCounter(_cashRegisterNumber) + "\n");
            topCheckRichTextBox.AppendText(
                "------------------------------------------------------------------------------------\n");
            bottomCheckRichTextBox.AppendText(
                "------------------------------------------------------------------------------------\n");
        }

        private void payTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                shortChangeTextBox.Text = (Convert.ToDouble(payTextBox.Text) - _summ).ToString("F");
            }
            catch (FormatException)
            {
                shortChangeTextBox.Text = @"0.00";
            }
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            PurchaseGridView.Rows.Clear();
            ChangeSumm();
        }

        private void PurchaseGridView_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (PurchaseGridView.CurrentRow != null)
            {
                PurchaseGridView.Rows.RemoveAt(PurchaseGridView.CurrentRow.Index);
                _logger.Info("The goods removed from the cart");
            }
            ChangeSumm();
        }
    }
}

