using UnityEngine;

public class PlayerMeshParticle : MonoBehaviour
{
    private ParticleSystem particle;

    public void Initialize(GameObject player) 
    {
        particle = GetComponent<ParticleSystem>();

        //get all skinned mesh renderers
        SkinnedMeshRenderer[] rends = player.GetComponentsInChildren<SkinnedMeshRenderer>();
        CombineInstance[] toCombine = new CombineInstance[rends.Length];
        for (int i = 0; i < rends.Length; i++)
        {
            Mesh current = new Mesh();
            rends[i].BakeMesh(current);
            toCombine[i].mesh = current;
            toCombine[i].transform = rends[i].transform.localToWorldMatrix;
        }
        Mesh mesh = new Mesh();
        mesh.CombineMeshes(toCombine);
        mesh.RecalculateBounds();

        //pass to particle system
        particle.GetComponent<ParticleSystemRenderer>().mesh = mesh;

        Instantiate(particle, player.transform.position, player.transform.rotation);
    }
}
