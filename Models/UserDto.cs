using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreApp.Models
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime? DOB { get; set; }
        public string Image { get; set; }
    }
}
