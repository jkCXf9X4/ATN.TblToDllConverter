using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

using ATN.TblsToDllCollector.Tlb;

namespace ATN.TblsToDllCollector.Backend
{
    public class TlbToDllConverter
    {
        public static void BuildXmlAndDll(
            DirectoryInfo typeLibFolder, 
            DirectoryInfo outputDir,
            TypeLibMergeOrder mergeOrder,
            string assemblyNamespace)
        {
            var files = typeLibFolder.GetFiles("*.tlb").ToList(); //Getting tlb files

            int max = files.Count+100;
            while (files.Count != 0)
            {
                var FilesLeft = new List<FileInfo>();
                foreach (var file in files)
                {
                    var typeLibInfo =  TlbConverter.CreateDll(file, outputDir, assemblyNamespace);
                    if (typeLibInfo != null)
                    {
                        mergeOrder.typelibInfos.Add(typeLibInfo);
                    }
                    else
                    {
                        FilesLeft.Add(file);
                    }
                }
                files = FilesLeft;

                max--;
                if (max == 0)
                {
                    throw new Exception("Could not finish, unknown");
                }
            }

            mergeOrder.WriteList();
        }

        public static void BuildDll(
            DirectoryInfo typeLibFolder,
            DirectoryInfo outputDir,
            TypeLibMergeOrder mergeOrder,
            string assemblyNamespace)
        {
            var typeLibs = mergeOrder.ReadList();

            foreach (var lib in typeLibs)
            {
                var file = new FileInfo(typeLibFolder + "/" + lib.OriginalName);

                TblsToDllCollector.Tlb.TlbConverter.CreateDll(file, outputDir, assemblyNamespace);
            }
        }
    }
}
