using Aspose.Pdf;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Loja
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        string valor_co = "";
        bool n1 = true;
        bool n2 = false;
        private void Form2_Load(object sender, EventArgs e)
        {
            //estoque
            using (var cn = new MySqlConnection(Conn.strConn))
            {
                try
                {
                    cn.Open();
                    var veri = new MySqlCommand("SELECT * FROM produtos", cn);
                    var reader = veri.ExecuteReader();
                    if (reader.Read())
                    {
                        var nm_ = reader["nm_produto"].ToString();
                        var vl_ = reader["vl_produto"].ToString();
                        var qt_ = reader["qt_produto"].ToString();
                        label4.Text += nm_ + " R$ " + vl_ + " tem " + qt_ + "\n";
                        while (reader.Read())
                        {
                            string nm = reader.GetString(1);
                            string vl = reader.GetString(2);
                            string qt = reader.GetString(3);
                            label4.Text += nm + " R$ " + vl + " tem " + qt + "\n";
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Nenhum produto encontrado." + ex.Message);
                }
                cn.Close();
            }
            //vendas
            using (var cn = new MySqlConnection(Conn.strConn))
            {
                DateTime hora = DateTime.Now;
                try
                {
                    cn.Open();
                    var veri = new MySqlCommand("SELECT * FROM tb_lista WHERE dt_venda = @dt", cn);
                    veri.Parameters.AddWithValue("@dt", hora.Date);
                    var reader = veri.ExecuteReader();
                    if (reader.Read())
                    {
                        var li_ = reader["ds_lista"].ToString();
                        label6.Text += "\n" + li_ + "\n";
                        while (reader.Read())
                        {
                            string li = reader.GetString(3);
                            label6.Text += "\n" + li + "\n";
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Nenhum produto encontrado." + ex.Message);
                }
                cn.Close();
            }
            //datas
            using (var cn = new MySqlConnection(Conn.strConn))
            {
                DateTime hora = DateTime.Now;
                try
                {
                    cn.Open();
                    var veri = new MySqlCommand("SELECT * FROM tb_lista", cn);
                    var reader = veri.ExecuteReader();
                    if (reader.Read())
                    {
                        var nnm = reader["dt_venda"].ToString();
                        comboBox1.Items.Add(nnm);
                        while (reader.Read())
                        {
                            string nm = reader.GetString(1);
                            comboBox1.Items.Add(nm);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Nenhum produto encontrado." + ex.Message);
                }
                cn.Close();
            }
        }
        //cadastro dos produtos 
        private void button1_Click(object sender, EventArgs e)
        {
            string nome = textBox1.Text;
            string quanti = textBox2.Text;
            string valor = textBox5.Text;
            nome = nome.Trim();
            quanti = quanti.Trim();
            valor = valor.Trim();
            nome = nome.ToLower();
            quanti = quanti.ToLower();

            if (!string.IsNullOrWhiteSpace(nome) && !string.IsNullOrWhiteSpace(quanti) && !string.IsNullOrWhiteSpace(valor))
            {
                try
                {
                    using (var cn = new MySqlConnection(Conn.strConn))
                    {
                        cn.Open();
                        var veri = new MySqlCommand("SELECT * FROM produtos WHERE nm_produto = @nome", cn);
                        veri.Parameters.AddWithValue("@nome", nome);

                        var reader = veri.ExecuteReader();

                        if (reader.Read())
                        {
                            MessageBox.Show("Error: Produto já cadastrado.");
                            n1 = false;
                        }
                        cn.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error");
                }
                if (n1 == true)
                {
                    try
                    {
                        using (var cn1 = new MySqlConnection(Conn.strConn))
                        {
                            cn1.Open();
                            var cadas = new MySqlCommand("INSERT INTO produtos VALUES(null,@nome,@valor,@quanti)", cn1);
                            cadas.Parameters.AddWithValue("@nome", nome);
                            cadas.Parameters.AddWithValue("@valor", valor);
                            cadas.Parameters.AddWithValue("@quanti", quanti);
                            cadas.ExecuteNonQuery();
                            cn1.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
            else
            {

            }
        }
        //atualizar o valor e quantidade do estoque 
        private void button7_Click(object sender, EventArgs e)
        {
            string nome = textBox1.Text;
            string quanti = textBox2.Text;
            string valor = textBox5.Text;
            nome = nome.Trim();
            nome = nome.ToLower();
            if (!string.IsNullOrWhiteSpace(nome) && !string.IsNullOrWhiteSpace(quanti) && !string.IsNullOrWhiteSpace(valor))
            {
                try
                {
                    using (var cn3 = new MySqlConnection(Conn.strConn))
                    {
                        cn3.Open();
                        var up = new MySqlCommand("UPDATE produtos SET vl_produto = @valor, qt_produto = @quanti WHERE nm_produto = @nome", cn3);
                        up.Parameters.AddWithValue("@valor", valor);
                        up.Parameters.AddWithValue("@quanti", quanti);
                        up.Parameters.AddWithValue("@nome", nome);
                        up.ExecuteNonQuery();
                        cn3.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
        //atualizar a exibição do estoque 
        private void button2_Click(object sender, EventArgs e)
        {
            using (var cn = new MySqlConnection(Conn.strConn))
            {
                try
                {
                    cn.Open();
                    var veri = new MySqlCommand("SELECT * FROM produtos", cn);
                    var reader = veri.ExecuteReader();
                    if (reader.Read())
                    {
                        label4.Text = "";
                        var nm_ = reader["nm_produto"].ToString();
                        var vl_ = reader["vl_produto"].ToString();
                        var qt_ = reader["qt_produto"].ToString();
                        label4.Text += nm_ + " R$ " + vl_ + " tem " + qt_ + "\n";
                        while (reader.Read())
                        {
                            string nm = reader.GetString(1);
                            string vl = reader.GetString(2);
                            string qt = reader.GetString(3);
                            label4.Text += nm + " R$ " + vl + " tem " + qt + "\n";
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Nenhum produto encontrado." + ex.Message);
                }
                cn.Close();
            }
        }
        //muda a data das vendas que aparecem 
        private void button5_Click(object sender, EventArgs e)
        {
            using (var cn = new MySqlConnection(Conn.strConn))
            {
                DateTime hora = DateTime.Now;
                string data = comboBox1.Text;
                if (data != "")
                {
                    try
                    {
                        cn.Open();
                        var veri = new MySqlCommand("SELECT * FROM tb_lista WHERE dt_venda = @dt", cn);
                        veri.Parameters.AddWithValue("@dt", data);
                        var reader = veri.ExecuteReader();
                        if (reader.Read())
                        {
                            label6.Text = "";
                            var li_ = reader["ds_lista"].ToString();
                            label6.Text += "\n" + li_ + "\n";
                            while (reader.Read())
                            {
                                string li = reader.GetString(3);
                                label6.Text += "\n" + li + "\n";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: Nenhum produto encontrado." + ex.Message);
                    }
                    cn.Close();
                }

            }
        }
        //baixa manual
        private void button4_Click(object sender, EventArgs e)
        {
            string nome = textBox4.Text;
            string quanti = textBox3.Text;
            nome = nome.Trim();
            quanti = quanti.Trim();
            nome = nome.ToLower();
            quanti = quanti.ToLower();
            if (!string.IsNullOrWhiteSpace(nome) && !string.IsNullOrWhiteSpace(quanti))
            {
                try
                {
                    using (var cn2 = new MySqlConnection(Conn.strConn))
                    {
                        cn2.Open();
                        var veri2 = new MySqlCommand("SELECT * FROM produtos WHERE nm_produto = @nome", cn2);
                        veri2.Parameters.AddWithValue("@nome", nome);
                        var reader2 = veri2.ExecuteReader();
                        if (reader2.Read())
                        {
                            var dado = reader2["qt_produto"].ToString();
                            int valor = int.Parse(dado);
                            int quanti_int = int.Parse(quanti);
                            int ok = valor - quanti_int;
                            valor_co = ok.ToString();
                            n2 = true;
                        }
                        else
                        {
                            n2 = false;
                        }
                        cn2.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }

                if (n2 == true)
                {
                    try
                    {
                        using (var cn3 = new MySqlConnection(Conn.strConn))
                        {
                            cn3.Open();
                            var vl_up = valor_co;
                            var up = new MySqlCommand("UPDATE produtos SET qt_produto = @valor WHERE nm_produto = @nome", cn3);
                            up.Parameters.AddWithValue("@valor", vl_up);
                            up.Parameters.AddWithValue("@nome", nome);
                            up.ExecuteNonQuery();
                            cn3.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
        }
        //atualizar os item da comboBox 
        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            string n1 = comboBox1.Text;
            using (var cn = new MySqlConnection(Conn.strConn))
            {
                try
                {
                    cn.Open();
                    var like = new MySqlCommand("SELECT * FROM tb_lista WHERE dt_venda LIKE @li", cn);
                    like.Parameters.AddWithValue("@li", "%" + n1 + "%");
                    var realike = like.ExecuteReader();
                    if (realike.Read())
                    {
                        while (realike.Read())
                        {
                            string nm = realike.GetString(1);
                            comboBox1.Items.Add(nm);
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                cn.Close();
            }
        }
        // atualizar a senha 
        private void button6_Click(object sender, EventArgs e)
        {
            Form4 novoForm = new Form4();
            novoForm.Show();
        }
        //dar baixa altomatica pela data de hoje
        private void button3_Click(object sender, EventArgs e)
        {
            DateTime hora = DateTime.Now;
            try
            {
                // Lista para armazenar os produtos para atualizar
                var produtosParaAtualizar = new List<(string Nome, string QuantidadeAtualizada, string HrVenda)>();

                using (var cn = new MySqlConnection(Conn.strConn))
                {
                    cn.Open();

                    // Consulta para obter as quantidades atualizadas
                    using (var cmd = new MySqlCommand("SELECT p.nm_produto, p.qt_produto, u.hr_venda, u.qt_produto AS qt_update FROM produtos p JOIN tb_update u ON p.nm_produto = u.nm_produto WHERE u.dt_produto = @dt", cn))
                    {
                        cmd.Parameters.AddWithValue("@dt", hora.Date);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string nomeProduto = reader["nm_produto"].ToString();
                                string qt_ini = reader["qt_produto"].ToString();
                                string qt_nega = reader["qt_update"].ToString();
                                string hr_venda = reader["hr_venda"].ToString();
                                double ini = double.Parse(qt_ini);
                                double nega = double.Parse(qt_nega);
                                double n1 = ini - nega;
                                string qtAtualizada = n1.ToString();
                                produtosParaAtualizar.Add((nomeProduto, qtAtualizada, hr_venda));
                            }
                        }
                    }


                    // Atualiza os produtos com base nas informações obtidas
                    foreach (var produto in produtosParaAtualizar)
                    {
                        string nomeProduto = produto.Nome;
                        string qtAtualizada = produto.QuantidadeAtualizada;
                        string hr = produto.HrVenda;

                        using (var cmdUpdate = new MySqlCommand("UPDATE produtos SET qt_produto = @qtAtualizada WHERE nm_produto = @nomeProduto", cn))
                        {
                            cmdUpdate.Parameters.AddWithValue("@qtAtualizada", qtAtualizada);
                            cmdUpdate.Parameters.AddWithValue("@nomeProduto", nomeProduto);
                            cmdUpdate.ExecuteNonQuery();
                        }
                        using (var cmdUpdate = new MySqlCommand("DELETE FROM tb_update WHERE dt_produto = @dt AND hr_venda = @hr AND nm_produto = @nomeProduto", cn))
                        {
                            cmdUpdate.Parameters.AddWithValue("@dt", hora.Date);
                            cmdUpdate.Parameters.AddWithValue("@hr", hr);
                            cmdUpdate.Parameters.AddWithValue("@nomeProduto", nomeProduto);
                            cmdUpdate.ExecuteNonQuery();
                        }
                    }
                    label6.Text = "";
                }//2024-08-05 00:00:00

            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        //filtrar o estoque por nome
        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            string n1 = textBox6.Text;
            using (var cn = new MySqlConnection(Conn.strConn))
            {
                try
                {
                    cn.Open();
                    var like = new MySqlCommand("SELECT * FROM produtos WHERE nm_produto LIKE @li", cn);
                    like.Parameters.AddWithValue("@li", "%" + n1 + "%");
                    var realike = like.ExecuteReader();
                    if (realike.Read())
                    {
                        label4.Text = "";
                        var nm_ = realike["nm_produto"].ToString();
                        var vl_ = realike["vl_produto"].ToString();
                        var qt_ = realike["qt_produto"].ToString();
                        label4.Text += nm_ + " R$ " + vl_ + " tem " + qt_ + "\n";
                        while (realike.Read())
                        {
                            string nm = realike.GetString(1);
                            string vl = realike.GetString(2);
                            string qt = realike.GetString(3);
                            label4.Text += nm + " R$ " + vl + " tem " + qt + "\n";
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                cn.Close();
            }
        
        }
    }
}
