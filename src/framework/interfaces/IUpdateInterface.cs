using System;
using System.Threading.Tasks;

namespace TinyScreen.Framework.Interfaces {
    public interface IUpdateInterface {
        
        Version GetCurrentVersion();

        Task<Version> GetLatestVersion();

        Task<long?> GetUpdateSize();

        Task DownloadLatestUpdate(EventHandler<float> onProgress);

        void Install();
        
    }
}