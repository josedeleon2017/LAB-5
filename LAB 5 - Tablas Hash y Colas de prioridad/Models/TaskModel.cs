using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using LAB_5___Tablas_Hash_y_Colas_de_prioridad.Helpers;

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


        public static void HashTable_Add(TaskModel task)
        {
            Storage.Instance.HashTable.GetKeyValue = KeyConverter;
            Storage.Instance.HashTable.Add(task , Storage.Instance.HashTable.GetHash(task));
        }

        /// <summary>
        /// Delegado para obtener la llave del T value
        /// </summary>
        public static Converter<TaskModel, string> KeyConverter = delegate (TaskModel task)
        {
            return task.Title;
        };

        
        
    }
}