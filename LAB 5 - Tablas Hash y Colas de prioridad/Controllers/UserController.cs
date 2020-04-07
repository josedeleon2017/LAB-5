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
               
        public ActionResult Index_user()
        {
            try
            {
                ViewBag.Message = Storage.Instance.currentUser;              
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
            ///Storage.Instance.Heap.Clear();          
            
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
                        List<TaskModel> FilteredList = new List<TaskModel>();
                        FilteredList = Storage.Instance.globalTaskList.Where(x => x.Developer.ToUpper() == Storage.Instance.currentUser).ToList();

                        for(int i = 0; i < FilteredList.Count(); i++)
                        {
                            TaskModel.Save_HashTable(FilteredList.ElementAt(i));
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
