using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace Dashboard.Models
{
    public class UserFriends
    {
        [Key]
        public long userFriendId { get; set; }

        public long friendId { get; set; }
        public User friend {get; set; }
        public long userId { get; set; }
        public User user { get; set; }

    }
}
