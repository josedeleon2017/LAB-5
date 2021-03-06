﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LAB_5___Tablas_Hash_y_Colas_de_prioridad.Models;

namespace LAB_5___Tablas_Hash_y_Colas_de_prioridad.Helpers
{
    public class Storage
    {
        private static Storage _instance = null;

        public static Storage Instance
        {
            get
            {
                if (_instance == null) _instance = new Storage();
                return _instance;
            }
        }

        ///<!--MANEJO DEL CSV-->
        ///<summary>
        ///Validador primera carga de users_data.cvs
        ///</summary>
        public bool first_load_users = true;

        ///<summary>
        ///Validador primera carga de tasks_data.cvs
        ///</summary>
        public bool first_load_tasks = true;

        ///<summary>
        ///Validador tasks_data.csv modificado
        ///</summary>
        public bool csvModified = false;

        ///<!--MANEJO DEL LOGIN-->
        /// <summary>
        /// Lista de usuarios para el Login
        /// </summary>
        public List<UserModel> usersList = new List<UserModel>();

        /// <summary>
        /// Usuario para el manejo de registros
        /// </summary>
        public string currentUser = null;


        ///<!--MANEJO DE LAS TAREAS-->    
        /// <summary>
        /// Lista global de todas las tareas, se lee de tasks_data.csv
        /// </summary>
        public List<TaskModel> globalTaskList = new List<TaskModel>();


        ///<!--ESTRUCTURAS-->
        ///<summary>
        ///Tabla Hash generica
        ///</summary>
        public NoLinealStructures.Structures.HashTable<TaskModel> HashTable = new NoLinealStructures.Structures.HashTable<TaskModel>();

        ///<summary>
        ///Heap generico
        ///</summary>
        public NoLinealStructures.Structures.Heap<string> Heap = new NoLinealStructures.Structures.Heap<string>();

        ///<summary>
        ///Resultado final de la busqueda
        ///</summary>
        public List<TaskModel> taskResult = new List<TaskModel>();

    }
}