using System;

namespace TinyScreen.Framework.Interfaces {
    
    public interface ISettingsInterface {
        bool IsAppInstalled();

        void InstallApp();

        void Set(Setting setting, string value);

        string Get(Setting setting);
    }

    public enum Setting {
        Author
    }
}