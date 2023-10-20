using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TestInputFieldEvents : MonoBehaviour
{
    private TMP_InputField inputField;
    void Start()
    {
        inputField = GetComponent<TMP_InputField>();

        //inputField.onSelect.AddListener((body) => Debug.Log(body));
        //inputField.onValueChanged.AddListener((body) => Debug.Log(body));
        //inputField.onEndEdit.AddListener((body) => Debug.Log(body));
        inputField.onDeselect.AddListener((body) => Debug.Log(body));
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Debug.Log("Space!");
        }

        //Debug.Log(Input.GetAxis("Vertical"));
        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log("Fire 1");
        }

        //Debug.Log(string.Format("x: {0}, y: {1}", Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")));
        //Debug.Log(string.Format("Scroll value: {0}", Input.GetAxis("Mouse ScrollWheel")));
    }
}
