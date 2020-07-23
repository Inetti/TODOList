using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace TODOList {    
    public class TODOWindow : EditorWindow {
        TaskEditor editor;
        Task[] tasks;
    
        [MenuItem("Tools/TODO List")]
        public static void Open() => GetWindow<TODOWindow>("TODO List");    

        void OnEnable() {
            editor = new TaskEditor();
        }

        int countUndoneTask = 0;
        void RefreshTitle() {
            countUndoneTask = editor.UndoneTasks.Length;
            titleContent.text = $"TODO List ({countUndoneTask})";
        }

        void OnDisable() {
            editor.Safe();
        }

        Vector2 scrollPos = Vector2.zero;
        string[] taskTypes = new string[] { "Undone", "Done", "All tasks" };
        int taskType;
        void OnGUI() {
            if (countUndoneTask != editor.UndoneTasks.Length) {
                RefreshTitle();
            }
            CreateToolBar();
            EditorGUILayout.Separator();
            using (var scrollView = new GUILayout.ScrollViewScope(scrollPos, EditorStyles.helpBox)) {
                scrollPos = scrollView.scrollPosition;
                    foreach (Task task in tasks) {
                        GUI.backgroundColor = task.isDone ? Color.green : Color.red;
                        CreateTask(task);
                        GUI.backgroundColor = Color.white;
                    }
                
            }
        }       

        void CreateToolBar() {
            using (new EditorGUILayout.HorizontalScope()) {
                taskType = GUILayout.Toolbar(taskType, taskTypes, EditorStyles.toggleGroup, GUILayout.Height(20), GUILayout.Width(250));
                SelectTask(taskType);
                if (GUILayout.Button("New Task")) {
                    TaskWindow.Open(editor);
                }
            }
        }

        void SelectTask(int typeId) {
            string type = taskTypes[typeId];
            switch (type) {
                case "Undone":
                    tasks = editor.UndoneTasks;
                    break;
                case "Done":
                    tasks = editor.DoneTasks;
                    break;
                case "All tasks":
                    tasks = editor.AllTasks;
                    break;
                default:
                    tasks = editor.AllTasks;
                    break;
            }

        }

        void CreateTask(Task task) {
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox)) {
                GUIStyle style = new GUIStyle();
                style.alignment = TextAnchor.MiddleCenter;
                style.fontStyle = FontStyle.Bold;
                EditorGUILayout.LabelField(task.Name, style);
                using (new EditorGUILayout.HorizontalScope()) {
                    CreateTaskDoneCheckBox(task);
                    CreateTaskDescription(task);
                }
                using (new EditorGUILayout.HorizontalScope()) {
                    Color taskColor = GUI.backgroundColor;
                    GUI.backgroundColor = Color.grey;
                    string data = task.isDone ? $"Done ({task.DataEnding})" : $"Created ({task.DataCreation})";
                    EditorGUILayout.LabelField(data);
                    if (task.isDone == false) {
                        if (GUILayout.Button("Edit", GUILayout.Width(60))) {
                            TaskWindow.Open(editor, new TaskWindowData(task));
                        }
                    }

                    if (GUILayout.Button("Delete", GUILayout.Width(60))) {
                        editor.RemoveTask(task);
                    }
                    GUI.backgroundColor = taskColor;
                }
            }
        }

        private void CreateTaskDoneCheckBox(Task task) {
            editor.DoneTask(task, GUILayout.Toggle(task.isDone, "", GUILayout.Width(20)));
        }

        void CreateTaskDescription(Task task) {
            GUIStyle stile = EditorStyles.helpBox;
            stile.fontSize = 14;
            GUILayout.Label(task.Description, stile);
        }
    }
}