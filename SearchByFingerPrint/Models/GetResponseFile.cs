using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SearchByFingerPrint.Models
{
    public class GetResponseFile
    {

        public string RequestId { get; set; }
        public string ResponseDateTime { get; set; }
        public string State { get; set; }
        public byte[] NistFile { get; set; }
    }
}