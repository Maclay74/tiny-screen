using System;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using TinyScreen.Framework.Interfaces;
using TinyScreen.Models;

namespace TinyScreen.Services;

public class EfcDatabaseService : DbContext, IDatabaseService {
    public DbSet<Game> Games { get; set; }
    public DbSet<Folder> Folders { get; set; }
    public DbSet<LibrarySource> LibrarySources { get; set; }

    public EfcDatabaseService() : base() { }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        optionsBuilder.UseSqlite(@"DataSource=application.db;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) { }
    public bool Exists() {
        return ((RelationalDatabaseCreator)Database.GetService<IDatabaseCreator>()).Exists();
    }

    public void InitDatabase() {
        Database.EnsureCreated();
        Database.Migrate();
        Console.WriteLine("MIGRATED");
    }
}