using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace ReadResultXml
{
    public class Class1
    {

            #region "Program Startup"
        static void Main(string[] args)
        {


            //  XmlSerializer serializer2 = new XmlSerializer(typeof(TestRunTestListsTestList));

            XmlSerializer serializer = new XmlSerializer(typeof(TestRun));

            FileStream stream = new FileStream(@"C:\dev\MSTestResultViewer\ReadResultXml\automation.trx", FileMode.Open);

            TestRun testRun = (TestRun)serializer.Deserialize(stream);

            


            #endregion

        }
    }
}
