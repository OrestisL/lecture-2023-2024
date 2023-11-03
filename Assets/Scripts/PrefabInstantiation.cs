using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;
using System;

public class PrefabInstantiation : MonoBehaviour
{
    public string prefabName;
    public GameObject prefab;
    public Button button;
    public int maxCount = 5;
    public UnityEvent<int> onTriggered;
    public Action action;

    private void Start()
    {
        if (prefab == null)
        {
            prefab = Resources.Load(Path.Combine("Prefabs", prefabName)) as GameObject;
        }
        //action += () => CreatePrefab(5);
        //action?.Invoke();
        button = GetComponent<Button>();
        button.onClick.AddListener(() => CreatePrefab(5));
        onTriggered.AddListener((i) => CreatePrefab(i));

        onTriggered.Invoke(123);
    }

    void CreatePrefab(int i)
    {
        //GameObject pf = Instantiate(prefab, Random.onUnitSphere, Quaternion.identity);
        //ItemProps props = pf.GetComponent<ItemProps>();
        //if (props != null)
        //{
        //    Debug.Log(props.name + " " + props.Info);
        //}
        //else
        //{
        //    props = pf.AddComponent<ItemProps>();
        //    Debug.Log(props.name + " " + props.Info);
        //}Z
        //for loop
        //for (int i = 0; i < maxCount; i++)
        //{
        //    Instantiate(prefab, Random.onUnitSphere, Quaternion.identity);
        //}
        //while loop
        //for (;;) { } //infinite loop
        //while (true) { } //infinite loop
        StartCoroutine(InstatiatePrefabs());
    }

    IEnumerator InstatiatePrefabs()
    {
        int i = 0;
        while (i < maxCount)
        {
            GameObject current = Instantiate(prefab, UnityEngine.Random.onUnitSphere, Quaternion.identity);
            i++;
            current.GetComponent<AbstractPrefabClass>().SetColor(UnityEngine.Random.ColorHSV());
            yield return new WaitForEndOfFrame();
        }

    }

}

