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
    public partial class DeliveryForm : Form
    {
        GoodsDBControl goodsBase = new GoodsDBControl();
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
            foreach (DataGridViewRow Row in dataGridView1.Rows) {
                try {
                    delivery.Add(new Goods(Row.Cells[0].Value.ToString()
                        , Row.Cells[1].Value.ToString()
                        , Row.Cells[2].Value.ToString()
                        , Convert.ToInt32(Row.Cells[3].Value)
                        , Convert.ToDouble(Row.Cells[4].Value)
                        , DateTime.Now
                        , Convert.ToDateTime(Row.Cells[5].Value)));
                    
                }
                catch (NullReferenceException) { }
            }
            goodsBase.newDelivery(delivery);
            this.Close();
        }

    }
}
