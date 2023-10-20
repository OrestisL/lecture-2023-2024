//create a namespace with your student code/name (will be used in all homeworks)
//change file name to Homework1_nmxxxxxxx

using System;
using UnityEngine;
using UnityEngine.UI;

//Create a script that uses a button to instantiate maxCount prefabs placed on a cirlce.
//Prefabs should sample their color from a gradient.
//The button should create the prefabs with a single click and be disabled while the objects 
//are being created. There should also be a time delay between each object being created.
//change this
namespace nmxxxxxx
{
    //change class name to match file name
    public class Homework2 : MonoBehaviour
    {
        //create a prefab to instantiate
        public GameObject prefab; //reference to prefab
        //used to sample colors
        public Gradient gradient;
        //amount of gameobjects to create
        public int maxCount;
        //button to press
        public Button button;

        private void Start()
        {
            //either assign prefab from the editor or load from resources
            button.onClick.AddListener(() => CreatePrefabs());
        }

        void CreatePrefabs() 
        {
            //complete this
        }
    }
}
