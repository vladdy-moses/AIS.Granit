using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace UD_Granit.Models
{
    public class DataContext : DbContext
    {
        public DataContext() : base("DefaultConnection") { }

        public DbSet<User> Users;
    }

    public class User
    {
        [Key]
        public int User_Id { set; get; }

        [Required]
        public string Email { set; get; }
        [Required]
        public string Password { set; get; }

        [Required]
        public string FirstName { set; get; }
        [Required]
        public string SecondName { set; get; }
        public string LastName { set; get; }
    }
}