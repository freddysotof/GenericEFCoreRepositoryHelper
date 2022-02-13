using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Context
{
    public class RepositoryDbContext : DbContext
    {
        private readonly IConfiguration _configuration;
        private readonly IHostEnvironment _env;
        public RepositoryDbContext(DbContextOptions<RepositoryDbContext> options,IConfiguration configuration, IHostEnvironment env)
        : base((DbContextOptions)options)
        {
            _configuration = configuration;
            _env = env;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EntityLog>()
            .ToTable("DotNetCoreLog", "dbo")
            //.HasKey(x => x.LogId)
            .Property(x => x.Id).HasColumnName("LogId");
           
        }

        public virtual DbSet<Log> Logs { get; set; }
        public virtual DbSet<EntityLog> EntityLogs { get; set; }

    }
}
