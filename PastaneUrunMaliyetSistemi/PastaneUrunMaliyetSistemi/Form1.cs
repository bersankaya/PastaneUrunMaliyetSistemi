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
namespace PastaneUrunMaliyetSistemi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection conn = new SqlConnection("Data Source=LAPTOP-AGQ4V6UP;Initial Catalog=PastaneUrunMaliyetlendirmeSistemi;Integrated Security=True");
        void malzemelistele()
        {
            SqlDataAdapter da = new SqlDataAdapter("select * From TBLMALZEMELER", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }
        void urunListele()
        {
            SqlDataAdapter da1 = new SqlDataAdapter("select * from TBLURUNLER", conn);
            DataTable dt1 = new DataTable();
            da1.Fill(dt1);
            dataGridView1.DataSource = dt1;
        }
        void kasa()
        {
            SqlDataAdapter da2 = new SqlDataAdapter("select * from TBLKASA", conn);
            DataTable dt2 = new DataTable();
            da2.Fill(dt2);
            dataGridView1.DataSource = dt2;
        
        }
        void cmburungetir()
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from tblurunler", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            cmbUrun.ValueMember = "URUNID";
            cmbUrun.DisplayMember = "AD";
            cmbUrun.DataSource = dt;
        }
        void cmbmalzemegetir()
        {
            SqlDataAdapter da1 = new SqlDataAdapter("select * from tblmalzemeler", conn);
            DataTable dt1 = new DataTable();
            da1.Fill(dt1);
            CmbMalzeme.ValueMember = "MALZEMEID";
            CmbMalzeme.DisplayMember = "AD";
            CmbMalzeme.DataSource = dt1;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            malzemelistele();
            cmburungetir();
            cmbmalzemegetir();
        }

        private void btnmalzemeekle_Click(object sender, EventArgs e)
        {
            conn.Open();
            SqlCommand komut = new SqlCommand("insert into TBLMALZEMELER (AD,STOK,FIYAT,NOTLAR) values (@p1,@p2,@p3,@p4)", conn);
            komut.Parameters.AddWithValue("@p1", txtmalzemead.Text);
            komut.Parameters.AddWithValue("@p2", decimal.Parse(txtmalzemestok.Text));
            komut.Parameters.AddWithValue("@p3", decimal.Parse(txtmalzemefiyat.Text));
            komut.Parameters.AddWithValue("@p4", txtnotlar.Text);
            komut.ExecuteNonQuery();
            conn.Close();
            MessageBox.Show("Mesaj Eklenmiştir");
            malzemelistele();
        }

        private void BtnurunListesi_Click(object sender, EventArgs e)
        {
            urunListele();
        }

        private void BtnMalzemeListesi_Click(object sender, EventArgs e)
        {
            malzemelistele();
        }

        private void BtnKasa_Click(object sender, EventArgs e)
        {
            kasa();
        }

        private void BtnCikis_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BtnUrunEkle_Click(object sender, EventArgs e)
        {
            conn.Open();
            SqlCommand komut = new SqlCommand("insert into TBLURUNLER (ad) values (@p1)", conn);
            komut.Parameters.AddWithValue("@p1", TxtUrunAd.Text);
            komut.ExecuteNonQuery();
            conn.Close();
            MessageBox.Show("Ürün Sisteme Eklendi");
            urunListele();
        }

        private void BtnurunOlustur_Click(object sender, EventArgs e)
        {
            conn.Open();
            SqlCommand komut = new SqlCommand("insert into tblfırın (URUNIID,MALZEMEID,MIKTAR,MALIYET) values (@p1,@p2,@p3,@p4)", conn);
            komut.Parameters.AddWithValue("@p1", cmbUrun.SelectedValue);
            komut.Parameters.AddWithValue("@p2", CmbMalzeme.SelectedValue);
            komut.Parameters.AddWithValue("@p3", decimal.Parse(TxtMiktar.Text));
            komut.Parameters.AddWithValue("@p4", decimal.Parse(TxtMaliyet.Text));
            komut.ExecuteNonQuery();
            conn.Close();
            MessageBox.Show("Malzeme Eklendi");
            listBox1.Items.Add(CmbMalzeme.Text + "-" + TxtMaliyet.Text);
            
        }

        private void TxtMiktar_TextChanged(object sender, EventArgs e)
        {
            double maliyet;
            if (TxtMiktar.Text == "")
            {
                TxtMiktar.Text = "0";
            }
            conn.Open();
            SqlCommand komut1 = new SqlCommand("select * from tblmalzemeler where malzemeıd=@p1", conn);
            komut1.Parameters.AddWithValue("@p1", CmbMalzeme.SelectedValue);
            SqlDataReader dr = komut1.ExecuteReader();
            while (dr.Read())
            {
                TxtMaliyet.Text = dr[3].ToString();
            }
            conn.Close();
            if (CmbMalzeme.Text == "YUMURTA")
            {
               
                maliyet = Convert.ToDouble(TxtMaliyet.Text) * Convert.ToDouble(TxtMiktar.Text);
                TxtMaliyet.Text = maliyet.ToString();
            }
            else
            {
               
                maliyet = Convert.ToDouble(TxtMaliyet.Text) / 1000 * Convert.ToDouble(TxtMiktar.Text);
                TxtMaliyet.Text = maliyet.ToString();
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen = dataGridView1.SelectedCells[0].RowIndex;
            TxtUrunİd.Text = dataGridView1.Rows[secilen].Cells[0].Value.ToString();
            TxtUrunAd.Text = dataGridView1.Rows[secilen].Cells[1].Value.ToString();

            conn.Open();
            SqlCommand komut = new SqlCommand("select sum(malıyet) from tblfırın where urunııd=@p1", conn);
            komut.Parameters.AddWithValue("@p1", TxtUrunİd.Text);
            SqlDataReader dr = komut.ExecuteReader();
            while (dr.Read())
            {
                TxtMfiyat.Text = dr[0].ToString();
            }
            conn.Close();
        }

        private void BtnGuncellle_Click(object sender, EventArgs e)
        {
            conn.Open();
            SqlCommand komut = new SqlCommand("update tblurunler set mfıyat=@p1,sfıyat=@p2,stok=@p3 where ad='"+TxtUrunAd.Text+"'", conn);
            komut.Parameters.AddWithValue("@p1", decimal.Parse(TxtMfiyat.Text));
            komut.Parameters.AddWithValue("@p2", decimal.Parse(TxtSfiyat.Text));
            komut.Parameters.AddWithValue("@p3", TxtUrunStok.Text);
            komut.ExecuteNonQuery();
            conn.Close();
            MessageBox.Show("Ürün Güncellendi");
            urunListele();
            decimal girdi, cikti;
            cikti = Convert.ToDecimal(TxtUrunStok.Text) * Convert.ToDecimal(TxtMfiyat.Text);
            girdi = (Convert.ToDecimal(TxtUrunStok.Text) * Convert.ToDecimal(TxtSfiyat.Text)) - cikti;
            conn.Open();
            SqlCommand komut1 = new SqlCommand("insert into tblkasa (GIRIS,CIKIS) values (@p4,@p5)", conn);
            komut1.Parameters.AddWithValue("@p4", girdi);
            komut1.Parameters.AddWithValue("@p5", cikti);
            komut1.ExecuteNonQuery();
            conn.Close();
                
        }
    }
}
