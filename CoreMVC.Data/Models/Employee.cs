using System;
using System.Collections.Generic;
using System.Text;
using CoreMVC.Data.Enums;

namespace CoreMVC.Data.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Dept? Department { get; set; }
    }
}
