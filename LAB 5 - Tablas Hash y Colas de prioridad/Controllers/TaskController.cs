using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LAB_5___Tablas_Hash_y_Colas_de_prioridad.Helpers;
using LAB_5___Tablas_Hash_y_Colas_de_prioridad.Models;
using PagedList;

namespace LAB_5___Tablas_Hash_y_Colas_de_prioridad.Controllers
{
    public class TaskController : Controller
    {
        // GET: Task
        public ActionResult Index()
        {
            return View();
        }

        // GET: Task/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Task/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Task/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Task/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Task/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Task/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Task/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult CreateTask()
        {
            ViewBag.Message = Storage.Instance.currentUser;
            return View();
        }

        [HttpPost]
        public ActionResult CreateTask(FormCollection collection)
        {
            try
            {
                ViewBag.Message = Storage.Instance.currentUser;
                var task = new TaskModel
                {
                    Title = collection["Title"],
                    Description = collection["Description"],
                    Project = collection["Project"],
                    Priority = Convert.ToInt32(collection["Priority"]),
                    Date = Convert.ToDateTime(collection["Date"]),
                    Developer = Storage.Instance.currentUser.ToLower(),
                };

                if (task.Date < DateTime.Now)
                {
                    return View("Error");
                }
                if (task.Title == "" || task.Description == "" || task.Project == "")
                {
                    return View("Error");
                }
                ///<!--VALIDA QUE EL TITULO NO EXISTA EN LA LISTA GLOBAL NI EN LA LISTA TEMPORAL-->
                if (Storage.Instance.globalTaskList.Where(x => x.Title == task.Title).Count() != 0 && Storage.Instance.currentTaskList.Where(x => x.Title == task.Title).Count() != 0)
                {
                    return View("Error");
                }

                ///<!--AGREGA LA NUEVA TAREA A LAS ESTRUCTURAS-->
                Storage.Instance.currentTaskList.Add(task);

                ///<!--AGREGAR AQUI LA INSERCION EN EL ARCHIVO-->                                           

                Storage.Instance.taskInserted = true;
                Storage.Instance.csvModified = true;

                return RedirectToAction("Index_user", "User");
            }
            catch
            {
                return View("Error");
            }
        }

        public ActionResult WatchTasks_user(int? page)
        {
            ViewBag.Message = Storage.Instance.currentUser;
            int pageSize = 5;
            int pageNumber = (page ?? 1);

            return View(Storage.Instance.currentTaskList.OrderBy(x => x.Priority).ToPagedList<TaskModel>(pageNumber, pageSize));
        }

        public ActionResult WatchTasks_admin(int? page)
        {
            ViewBag.Message = Storage.Instance.currentUser;
            int pageSize = 5;
            int pageNumber = (page ?? 1);

            return View(Storage.Instance.globalTaskList.OrderBy(x => x.Priority).OrderBy(x => x.Developer).ToPagedList<TaskModel>(pageNumber, pageSize));
        }
    }
}
