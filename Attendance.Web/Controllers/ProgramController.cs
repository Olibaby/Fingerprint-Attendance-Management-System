using Attendance.Core;
using Attendance.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Attendance.Web.Controllers
{
    public class ProgramController : Controller
    {
        private IAttendanceManager _attmgr;
        public ProgramController(IAttendanceManager attmgr)
        {
            _attmgr = attmgr;
        }
        // GET: Program
        public ActionResult Index()
        {
            var models = _attmgr.GetProgrammes();
            return View(models);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.colleges = new SelectList(_attmgr.GetColleges(), "CollegeId", "CollegeName");
            return View();
        }
        [HttpPost]
        public ActionResult Create(ProgrammeModel model)
        {
            ViewBag.colleges = new SelectList(_attmgr.GetColleges(), "CollegeId", "CollegeName");
            if (!ModelState.IsValid)
            {
                TempData["Message"] = "Program is not valid";
                return RedirectToAction("Index", model);
            }
            _attmgr.Add(model);
            TempData["Message"] = "Program has been successfully added";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            ViewBag.colleges = new SelectList(_attmgr.GetColleges(), "CollegeId", "CollegeName");
            if (id == null)
            {
                TempData["Message"] = "Program does not exist";
                return View("Index");
            }
            var model = _attmgr.GetProgramme(id);
            if (model == null)
            {
                TempData["Message"] = "Program cannot be found";
                return View("Index");
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(ProgrammeModel model)
        {
            ViewBag.colleges = new SelectList(_attmgr.GetColleges(), "CollegeId", "CollegeName");
            if (ModelState.IsValid)
            {
                if (model == null)
                {
                    TempData["Message"] = "Program is not found";
                    return View("Index");
                }
                _attmgr.Update(model.ProgrammeId, model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id, string programmeName)
        {
            if (id > 0)
            {
                try
                {
                    _attmgr.RemoveProgramme(id);
                    return Json(new { status = true, message = $" Programme has been successfully deleted!", JsonRequestBehavior.AllowGet });
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
                            _attmgr.RemoveProgramme(intid);
                        }
                    }
                    return Json(new { status = true, message = " All selected programme(s) has been successfully deleted!", JsonRequestBehavior.AllowGet });

                    //return Json(new { status = false, error = result.Message }, JsonRequestBehavior.AllowGet);
                }


            }

            return Json(new { status = false, error = "Invalid Id" }, JsonRequestBehavior.AllowGet);
        }
    }
}