using Dashboard.Models.Database;
using MySql.Data.MySqlClient;
using MySqlConnector;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Dashboard.Models
{
    public class Admin
    {
    
        [Key]
        public long adminId { get; set; }
        public string name { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string mail { get; set; }
    }
}
