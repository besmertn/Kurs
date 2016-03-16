using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            DBControl a = new DBControl();
            InitializeComponent();
            SQLiteDataAdapter b = a.main();
            using (DataTable dt = new DataTable())
            {
                b.Fill(dt);
                dataGridView1.DataSource = dt;
            }
    
           
           
        }

       
    }
}
