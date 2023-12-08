using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System;
using Lecture;

[Serializable]
public class SoundSettings 
{
    [Range(0f, 1f)]
    public float BGMusicVolume = 0.85f;
    [Range(0f, 1f)]
    public float SFXVolume = 0.3f;
    public bool MuteBGM, MuteSFX;
    [NonSerialized]
    public bool Created;
    public List<string> BGMs;

    public SoundSettings() { }

    public SoundSettings(float bgm, float sfx, bool muteBGM, bool muteSFX, List<string> names) 
    {
        BGMusicVolume = bgm;
        SFXVolume = sfx;
        MuteBGM = muteBGM;
        MuteSFX = muteSFX;
        BGMs = names;
    }

    public void ToJson() 
    {
        string name = "SoundSettings.json";
        Util.SaveData(this, name);
    }

    public void FromJson() 
    {
        string filePath = Path.Combine(Application.persistentDataPath,"Saved Data", "SoundSettings.json");

        if (!File.Exists(filePath))
            return;
        SoundSettings ss = Util.LoadData<SoundSettings>(filePath);

        BGMusicVolume = ss.BGMusicVolume; 
        SFXVolume = ss.SFXVolume;
        MuteBGM = ss.MuteBGM;
        MuteSFX = ss.MuteSFX;
        BGMs = ss.BGMs;

        Created = true;
    }
}
