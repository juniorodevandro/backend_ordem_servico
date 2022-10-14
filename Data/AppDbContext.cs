using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Ordem> Ordem { get; set; }
        public DbSet<OrdemItem> OrdemItem { get; set; }
        public DbSet<OrdemServico> OrdemServico { get; set; }
        public DbSet<Estoque> Estoque { get; set; }
        public DbSet<Item> Item { get; set; }
        public DbSet<Pessoa> Pessoa { get; set; }
    }
}
