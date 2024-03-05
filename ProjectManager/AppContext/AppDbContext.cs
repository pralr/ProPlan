using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Models;
using ProjectManager.ViewModels;

namespace ProjectManager.AppContext
{
    //public class AppDbContext : IdentityDbContext<IdentityUser>
    //{
    //    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
    //    { 
    //    }

    //    public DbSet<Projeto> Projetos { get; set; }
    //    public DbSet<Atividade> Atividades { get; set; }
    //}

    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Projeto> Projetos { get; set; }
        public DbSet<Atividade> Atividades { get; set; }
        public DbSet<ApplicationUser> Usuarios { get; set; }
        public DbSet<PostagemFeed> PostagensFeed { get; set; }
        
        }
    }
