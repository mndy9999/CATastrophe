using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeKill : MonoBehaviour
{
    IEnumerator Cleanup()
    {
        yield return new WaitForEndOfFrame();
        GameObject.Destroy(gameObject);
    }
    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyStats>().TakeDamage(WEAPON.GRENADE);   
        }
            
        StartCoroutine(Cleanup());
    }
}
