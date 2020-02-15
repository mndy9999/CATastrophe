using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGHit : MonoBehaviour
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
            Debug.Log("Here");
            collision.gameObject.GetComponent<EnemyStats>().TakeDamage(WEAPON.RPG);
        }

        StartCoroutine(Cleanup());
    }
}
