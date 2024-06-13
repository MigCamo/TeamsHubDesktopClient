using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamsHubDesktopClient.SinglentonClasses
{
    public static class StudentSinglenton
    {
        public static int ID { get; set; }
        public static string FullName { get; set; }
        public static string Email { get; set; }
        public static string Token { get; set; }
    }
}
