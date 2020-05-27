using System;
using System.Collections.Generic;
using System.Linq;
using Attendance.Core.Interface;
using System.Web.Mvc;
using Attendance.Core;

namespace Attendance.Web.Controllers
{
    public class ReportController : Controller
    {

        private IReportManager _rptmgr;
        private IAttendanceManager _attmgr;

        public ReportController(IReportManager rptmgr, IAttendanceManager attmgr)
        {
            _rptmgr = rptmgr;
            _attmgr = attmgr;
        }
        
        
        public ActionResult Index(CourseModel model)
        {
            var report = _rptmgr.getAttendanceReport(model.CollegeId,model.ProgrammeId,model.LevelId,model.Semester,model.CourseId);
                return View(report);
        }

        [HttpGet]
        public ActionResult GenerateReport()
        {
            var model = new CourseModel();
            ViewBag.levels = new SelectList(_attmgr.GetLevels(), "LevelId", "LevelName");
            ViewBag.colleges = new SelectList(_attmgr.GetColleges(), "CollegeId", "CollegeName");
            ViewBag.programmes = new SelectList(_attmgr.GetProgrammeByCollegeId(model.CollegeId), "ProgrammeId", "ProgrammeName");
            ViewBag.courses = new SelectList(_attmgr.GetCourses(model.CollegeId, model.ProgrammeId, model.LevelId, model.Semester), "CourseId", "CourseCode", model.CourseCode);
            return View();
        }

        [HttpPost]
        public ActionResult GenerateReport(CourseModel model)
        {
            ViewBag.levels = new SelectList(_attmgr.GetLevels(), "LevelId", "LevelName");
            ViewBag.colleges = new SelectList(_attmgr.GetColleges(), "CollegeId", "CollegeName");
            ViewBag.programmes = new SelectList(_attmgr.GetProgrammeByCollegeId(model.CollegeId), "ProgrammeId", "ProgrammeName");
            ViewBag.courses = new SelectList(_attmgr.GetCourses(model.CollegeId,model.ProgrammeId, model.LevelId, model.Semester), "CourseId", "CourseCode", model.CourseCode);
            return RedirectToAction("Index", model);
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

        public JsonResult GetProgrammeCourse(int collegeId, int programmeId, int levelId, string semester)
        {
            try
            {
                var value = _attmgr.GetCourses(collegeId, programmeId, levelId, semester);
                return Json(new { ok = true, data = value, message = "ok" });
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, message = ex.Message });
            }


        }
    }
}