using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace TinyScreen.Models;

public class Game {
    public int Id { get; set; }

    public string OriginalId { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }
    public string Artwork { get; set; }

    public string Background { get; set; }

    public LibrarySource Source { get; set; }

    public DateTime? LastPlayed { get; set; }

    public ICollection<Folder> Folders {
        get => LazyLoader.Load(this, ref _folders);
        set => _folders = value;
    }

    private ICollection<Folder> _folders;

    private ILazyLoader LazyLoader { get; set; }

    public Game() { }

    private Game(ILazyLoader lazyLoader) {
        LazyLoader = lazyLoader;
    }
}