using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class SoundSettings : ScriptableObject
{
    [Range(0f, 1f)]
    public float BGMusicVolume = 0.85f;
    [Range(0f, 1f)]
    public float SFXVolume = 0.3f;
    public bool MuteBGM, MuteSFX;

    public List<AudioClip> BGMs;

    [MenuItem("Assets/Create/Sound Settings")]
    public static void CreateMyAsset()
    {
        SoundSettings asset = CreateInstance<SoundSettings>();

        ProjectWindowUtil.CreateAsset(asset, "Sound Settings.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }
}
