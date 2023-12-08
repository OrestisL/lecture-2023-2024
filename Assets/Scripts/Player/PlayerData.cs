using Lecture;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerData : GenericSingleton<PlayerData>
{
    [Serializable]
    public class Data
    {
        public string Pname { get; set; }
        public int Level { get; set; }
        public float Experience { get; set; }
        public float HP { get; set; }
        public float[] Position { get; set; }
        public float[] Rotation { get; set; }
        public int SceneIndex { get; set; }

        public Data(string Pname, int Level, float Experience, float HP, float[] Position, float[] Rotation, int sceneIndex)
        {
            this.Pname = Pname;
            this.Level = Level;
            this.Experience = Experience;
            this.HP = HP;
            this.Position = Position;
            this.Rotation = Rotation;
            SceneIndex = sceneIndex;
        }

    }
    public PlayerDataObject dataObject;
    public Data data;
    private Transform player;
    private string _playerName;
    public string PlayerName { get { return _playerName; } }
    private int _level;
    public int Level { get { return _level; } }
    private float _experience;
    public float Experience { get { return _experience; } }
    private float _hp;
    public float HP { get { return _hp; } }

    public void CalculateExperienceThreshold(float addedExp)
    {
        _experience += addedExp;
        float required = Mathf.Exp(_level) / _level; //change if you want
        if (_experience >= required)
        {
            _experience -= required;
            _level++;
        }
    }
    public void SetPlayerName(string pName)
    {
        _playerName = pName;
    }
    public override void Awake()
    {
        base.Awake();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.Equals("CharacterSetup"))
            return;

        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (data != null)
        {
            if (scene.buildIndex == data.SceneIndex)
            {
                CharacterController controller = player.GetComponent<CharacterController>();
                controller.enabled = false;
                player.transform.position = data.Position.ToVector3();
                controller.enabled = true;
                Quaternion startRotation = data.Rotation.ToQuaternion();
                FirstPersonMovement movement = player.GetComponent<FirstPersonMovement>();
                if (movement)
                {
                    movement.SetStartingValues(startRotation.eulerAngles.x, startRotation.eulerAngles.y);
                }
                switch (SampleInputMapCreation.instance.perspectiveType)
                {
                    case SampleInputMapCreation.PerspectiveType.firstPerson:
                        Camera.main.transform.rotation = startRotation;
                        break;
                    case SampleInputMapCreation.PerspectiveType.thirdPerson:
                        Camera.main.transform.parent.rotation = startRotation;
                        break;

                }
            }
        }
    }

    public void LoadPlayerData(string fullPath)
    {
        Util.LoadData(fullPath, out data);
        //data = Util.LoadData<Data>(fullPath);
        _playerName = data.Pname;
        _level = data.Level;
        _experience = data.Experience;
        _hp = data.HP;
    }
    void SaveData()
    {
        if (player == null)
            return;

        data = new Data(PlayerName, Level, Experience, HP, player.transform.position.ToFloatArray(), Camera.main.transform.rotation.ToFloatArray(), SceneManager.GetActiveScene().buildIndex);
        Util.SaveData(data, string.Format("{0}_SavedData.json", PlayerName));
//#if UNITY_EDITOR
//        string directory = Path.Combine(Application.dataPath, "SavedData");
//#else
//        string directory = Path.Combine(Directory.GetCurrentDirectory(), "SavedData");
//#endif
//        if (dataObject == null)
//        {

//            if (!Directory.Exists(directory))
//                Directory.CreateDirectory(directory);

//            dataObject = ScriptableObject.CreateInstance<PlayerDataObject>();
//            dataObject.name = "Player Data.asset";
//        }
//        dataObject.UpdateData(data);
//        string filePath = Path.Combine(directory, dataObject.name);
//        File.WriteAllText(filePath, dataObject.ToJson());
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }
}
