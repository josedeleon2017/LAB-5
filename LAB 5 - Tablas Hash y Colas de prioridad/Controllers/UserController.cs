using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LAB_5___Tablas_Hash_y_Colas_de_prioridad.Models;
using LAB_5___Tablas_Hash_y_Colas_de_prioridad.Helpers;
using System.IO;
using System.Text.RegularExpressions;

namespace LAB_5___Tablas_Hash_y_Colas_de_prioridad.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        // GET: User/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
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

        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: User/Edit/5
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

        // GET: User/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: User/Delete/5
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

        
        public ActionResult Index_user()
        {
            try
            {
                ViewBag.Message = Storage.Instance.currentUser;

                if (Storage.Instance.taskInserted)
                {
                    ///<!--AGREGAR AQUI LA INSERCION EN LAS ESTRUCTURAS TEMPORALES-->
                    TaskModel.HashTable_Add(Storage.Instance.currentTaskList.Last());
                    ///Monticulo.Add(Storage.Instance.currentTaskList.Last());
                    
                    Storage.Instance.taskInserted = false;
                }

                var test = new TaskModel
                {
                    Title= "Donec Pharetra Magna Vestibulum",
                };

                TaskModel testFind = Storage.Instance.HashTable.Find(Storage.Instance.HashTable.GetHash(test), test.Title);
                return View();
            }
            catch 
            {
                return View("Error");
            }      
        }


        public ActionResult Index_admin()
        {
            ViewBag.Message = Storage.Instance.currentUser;
            return View();
        }

        public ActionResult Login()
        {
            ///<!--VACIA TODAS LAS ESTRUCTURAS TEMPORALES-->
            Storage.Instance.HashTable.Clear();
            ///Storage.Instance.Monticulo = null;
            Storage.Instance.currentTaskList.Clear();          
            
            return View();   
        }

        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            try
            {
                UserModel currentUser = Storage.Instance.usersList.Where(x => x.Id == collection["Id"]).FirstOrDefault();

                if (currentUser != null)
                {
                    Storage.Instance.currentUser = currentUser.Id.ToUpper();
                    if (collection["Password"].ToUpper() == currentUser.Password.ToUpper() && currentUser.Password.ToUpper() == "ADMIN")
                    {
                        return RedirectToAction("Index_admin", "User");
                    }
                    if (collection["Password"].ToUpper() == currentUser.Password.ToUpper())
                    {
                        Storage.Instance.currentTaskList = Storage.Instance.globalTaskList.Where(x => x.Developer.ToUpper() == Storage.Instance.currentUser).ToList();

                        for(int i = 0; i < Storage.Instance.currentTaskList.Count(); i++)
                        {
                            TaskModel.HashTable_Add(Storage.Instance.currentTaskList.ElementAt(i));
                        }     
                        return RedirectToAction("Index_user", "User");
                    }
                }
                return View("Error");
            }
            catch
            {
                return View("Error");
            }
        }

        public ActionResult Exit()
        { 
            return RedirectToAction("Index", "Home");
        }

    }
}
