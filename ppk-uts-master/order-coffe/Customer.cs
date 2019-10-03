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

namespace order_coffe
{
    public partial class Customer : Form
    {
        private static string connectionString = "server=localhost;port=3306;username=root;password=;database=seteguk;";
        private MySqlConnection databaseConn = new MySqlConnection(connectionString);
        public string id;

        public Customer()
        {
            InitializeComponent();
        }

        private void Button_pesan_Click(object sender, EventArgs e)
        {
            CheckOut Check = new CheckOut(id);
            Check.Show();
        }

        public void refresh()
        {
            listView1.Items.Clear();
            
            string query = "SELECT * FROM stgk_pemesanan WHERE tanggal ='"+DateTime.Now.ToString("yyyy-MM-dd")+"' ";
            MySqlCommand cmd = new MySqlCommand(query, databaseConn);
            cmd.CommandTimeout = 60;
            MySqlDataReader r = cmd.ExecuteReader();

            if (r.HasRows)
            {
                while (r.Read())
                {
                    ListViewItem listcust = new ListViewItem(r["id_pemesanan"].ToString());
                    listcust.SubItems.Add(r["nama_pembeli"].ToString());
                    listcust.SubItems.Add(r["jumlah_orang"].ToString());
                    listcust.SubItems.Add(r["type"].ToString());
                    listcust.SubItems.Add(r["total_harga"].ToString());
                    listcust.SubItems.Add(r["status"].ToString());
                    listView1.Items.Add(listcust);
                }
                r.Close();
            }
            else
            {
                MessageBox.Show("Tidak ada data customer");
            }
        }

        private void Customer_Load(object sender, EventArgs e)
        {
            string query = "SELECT * FROM stgk_pemesanan WHERE tanggal ='" + DateTime.Now.ToString("yyyy-MM-dd") + "' ";

            databaseConn.Open();
            MySqlCommand cmd = new MySqlCommand(query, databaseConn);
            cmd.CommandTimeout = 60;
            MySqlDataReader r = cmd.ExecuteReader();

            if (r.HasRows)
            {
                while (r.Read())
                {
                    ListViewItem listcust = new ListViewItem(r["id_pemesanan"].ToString());
                    listcust.SubItems.Add(r["nama_pembeli"].ToString());
                    listcust.SubItems.Add(r["jumlah_orang"].ToString());
                    listcust.SubItems.Add(r["type"].ToString());
                    listcust.SubItems.Add(r["total_harga"].ToString());
                    listcust.SubItems.Add(r["status"].ToString());
                    listView1.Items.Add(listcust);
                }
                r.Close();
            }
            else
            {
                MessageBox.Show("Tidak ada data customer");
            }
            databaseConn.Close();
        }

        ListViewItem cust;
        private void ListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string query = "SELECT * FROM stgk_pemesanan WHERE id_pemesanan = @id";

            if (listView1.SelectedItems.Count > 0)
            {
                cust = listView1.SelectedItems[0];
                string id = cust.SubItems[0].Text;

                databaseConn.Open();
                MySqlCommand cmd = new MySqlCommand(query, databaseConn);
                cmd.CommandTimeout = 60;
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                MySqlDataReader r = cmd.ExecuteReader();

                if (r.HasRows)
                {
                    while (r.Read())
                    {
                        id = r["id_pemesanan"].ToString();
                        textBox1.Text = r["nama_pembeli"].ToString();
                        textBox2.Text = r["jumlah_orang"].ToString();
                        if(r["type"].ToString() == "Dine In")
                        {
                            radioButton1.Checked = true;
                        }
                        else
                        {
                            radioButton2.Checked = true;
                        }
                        label4.Text = id;
                    }
                    r.Close();
                }
                else
                {
                    MessageBox.Show("Tidak ada data pelanggan yang bisa dipilih");
                }
            }
            else
            {
                textBox1.Text = "";
                textBox2.Text = "";
            }
            databaseConn.Close();
        }

        string tipe;
        private void Button2_Click(object sender, EventArgs e)
        {
            string query = "INSERT INTO stgk_pemesanan (nama_pembeli, jumlah_orang, type, tanggal) VALUES (@nama, @jumlah, @type, NOW());";
            if (radioButton1.Checked)
            {
                tipe = radioButton1.Text;
            }
            else
            {
                tipe = radioButton2.Text;
            }
            try
            {
                databaseConn.Open();
                MySqlCommand cmd = new MySqlCommand(query, databaseConn);
                cmd.Parameters.AddWithValue("@nama", textBox1.Text);
                cmd.Parameters.AddWithValue("@jumlah", textBox2.Text);
                cmd.Parameters.AddWithValue("@type", tipe);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Berhasil menambahkan customer");
                refresh();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                databaseConn.Close();
                textBox1.Text = "";
                textBox2.Text = "";
                text_cari.Text = "";
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            string query = "UPDATE stgk_pemesanan SET nama_pembeli = @nama, jumlah_orang = @jumlah, type = @type WHERE id_pemesanan = @id;";
            if (radioButton1.Checked)
            {
                tipe = radioButton1.Text;
            }
            else
            {
                tipe = radioButton2.Text;
            }
            string id = label4.Text;

            try
            {
                databaseConn.Open();
                MySqlCommand cmd = new MySqlCommand(query, databaseConn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@nama", textBox1.Text);
                cmd.Parameters.AddWithValue("@jumlah", textBox2.Text);
                cmd.Parameters.AddWithValue("@type", tipe);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Berhasil mengubah customer");
                refresh();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                databaseConn.Close();
                textBox1.Text = "";
                textBox2.Text = "";
                text_cari.Text = "";
            }
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            string query = "DELETE FROM stgk_pemesanan WHERE id_pemesanan = @id;";
            string id = label4.Text;
            try
            {
                databaseConn.Open();
                MySqlCommand cmd = new MySqlCommand(query, databaseConn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Berhasil menghapus customer");
                refresh();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                databaseConn.Close();
                textBox1.Text = "";
                textBox2.Text = "";
                text_cari.Text = "";
            }
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            Orderlist order = new Orderlist();
            order.Show();
        }

        private void Button7_Click(object sender, EventArgs e)
        {
                string query = "SELECT * FROM stgk_pemesanan WHERE nama_pembeli LIKE '%" + text_cari.Text + "%' AND tanggal = '"+DateTime.Now.ToString("yyyy-MM-dd")+"';";
            try
            {
                listView1.Items.Clear();
                databaseConn.Open();
                MySqlCommand cmd = new MySqlCommand(query, databaseConn);
                cmd.CommandTimeout = 60;
                MySqlDataReader r = cmd.ExecuteReader();
                if (r.HasRows)
                {
                    while (r.Read())
                    {
                        ListViewItem cust = new ListViewItem(r["id_pemesanan"].ToString());
                        cust.SubItems.Add(r["nama_pembeli"].ToString());
                        cust.SubItems.Add(r["jumlah_orang"].ToString());
                        cust.SubItems.Add(r["type"].ToString());
                        cust.SubItems.Add(r["total_harga"].ToString());
                        cust.SubItems.Add(r["status"].ToString());
                        listView1.Items.Add(cust);
                    }
                    r.Close();
                }
                else
                {
                    MessageBox.Show("Data tidak ditemukan");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                databaseConn.Close();
            }
        }
    }
}
