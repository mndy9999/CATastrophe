using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGExplosion : MonoBehaviour
{
    public ParticleSystem RPGFX;
    public GameObject RPGTrigger;

    void Explode()
    {
        Instantiate(RPGFX, transform.position, Quaternion.identity);
        Instantiate(RPGTrigger, transform.position, Quaternion.identity);
        GameObject.Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        FMODUnity.RuntimeManager.CreateInstance("event:/Explosion 1").start();
        Explode();
    }
}
