using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace ATN.TblsToDllCollector.Backend
{
    public class External
    {
        public enum RegKind
        {
            RegKind_Default = 0,
            RegKind_Register = 1,
            RegKind_None = 2
        }

        [DllImport("oleaut32.dll", CharSet = CharSet.Unicode, PreserveSig = false)]
        public static extern void LoadTypeLibEx(String strTypeLibName, RegKind regKind,
        [MarshalAs(UnmanagedType.Interface)] out ITypeLib typeLib);
    }
}
