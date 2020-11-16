using ATN.TblsToDllCollector.Backend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ATN.TblsToDllCollector
{
    public class Application
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args">/tbl_path /output_path /namespace</param>
        [STAThread]
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Need arguments, /tbl_path /output_path /namespace");
            }
            else
            {
                Console.WriteLine($"Args {args}");
                run(args[0], args[1], args[2]);
            }
            Console.ReadLine();
        }

        public static void run(string tbl_path, string output_path, string assemblyNamespace, bool optimized=false)
        {
            // set variables
            var typeLibFolder = new DirectoryInfo(tbl_path);

            var outputPath = new DirectoryInfo(output_path);

            var creationOrderList = new DirectoryInfo(outputPath.FullName + @"\TblCreationOrder.xml");
            
            var creationOrder = new TypeLibMergeOrder(creationOrderList.FullName);

            assemblyNamespace = assemblyNamespace + ".";

            // Run
            Console.WriteLine("---------Input--------");
            Console.WriteLine($"typeLibFolder {typeLibFolder.FullName}");
            Console.WriteLine($"outputPath {outputPath.FullName}");
            Console.WriteLine($"creationOrderList {creationOrderList.FullName}");

            Console.WriteLine("---------Main run--------");
            Console.ReadLine();

            FolderExt.ClearFolder(outputPath);

            Directory.SetCurrentDirectory(outputPath.FullName);

            if (optimized && creationOrder.Exist())
            {
                TlbToDllConverter.BuildDll(typeLibFolder, outputPath, creationOrder, assemblyNamespace);
            }
            else
            {
                TlbToDllConverter.BuildXmlAndDll(typeLibFolder, outputPath, creationOrder, assemblyNamespace);
            }

            Console.WriteLine("---------------Conversion complete------------------");
        }

    }
}
