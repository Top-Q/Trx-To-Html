using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSTestResultViewer.Consol
{
    class TableItemInfo
    {

        public int id { get; set; }
        public int parent { get; set; }
        public string level { get; set; }
        public string Name { get; set; }
        public int Passed { get; set; }
        public int Failed { get; set; }
        public int Ignored { get; set; }
        public string Percent { get; set; }
        public string Progress { get; set; }
        public double Time { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string LineNo { get; set; }
        public bool isLeaf { get; set; }
        public bool expanded { get; set; }
        public bool loaded { get; set; }
        public string TestOutput { get; set; }
                 
               
       
    }
   
}
