using System;
using System.Web.Mvc;
using TaskManagementSystem1.Models;
using TaskManagementSystem1.Repositories;

namespace TaskManagementSystem1.Controllers
{
    public class TaskController : Controller
    {
        private readonly ITaskRepository _repo;

        // Unity will inject
        public TaskController(ITaskRepository repo)
        {
            _repo = repo;
        }

        // GET: /Task
        public ActionResult Index()
        {
            return View();
        }

        // AJAX: GET tasks JSON
        [HttpGet]
        public JsonResult GetTasks(string status = null, string assignedTo = null)
        {
            var list = _repo.GetAll(status, assignedTo);
            return Json(new { success = true, data = list }, JsonRequestBehavior.AllowGet);
        }

        // GET: create view
        public ActionResult Create()
        {
            return View();
        }

        // POST create via AJAX
        [HttpPost]
        public JsonResult CreateTask(TaskModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, errors = ModelStateErrors() });
            }

            try
            {
                var newId = _repo.Add(model);
                return Json(new { success = true, id = newId });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // GET single task
        [HttpGet]
        public JsonResult GetTask(int id)
        {
            var task = _repo.GetById(id);
            if (task == null) return Json(new { success = false, message = "Not found" }, JsonRequestBehavior.AllowGet);
            return Json(new { success = true, data = task }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateTask(TaskModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, errors = ModelStateErrors() });

            var ok = _repo.Update(model);
            return Json(new { success = ok });
        }

        [HttpPost]
        public JsonResult DeleteTask(int id)
        {
            var ok = _repo.SoftDelete(id);
            return Json(new { success = ok });
        }

        private object ModelStateErrors()
        {
            var errors = new System.Collections.Generic.List<string>();
            foreach (var item in ModelState.Values)
                foreach (var err in item.Errors)
                    errors.Add(err.ErrorMessage);
            return errors;
        }
    }
}
