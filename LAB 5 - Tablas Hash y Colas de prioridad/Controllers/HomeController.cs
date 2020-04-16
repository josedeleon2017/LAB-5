using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Text.RegularExpressions;
using LAB_5___Tablas_Hash_y_Colas_de_prioridad.Models;
using LAB_5___Tablas_Hash_y_Colas_de_prioridad.Helpers;

namespace LAB_5___Tablas_Hash_y_Colas_de_prioridad.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            try
            {
                if (Storage.Instance.first_load_users)
                {
                    string Path_users = Server.MapPath("~/App_Data/");

                    string FilePath_users = Path_users + "users_data.csv";
                    using (var fileStream = new FileStream(FilePath_users, FileMode.Open))
                    {
                        using (var streamReader = new StreamReader(fileStream))
                        {
                            streamReader.ReadLine();
                            while (!streamReader.EndOfStream)
                            {
                                var row = streamReader.ReadLine();

                                Regex regx = new Regex("," + "(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
                                string[] line = regx.Split(row);

                                var user = new UserModel
                                {
                                    Id = line[0],
                                    Password = line[1],
                                };
                                Storage.Instance.usersList.Add(user);
                            }
                        }
                    }
                    Storage.Instance.first_load_users = false;
                }

                if (Storage.Instance.first_load_tasks == true || Storage.Instance.csvModified == true)
                {
                    if (Storage.Instance.csvModified == true)
                    {
                        Storage.Instance.globalTaskList.Clear();
                        Storage.Instance.csvModified = false;
                    }

                    string Path_tasks = Server.MapPath("~/App_Data/");

                    string FilePath_tasks = Path_tasks + "final_data.csv";
                    using (var fileStream = new FileStream(FilePath_tasks, FileMode.Open))
                    {
                        using (var streamReader = new StreamReader(fileStream))
                        {
                            streamReader.ReadLine();
                            while (!streamReader.EndOfStream)
                            {
                                var row = streamReader.ReadLine();

                                Regex regx = new Regex(";" + "(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
                                string[] line = regx.Split(row);

                                var task = new TaskModel
                                {
                                    Title = line[0],
                                    Description = line[1],
                                    Project = line[2],
                                    Priority = Convert.ToInt32(regx.Split(row)[3]),
                                    Date = Convert.ToDateTime(line[4]),
                                    Developer = line[5],
                                };
                                Storage.Instance.globalTaskList.Add(task);
                            }
                        }
                    }                   
                    Storage.Instance.first_load_tasks = false;                   
                }
                ViewBag.Message = Storage.Instance.globalTaskList.Count(); 
                return View();
            }
            catch 
            {
                return View("Error");
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}