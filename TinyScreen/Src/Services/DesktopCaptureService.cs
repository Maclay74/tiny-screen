using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace TinyScreen.Services;

// This service is supposed to capture the screen before showing blurred UI with overlay.
// It won't work in realtime, because it captures the whole screen
// The best possible solution would be detecting focused application before showing the overlay
// and capturing this specific window
// c# screenshot - https://stackoverflow.com/questions/1163761/capture-screenshot-of-active-window/24879511#24879511

public class DesktopCaptureService {
    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
    public static extern IntPtr GetDesktopWindow();

    [StructLayout(LayoutKind.Sequential)]
    private struct Rect {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    [DllImport("user32.dll")]
    private static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rect rect);

    public static Image CaptureDesktop() {
        return CaptureWindow(GetDesktopWindow());
    }

    public static Bitmap CaptureActiveWindow() {
        return CaptureWindow(GetForegroundWindow());
    }

    public static Bitmap CaptureWindow(IntPtr handle) {
        var rect = new Rect();
        GetWindowRect(handle, ref rect);
        var bounds = new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
        var result = new Bitmap(bounds.Width, bounds.Height);

        using var graphics = Graphics.FromImage(result);
        graphics.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);

        return result;
    }
}