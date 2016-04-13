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
    public partial class Form2 : Form
    {
        private Form1 m_parent;
        public Form2(Form1 frm1)
        {
            InitializeComponent();
            m_parent = frm1;
            label1.Text = m_parent.textBox1.Text;

        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            m_parent.Show();
            Hide();
           
        }
    }
}
