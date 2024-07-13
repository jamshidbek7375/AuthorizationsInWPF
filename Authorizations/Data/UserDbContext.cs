using Authorizations.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.ApplicationServices;
using System;

namespace Authorizations.Data;

internal class UserDbContext:DbContext
{
    public virtual DbSet<UserAccount> UserAccounts { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Data Source=DESKTOP-J7QN3MV\\SQLEXPRESS;Initial Catalog=UserDb;Integrated Security=True; TrustServerCertificate=True");
        base.OnConfiguring(optionsBuilder);
    }
    public UserDbContext() { }  
    
}
