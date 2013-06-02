using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSTestResultViewer.Consol.Enums;

namespace MSTestResultViewer.Consol
{
    class TestClassMethods
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public ErrorInfo Error { get; set; }
        public string Duration { get; set; }
        public string Id { get; set; }
        public string Category { get; set; }
        public List<string> Output { get; set; }
        public string Type { get; set; }
    }
}
