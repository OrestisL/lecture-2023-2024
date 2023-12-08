using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

namespace Lecture
{
    public class Person
    {
        public string Name { get { return _name; } private set { _name = value; } }
        private string _name;
        public string LastName { get { return _lastName; } private set { _lastName = value; } }
        private string _lastName;
        public int Age { get { return _age; } private set { _age = value; } }
        private int _age;

        public Person() { }

        public Person(string name, string lastname, int age)
        {
            Name = name;
            LastName = lastname;
            Age = age;
        }
    }

    public class Util
    {

        public static void SaveData<T>(T data, string name) where T : class
        {
            string savePath = Path.Combine(Application.persistentDataPath, "Saved Data");
            if (!Directory.Exists(savePath))
                Directory.CreateDirectory(savePath);

            string json = JsonConvert.SerializeObject(data);

            string fullPath = Path.Combine(savePath, name);
            if (!File.Exists(fullPath))
                File.Create(fullPath).Close();

            using (StreamWriter sw = new StreamWriter(fullPath))
            {
                sw.Write(json);
            }
        }

        public static T LoadData<T>(string fullPath) where T : class
        {
            T data = null;
            using (StreamReader sr = new StreamReader(fullPath)) 
            {
                data = JsonConvert.DeserializeObject<T>(sr.ReadToEnd());
            }
            return data;
        }

        public static void LoadData<T>(string fullPath, out T data) where T : class
        {
            data = default;
            using (StreamReader sr = new StreamReader(fullPath))
            {
                data = JsonConvert.DeserializeObject<T>(sr.ReadToEnd());
            }
        }
    }

}