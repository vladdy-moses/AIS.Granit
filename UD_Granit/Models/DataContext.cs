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

        public DbSet<User> Users { set; get; }
        public DbSet<Applicant> Applicants { set; get; }

        public DbSet<Dissertation> Dissertations { set; get; }
        public DbSet<Reply> Replies { set; get; }
    }

    public enum UserRole: int
    {
        Applicant = 1,
        Member = 2,
        Secretary = 3,
        Chairman = 4,
        Administrator = 5
    };

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

        [Required]
        public UserRole Role { set; get; }
    }
}