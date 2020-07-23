namespace TODOList {    
    [System.Serializable]
    public class Task {
        public bool isDone;
        public string Name;
        public string Description;
        public string DataCreation;
        public string DataEnding;

        public Task() : this("", "", "", "", false) {}

        public Task(string name, string desription) : this (name, desription, "", "", false) { }    

        public Task(string name, string desription, string dataCreation, string dataEnding, bool isDone) {
            Name = name;
            Description = desription;
            DataCreation = dataCreation;
            DataEnding = dataEnding;
            this.isDone = isDone;
        }
    }
}