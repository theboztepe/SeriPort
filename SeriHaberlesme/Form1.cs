using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;



namespace SeriHaberlesme
{
    public partial class Form1 : Form

    {
        DataTable tablo = new DataTable();
        DataTable tablo2 = new DataTable();
        DataTable tablo3 = new DataTable();

        public Form1()
        {
            InitializeComponent();
        }
        private void COMPortlariListele()
        {
            string[] myPort;

            myPort = System.IO.Ports.SerialPort.GetPortNames();
            comboBoxCOMPorts.Items.AddRange(myPort);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            COMPortlariListele();
            comboBoxVeriGonderKarakteri.SelectedIndex = 0;
            
            
        }

        delegate void GelenVerileriGuncelleCallback(string veri);

        private void GelenVerileriGuncelle(string veri)
        {
            if (this.textBoxGelenVeri.InvokeRequired)
            {
                GelenVerileriGuncelleCallback d = new GelenVerileriGuncelleCallback(GelenVerileriGuncelle);
                this.Invoke(d, new object[] { veri });
            }
            else
            {
                this.textBoxGelenVeri.AppendText(veri);
            }
        }

        private void buttonSerialPortGonder_Click(object sender, EventArgs e)
        {
           // byte[] veri = new byte[2] {0x0D,0x0A};
            
            if (serialPort1.IsOpen == true)
            {
                serialPort1.Write(textBoxSerialPortGonderilecekVeri.Text);
               /* if (comboBoxVeriGonderKarakteri.SelectedIndex == 1)
                {
                    serialPort1.Write(veri,0,1);
                }
                else if (comboBoxVeriGonderKarakteri.SelectedIndex == 2)
                {
                    serialPort1.Write(veri,0,2);
                }*/
            }
        }

        private void buttonSerialPortBaglanti_Click(object sender, EventArgs e)
        {
            if (comboBoxCOMPorts.SelectedIndex < 0)
            {
                MessageBox.Show("COM port bulunamadi");
                return;
            }

            if (comboBoxSerialPortBaudRate.SelectedIndex < 0)
            {
                MessageBox.Show("Bağlantı hızı seçiniz");
                return;
            }

            try
            {
                if (serialPort1.IsOpen == false)
                {
                    serialPort1.PortName = comboBoxCOMPorts.SelectedItem.ToString();
                    serialPort1.BaudRate = Convert.ToInt32(comboBoxSerialPortBaudRate.Text);
                    serialPort1.Open();
                    buttonSerialPortBaglanti.Text = "BAĞLANTI KES";
                    
                    timerSerialPort.Enabled = true;
                }
                else
                {
                    timerSerialPort.Enabled = false;
                    serialPort1.Close();
                    buttonSerialPortBaglanti.Text = "BAĞLANTI AÇ";
                }
            }
            catch
            {
                MessageBox.Show("Bağlantı açılamadı!");
            }
        }

        private void timerSerialPort_Tick(object sender, EventArgs e)
        {
            if (serialPort1.BytesToRead > 0)
            {
                string gelen_veri = serialPort1.ReadExisting();
                string[] dizi = gelen_veri.Split('-');
                string sicaklik = dizi[0];
                string nem = dizi[1];
                string basinc = dizi[2];
                GelenVerileriGuncelle(gelen_veri);
                sicaklikKayit(dizi[0]);
                nemKayit(dizi[1]);
                basincKayit(dizi[2]);
                Array.Clear(dizi, 0, 2);
               
            }
        }
        public void sicaklikKayit(string gelen)
        {
            SqlConnection con = new SqlConnection("Data Source=localhost\\SQLExpress;Initial Catalog=Sensor;Integrated Security=True");
            SqlCommand command = new SqlCommand("insert into sicaklik (s_deger) values (@deger) ", con);
            if (con.State == ConnectionState.Closed) con.Open();
            command.Parameters.AddWithValue("deger", gelen);
            command.ExecuteReader();
        }
        public void nemKayit(string gelen)
        {
            SqlConnection con = new SqlConnection("Data Source=localhost\\SQLExpress;Initial Catalog=Sensor;Integrated Security=True");
            SqlCommand command = new SqlCommand("insert into nem (n_deger) values (@deger) ", con);
            if (con.State == ConnectionState.Closed) con.Open();
            command.Parameters.AddWithValue("deger", gelen);
            command.ExecuteReader();
        }
        public void basincKayit(string gelen)
        {
            SqlConnection con = new SqlConnection("Data Source=localhost\\SQLExpress;Initial Catalog=Sensor;Integrated Security=True");
            SqlCommand command = new SqlCommand("insert into basinc (b_deger) values (@deger) ", con);
            if (con.State == ConnectionState.Closed) con.Open();
            command.Parameters.AddWithValue("deger", gelen);
            command.ExecuteReader();
        }
        private void comboBoxVeriGonderKarakteri_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }



        private void button2_Click(object sender, EventArgs e)
        {
            tablo.Clear();
            SqlConnection con = new SqlConnection("Data Source=localhost\\SQLExpress;Initial Catalog=Sensor;Integrated Security=True");
            SqlDataAdapter adaptr = new SqlDataAdapter("SELECT s_deger FROM sicaklik ORDER BY id DESC", con);
            SqlDataAdapter nadaptr = new SqlDataAdapter("SELECT n_deger FROM nem ORDER BY id DESC", con);
            SqlDataAdapter badaptr = new SqlDataAdapter("SELECT b_deger FROM basinc ORDER BY id DESC", con);
            //SqlCommand command = new SqlCommand();
            adaptr.Fill(tablo);
            nadaptr.Fill(tablo2);
            badaptr.Fill(tablo3);
            dataGridView1.DataSource = tablo;
            dataGridView2.DataSource = tablo2;
            dataGridView3.DataSource = tablo3;
        }

  


    }
}

