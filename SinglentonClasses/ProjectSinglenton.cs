using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamHubServiceProjects.DTOs;

namespace TeamsHubDesktopClient.SinglentonClasses
{
    public static class ProjectSinglenton
    {
        public static ProjectDTO projectDTO { get; set; }
    }
}
