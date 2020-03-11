﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Demo_DotNetCoreMVCSample.ViewModels
{
    public class RoleViewModel
    {
        public RoleViewModel()
        {
            Users = new List<string>();
        }

        public string Id { get; set; }

        [Display(Name = "Role")]
        [Required(ErrorMessage = "Role Name is required")]
        public string RoleName { get; set; }

        public List<string> Users { get; set; }
    }
}
