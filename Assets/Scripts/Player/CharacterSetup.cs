using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSetup : MonoBehaviour
{
    public TMP_InputField input;
    public Button button;
    public GameObject buttonPrefab;
    public Transform savedGamesButtonsParent;

    private void Start()
    {
        button.enabled = false;
        button.onClick.AddListener(() => LoadingScreen.Instance.LoadLevel(2));

        input.onEndEdit.AddListener(CheckEntry);
        input.onDeselect.AddListener(CheckEntry);

        SetupSavedGames();
    }

    public void CheckEntry(string s)
    {
        if (!string.IsNullOrEmpty(s))
        {
            button.enabled = true;
            PlayerData.Instance.SetPlayerName(s);
        }
    }

    private void SetupSavedGames()
    {
        string path = Path.Combine(Application.persistentDataPath, "Saved Data");
        if (!Directory.Exists(path))
        {
            Debug.Log("No saved data found");
            return;
        }

        string[] files = Directory.GetFiles(path);
        foreach (string file in files)
        {
            string fileName = Path.GetFileName(file);
            if (fileName.Equals("SoundSettings.json"))
                continue;

            GameObject button = Instantiate(buttonPrefab, savedGamesButtonsParent);
            string name = fileName.Split('_')[0];
            button.name = name;
            button.GetComponent<SavedGameButton>().Init(() => {
                PlayerData.Instance.LoadPlayerData(file);
                LoadingScreen.Instance.LoadLevel(PlayerData.Instance.data.SceneIndex);
            }, name);
        }
    }
}
