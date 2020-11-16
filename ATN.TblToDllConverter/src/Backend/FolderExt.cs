using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace ATN.TblsToDllCollector.Backend
{
    public static class FolderExt
    {
        public static void ClearFolder(DirectoryInfo dir)
        {
            if (dir.Exists)
            {
                //dir.Delete(true);
                System.Threading.Thread.Sleep(50);
            }
            dir.Create();
            while (!dir.Exists)
            {
                System.Threading.Thread.Sleep(20);
                dir.Refresh();
            }
        }
    }
}