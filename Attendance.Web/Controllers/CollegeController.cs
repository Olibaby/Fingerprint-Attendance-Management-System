using Attendance.Core;
using Attendance.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Attendance.Web.Controllers
{
    public class CollegeController : Controller
    {
        private IAttendanceManager _attmgr;
        public CollegeController(IAttendanceManager attmgr)
        {
            _attmgr = attmgr;
        }
        // GET: College
        public ActionResult Index()
        {
            var models = _attmgr.GetColleges();
            return View(models);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(CollegeModel model)
        {
            //model.CreatedBy = User.Identity.GetUserName();
            if (!ModelState.IsValid)
            {
                TempData["Message"] = "College is not valid";
                return RedirectToAction("Index", model);
            }
            _attmgr.Add(model);
            TempData["Message"] = $"{ model.CollegeName} was successfully added!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["Message"] = "College does not exist";
                return View("Index");
            }
            var model = _attmgr.GetCollege(id);
            if (model == null)
            {
                TempData["Message"] = "College cannot be found";
                return View("Index");
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(CollegeModel model)
        {
            if (ModelState.IsValid)
            {
                if (model == null)
                {
                    TempData["Message"] = "College is not found";
                    return View("Index");
                }
                //model.ModifiedBy = User.Identity.GetUserName();
                _attmgr.Update(model.CollegeId, model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id, string collegeName)
        {
            if (id > 0)
            {
                try
                {
                    _attmgr.RemoveCollege(id);
                    return Json(new { status = true, message = $" College has been successfully deleted!", JsonRequestBehavior.AllowGet });
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
                            _attmgr.RemoveCollege(intid);
                        }
                    }
                    return Json(new { status = true, message = " All selected college(s) has been successfully deleted!", JsonRequestBehavior.AllowGet });

                    //return Json(new { status = false, error = result.Message }, JsonRequestBehavior.AllowGet);
                }


            }

            return Json(new { status = false, error = "Invalid Id" }, JsonRequestBehavior.AllowGet);
        }
    }
}