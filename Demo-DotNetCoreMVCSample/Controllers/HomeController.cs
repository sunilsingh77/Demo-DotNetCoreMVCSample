using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Demo_DotNetCoreMVCSample.DTOs;
using Demo_DotNetCoreMVCSample.Repository;
using Demo_DotNetCoreMVCSample.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Demo_DotNetCoreMVCSample.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IEmployeeRepository _employeeRepository;

        public HomeController(ILogger<HomeController> logger, IEmployeeRepository employeeRepository)
        {
            _logger = logger;
            _employeeRepository = employeeRepository;
        }

        public IActionResult Index()
        {
            var model = _employeeRepository.GetAllEmployee();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Test()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Details(int id)
        {
            var homeDetailsViewModel = new HomeDetailsViewModel()
            {
                Employee = _employeeRepository.GetEmployee(id),
                PageTitle = "Employee Details"
            };
            
            return View(homeDetailsViewModel);
        }

        public IActionResult List()
        {
            var model = _employeeRepository.GetAllEmployee();
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Employee emp)
        {
            if (ModelState.IsValid)
            {
                var model = _employeeRepository.Add(emp);
                
                return RedirectToAction("details", new { id = model.Id });
            }

            return View();
        }

    }
}
