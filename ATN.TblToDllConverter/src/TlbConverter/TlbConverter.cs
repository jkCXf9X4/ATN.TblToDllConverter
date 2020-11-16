using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

using ATN.TblsToDllCollector.Backend;

using System.Threading.Tasks;

namespace ATN.TblsToDllCollector.Tlb
{
    public class TlbConverter
    {
        // public static TypeLibInfo CreateDll_1(FileInfo file, DirectoryInfo outputDir, string Namespace)
        // {
        //     var task = Task.Factory.StartNew(() => TlbConverter.CreateDllTask(file, outputDir, Namespace ));

        //     var result = task.Result;
        //     task = null;

        //     System.GC.Collect();
        //     GC.WaitForPendingFinalizers();
        //     return result;
        // }

        // public static TypeLibInfo CreateDll(FileInfo file, DirectoryInfo outputDir, string Namespace)
        // {
        //     TypeLibInfo i = null;
        //     var thread  = new System.Threading.Thread(() => {
        //         i = TlbConverter.CreateDllTask(file, outputDir, Namespace);
        //     });

        //     thread.IsBackground = true;

        //     thread.Start();
        //     thread.Join();

        //     thread = null;

        //     System.GC.Collect();
        //     GC.WaitForPendingFinalizers();

        //     return i;
        // }

        public static TypeLibInfo CreateDll(FileInfo file, DirectoryInfo outputDir, string Namespace)
        {
            Console.WriteLine($"-----------------File: {file.Name} is now processing-------------------");

            if (file.Name == "InfTypeLib.tlb")
            {
                // Console.ReadLine();
            }

            var typeLibInfo = new TypeLibInfo()
            {
                OriginalName = file.Name,
                path = outputDir.FullName
            };

            AssemblyBuilder asm = null;

            var tlc = new TypeLibConverter();

            External.LoadTypeLibEx(file.FullName, External.RegKind.RegKind_None, out ITypeLib typeLib);

            if( typeLib == null )
            {
                Console.WriteLine( "LoadTypeLibEx failed." );
                return null;
            }

            typeLibInfo.TlbName = Marshal.GetTypeLibName(typeLib);
            typeLibInfo.AsmName = typeLibInfo.TlbName + ".dll";

            
            Console.WriteLine(typeLibInfo.GetFileinfo());

            if (typeLibInfo.Exists)
            {
                Console.WriteLine($"AsmName {typeLibInfo.AsmName} exists, no need to convert");
                return typeLibInfo;
            }

            TypeLibConverter tlbConv = new TypeLibConverter();

            ConversionEventHandler eventHandler = new ConversionEventHandler(typeLibInfo);

            try
            {
                asm = tlbConv.ConvertTypeLibToAssembly(typeLib, typeLibInfo.AsmName, TypeLibImporterFlags.SafeArrayAsSystemArray, eventHandler, null, null, Namespace + typeLibInfo.TlbName,null);
            }
            catch (Exception e)
            {
                    Console.WriteLine("Failed to convert, trying again", e.Message, e.StackTrace);
                    Console.WriteLine($"OriginalName {typeLibInfo.OriginalName}");
                    Console.WriteLine($"TlbName {typeLibInfo.TlbName}");
                    Console.WriteLine($"AsmName {typeLibInfo.AsmName}");
                    return null;
            }

            try
            {
                asm.Save(typeLibInfo.AsmName);
            }
            catch (Exception e)
            {
                    Console.WriteLine("Failed to save, trying again", e.Message, e.StackTrace);
                    Console.WriteLine($"OriginalName {typeLibInfo.OriginalName}");
                    Console.WriteLine($"TlbName {typeLibInfo.TlbName}");
                    Console.WriteLine($"AsmName {typeLibInfo.AsmName}");
                    return null;
            }
            return typeLibInfo;
        }
    }
}
