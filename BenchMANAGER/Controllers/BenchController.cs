using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BenchMANAGER.Models;
using BenchMANAGER.ViewModels;

namespace BenchMANAGER.Controllers
{
    public class BenchController : Controller
    {
         BenchMANAGERDBEntities1 _db = new BenchMANAGERDBEntities1();
        // GET: Bench
         public ActionResult Resources(string page = "", string ddlPractise = "", string EmpFName = "")
         {
             if (!string.IsNullOrEmpty(ddlPractise) || !string.IsNullOrEmpty(EmpFName))
             {
                 if (ddlPractise == "")
                 {
                     ddlPractise = null;
                 }
                 if (EmpFName == "")
                 {
                     EmpFName = null;
                 }
                 var employees = (from emp in _db.Employees
                                  where emp.ReservationStatus != "Free" &&
                                  (emp.Practice == ddlPractise || ddlPractise == null) &&
                                  (emp.FirstName == EmpFName || EmpFName == null)
                                  select emp);
                 ViewBag.DefaultEmployee = employees.FirstOrDefault();
                 return View(employees.ToList());
             }
             else
             {
                 var employees = (from emp in _db.Employees
                                  where emp.ReservationStatus != "Free"
                                  select emp);
                 ViewBag.DefaultEmployee = employees.FirstOrDefault();
                 return View(employees.ToList());
             }



             //var displayEmployee = (from emp in _db.Employees
             //                       where emp.ReservationStatus != "Free"
             //                       select emp).FirstOrDefault();



         }

        public ActionResult GetResource(string term)
        {
            var result = (from r in _db.Employees
                          where r.FirstName.ToLower().Contains(term)
                          select r.FirstName).Distinct();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetBenchResource(string term)
        {
            var result = (from e in _db.Employees
                          join be in _db.BenchEmployees on e.EmployeeNumber
                          equals be.EmployeeNumber
                          where e.FirstName.ToLower().Contains(term)
                          select e.FirstName).Distinct();

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetBenchSPOC(string term)
        {

            var result = (from r in _db.Employees
                          where r.Supervisor.ToLower().Contains(term)
                          select r.Supervisor).Distinct();

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddResources(string page = "", string BenchEmpFirstName = "", string AutoSPOC = "", string ddlPractise="")
        {

            if (!string.IsNullOrEmpty(BenchEmpFirstName) || !string.IsNullOrEmpty(AutoSPOC) || !string.IsNullOrEmpty(ddlPractise))
            {
                if (BenchEmpFirstName == "")
                {
                    BenchEmpFirstName = null;
                }
                if (ddlPractise == "")
                {
                    ddlPractise = null;
                }
                if (AutoSPOC == "")
                {
                    AutoSPOC = null;
                }
                var benchEmployees = (from e in _db.Employees
                                      join be in _db.BenchEmployees on e.EmployeeNumber
                                      equals be.EmployeeNumber
                                      where (e.Practice == ddlPractise || ddlPractise == null) &&
                                      (e.FirstName == BenchEmpFirstName || BenchEmpFirstName == null) &&
                                      (be.SPOC == AutoSPOC || AutoSPOC == null)


                                      select new BenchEmployeeViewModel
                                      {
                                          BenchEmployeeId = be.BenchEmployeeId,
                                          EmployeeNumber = be.EmployeeNumber,
                                          FirstName = e.FirstName,
                                          LastName = e.LastName,
                                          SPOC = be.SPOC,
                                          ProjectCode = be.ProjectCode,
                                          StartDate = be.StartDate,
                                          EndDate = be.EndDate,
                                          Utilization = be.Utilization,
                                          LocationType = be.LocationType,
                                          Location = be.Location,
                                          AssignmentStatus = be.AssignmentStatus,
                                          Comments = be.Comments
                                      }).OrderByDescending(be => be.BenchEmployeeId);
                return View(benchEmployees.ToList());
            }
            else
            {
                var benchEmployees = (from e in _db.Employees
                                      join be in _db.BenchEmployees on e.EmployeeNumber
                                      equals be.EmployeeNumber
                                      select new BenchEmployeeViewModel
                                      {
                                          BenchEmployeeId = be.BenchEmployeeId,
                                          EmployeeNumber = be.EmployeeNumber,
                                          FirstName = e.FirstName,
                                          LastName = e.LastName,
                                          SPOC = be.SPOC,
                                          ProjectCode = be.ProjectCode,
                                          StartDate = be.StartDate,
                                          EndDate = be.EndDate,
                                          Utilization = be.Utilization,
                                          LocationType = be.LocationType,
                                          Location = be.Location,
                                          AssignmentStatus = be.AssignmentStatus,
                                          Comments = be.Comments
                                      }).OrderByDescending(be => be.BenchEmployeeId);

                return View(benchEmployees.ToList());
            }
            
            //return View();
        }
        [HttpPost]
        public ActionResult AddResources(string empNumbers)
        {
            List<BenchEmployee> _benchEmployee = new List<BenchEmployee>();
            
            int empNumber;
            //string[] employees = empNumbers.Split(',');
            //employees = empNumbers.Split('\n');
            char[] delimiters = new[] { ',', ' ' };

            string existingEmpNumber = string.Empty;
            foreach (var empID in empNumbers.Split(delimiters,StringSplitOptions.RemoveEmptyEntries))
            {
                //var _employee = new Employee { EmployeeNumber= Convert.ToInt32(empID.Trim()), ReservationStatus = "Free" };
                

                empNumber = int.Parse(empID.Trim());
                var empnumberExists = (from t in _db.BenchEmployees
                                       where t.EmployeeNumber == empNumber
                                       select t).Count();
                
                
                //bool exists = _db.BenchEmployees.Any(t => t.EmployeeNumber == Convert.ToInt32(empID.Trim()));
                if (empnumberExists == 0)
                {
                    Employee _employee = _db.Employees.Single(e => e.EmployeeNumber == empNumber);
                    _employee.ReservationStatus = "Free";

                    var benchEmployee = new BenchEmployee { EmployeeNumber = Convert.ToInt32(empID.Trim()) };
                    _db.BenchEmployees.Add(benchEmployee);
                }
                else
                {
                    existingEmpNumber = existingEmpNumber + "," + empID;
                }
                
            }
            _db.SaveChanges();
            
            if (!string.IsNullOrEmpty(existingEmpNumber))
            {
                existingEmpNumber = existingEmpNumber.Substring(1);
                return Json(new { success = true, responseText = existingEmpNumber }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(_benchEmployee, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult SelectedResources(int? empNum)
        {


            var selectedEmployee = (from emp in _db.Employees
                                    where emp.EmployeeNumber == empNum
                                    select emp);

            //var displayEmployee = (from emp in _db.Employees
            //                       where emp.ReservationStatus != "Free"
            //                       select emp).FirstOrDefault();

            ViewBag.SelectedEmployee = selectedEmployee.FirstOrDefault();
            return View("Resources", selectedEmployee.ToList());
            
        }

        [HttpPost]
        public ActionResult SearchResources(string employeeName)
        {
            var searchEmployee = from e in _db.Employees
                           where e.FirstName == employeeName
                           select e;
            ViewBag.DefaultEmployee = searchEmployee.FirstOrDefault();
            return View("Resources",searchEmployee.ToList());

            //return Json(searchEmployee.ToList(), JsonRequestBehavior.AllowGet);
            //return PartialView("_EmployeeGrid", searchEmployee.ToList());
            
        }
        [HttpGet]
        public ActionResult SearchBenchEmployee()
        {
            List<string> tempSearchData = TempData["SearchParameters"] as List<string>;
            TempData.Keep();
            string practice=tempSearchData[1];
            var benchEmployees = (from e in _db.Employees
                                  join be in _db.BenchEmployees on e.EmployeeNumber
                                  equals be.EmployeeNumber
                                  where e.Practice == practice
                                  select new BenchEmployeeViewModel
                                  {
                                      BenchEmployeeId = be.BenchEmployeeId,
                                      EmployeeNumber = be.EmployeeNumber,
                                      FirstName = e.FirstName,
                                      LastName = e.LastName,
                                      SPOC = be.SPOC,
                                      ProjectCode = be.ProjectCode,
                                      StartDate = be.StartDate,
                                      EndDate = be.EndDate,
                                      Utilization = be.Utilization,
                                      LocationType = be.LocationType,
                                      Location = be.Location,
                                      AssignmentStatus = be.AssignmentStatus,
                                      Comments = be.Comments
                                  }).OrderByDescending(be => be.BenchEmployeeId);
            return View("AddResources", benchEmployees.ToList());
            
        }
        [HttpPost]
        public ActionResult SearchBenchEmployee(string SPOC, string Practice, string Resource)
        {
            List<string> TempSearchParameters = new List<string>();
            TempSearchParameters.Add(SPOC);
            TempSearchParameters.Add(Practice);
            TempSearchParameters.Add(Resource);
            TempData["SearchParameters"] = TempSearchParameters;

            if (Resource == "")
            {
                Resource = null;
            }
            if (Practice == "")
            {
                Practice = null;
            }
            if (SPOC == "")
            {
                SPOC = null;
            }
            //var benchEmployees = (from e in _db.Employees
            //                      join be in _db.BenchEmployees on e.EmployeeNumber
            //                      equals be.EmployeeNumber
            //                      where (Practice == "" ? e.Practice == null : e.Practice == Practice) &&
            //                                            (SPOC == "" ? be.SPOC == null : be.SPOC == SPOC) &&
            //                                            (Resource == "" ? e.FirstName == null : e.FirstName == Resource)
            var benchEmployees = (from e in _db.Employees
                                                        join be in _db.BenchEmployees on e.EmployeeNumber
                                                        equals be.EmployeeNumber
                                                        where (e.Practice == Practice || Practice==null) &&
                                                        (e.FirstName==Resource || Resource==null) &&
                                                        (be.SPOC==SPOC || SPOC==null)
                                                        
            
                                  select new BenchEmployeeViewModel
                                  {
                                      BenchEmployeeId = be.BenchEmployeeId,
                                      EmployeeNumber = be.EmployeeNumber,
                                      FirstName = e.FirstName,
                                      LastName = e.LastName,
                                      SPOC = be.SPOC,
                                      ProjectCode = be.ProjectCode,
                                      StartDate = be.StartDate,
                                      EndDate = be.EndDate,
                                      Utilization = be.Utilization,
                                      LocationType = be.LocationType,
                                      Location = be.Location,
                                      AssignmentStatus = be.AssignmentStatus,
                                      Comments = be.Comments
                                  }).OrderByDescending(be => be.BenchEmployeeId);

            
            //return PartialView("_BenchEmployeeGrid", benchEmployees.ToList());
            //return Json(benchEmployees.ToList(), JsonRequestBehavior.AllowGet);
            return View("AddResources", benchEmployees.ToList());

            //return View();
        }

        [HttpGet]
        public ActionResult SearchEmployeeWithPractice()
        {
            string strPractice=string.Empty;
            //if (TempData["Practice"] != null)
            //{
            //    strPractice =Convert.ToString(TempData["Practice"]);
            //}

            if (Session["Practice"] != null)
            {
                strPractice = Convert.ToString(Session["Practice"]);
            }
            var searchPractice = from e in _db.Employees
                                 where e.Practice == strPractice
                                 select e;
            ViewBag.DefaultEmployee = searchPractice.FirstOrDefault();
            //return Json(searchEmployee.ToList(), JsonRequestBehavior.AllowGet);
            return View("Resources", searchPractice.ToList());
            //return Json(searchEmployee.ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SearchEmployeeWithPractice(string Practice)
        {
            var searchPractice = from e in _db.Employees
                                 where e.Practice == Practice
                                 select e;
            //TempData["Practice"] = Practice;
            Session["Practice"] = Practice;
            ViewBag.DefaultEmployee = searchPractice.FirstOrDefault();
            //return Json(searchEmployee.ToList(), JsonRequestBehavior.AllowGet);
            return View("Resources", searchPractice.ToList());
            //return Json(searchEmployee.ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateBenchResource(int BenchEmployeeID, string SPOC, string ProjectCode, DateTime StartDate, DateTime EndDate, string LocationType, string Location, string Status, int Utilization, string Comments)
        {
            var benchEmployee = (from be in _db.BenchEmployees
                                 where be.BenchEmployeeId == BenchEmployeeID
                                 select be).Single();
            benchEmployee.SPOC = SPOC;
            benchEmployee.ProjectCode = ProjectCode;
            benchEmployee.StartDate = StartDate;
            benchEmployee.EndDate = EndDate;
            benchEmployee.LocationType = LocationType;
            benchEmployee.Location = Location;
            benchEmployee.AssignmentStatus = Status;
            benchEmployee.Utilization = Utilization;
            benchEmployee.Comments = Comments;

            _db.SaveChanges();

            return Json(benchEmployee, JsonRequestBehavior.AllowGet);
        }
    }
}