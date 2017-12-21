using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace SeriHaberlesme
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
 
        OleDbConnection con;
        OleDbCommand cmd;
        OleDbDataReader dr;
        

        private void button1_Click_1(object sender, EventArgs e)
        {
            string ad = textBox1.Text;
            string sifre = textBox2.Text;
            con = new OleDbConnection("Provider=Microsoft.ACE.Oledb.12.0;Data Source=giris.accdb");
            cmd = new OleDbCommand();
            con.Open();
            cmd.Connection = con;
            cmd.CommandText = "SELECT * FROM kullanici where k_ad='" + textBox1.Text + "' AND k_sifre='" + textBox2.Text + "'";
            dr = cmd.ExecuteReader();
            if(dr.Read())
            {
                Form1 f2= new Form1();
                f2.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Kullanıcı adı ya da şifre yanlış");
            }
 
            con.Close();
 
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            textBox2.PasswordChar = '*'; 
        }


        }

        
    }


