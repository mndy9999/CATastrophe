using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cleanup : MonoBehaviour
{
    void Start()
    {
        GameObject.Destroy(transform.gameObject, 2f); //Deletes this object after 2 seconds
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player" && gameObject.tag != "Bullet")
        {
            Destroy(transform.gameObject, 0.1f);
        }
    }
}
