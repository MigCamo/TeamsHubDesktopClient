﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamsHubDesktopClient.DTOs
{
    public partial class StudentDTO
    {
        public int IdStudent { get; set; }

        public string Name { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string SurName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }
}
