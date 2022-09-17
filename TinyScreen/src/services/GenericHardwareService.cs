using System;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using TinyScreen.Framework.Interfaces;

namespace TinyScreen.Services {
    public class GenericHardwareService : IHardwareService {
        
        
         public bool IsOnline() {
            // Isn't ver reliable.. Probably reimplement with more specific condition
            return InternetGetConnectedState(out _, 0);
        }

        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);
        
    }
}