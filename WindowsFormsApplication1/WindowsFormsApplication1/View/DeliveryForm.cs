using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using WindowsFormsApplication1.Controller;

namespace WindowsFormsApplication1.View
{
    public partial class DeliveryForm : Form
    {
        readonly GoodsDbControl _goodsBase = new GoodsDbControl();

        public DeliveryForm()
        {
            InitializeComponent();
            CalendarColumn col = new CalendarColumn();
            dataGridView1.Columns.Add(col);
            dataGridView1.Columns[5].Name = "Срок хранения";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<Goods> delivery = new List<Goods>();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                try
                {
                    if (!Regex.IsMatch(row.Cells[0].Value.ToString(), @"[0-9]") ||
                        !Regex.IsMatch(row.Cells[2].Value.ToString(), @"^[a-zA-Z][a-zA-Z\.]") ||
                        !Regex.IsMatch(row.Cells[3].Value.ToString(), @"[0-9]+$") ||
                        !Regex.IsMatch(row.Cells[4].Value.ToString(), (@"\-?\d+(\.\d{0,})?")))
                    {
                        throw new FormatException("Incorrect input data in the row № " + (row.Index + 1));
                    }
                    delivery.Add(new Goods(row.Cells[0].Value.ToString()
                        , row.Cells[1].Value.ToString()
                        , row.Cells[2].Value.ToString()
                        , Convert.ToInt32(row.Cells[3].Value)
                        , Convert.ToDouble(row.Cells[4].Value)
                        , DateTime.Now
                        , Convert.ToDateTime(row.Cells[5].Value)));
                }
                catch (NullReferenceException)
                {
                }
                catch (FormatException ex)
                {
                    MessageBox.Show(ex.Message, @"Error in input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            _goodsBase.NewDelivery(delivery);
            Close();
        }
    }
}
