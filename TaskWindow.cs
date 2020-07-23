using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace TODOList {    
    public class TaskWindow : EditorWindow {
        TaskEditor editor;
        TaskWindowData editData;
        SerializedObject taskObj;
        SerializedProperty taskName;
        SerializedProperty tastDescription;
        string oldName;

        public void CreateField(TaskWindowData data) {
            editData = data;
            oldName = editData.Name;
            taskObj = new SerializedObject(editData);
            taskName = taskObj.FindProperty("Name");
            tastDescription = taskObj.FindProperty("Description");
        }

        public static void Open(TaskEditor editor) {
            Open(editor, new TaskWindowData());
        }

        public static void Open(TaskEditor editor, TaskWindowData data) {
            TaskWindow window = GetWindow<TaskWindow>("Task editor");
            window.minSize = new Vector2(300, 100);
            window.editor = editor;
            window.CreateField(data);
        }

        void OnGUI() {
            taskObj.Update();
            EditorGUILayout.PropertyField(taskName);
            EditorGUILayout.PropertyField(tastDescription, GUILayout.ExpandHeight(true));
            taskObj.ApplyModifiedProperties();
            if (GUILayout.Button("Save task")) {
                if (editData.Name != null && editData.Name.Trim() != "") {
                    if (editor.EditTask(oldName, editData.Name, editData.Description, false) == false) {
                        Task task = new Task(editData.Name, editData.Description);
                        editor.AddNewTask(task);
                    }
                    Close();
                }
            }
        }
    }
}
