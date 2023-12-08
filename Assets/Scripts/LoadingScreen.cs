using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen : GenericSingleton<LoadingScreen>
{
    public ProgressBar progressBar;
    public SampleInputMapCreation controls;
    public TextMeshProUGUI loadingText;
    public Canvas canvas;
    public override void Awake()
    {
        base.Awake();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene s, LoadSceneMode m)
    {
        if (s.name.Equals("CharacterSetup"))
            return;

        if (controls == null)
            controls = SampleInputMapCreation.Instance;

        if (s.buildIndex == 3)
            controls.perspectiveType = SampleInputMapCreation.PerspectiveType.thirdPerson;
        else if (s.buildIndex == 2)
            controls.perspectiveType = SampleInputMapCreation.PerspectiveType.firstPerson;
    }

    public void LoadLevel(int index)
    {
        //disable player inputs
        if (controls != null)
            controls.playerInput.actions.Disable();
        //show loading screen
        canvas.enabled = true;
        StartCoroutine(ChangeSceneAsync(index));
    }

    private IEnumerator ChangeSceneAsync(int index)
    {
        loadingText.text = "Loading scene";
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(index);
        asyncOp.allowSceneActivation = false;
        while (asyncOp.progress < 0.9f)
        {
            progressBar.Progress = asyncOp.progress;
            yield return new WaitForEndOfFrame();
        }
        while (asyncOp.progress >= 0.9f)
        {
            progressBar.Progress = 1.0f;
            loadingText.text = "Press any key to continue";
            if (Input.anyKey)
            {
                asyncOp.allowSceneActivation = true;
                canvas.enabled = false;
                //controls.playerInput.actions.Enable();
            }
            yield return new WaitForEndOfFrame();
        }

    }

    private IEnumerator ChangeSceneAsync(string sceneName)
    {
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(sceneName);
        asyncOp.allowSceneActivation = false;
        while (!asyncOp.isDone)
        {
            yield return null;
        }

    }
}
