using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSTestResultViewer.Consol
{
    class TestClasses
    {

        public TestClasses()
        {
            Methods = new List<TestClassMethods>();
            Duration = TimeSpan.Zero;
        }

        public string Name { get; set; }
        public Int32 Passed { get; set; }
        public Int32 Failed { get; set; }
        public Int32 Ignored { get; set; }
        public Int32 Total { get; set; }
        public TimeSpan Duration { get; set; }
        public List<TestClassMethods> Methods { get; set; }
    }
}
