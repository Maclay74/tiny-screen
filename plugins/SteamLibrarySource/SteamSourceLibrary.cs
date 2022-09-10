using System;
using TinyScreen.Framework.Interfaces;

namespace SteamLIbrarySource
{
    public class SteamSourceLibrary: ILibrarySource
    {
        public string GetName()
        {
            return "Steam";
        }
    }
}
