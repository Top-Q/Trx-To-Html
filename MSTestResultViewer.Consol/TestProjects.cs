using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSTestResultViewer.Consol
{
    class TestProjects
    {

        public TestProjects()
        {
           Classes = new List<TestClasses>();
        }
        public string Name { get; set; }
        public Int32 Passed { get; set; }
        public Int32 Failed { get; set; }
        public Int32 Ignored { get; set; }
        public Int32 Total { get; set; }
        public string Duration { get; set; }
        public List<TestClasses> Classes { get; set; }
    }
}
