using Aspose.Pdf;
using MySql.Data.MySqlClient;
using System.Xml;

namespace Loja
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string nada;
        double val = 0;
        List<string> nomes = new List<string>();
        static public List<string> Nomes = new List<string>();
        string n_p;
        string q_p;
        //carrega os items no combobox
        private void Form1_Load(object sender, EventArgs e)
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
                        var nnm = reader["nm_produto"].ToString();
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
        //adiciona os produtos vendidos na label e no banco de dados 
        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "" && textBox1.Text != "")
            {
                nada += "\n" + comboBox1.Text + " " + textBox1.Text;
                label5.Text = nada;
                string v = comboBox1.Text;
                n_p = comboBox1.Text;
                double vi = double.Parse(textBox1.Text);
                string li = textBox1.Text;
                q_p = textBox1.Text;
                DateTime hora = DateTime.Now;
                // pega o valor e exibe o valor total
                using (var cn = new MySqlConnection(Conn.strConn))
                {
                    try
                    {
                        cn.Open();
                        var like = new MySqlCommand("SELECT vl_produto FROM produtos WHERE nm_produto = @nome", cn);
                        like.Parameters.AddWithValue("@nome", v);
                        var realike = like.ExecuteReader();
                        if (realike.Read())
                        {
                            var vvl = realike["vl_produto"].ToString();
                            double n1 = double.Parse(vvl);
                            val = val + (vi * n1);
                            string n2 = val.ToString();
                            label6.Text = "Preço total " + n2 + "R$";
                        }
                        else
                        {
                            MessageBox.Show("Error: Preço não encontrado");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error");
                    }
                    cn.Close();
                }
                // insere os produtos no banco de dados
                string n1_B = "";
                try
                {
                    using (var cn = new MySqlConnection(Conn.strConn))
                    {
                        cn.Open();
                        var veri = new MySqlCommand("SELECT * FROM tb_update WHERE nm_produto = @nome", cn);
                        veri.Parameters.AddWithValue("@nome", v);

                        var reader = veri.ExecuteReader();

                        if (reader.Read())
                        {
                            n1_B = "ja_existe";
                        }
                        else
                        {
                            n1_B = "nao_existe";
                        }
                        cn.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error");
                }
                if (n1_B == "nao_existe")
                {
                    try
                    {
                        using (var cn1 = new MySqlConnection(Conn.strConn))
                        {
                            cn1.Open();
                            var like = new MySqlCommand("INSERT INTO tb_update VALUES(null,@da,@hr,@pr,@qu)", cn1);
                            like.Parameters.AddWithValue("@da", hora.Date);
                            like.Parameters.AddWithValue("@hr", hora.TimeOfDay);
                            like.Parameters.AddWithValue("@pr", v);
                            like.Parameters.AddWithValue("@qu", li);
                            like.ExecuteNonQuery();
                            cn1.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
                else
                {
                    string vl_fin = "";

                        using (var cn1 = new MySqlConnection(Conn.strConn))
                        {
                            cn1.Open();

                            using ( var like1 = new MySqlCommand("SELECT qt_produto FROM tb_update WHERE nm_produto = @nome", cn1))
                            {
                                like1.Parameters.AddWithValue("@nome", v);
                                using (var reader = like1.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                    string vl_ini = reader["qt_produto"].ToString();
                                    double Dvl_ini = double.Parse(vl_ini);
                                    double vl_adic = double.Parse(li);
                                    double Dvl_fin = Dvl_ini + vl_adic;
                                    vl_fin = Dvl_fin.ToString();
                                    }
                                }

                                
                            }

                            using ( var like = new MySqlCommand("UPDATE tb_update SET qt_produto = @qtAtualizada WHERE nm_produto = @nomeProduto", cn1))
                            {
                                like.Parameters.AddWithValue("@qtAtualizada", vl_fin);
                                like.Parameters.AddWithValue("@nomeProduto", v);
                                like.ExecuteNonQuery();
                            }

                            cn1.Close();
                        }
                }
            }

        }
        //adiona a lista de produtos no banco de dados e gera um XML 
        private void button1_Click(object sender, EventArgs e)
        {
            DateTime hora = DateTime.Now;
            string nome = textBox2.Text;
            nome = nome.Trim();
            string pro = comboBox1.Text;
            pro = pro.Trim();
            string qua = textBox1.Text;
            qua = qua.Trim();
            string paga = comboBox2.Text;
            paga = paga.Trim();
            string npro = label5.Text;
            npro = npro.Trim();
            if (label5.Text != "" && comboBox1.Text != "" && textBox1.Text != "" && comboBox2.Text != "")
            {
                string compras = textBox2.Text + label5.Text + "\n" + comboBox2.Text + "\n" + label6.Text + "\n" + hora.Date + " " + hora.TimeOfDay;
                using (var cn = new MySqlConnection(Conn.strConn))
                {
                    try
                    {
                        cn.Open();
                        var like = new MySqlCommand("INSERT INTO tb_lista VALUES(null,@da,@hr,@pr)", cn);
                        like.Parameters.AddWithValue("@da", hora.Date);
                        like.Parameters.AddWithValue("@hr", hora.TimeOfDay);
                        like.Parameters.AddWithValue("@pr", compras);
                        like.ExecuteNonQuery();
                        cn.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error");
                    }
                } 
                label5.Text = "";
                comboBox1.Text = "";
                textBox1.Text = "1";
                comboBox2.Text = "";
                textBox2.Text = "";
                nada = "";
                val = 0;
                label6.Text = "";
                try
                {
                    string hd = hora.Date.ToString();
                    string hh = hora.TimeOfDay.ToString();
                    // Cria um novo objeto XmlWriter
                    XmlWriter writer = XmlWriter.Create("produtos.xml");

                    // Escreve a declaração XML
                    writer.WriteStartDocument();

                    // Escreve o elemento raiz "produtos"
                    writer.WriteStartElement("produtos");

                    // Escreve o elemento "nome"
                    writer.WriteStartElement("nome");
                    writer.WriteString(textBox2.Text);
                    writer.WriteEndElement();

                    writer.WriteStartElement("lista_produto");
                    writer.WriteString(label5.Text);
                    writer.WriteEndElement();

                    writer.WriteStartElement("pagamento");
                    writer.WriteString(comboBox2.Text);
                    writer.WriteEndElement();

                    writer.WriteStartElement("total");
                    writer.WriteString(label6.Text);
                    writer.WriteEndElement();

                    writer.WriteStartElement("dia");
                    writer.WriteString(hd);
                    writer.WriteEndElement();

                    writer.WriteStartElement("hora");
                    writer.WriteString(hh);
                    writer.WriteEndElement();

                    // Fecha o elemento raiz "produtos"
                    writer.WriteEndElement();

                    // Fecha o documento XML
                    writer.WriteEndDocument();

                    // Fecha o objeto XmlWriter
                    writer.Close();
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                using (var cn = new MySqlConnection(Conn.strConn))
                {
                    try
                    {
                        cn.Open();
                        var like = new MySqlCommand("DELETE FROM tb_update WHERE nm_produto = @pr AND qt_produto = @qu ", cn);
                        like.Parameters.AddWithValue("@pr", n_p);
                        like.Parameters.AddWithValue("@qu", q_p);
                        like.ExecuteNonQuery();
                        cn.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error");
                    }
                    label5.Text = "";
                    nada = "";
                    val = 0;
                    label6.Text = "";
                }
                MessageBox.Show("Preencha todos os campos obrigatorios \n (produtos ; quantidade ; forma de pagamento) \n venda cancelada reinicie o procedimento!");
            }
        }
        //atualiza os item do comboBox com os novos valores digitados 
        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            string n1 = comboBox1.Text;
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
                        /* comboBox1.Items.Clear();
                         var nnm = realike["nm_produto"].ToString();
                         var vvl = realike["vl_produto"].ToString();
                         comboBox1.Items.Add(nnm);*/
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
        //isso foi um acidente e pode ser ignorado
        private void label6_Click(object sender, EventArgs e)
        {

        }
        //remove os produtos selecionados erroneamente tanto do banco de dados quanto da interface
        private void button3_Click(object sender, EventArgs e)
        {
            //Remove
            using (var cn = new MySqlConnection(Conn.strConn))
            {
                try
                {
                    cn.Open();
                    var like = new MySqlCommand("DELETE FROM tb_update WHERE nm_produto = @pr AND qt_produto = @qu ", cn);
                    like.Parameters.AddWithValue("@pr", n_p);
                    like.Parameters.AddWithValue("@qu", q_p);
                    like.ExecuteNonQuery();
                    cn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error");
                }
            }
            // Encontra a posição do último '\n'
            int ultimaPosicaoQuebraDeLinha = nada.LastIndexOf('\n');

            // Extrai a substring até a posição do último '\n'
            string primeiraString = nada.Substring(0, ultimaPosicaoQuebraDeLinha);
            nada = primeiraString;
            label5.Text = nada;
        }
        //vai para pagina de administrador 
        private void button4_Click(object sender, EventArgs e)
        {
            Form3 novoForm = new Form3();
            novoForm.Show();
        }
    }
}