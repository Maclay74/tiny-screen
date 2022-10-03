using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Extensions {
    public static class ProcessExtensions {
        public static async Task<int> WaitForExitAsync(this Process process,
            CancellationToken cancellationToken = default) {
            var tcs = new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously);

            void Process_Exited(object sender, EventArgs e) {
                tcs.TrySetResult(process.ExitCode);
            }

            try {
                process.EnableRaisingEvents = true;
            }
            catch (InvalidOperationException) when (process.HasExited) {
                // This is expected when trying to enable events after the process has already exited.
                // Simply ignore this case.
                // Allow the exception to bubble in all other cases.
            }

            using (cancellationToken.Register(() => tcs.TrySetCanceled())) {
                process.Exited += Process_Exited;

                try {
                    if (process.HasExited) {
                        tcs.TrySetResult(process.ExitCode);
                    }

                    return await tcs.Task.ConfigureAwait(false);
                }
                finally {
                    process.Exited -= Process_Exited;
                }
            }
        }
    }
}