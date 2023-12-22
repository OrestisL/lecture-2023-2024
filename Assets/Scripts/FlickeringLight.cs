using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    public float flickerDuration = 1.0f;
    private float _duration;
    private float startIntesity;
    private new Light light;

    private void Start()
    {
        light = GetComponent<Light>();
        startIntesity = light.intensity;
        _duration = flickerDuration;
    }

    private void Update()
    {
        _duration -= Time.deltaTime;
        if (_duration < -flickerDuration)
        {
            light.intensity = startIntesity;
            _duration = flickerDuration + Random.Range(0.0f, 0.2f);
        }
        else if (_duration <= 0)
        {
            light.intensity = 0;
        }

    }

}
