using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CoreMVC.Data.Enums;
using CoreMVC.Data.Models;
using Demo_DotNetCoreMVCSample;
using Demo_DotNetCoreMVCSample.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace Demo_DotNetCoreMVCSample.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepo _employeeRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public EmployeeController(IEmployeeRepo employeeRepository, IWebHostEnvironment hostingEnvironment)
        {
            _employeeRepository = employeeRepository;
            _hostingEnvironment = hostingEnvironment;
        }
       
        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(DTOs.Employee model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;

                // If the Photo property on the incoming model object is not null, then the user
                // has selected an image to upload.
                if (model.Photo != null)
                {
                    // The image must be uploaded to the images folder in wwwroot
                    // To get the path of the wwwroot folder we are using the inject
                    // HostingEnvironment service provided by ASP.NET Core
                    string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                    // To make sure the file name is unique we are appending a new
                    // GUID value and and an underscore to the file name
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    // Use CopyTo() method provided by IFormFile interface to
                    // copy the file to wwwroot/images folder
                    model.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
                }

                Employee newEmployee = new Employee
                {
                    Name = model.Name,
                    Email = model.Email,
                    Department =  (Dept)model.Department,
                    // Store the file name in PhotoPath property of the employee object
                    // which gets saved to the Employees database table
                    PhotoPath = uniqueFileName
                };

                var emp = _employeeRepository.Add(newEmployee);
                return RedirectToAction("details", new { id = emp.Id });
            }

            return View();
        }

        public IActionResult Details(int id)
        {
            var emp = _employeeRepository.GetEmployee(id);
            if (emp is null)
            {
                Response.StatusCode = 404;
                return View("EmployeeNotFound", id);
            }

            var empDTO = new DTOs.Employee
            {
                Id=emp.Id,
                Name = emp.Name,
                Email = emp.Email,
                PhotoPath = emp.PhotoPath,
                Department = (DTOs.Dept)emp.Department
            };
            return View(empDTO);
        }

        public IActionResult Index()
        {
            var model = _employeeRepository.GetAllEmployee().ToList();
            var empList = new List<DTOs.Employee>();
            foreach (var item in model)
            {
                var emp = new DTOs.Employee()
                {
                    Name = item.Name,
                    Email = item.Email,
                    Id = item.Id,
                    Department = (DTOs.Dept)item.Department,
                    PhotoPath = item.PhotoPath
                };
                empList.Add(emp);
            }
            return View(empList);
        }

        public IActionResult Edit(int id)
        {
            var result = _employeeRepository.GetEmployee(id);

            if (result is null)
            {
                Response.StatusCode = 404;
                return View("EmployeeNotFound", id);
            }

            var emp = new DTOs.Employee
            {
                Id = result.Id,
                Name = result.Name,
                Email = result.Email,
                PhotoPath = result.PhotoPath,
                Department = (DTOs.Dept)result.Department,
                ExistingPhotoPath = result.PhotoPath
            };
            return View(emp);
        }

        // Through model binding, the action method parameter
        // EmployeeEditViewModel receives the posted edit form data
        [HttpPost]
        public IActionResult Edit(DTOs.Employee model)
        {
            // Check if the provided data is valid, if not rerender the edit view
            // so the user can correct and resubmit the edit form
            if (ModelState.IsValid)
            {
                // Retrieve the employee being edited from the database
                Employee employee = _employeeRepository.GetEmployee(model.Id);
                // Update the employee object with the data in the model object
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Department = (Dept)model.Department;

                // If the user wants to change the photo, a new photo will be
                // uploaded and the Photo property on the model object receives
                // the uploaded photo. If the Photo property is null, user did
                // not upload a new photo and keeps his existing photo
                if (model.Photo != null)
                {
                    // If a new photo is uploaded, the existing photo must be
                    // deleted. So check if there is an existing photo and delete
                    if (model.ExistingPhotoPath != null)
                    {
                        string filePath = Path.Combine(_hostingEnvironment.WebRootPath,
                            "images", model.ExistingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }
                    // Save the new photo in wwwroot/images folder and update
                    // PhotoPath property of the employee object which will be
                    // eventually saved in the database
                    employee.PhotoPath = ProcessUploadedFile(model);
                }

                // Call update method on the repository service passing it the
                // employee object to update the data in the database table
                Employee updatedEmployee = _employeeRepository.Update(employee);

                return RedirectToAction("index");
            }

            return View(model);
        }

        private string ProcessUploadedFile(DTOs.Employee model)
        {
            string uniqueFileName = null;

            if (model.Photo != null)
            {
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Photo.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }
    }
}