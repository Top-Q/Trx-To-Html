using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSTestResultViewer.Consol
{
    class TestEnvironmentInfo
    {
        public string TestCodebase { get; set; }
        public string MachineName { get; set; }
        public string UserName { get; set; }
        public string OriginalTRXFile { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
