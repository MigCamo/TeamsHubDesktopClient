using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamsHubDesktopClient.DTOs
{
    public partial class TaskDTO
    {
        public int IdTask { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int? IdProject { get; set; }

        public string? Status { get; set; }

    }
}
