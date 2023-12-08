using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Sounds : GenericSingleton<Sounds>
{
    private AudioSource bgMusicSource;
    private AudioSource sfxSource;
    public SoundSettings soundSettings;

    [Range(1f, 5f)]
    public float FadeOutSeconds = 2f;
    public Slider SFXSlider;
    public Slider BGMSlider;
    public Toggle BGMToggle;
    public Toggle SFXToggle;

    public override void Awake()
    {
        base.Awake();

        bgMusicSource = gameObject.AddComponent<AudioSource>();
        bgMusicSource.spatialBlend = 0.0f;
        bgMusicSource.volume = soundSettings.BGMusicVolume;
        bgMusicSource.loop = true;
        bgMusicSource.mute = soundSettings.MuteBGM;
        if (bgMusicSource.mute)
        {
            bgMusicSource.Stop();
            NotificationManager.Instance.AddNotification("BGM Source is muted!", 2f);
        }

        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.spatialBlend = 1.0f;
        sfxSource.volume = soundSettings.SFXVolume;
        sfxSource.mute = soundSettings.MuteBGM;

        BGMSlider.value = soundSettings.BGMusicVolume;
        SFXSlider.value = soundSettings.SFXVolume;

        BGMSlider.onValueChanged.AddListener((level) => { bgMusicSource.volume = level; soundSettings.BGMusicVolume = level; });
        SFXSlider.onValueChanged.AddListener((level) => { sfxSource.volume = level; soundSettings.SFXVolume = level; });

        BGMToggle.isOn = soundSettings.MuteBGM;
        SFXToggle.isOn = soundSettings.MuteSFX;

        BGMToggle.onValueChanged.AddListener((value) => { 
            bgMusicSource.mute = value; 
            soundSettings.MuteBGM = value;
            if (!value)
                ChangeBGM(soundSettings.BGMs[SceneManager.GetActiveScene().buildIndex]);
            else
                bgMusicSource.Stop();
        });
        SFXToggle.onValueChanged.AddListener((value) => { sfxSource.mute = value; soundSettings.MuteSFX = value; });

        SceneManager.sceneLoaded += (scene, _) =>
        {
            NotificationManager.Instance.AddNotification($"Loaded scene {scene.name}!", 5f);
            if (scene.buildIndex > 2)
                return;

            if (!bgMusicSource.mute)
                ChangeBGM(soundSettings.BGMs[scene.buildIndex]);
        };
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void ChangeBGM(AudioClip newBGM)
    {
        StartCoroutine(FadeOutAndChangeBGM(newBGM));
    }

    private IEnumerator FadeOutAndChangeBGM(AudioClip newBGM)
    {
        float time = FadeOutSeconds;
        if (bgMusicSource.clip != null)
        {
            while (time > 0)
            {
                bgMusicSource.volume -= Time.deltaTime / FadeOutSeconds;
                time -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            bgMusicSource.Pause();
        }
        else
        {
            bgMusicSource.volume = 0;
        }

        bgMusicSource.clip = newBGM;
        bgMusicSource.Play();
        time = FadeOutSeconds;
        while (time > 0)
        {
            bgMusicSource.volume += Time.deltaTime / FadeOutSeconds * soundSettings.BGMusicVolume;
            time -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
}
