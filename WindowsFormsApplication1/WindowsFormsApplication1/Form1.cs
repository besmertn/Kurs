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
    public partial class Form1 : Form
    {
        //DBControl a = new DBControl();
        
        public Form1()
        {
         
            
            InitializeComponent();
            //a.Query("CREATE TABLE goodsBase (id INTEGER PRIMARY KEY, name CHAR, measure CHAR, count INTEGER, price DOUBLE UNSIGNED, last CHAR);");           
            /*SQLiteDataAdapter b = a.main();
            using (DataTable dt = new DataTable())
            {
                  b.Fill(dt);
                  dataGridView1.DataSource = dt;
                  
            }*/
    
           
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*Form2 tempDialog = new Form2(this);
            Hide();
            tempDialog.ShowDialog();
            String query = "";
            for (int i = 0; i < dataGridView1.RowCount - 1; i++)
            {
                query = "UPDATE example SET value = '" + dataGridView1.Rows[i].Cells[1].Value + "' WHERE id=" + dataGridView1.Rows[i].Cells[0].Value + ";";
                
                a.Query(query);*/
            }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "sMPDataSet3.notes". При необходимости она может быть перемещена или удалена.
            this.notesTableAdapter.Fill(this.sMPDataSet3.notes);
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            String query = "";
            for (int i = 0; i < dataGridView1.RowCount - 1; i++)
            {
                MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;persistsecurityinfo=True;database=SMP");
                connection.Open();
                MySqlCommand command = new MySqlCommand("SELECT * FROM users WHERE user_id=11", connection);
                MySqlDataAdapter sqlda = new MySqlDataAdapter(command);
                query = "UPDATE example SET value = '" + dataGridView1.Rows[i].Cells[1].Value + "' WHERE id=" + dataGridView1.Rows[i].Cells[0].Value + ";";
                using (DataTable dt = new DataTable())
                {
                    sqlda.Fill(dt);
                    dataGridView1.DataSource = dt;

                }
                connection.Close();
            }
        }

            

        }

       


       
    }

