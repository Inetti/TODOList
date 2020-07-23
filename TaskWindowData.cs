using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace TODOList {    
    public class TaskWindowData : Editor {
        public string Name;
        public string Description;

        public TaskWindowData(Task task) {
            Name = task.Name;
            Description = task.Description;
        }

        public TaskWindowData() {
            Name = "";
            Description = "";
        }
    }
}