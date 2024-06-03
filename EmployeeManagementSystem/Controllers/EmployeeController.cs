using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployeeManagementSystem.Controllers
{
    public class EmployeeController : Controller
    {
        private ApplicationDbContext toDb = new ApplicationDbContext();

        // GET: Employee
        public ActionResult Index()
        {
            return View(toDb.Employees.ToList());
        }

        [HttpGet]
        public ActionResult ManageEmp(int? id)
        {
            var _newEmployee = new Employee();
            if (id > 0)
            {
                _newEmployee = toDb.Employees.Find(id);
            }
            return View(_newEmployee);
        }

        [HttpPost]
        public ActionResult Save(Employee pEmployee)
        {
            try
            {
                // Check if the employee already exists in the database
                var existingEmployee = toDb.Employees.FirstOrDefault(e => e.Id == pEmployee.Id);

                if (existingEmployee != null)
                {
                    // Update the existing employee
                    existingEmployee.FirstName = pEmployee.FirstName;
                    existingEmployee.LastName = pEmployee.LastName;
                    existingEmployee.DateOfBirth = pEmployee.DateOfBirth;
                    existingEmployee.ContactNumber = pEmployee.ContactNumber;
                    existingEmployee.EmailAddress = pEmployee.EmailAddress;
                }
                else
                {
                    // Check if the employee number is unique
                    if (toDb.Employees.Any(e => e.EmployeeNumber == pEmployee.EmployeeNumber))
                    {
                        ModelState.AddModelError("EmployeeNumber", "Employee number must be unique.");
                        return View("ManageEmp", pEmployee);
                    }

                    // Check if the combination of first name and last name is not duplicated
                    if (toDb.Employees.Any(e => e.FirstName == pEmployee.FirstName && e.LastName == pEmployee.LastName))
                    {
                        ModelState.AddModelError("", "Employee name is already in the database.");
                        return View("ManageEmp", pEmployee);
                    }

                    // Generate the employee number for a new employee
                    pEmployee.GenerateEmployeeNum();

                    // Add a new employee
                    toDb.Employees.Add(pEmployee);
                }

                toDb.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception excemption)
            {
                // Handle exceptions
                ModelState.AddModelError("", "An error occurred while saving the employee. Please try again.");
                return View("ManageEmp", pEmployee);
            }
        }

        [HttpPost]
        public ActionResult Delete(int Id)
        {
            var _removeEmployee = toDb.Employees.Find(Id);
            if (_removeEmployee != null)
            {
                toDb.Employees.Remove(_removeEmployee);
            }
            toDb.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
