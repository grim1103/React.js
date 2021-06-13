using Microsoft.VisualBasic.CompilerServices;

namespace Hanyokessai.Models
{
    public class DownloadDto
    {
        public string status { get; set; }

        public string[] stampInfo { get; set; }

        public string[] stampInfoSize { get; set; }

        public string[] stampInfoPlace { get; set; }
        
        public string file_name { get; set; }

        public string file_binary { get; set; }
        
        public string report_file_size { get; set; }
    }
}
