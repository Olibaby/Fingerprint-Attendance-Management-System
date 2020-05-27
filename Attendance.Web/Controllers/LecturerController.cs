using Attendance.Core;
using Attendance.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Attendance.Web.Controllers
{
    public class LecturerController : Controller
    {
        private IAttendanceManager _attmgr;
        public LecturerController(IAttendanceManager attmgr)
        {
            _attmgr = attmgr;
        }
        // GET: Lecturer
        public ActionResult Index()
        {
            var model = _attmgr.GetLecturers();
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.programmes = new SelectList(_attmgr.GetProgrammes(), "ProgrammeId", "ProgrammeName");
            ViewBag.colleges = new SelectList(_attmgr.GetColleges(), "CollegeId", "CollegeName");
            return View();
        }
        [HttpPost]
        public ActionResult Create(LecturerModel model)
        {
            ViewBag.programmes = new SelectList(_attmgr.GetProgrammes(), "ProgrammeId", "ProgrammeName");
            ViewBag.colleges = new SelectList(_attmgr.GetColleges(), "CollegeId", "CollegeName");

            if (!ModelState.IsValid)
            {
                TempData["Message"] = "Lecturer is not valid";
                return View("Create", model);
            }

            _attmgr.Add(model);
            TempData["Message"] = "Lecturer has been successfully added";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            var model = _attmgr.GetLecturer(id);
            ViewBag.colleges = new SelectList(_attmgr.GetColleges(), "CollegeId", "CollegeName", model.CollegeId);
            ViewBag.programmes = new SelectList(_attmgr.GetProgrammeByCollegeId(model.CollegeId ?? 0), "ProgrammeId", "ProgrammeName", model.ProgrammeId);

            if (id == null)
            {
                TempData["Message"] = "Lecturer does not exist";
                return View("Index");
            }
            if (model == null)
            {
                TempData["Message"] = "Lecturer cannot be found";
                return View("Index");
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(LecturerModel model)
        {
            ViewBag.colleges = new SelectList(_attmgr.GetColleges(), "CollegeId", "CollegeName");
            ViewBag.programmes = new SelectList(_attmgr.GetProgrammeByCollegeId(model.CollegeId ?? 0), "ProgrammeId", "ProgrammeName");

            if (ModelState.IsValid)
            {
                if (model == null)
                {
                    TempData["Message"] = "Lecturer is not found";
                    return View("Index");
                }
                _attmgr.Update(model.LecturerId, model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id, string lecturerName)
        {
            if (id > 0)
            {
                try
                {
                    _attmgr.RemoveLecturer(id);
                    return Json(new { status = true, message = $" Lecturer has been successfully deleted!", JsonRequestBehavior.AllowGet });
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
                            _attmgr.RemoveLecturer(intid);
                        }
                    }
                    return Json(new { status = true, message = " All selected lecturer(s) has been successfully deleted!", JsonRequestBehavior.AllowGet });

                    //return Json(new { status = false, error = result.Message }, JsonRequestBehavior.AllowGet);
                }


            }

            return Json(new { status = false, error = "Invalid Id" }, JsonRequestBehavior.AllowGet);
        }

    }
}