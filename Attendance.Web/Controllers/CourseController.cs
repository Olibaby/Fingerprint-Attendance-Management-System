using Attendance.Core;
using Attendance.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Attendance.Web.Controllers
{
    public class CourseController : Controller
    {
        private IAttendanceManager _attmgr;
        public CourseController(IAttendanceManager attmgr)
        {
            _attmgr = attmgr;
        }
        // GET: Course
        public ActionResult Index()
        {
            var model = _attmgr.GetCourses();
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var model = new CourseModel();
            ViewBag.levels = new SelectList(_attmgr.GetLevels(), "LevelId", "LevelName");
            ViewBag.lecturer = new SelectList(_attmgr.GetLecturers(), "LecturerId", "FirstName");
            ViewBag.colleges = new SelectList(_attmgr.GetColleges(), "CollegeId", "CollegeName");
            ViewBag.programmes = new SelectList(_attmgr.GetProgrammeByCollegeId(model.CollegeId), "ProgrammeId", "ProgrammeName");
            return View();
        }
        [HttpPost]
        public ActionResult Create(CourseModel model)
        {
            ViewBag.levels = new SelectList(_attmgr.GetLevels(), "LevelId", "LevelName");
            ViewBag.lecturer = new SelectList(_attmgr.GetLecturers(), "LecturerId", "FirstName");
            ViewBag.colleges = new SelectList(_attmgr.GetColleges(), "CollegeId", "CollegeName");
            ViewBag.programmes = new SelectList(_attmgr.GetProgrammeByCollegeId(model.CollegeId), "ProgrammeId", "ProgrammeName");
            if (!ModelState.IsValid)
            {
                TempData["Message"] = "Course is not valid";
                return View("Create", model);
            }

            _attmgr.Add(model);
            TempData["Message"] = "Course has been successfully added";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            var model = _attmgr.GetCourse(id);
            ViewBag.levels = new SelectList(_attmgr.GetLevels(), "LevelId", "LevelName");
            ViewBag.lecturer = new SelectList(_attmgr.GetLecturers(), "LecturerId", "FirstName");
            ViewBag.colleges = new SelectList(_attmgr.GetColleges(), "CollegeId", "CollegeName", model.CollegeId);
            ViewBag.programmes = new SelectList(_attmgr.GetProgrammeByCollegeId(model.CollegeId), "ProgrammeId", "ProgrammeName",model.ProgrammeId);

            if (id == null)
            {
                TempData["Message"] = "Course does not exist";
                return View("Index");
            }
            if (model == null)
            {
                TempData["Message"] = "Course cannot be found";
                return View("Index");
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(CourseModel model)
        {
            ViewBag.lecturer = new SelectList(_attmgr.GetLecturers(), "LecturerId", "FirstName");
            ViewBag.levels = new SelectList(_attmgr.GetLevels(), "LevelId", "LevelName");
            ViewBag.colleges = new SelectList(_attmgr.GetColleges(), "CollegeId", "CollegeName");
            ViewBag.programmes = new SelectList(_attmgr.GetProgrammeByCollegeId(model.CollegeId), "ProgrammeId", "ProgrammeName");

            if (ModelState.IsValid)
            {
                if (model == null)
                {
                    TempData["Message"] = "Course is not found";
                    return View("Index");
                }
                _attmgr.Update(model.CourseId, model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id, string courseName)
        {
            if (id > 0)
            {
                try
                {
                    _attmgr.RemoveCourse(id);
                    return Json(new { status = true, message = $" Course has been successfully deleted!", JsonRequestBehavior.AllowGet });
                }
                catch (Exception ex)
                {
                    return Json(new { status = false, error = ex.Message }, JsonRequestBehavior.AllowGet);
                }

            }
            return Json(new { status = false, error = "Invalid Id" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteIds(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                List<string> idss = ids.Split('*').ToList();
                if (idss.Count() > 0)
                {
                    foreach (var strid in idss)
                    {
                        if (!string.IsNullOrEmpty(strid))
                        {
                            int intid = Convert.ToInt32(strid);
                            _attmgr.RemoveCourse(intid);
                        }
                    }
                    return Json(new { status = true, message = " All selected course(s) has been successfully deleted!", JsonRequestBehavior.AllowGet });

                    //return Json(new { status = false, error = result.Message }, JsonRequestBehavior.AllowGet);
                }


            }

            return Json(new { status = false, error = "Invalid Id" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProgrammeByCollegeId(int id)
        {
            var result = _attmgr.GetProgrammeByCollegeId(id);
            if (result != null)
            {
                var value = result.Select(p => new
                {
                    ProgrammeId = p.ProgrammeId,
                    ProgrammeName = p.ProgrammeName
                });
                return Json(new { ok = true, data = value, message = "ok", JsonRequestBehavior.AllowGet });
            }
            return Json(new { ok = false, message = "Error Something Happened" }, JsonRequestBehavior.AllowGet);
        }
    }
}