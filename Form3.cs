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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Loja
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string senha = this.textBox1.Text;
            if (senha == "")
            {

            }
            else
            {
                 using (var cn = new MySqlConnection(Conn.strConn))
                 {
                    try
                    {
                        cn.Open();
                        string query = "SELECT sn_user FROM usuarios";
                        var dado = new MySqlCommand(query, cn);
                        var reader = dado.ExecuteReader();
                        if (reader.Read())
                        {
                            var hash = reader["sn_user"].ToString();
                            if (BCrypt.Net.BCrypt.Verify(senha, hash))
                            {
                                Form2 novoForm = new Form2();
                                novoForm.Show();
                            }
                            else
                            {
                                label2.Text = "SENHA INCORRETA!";
                            }
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
