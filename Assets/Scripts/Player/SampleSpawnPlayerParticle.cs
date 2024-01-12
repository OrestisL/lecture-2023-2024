using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleSpawnPlayerParticle : MonoBehaviour
{
    public float interval = 5.0f;
    public GameObject particle;
    void Start()
    {
        StartCoroutine(SpawnParticles(interval));
    }

    private IEnumerator SpawnParticles(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            particle.GetComponent<PlayerMeshParticle>().Initialize(gameObject);
        }
    }
}
