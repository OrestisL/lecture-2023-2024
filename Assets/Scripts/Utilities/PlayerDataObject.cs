using UnityEditor;
using UnityEngine;

public class PlayerDataObject : ScriptableObject
{
    public string Pname;
    public int Level;
    public float Experience;
    public float HP;
    public Vector3 Position;
    public Quaternion Rotation;
    public int SceneIndex;

    public void UpdateData(PlayerData.Data data)
    {
        Pname = data.Pname; 
        Level = data.Level; 
        Experience = data.Experience;   
        HP = data.HP;
        SceneIndex = data.SceneIndex;
        Position = data.Position.ToVector3();
        Rotation = data.Rotation.ToQuaternion();
    }
#if UNITY_EDITOR
    [MenuItem("Assets/Create/Player Data")]
    public static void CreateMyAsset()
    {
        PlayerDataObject asset = CreateInstance<PlayerDataObject>();

        ProjectWindowUtil.CreateAsset(asset, "Player Data.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }
#endif
    public string ToJson()
    {
        return JsonUtility.ToJson(this, true);
    }
}
