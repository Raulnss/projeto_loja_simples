using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loja
{
    internal class Conn
    {
        static private string server = "localhost";
        static private string banco = "loja_andre";
        static private int port = 3306;
        static private string user = "root";
        static private string pass = "";
        static public string strConn = $"server={server};port={port};User Id={user};database={banco};password={pass}";
    }
}
