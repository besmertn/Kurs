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
    public partial class StitchedGoodsForm : Form
    {
        StitchedGoodsDBControl stitchedGoodsControl = new StitchedGoodsDBControl();
        
        public StitchedGoodsForm()
        {
            InitializeComponent();
            foreach (Goods product in stitchedGoodsControl.getAllStitchedGoods()) {
                dataGridView1.Rows.Insert(0, 1);
                dataGridView1.Rows[0].SetValues(product.Barcode
                    , product.Name
                    , product.Measure
                    , product.Count.ToString()
                    , product.Price.ToString()
                    , product.ShelfLife.ToString());
            }        }

        private void removeGoodsButton_Click(object sender, EventArgs e)
        {
            List<string> barcodes = new List<string>();
            foreach (DataGridViewRow Row in dataGridView1.Rows) {
                if (Row.Selected) barcodes.Add(Row.Cells[0].Value.ToString());
            }
            stitchedGoodsControl.RemoveStitchedGoods(barcodes);
        }

        
    }
}
