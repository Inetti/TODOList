using System;
using System.IO;
using System.Text; 
using System.Collections.Generic;
using UnityEngine;

namespace TODOList {    
    public class TaskEditor {
        static readonly string PATH_TO_FILE = Application.dataPath + "/todolist.json";

        List<Task> taskList;
        StringBuilder jsonBuilder;
        DateTime dateTime;

        public TaskEditor() {
            taskList = new List<Task>();
            jsonBuilder = new StringBuilder();
            if (Load() == false) {
                File.Create(PATH_TO_FILE);
            }
        }

        public bool Load() {
            if (File.Exists(PATH_TO_FILE)) {
                string[] json = File.ReadAllLines(PATH_TO_FILE);
                taskList.Clear();
                for (int i = 0; i < json.Length; i++) {
                    Task task = JsonUtility.FromJson<Task>(json[i].Trim());
                    taskList.Add(task);
                }
                return true;
            }
            return false;
        }

        public bool Safe() {
            if (File.Exists(PATH_TO_FILE)) {
                jsonBuilder.Clear();
                foreach (Task task in taskList) {
                    jsonBuilder.AppendLine(JsonUtility.ToJson(task));
                }
                File.WriteAllText(PATH_TO_FILE, jsonBuilder.ToString());
                return true;
            }
            return false;
        }

        public bool EditTask(string name, string newName, string newDescription, bool isDone) {
            Task task = GetTaskByName(name);
            if (task != null) {
                task.Name = newName;
                task.Description = newDescription;
                task.isDone = isDone;
                return Safe();
            }
            return false;
        }

        public bool AddNewTask(Task task) {
            if (GetTaskByName(task.Name) != null) {
                return false;
            }
            task.DataCreation = System.DateTime.Now.ToShortDateString();
            taskList.Add(task);
            return Safe();
        }

        public bool RemoveTask(Task task) {
            if (taskList.Contains(task)) {
                bool remove = taskList.Remove(task);
                Safe();
                return remove;
            }
            return false;
        }

        public Task[] AllTasks { get => taskList.ToArray(); }
        public Task[] DoneTasks {
            get {
                return taskList.FindAll(t => t.isDone).ToArray();
            }
        }

        public Task[] UndoneTasks {
            get {
                return taskList.FindAll(t => !t.isDone).ToArray();
            }
        }

        public Task GetTaskByName(string name) {
            return taskList.Find(t => t.Name == name);
        }

        public bool DoneTask(Task task, bool done) {
            if (taskList.Contains(task)) {
                task.isDone = done;
                task.DataEnding = done ? System.DateTime.Now.ToShortDateString() : "";
                Safe();
            }
            return false;
        }
    }
}