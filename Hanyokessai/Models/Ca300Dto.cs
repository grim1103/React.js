using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;

namespace Hanyokessai.Models
{
    public class Ca300Dto
    {
        public int formNum { get; set; }

        public string reportID { get; set; }

        public string fileName { get; set; }
        
        public string reportName { get; set; }

        public string templeteID { get; set; }

        public string befReportId { get; set; }

        public string regYmd { get; set; }

        public string updYmd { get; set; }

        public string approveStatus { get; set; }

        public string binaryFile { get; set; }

        public string fileSize { get; set; }
    }
}
