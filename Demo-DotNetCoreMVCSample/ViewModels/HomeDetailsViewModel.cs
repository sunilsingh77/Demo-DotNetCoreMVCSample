using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Demo_DotNetCoreMVCSample.DTOs;

namespace Demo_DotNetCoreMVCSample.ViewModels
{
    public class HomeDetailsViewModel
    {
        public Employee Employee { get; set; }
        public string PageTitle { get; set; }
    }
}
