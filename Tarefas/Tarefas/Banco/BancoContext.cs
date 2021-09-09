using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Tarefas.Model;

namespace Tarefas.Banco
{
    public class BancoContext : DbContext
    {
        public DbSet<Tarefa> Tarefas { get; set; }

        public BancoContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={Constants.CaminhoDoBanco}");
        }
    }
}
