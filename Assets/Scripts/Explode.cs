using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{
    public ParticleSystem ps;
    public GameObject grenadeHit;
    void Explosion()
    {
        ParticleSystem spawnedPS = Instantiate(ps, transform.position, Quaternion.identity);
        GameObject.Instantiate(grenadeHit, transform.position, Quaternion.identity);
        GameObject.Destroy(spawnedPS, 1f);
        GameObject.Destroy(gameObject);

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            Explosion();
            FMODUnity.RuntimeManager.CreateInstance("event:/Explosion 1").start();
        }
            
    }
}
