using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace ATN.TblsToDllCollector.Tlb
{
    public class ConversionEventHandler : ITypeLibImporterNotifySink
    {
        TypeLibInfo typeLibInfo;

        public ConversionEventHandler(TypeLibInfo typeLibInfo)
        {
            this.typeLibInfo = typeLibInfo;
        }

        public void ReportEvent(ImporterEventKind eventKind, int eventCode, string eventMsg)
        {
            //Console.WriteLine($"Event: {eventMsg}");
        }

        public Assembly ResolveRef(object typeLib)
        {
            var refTblName = Marshal.GetTypeLibName((ITypeLib)typeLib);
            var asmName = $"{refTblName}.dll";

            typeLibInfo.References.Add(asmName);

            // does not work, loads assemblies untill stack overflow exception
            // Assembly preloadedAssembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName.Contains(refTblName));

            // foreach (var item in AppDomain.CurrentDomain.GetAssemblies())
            // {
            //     Console.WriteLine(item.FullName);
            // }

            
            // if (preloadedAssembly != null)
            // {
            //     Console.WriteLine($"Reference loaded from memmory correctly: {asmName}");
            //     Console.WriteLine(preloadedAssembly);
            //     return preloadedAssembly;
            // }

            string lib_path = this.typeLibInfo.path + asmName;
                
            try
            {
                Assembly asm = Assembly.LoadFrom(lib_path);
                Console.WriteLine($"Reference loaded from file correctly: {asmName}");
                return asm;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Custom resolve ref exeption: {ex.Message}");
                Console.WriteLine($"asmName: {asmName}");
                Console.WriteLine($"lib_path: {lib_path}");

                throw new Exception("Conversion failed- missing reference");
            }
            return null;
        }
    }
}
