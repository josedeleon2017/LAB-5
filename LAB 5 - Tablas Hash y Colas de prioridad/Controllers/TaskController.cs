using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LAB_5___Tablas_Hash_y_Colas_de_prioridad.Helpers;
using LAB_5___Tablas_Hash_y_Colas_de_prioridad.Models;
using PagedList;
using System.IO;

namespace LAB_5___Tablas_Hash_y_Colas_de_prioridad.Controllers
{
    public class TaskController : Controller
    {
        
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
                ///<!--VALIDA QUE EL TITULO NO EXISTA EN LA LISTA TEMPORAL-->
                if (Storage.Instance.HashTable.ToList().Where(x => x.Title == task.Title).Count() != 0)
                {
                    return View("Error");
                }

                ///<!--AGREGA LA NUEVA TAREA A LAS ESTRUCTURAS-->
                TaskModel.Save_HashTable(task);
                TaskModel.Save_Heap(task.Title);

                ///<!--AGREGAR AQUI LA INSERCION EN EL ARCHIVO--> 
                string Path_csv = Server.MapPath("~/App_Data/");
                string FilePath_csv = Path_csv + "final_data.csv";
                if (TaskModel.SaveCSV(task, FilePath_csv))
                {
                    Storage.Instance.csvModified = true;
                }
                else
                {
                    return View("Error");
                }

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

            return View(Storage.Instance.HashTable.ToList().OrderBy(x => x.Priority).ToPagedList<TaskModel>(pageNumber, pageSize));
        }

        public ActionResult WatchTasks_general(int? page)
        {
            ViewBag.Message = Storage.Instance.currentUser;
            int pageSize = 5;
            int pageNumber = (page ?? 1);

            return View(Storage.Instance.globalTaskList.OrderBy(x => x.Priority).OrderBy(x => x.Developer).ToPagedList<TaskModel>(pageNumber, pageSize));
        }

        public ActionResult GenerateTask()
        {
            ViewBag.Message = Storage.Instance.currentUser + "  [" + Storage.Instance.HashTable.Count() + "]  ";
            Storage.Instance.taskResult.Clear();

            return View(Storage.Instance.taskResult);
        }

        [HttpPost]
        public ActionResult GenerateTask(FormCollection collection)
        {
            ViewBag.Message = Storage.Instance.currentUser + "  [" + Storage.Instance.HashTable.Count() + "]  ";
            Storage.Instance.taskResult.Clear();

            ///<!--SACA EL DE MAYOR PRIORIDAD DEL HEAP-->
            string keyTitle = Storage.Instance.Heap.RemoveRoot();
            if (keyTitle == " " || keyTitle == null)
            {
                return RedirectToAction("Index_user", "User");
            }
            else
            {
                var TaskToFind = new TaskModel
                {
                    Title = keyTitle,
                };
                ///<!--BUSQUEDA EN TABLA HASH-->
                Storage.Instance.taskResult.Add(Storage.Instance.HashTable.Find(Storage.Instance.HashTable.GetHash(TaskToFind), TaskToFind.Title));

                ///<!--BORRA DE TODAS LAS ESTRUCTURAS-->
                TaskModel TaskToDelete = Storage.Instance.HashTable.Find(Storage.Instance.HashTable.GetHash(TaskToFind), TaskToFind.Title);
                Storage.Instance.HashTable.Remove(TaskToDelete, Storage.Instance.HashTable.GetHash(TaskToDelete));
                Storage.Instance.globalTaskList.Remove(TaskToDelete);

                ///<!--AGREGAR AQUI LA SOBRE INSCRIPCION DEL ARCHIVO--> 
                string Path_csv = Server.MapPath("~/App_Data/");
                string FilePath_csv = Path_csv + "final_data.csv";
               if (TaskModel.WriteCSV(FilePath_csv))
                {
                    Storage.Instance.csvModified = true;
                    return View(Storage.Instance.taskResult);
                }
                else
                {
                    return View("Error");
                }                
                
            }
        }

        public ActionResult WatchDevelopers_admin(int? page)
        {
            ViewBag.Message = Storage.Instance.currentUser;
            int pageSize = 5;
            int pageNumber = (page ?? 1);

            return View(Storage.Instance.usersList.ToPagedList<UserModel>(pageNumber, pageSize));
        }


        public ActionResult WatchTasks_admin(string id, int? page)
        {
            ViewBag.Message = Storage.Instance.currentUser;
            int pageSize = 5;
            int pageNumber = (page ?? 1);

            Storage.Instance.taskResult.Clear();
            Storage.Instance.taskResult = Storage.Instance.globalTaskList.Where(x => x.Developer == id).OrderBy(x => x.Priority).ToList();
            return View(Storage.Instance.taskResult.ToPagedList<TaskModel>(pageNumber, pageSize));
        }
    }
}
