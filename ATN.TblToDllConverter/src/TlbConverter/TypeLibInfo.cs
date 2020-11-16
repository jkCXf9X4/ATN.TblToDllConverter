using System.Collections.Generic;
using System.Xml.Serialization;

using System.IO;

namespace ATN.TblsToDllCollector.Tlb
{

    [XmlRootAttribute("TypeLibs", IsNullable = true)]
    public class TypeLibInfo
    {
        public string OriginalName;
        public string TlbName;
        public string AsmName;
        public string path;
        public List<string> References = new List<string>();

        public FileInfo GetFileinfo()
        {
            return new FileInfo(this.path +  this.AsmName);
        }

        public bool Exists
        {
            get
            {
                return GetFileinfo().Exists;        
            }
        }
    }
}
