﻿using System.Diagnostics;
using System.Windows;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace MvvmExample;

/// <summary>
/// 
/// </summary>
public partial class MainWindow : Window
{
    private MainWindowViewModel ViewModel => (MainWindowViewModel)DataContext;
    public MainWindow()
    {
        DataContext = new MainWindowViewModel();
        InitializeComponent();
    }


    // The following code enables global hotkeys


    [DllImport("user32.dll")]
    private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

    [DllImport("user32.dll")]
    private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

    private const int HOTKEY_ID = 9_876;

    //Modifiers:
    private const uint MOD_NONE = 0x0000; //(none)
    private const uint MOD_ALT = 0x0001; //ALT
    private const uint MOD_CONTROL = 0x0002; //CTRL
    private const uint MOD_SHIFT = 0x0004; //SHIFT
    private const uint MOD_WIN = 0x0008; //WINDOWS
    private const uint VK_CAPITAL = 0x14; //CAPS LOCK:
    private const uint VK_W = 0x57; //CAPS LOCK:
    private IntPtr _windowHandle;
    private HwndSource _source;
    protected override void OnSourceInitialized(EventArgs e)
    {
        base.OnSourceInitialized(e);

        _windowHandle = new WindowInteropHelper(this).Handle;
        _source = HwndSource.FromHwnd(_windowHandle);
        _source.AddHook(HwndHook);

        RegisterHotKey(_windowHandle, HOTKEY_ID, MOD_CONTROL, VK_W); //CTRL + CAPS_LOCK
    }

    private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        const int WM_HOTKEY = 0x0312;
        switch (msg)
        {
            case WM_HOTKEY:
                switch (wParam.ToInt32())
                {
                    case HOTKEY_ID:
                        int vkey = (((int)lParam >> 16) & 0xFFFF);
                        if (vkey == VK_W)
                        {
                            Debug.WriteLine("Ctrl + W was pressed");
                        }
                        handled = true;
                        break;
                }
                break;
        }
        return IntPtr.Zero;
    }

    protected override void OnClosed(EventArgs e)
    {
        _source.RemoveHook(HwndHook);
        UnregisterHotKey(_windowHandle, HOTKEY_ID);
        base.OnClosed(e);
    }

    // End of Global Hot Keys code
}
