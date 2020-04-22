using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using LAB_5___Tablas_Hash_y_Colas_de_prioridad.Helpers;
using System.IO;


namespace LAB_5___Tablas_Hash_y_Colas_de_prioridad.Models
{
    public class TaskModel 
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Project { get; set; }
        [Required]
        public int Priority { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-mm-yyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date")]
        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string Developer { get; set; }

        /// <summary>
        /// Metodo para guardar objeto en la Tabla Hash
        /// </summary>
        public static void Save_HashTable(TaskModel task)
        {
            Storage.Instance.HashTable.GetKeyValue = KeyConverter;
            Storage.Instance.HashTable.Add(task , Storage.Instance.HashTable.GetHash(task));
        }

        public static void Save_Heap(string Title)
        {
            Storage.Instance.Heap.GetPriorityValue = GetPriorityValue;
            Storage.Instance.Heap.Comparer = TitleComparison;
            Storage.Instance.Heap.Add(Title);
        }

        /// <summary>
        /// Metodo para registrar el objeto en el CSV
        /// </summary>
        public static bool SaveCSV(TaskModel task, string pathcsv)
        {
            try
            {
                StreamWriter streamWriter = File.AppendText(pathcsv);

                string[] row = new string[6];
                row[0] = task.Title;
                row[1] = '"' + task.Description + '"';
                row[2] = task.Project;
                row[3] = Convert.ToString(task.Priority);
                row[4] = task.Date.ToShortDateString();
                row[5] = task.Developer;

                string lineToAdd = "\n";
                for (int i = 0; i < 5; i++)
                {
                    lineToAdd += row[i] + ";";

                    if(i == 4)
                    {
                        lineToAdd += row[5];
                        i++;
                    }
                }
                streamWriter.Write(lineToAdd);
                streamWriter.Close();
                return true;
            }
            catch 
            {
                return false;
            }
        }

        /// <summary>
        /// Metodo para sobre escribir por completo el csv con las modificaciones de la lista global
        /// </summary>
        /// <param name="pathcsv"></param>
        public static bool WriteCSV(string pathcsv)
        {
            try
            {
                StreamWriter streamWriter = new StreamWriter(pathcsv);
                string header = "Title;Description;Project;Priority;Date;Developer";
                streamWriter.Write(header);
                streamWriter.Close();

                for (int i = 0; i < Storage.Instance.globalTaskList.Count() ; i++)
                {
                    StreamWriter streamOverWriter = File.AppendText(pathcsv);
                    TaskModel currentTask = Storage.Instance.globalTaskList.ElementAt(i);

                    string[] row = new string[6];
                    row[0] = currentTask.Title;
                    row[1] = currentTask.Description;
                    row[2] = currentTask.Project;
                    row[3] = Convert.ToString(currentTask.Priority);
                    row[4] = currentTask.Date.ToShortDateString();
                    row[5] = currentTask.Developer;

                    string lineToAdd = "\n";
                    for (int j = 0; j < 5; j++)
                    {
                        lineToAdd += row[j] + ";";

                        if (j == 4)
                        {
                            lineToAdd += row[5];
                            j++;
                        }
                    }
                    streamOverWriter.Write(lineToAdd);
                    streamOverWriter.Close();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Delegado para obtener la llave del T value
        /// </summary>
        /// 
        public static Converter<TaskModel, string> KeyConverter = delegate (TaskModel task)
        {
            return task.Title;
        };

        public static Func<string, int> GetPriorityValue = delegate (string Title)
         {
             TaskModel TaskTitle= new TaskModel();
             TaskTitle.Title = Title;

             TaskModel Temp = Storage.Instance.HashTable.Find(Storage.Instance.HashTable.GetHash(TaskTitle), Title);
             return Temp.Priority;
         };

        public static Comparison<string> TitleComparison = delegate (string Title1, string Title2)
        {
            return Title1.CompareTo(Title2);
        };


    }
}