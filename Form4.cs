using Aspose.Pdf;
using Aspose.Pdf.Operators;
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
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }
        string novo_hash;
        string hash;
        bool n1 = false;
        private void button1_Click(object sender, EventArgs e)
        {
            string senha_antiga = this.textBox1.Text;
            string senha_nova = this.textBox2.Text;
            string senha_conf = this.textBox3.Text;
            if (senha_antiga == "" || senha_nova == "" || senha_conf == "")
            {
                label5.Text = "Preencha todos os campos";
            }
            else
            {
                try
                {
                    using (var cn = new MySqlConnection(Conn.strConn))
                    {
                    
                        cn.Open();
                        string query = "SELECT sn_user FROM usuarios";
                        var dado = new MySqlCommand(query, cn);
                        var reader = dado.ExecuteReader();
                        if (reader.Read())
                        {
                            var has = reader["sn_user"].ToString();
                            hash = has;
                            if (BCrypt.Net.BCrypt.Verify(senha_antiga, hash))
                            {
                                if (senha_nova == senha_conf)
                                {
                                    novo_hash = BCrypt.Net.BCrypt.HashPassword(senha_nova);
                                    n1 = true;
                                }
                                else
                                {
                                    label5.Text = "A senha nova é diferente da confirmação";
                                }
                            }
                            else
                            {
                                label5.Text = "SENHA INCORRETA!";
                            }
                        }
                    
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
                        using (var cn = new MySqlConnection(Conn.strConn))
                        {

                            cn.Open();
                            string query = "UPDATE usuarios SET sn_user = @valor WHERE sn_user = @nome";
                            var dado = new MySqlCommand(query, cn);
                            dado.Parameters.AddWithValue("@valor", novo_hash);
                            dado.Parameters.AddWithValue("@nome", hash);
                            var reader = dado.ExecuteNonQuery();
                            cn.Close();
                            label5.Text = "Senha atualizada";
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error");
                    }
                }
            }
        }
    }
}
