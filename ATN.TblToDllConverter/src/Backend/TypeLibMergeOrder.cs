using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using ATN.TblsToDllCollector.Tlb;

namespace ATN.TblsToDllCollector.Backend
{
    public class TypeLibMergeOrder
    {
        FileInfo file;

        public List<TypeLibInfo> typelibInfos = new List<TypeLibInfo>();

        public TypeLibMergeOrder(string path)
        {
            this.file = new FileInfo(path);
        }

        public bool Exist()
        {
            return this.file.Exists;
        }

        public class TypeLibInfoListWraper
        {
            public List<TypeLibInfo> Instances;
        }

        public void WriteList()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(TypeLibInfoListWraper));
            TextWriter writer = new StreamWriter(this.file.FullName);

            serializer.Serialize(writer, new TypeLibInfoListWraper() { Instances = this.typelibInfos });

            writer.Close();
        }

        public List<TypeLibInfo> ReadList()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(TypeLibInfoListWraper));

            TypeLibInfoListWraper wrapper;

            using (Stream reader = new FileStream(this.file.FullName, FileMode.Open))
            {
               wrapper = (TypeLibInfoListWraper)serializer.Deserialize(reader);
            }

            this.typelibInfos = wrapper.Instances;

            return this.typelibInfos;

        }
    }
}
