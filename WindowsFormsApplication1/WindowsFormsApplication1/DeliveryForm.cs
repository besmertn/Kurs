using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WindowsFormsApplication1
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
            foreach (DataGridViewRow row in dataGridView1.Rows) {
                try {
                    delivery.Add(new Goods(row.Cells[0].Value.ToString()
                        , row.Cells[1].Value.ToString()
                        , row.Cells[2].Value.ToString()
                        , Convert.ToInt32(row.Cells[3].Value)
                        , Convert.ToDouble(row.Cells[4].Value)
                        , DateTime.Now
                        , Convert.ToDateTime(row.Cells[5].Value)));
                    
                }
                catch (NullReferenceException) { }
            }
            _goodsBase.NewDelivery(delivery);
            Close();
        }

    }
}
