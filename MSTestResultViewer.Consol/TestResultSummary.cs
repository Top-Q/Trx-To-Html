using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSTestResultViewer.Consol
{
    class TestResultSummary
    {
        public Int16 Total { get; set; }
        public Int16 Passed { get; set; }
        public Int16 Failed { get; set; }
        public Int16 Ignored { get; set; }
        public string Duration { get; set; }
        public TestEnvironmentInfo TestEnvironment { get; set; }
    }
}
