using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Lecture
{
    public class ExampleCsharp : ManagedClass
    {
        public int Integer;
        public List<int> List = new List<int>();
        public List<Person> Persons = new List<Person>();

        public Person George = new Person("George", "Papageorgiou", 35);

        public Button button;

        private void Awake()
        {
            Debug.Log("awake");
            List.Add(5);
            Persons.Add(George);
            Persons.Add(new Person());
            Persons.Add(new Person());
            Persons.Add(new Person());
            Persons.Add(new Person("Orestis", "Liaskos", 29));
            Debug.Log(George.Name);

            button.onClick.AddListener(() => { Debug.Log("button press"); });
        }

        private void OnEnable()
        {
            Debug.Log("enable");
            List.Add(23);

        }

        // Start is called before the first frame update
        void Start()
        {
            Debug.Log("start");
            List.Add(0);
        }

        // Update is called once per frame
        public override void OnUpdate()
        {
            Debug.Log("OnUpdate");
        }

        private void OnApplicationFocus(bool focus)
        {
            if (focus)
            {
                Debug.Log("focused");
                List.Insert(List.Count / 2, Random.Range(100, 300));
            }
            else
            {
                Debug.Log("not focused");
            }
        }

        private void OnDisable()
        {
            Debug.Log("disable");
            List.Remove(23);
            Debug.Log(Persons.Count(p => p.Name == null));
        }

        private void OnApplicationQuit()
        {
            Debug.Log("application quit");
        }
    }
}
