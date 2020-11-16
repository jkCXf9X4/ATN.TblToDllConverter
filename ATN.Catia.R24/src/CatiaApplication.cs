
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

using Dassault.Catia.R24.INFITF;


namespace ATN.Catia.R24.COM
{
    public class CatiaApplication
    {
        private static Lazy<CatiaApplication> lazy = null;

        public static Application Instance
        {
            get
            {
                if (lazy == null)
                {
                    lazy = new Lazy<CatiaApplication>(() => new CatiaApplication());
                }
                return lazy.Value.catia;
            }
        }

        private Application catia = null;

        private CatiaApplication()
        {
            try
            {
                catia = (Application)Marshal.GetActiveObject("CATIA.Application");
                // catia.Visible = true;
            }
            catch
            {
                try
                {
                    catia = (Application)Activator.CreateInstance(Type.GetTypeFromProgID("CATIA.Application"));
                    // catia.Visible = true;
                }
                catch
                {
                    throw new Exception("No application found");
                }
            }
        }

        ~CatiaApplication()
        {
            Marshal.ReleaseComObject(catia);
            catia = null;
        }

        public static IntPtr GetHandle()
        {
            Process[] p = Process.GetProcessesByName("CNEXT");
            return p.FirstOrDefault().MainWindowHandle;
        }

        [DllImport("User32.dll", EntryPoint = "SetForegroundWindow")]
        public static extern bool SetForegroundWindow(IntPtr point);

        [DllImport("user32.dll")]
        public static extern bool ShowWindowAsync(IntPtr hWnd, UInt32 nCmdShow);

        public static void ToFront()
        {
            var handle = GetHandle();

            if (!handle.Equals(IntPtr.Zero))
            {
                SetForegroundWindow(handle);
                ShowWindowAsync(handle, 3); //SW_SHOWMAXIMIZED = 3;
            }
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        public static bool IsForemost()
        {
            if (GetForegroundWindow() == GetHandle())
            {
                return true;
            }
            return false;
        }

        public static void StartCommand(string msg)
        {
            var catia = CatiaApplication.Instance;
            catia.StartCommand(msg);
            catia.RefreshDisplay = true;
            System.Threading.Thread.Sleep(100);
        }
    }
}
