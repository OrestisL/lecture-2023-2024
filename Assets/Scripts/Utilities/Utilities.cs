using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;

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
        public static void ChangeScene(int index)
        {
            //get loading scene
            SceneManager.LoadScene(0);
            SceneLoader.Instance.StartCoroutine(ChangeSceneAsync(index));
        }
        public static void ChangeScene(string name)
        {
            //get loading scene
            SceneManager.LoadScene(0);
            SceneLoader.Instance.StartCoroutine(ChangeSceneAsync(name));
        }
        private static IEnumerator ChangeSceneAsync(int index)
        {
            AsyncOperation asyncOp = SceneManager.LoadSceneAsync(index);
            //asyncOp.allowSceneActivation = false;
            while (!asyncOp.isDone)
            {
                yield return null;
            }

        }

        private static IEnumerator ChangeSceneAsync(string sceneName) 
        {
            AsyncOperation asyncOp = SceneManager.LoadSceneAsync(sceneName);
            //asyncOp.allowSceneActivation = false;
            while (!asyncOp.isDone)
            {
                yield return null;
            }

        }
    }

}