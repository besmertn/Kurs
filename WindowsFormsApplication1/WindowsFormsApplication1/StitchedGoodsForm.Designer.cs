namespace WindowsFormsApplication1
{
    partial class StitchedGoodsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.removeGoodsButton = new System.Windows.Forms.Button();
            this.barcode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.measure = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.count = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.barcode,
            this.name,
            this.measure,
            this.count,
            this.price});
            this.dataGridView1.Location = new System.Drawing.Point(-2, -1);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(644, 228);
            this.dataGridView1.TabIndex = 1;
            // 
            // removeGoodsButton
            // 
            this.removeGoodsButton.Location = new System.Drawing.Point(507, 254);
            this.removeGoodsButton.Name = "removeGoodsButton";
            this.removeGoodsButton.Size = new System.Drawing.Size(104, 23);
            this.removeGoodsButton.TabIndex = 2;
            this.removeGoodsButton.Text = "RemoveGoods";
            this.removeGoodsButton.UseVisualStyleBackColor = true;
            this.removeGoodsButton.Click += new System.EventHandler(this.removeGoodsButton_Click);
            // 
            // barcode
            // 
            this.barcode.HeaderText = "Код товара";
            this.barcode.MaxInputLength = 9;
            this.barcode.Name = "barcode";
            // 
            // name
            // 
            this.name.HeaderText = "Наименование";
            this.name.MaxInputLength = 40;
            this.name.Name = "name";
            this.name.Width = 200;
            // 
            // measure
            // 
            this.measure.HeaderText = "Еденица измерения";
            this.measure.MaxInputLength = 10;
            this.measure.Name = "measure";
            // 
            // count
            // 
            this.count.HeaderText = "Количество";
            this.count.MaxInputLength = 11;
            this.count.Name = "count";
            // 
            // price
            // 
            this.price.HeaderText = "Цена за еденицу";
            this.price.Name = "price";
            // 
            // StitchedGoodsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(644, 345);
            this.Controls.Add(this.removeGoodsButton);
            this.Controls.Add(this.dataGridView1);
            this.Name = "StitchedGoodsForm";
            this.Text = "StitchedGoodsForm";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button removeGoodsButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn barcode;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn measure;
        private System.Windows.Forms.DataGridViewTextBoxColumn count;
        private System.Windows.Forms.DataGridViewTextBoxColumn price;
    }
}