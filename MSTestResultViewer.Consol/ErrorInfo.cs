using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSTestResultViewer.Consol
{
    class ErrorInfo
    {
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public Int32 LineNo { get; set; }
    }
}
