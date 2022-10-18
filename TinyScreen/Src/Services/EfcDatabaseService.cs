using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using TinyScreen.Framework.Interfaces;
using TinyScreen.Models;

namespace TinyScreen.Services;

public class EfcDatabaseService : DbContext, IDatabaseService {
    private DbSet<Game>? Games { get; set; }
    
    public DbSet<Folder>? Folders { get; set; }
    
    public DbSet<Settings>? Settings { get; set; }
    private DbSet<LibrarySource>? LibrarySources { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        optionsBuilder
            .UseSqlite(@"DataSource=application.db;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) { }
    public bool Exists() => ((RelationalDatabaseCreator)Database.GetService<IDatabaseCreator>()).Exists();

    public void InitDatabase() {
        Database.Migrate();
    }

    // Library sources

    public void Add(LibrarySource source) {
        LibrarySources?.Add(source);
        SaveChanges();
    }

    public LibrarySource? GetLibrarySourceByName(string name) {
        return LibrarySources?.FirstOrDefault(s => s.Name == name);
    }

    public void Delete(LibrarySource source) {
        LibrarySources?.Remove(source);
        SaveChanges();
    }

    public void DeleteRange(List<LibrarySource> sources) {
        LibrarySources?.RemoveRange(sources);
        SaveChanges();
    }

    // Games

    public void Add(Game source) {
        Games?.Add(source);
        SaveChanges();
    }

    public List<Game>? GetAllGames(LibrarySource source) {
        return Games?.Where(g => g.Source.Id == source.Id).ToList();
    }
    
    public List<Game>? GetAllGames() {
        return Games?.ToList();
    }
    
    public Game? GetGame(int id) {
        return Games?.FirstOrDefault(f => f.Id == id);
    }

    public void Delete(Game game) {
        Games?.Remove(game);
        SaveChanges();
    }

    public void DeleteRange(List<Game> games) {
        Games?.RemoveRange(games);
        SaveChanges();
    }

    public void DeleteByOriginalIds(List<string> originalIds) {
        var games = Games?.AsQueryable().Where(g => originalIds.Contains(g.OriginalId));
        if (games != null && games.Any())
            Games?.RemoveRange(games);
    }
    
    // Settings
    
    public void SetSettings(string name, string value) {
        var record = Settings.FirstOrDefault(s => s.Name == name);
        
        if (record == null) {
            Settings.Add(new Settings {
                Name = name,
                Value = value
            });
        }
        else {
            record.Value = value;
            Settings.Update(record);
        }

        SaveChanges();
    }

    public string GetSettings(string name) {
        return Settings.FirstOrDefault(s => s.Name == name)?.Value;
    }
    
    // Folders

    public List<Folder>? GetAllFolders() {
        return Folders?.ToList();
    }

    public Folder? GetFolder(int id) {
        return Folders?.FirstOrDefault(f => f.Id == id);
    }
}