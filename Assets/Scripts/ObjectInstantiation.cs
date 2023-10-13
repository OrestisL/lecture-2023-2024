using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Lecture
{
    public class ObjectInstantiation : MonoBehaviour
    {
        private class OBJ
        {
            private static int count = 0;
            [SerializeField]
            private string name;
            [SerializeField]
            private PrimitiveType primitiveType;
            [SerializeField]
            private Material material;

            public OBJ(PrimitiveType primitiveType, Material material)
            {
                this.primitiveType = primitiveType;
                this.material = material;
            }

            public void CreateOBJ() 
            {
                name = string.Format("object {0}", count); 
                GameObject obj = GameObject.CreatePrimitive(primitiveType);
                obj.transform.position = RandomPosition(5);
                obj.name = name;
                obj.GetComponent<MeshRenderer>().material = material;
                count++;
            }

            private Vector3 RandomPosition(int radius) 
            {
                return UnityEngine.Random.insideUnitSphere * radius + Vector3.up * radius;
            }
        }

        private Button button;
        public Gradient gradient;
        public Material mat;

        void Start()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(CreateObject);
        }

        private void CreateObject()
        {
            Material material = new Material(mat);
            //change this
            Color color = UnityEngine.Random.ColorHSV();
            material.color = color;
            PrimitiveType pType = PrimitiveType.Capsule;
            GetRandomPrimitiveType(out pType);
            OBJ obj = new OBJ(pType, material);
            obj.CreateOBJ();
        }

        private void GetRandomPrimitiveType(out PrimitiveType pType) //PrimitiveType&
        {
            Array array = Enum.GetValues(typeof(PrimitiveType));
            pType = PrimitiveType.Plane;
            int rnd = UnityEngine.Random.Range(0, array.Length);
            if (rnd > 3)
            {
                GetRandomPrimitiveType(out pType);
                return;
            }
            pType = (PrimitiveType)array.GetValue(rnd);
        }
    }
}
