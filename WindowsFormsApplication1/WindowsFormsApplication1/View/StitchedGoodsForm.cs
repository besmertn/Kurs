using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;
using WindowsFormsApplication1.Controller;

namespace WindowsFormsApplication1.View
{
    public partial class StitchedGoodsForm : Form
    {
        private readonly StitchedGoodsDbControl _stitchedGoodsControl = new StitchedGoodsDbControl();

        public StitchedGoodsForm()
        {
            _stitchedGoodsControl.ShelfLifeControl();
            InitializeComponent();
            foreach (Goods product in _stitchedGoodsControl.GetAllStitchedGoods())
            {
                dataGridView1.Rows.Insert(0, 1);
                dataGridView1.Rows[0].SetValues(product.Barcode
                    , product.Name
                    , product.Measure
                    , product.Count.ToString()
                    , product.Price.ToString("F")
                    , product.ShelfLife.ToString(CultureInfo.CurrentCulture));
            }
        }

        private void removeGoodsButton_Click(object sender, EventArgs e)
        {
            var barcodes = new List<string>();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (!row.Selected) continue;
                barcodes.Add(row.Cells[0].Value.ToString());
                dataGridView1.Rows.Remove(row);
            }
            _stitchedGoodsControl.RemoveStitchedGoods(barcodes);
        }
    }
}
