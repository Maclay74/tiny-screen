using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Common.Extensions;
using TinyScreen.Framework.Interfaces;

namespace TinyScreen.Services; 

public class LocalUpdateService: IUpdateService {
        
    private string archiveUrl = "https://cdn.ayaneo.com/ayaneo/downloads/AYASpaceGlobalSetup1.0.1.144.zip";
        
    public Version GetCurrentVersion() {
        return Assembly.GetExecutingAssembly().GetName().Version;
    }

    public async Task<Version> GetLatestVersion() {
        return new Version(1, 2);
    }

    public async Task<long?> GetUpdateSize() {
            
        var client = new HttpClient();
        var head = await client.GetAsync(archiveUrl, HttpCompletionOption.ResponseHeadersRead);
        if (head.Content.Headers.Contains("Content-Length"))
            return head.Content.Headers.ContentLength;
            
        return null;
    }

    public async Task DownloadLatestUpdate(EventHandler<float> onProgress) {
            
        var client = new HttpClient();
        var filePath = Path.Combine (Path.GetTempPath (), "tiny-screen-latest.zip");
            
        var progress = new Progress<float> ();
        progress.ProgressChanged += onProgress;
            
        using (var file = new FileStream (filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            await client.DownloadDataAsync (archiveUrl, file, progress);

    }

    public void Install() {
            
        // TODO implement
        // 0. Close currently running application
        // 1. Check is there is an archive downloaded for update
        // 2. Unzip it to the application folder
        // 3. Run application
        throw new NotImplementedException();
    }
}