using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamsHubDesktopClient.DTOs
{
    public class DocumentDTO
    {
        public int IdDocument { get; set; }

        public string? Name { get; set; }

        public int Extension { get; set; }

        public string? Ruta { get; set; }
    }
}
