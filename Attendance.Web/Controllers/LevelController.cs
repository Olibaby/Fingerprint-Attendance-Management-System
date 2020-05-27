using Attendance.Core;
using Attendance.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Attendance.Web.Controllers
{
    public class LevelController : Controller
    {
        private IAttendanceManager _attmgr;
        public LevelController(IAttendanceManager attmgr)
        {
            _attmgr = attmgr;
        }
        // GET: Level
        public ActionResult Index()
        {
            var model = _attmgr.GetLevels();
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(LevelModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Message"] = "Level is not valid";
                return View("Create", model);
            }
            _attmgr.Add(model);
            TempData["Message"] = "Level has been successfully added";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["Message"] = "Level does not exist";
                return View("Index");
            }
            var model = _attmgr.GetLevel(id);
            if (model == null)
            {
                TempData["Message"] = "Level cannot be found";
                return View("Index");
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(LevelModel model)
        {
            if (ModelState.IsValid)
            {
                if (model == null)
                {
                    TempData["Message"] = "Level is not found";
                    return View("Index");
                }
                _attmgr.Update(model.LevelId, model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id, string levelName)
        {
            if (id > 0)
            {
                try
                {
                    _attmgr.RemoveLevel(id);
                    return Json(new { status = true, message = $" Level has been successfully deleted!", JsonRequestBehavior.AllowGet });
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
                            _attmgr.RemoveLevel(intid);
                        }
                    }
                    return Json(new { status = true, message = " All selected level(s) has been successfully deleted!", JsonRequestBehavior.AllowGet });

                    //return Json(new { status = false, error = result.Message }, JsonRequestBehavior.AllowGet);
                }


            }

            return Json(new { status = false, error = "Invalid Id" }, JsonRequestBehavior.AllowGet);
        }
    }
}