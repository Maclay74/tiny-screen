using System;

namespace TinyScreen.framework.interfaces {
    
    public interface ISettingsInterface {
        bool IsAppInstalled();

        void InstallApp();

        void Set(Setting setting);

        string Get(Setting setting);
    }

    public enum Setting {
        Author
    }
    
    public abstract class SettingType<T> {

        protected T _value;
        
        T Value { get => _value; }
        
        public abstract string Serialize();
        
        public abstract void Unserialize(string value);

    }

    public class SettingNumber : SettingType<int> {
        public override string Serialize() => _value.ToString();
        
        public override void Unserialize(string value) {
            _value = Int32.Parse(value);
        }
    }

    public class SettingString : SettingType<string> {
        public override string Serialize() => _value;
        
        public override void Unserialize(string value) {
            _value =value;
        }
    }
}