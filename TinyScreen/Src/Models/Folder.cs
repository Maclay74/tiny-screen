using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace TinyScreen.Models;

public class Folder {
    public int Id { get; set; }

    public string Name { get; set; }

    public string Artwork { get; set; }

    public ICollection<Game> Games {
        get => LazyLoader.Load(this, ref _games);
        set => _games = value;
    }

    private ICollection<Game> _games;

    private ILazyLoader LazyLoader { get; set; }

    public Folder() { }

    private Folder(ILazyLoader lazyLoader) {
        LazyLoader = lazyLoader;
    }
}