using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{

    AIFieldOfView fieldOfView;
    public float health = 100;
    public GameObject BloodParticles;


    // Start is called before the first frame update
    void Start()
    {
        fieldOfView = GetComponent<AIFieldOfView>();
    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0)
        {
            var particlePos = transform.position;
            particlePos.y += 0.5f;
            Instantiate(BloodParticles, particlePos, BloodParticles.transform.rotation);
            Destroy(gameObject);
        }
    }

    public void TakeDamage(WEAPON wpn)
    {
        fieldOfView.IsAttacked = true;
        if (wpn == WEAPON.GATTLING)
            health -= 2;
        if (wpn == WEAPON.GRENADE)
            health -= 100;
        if (wpn == WEAPON.RPG)
            health -= 50;
    }

}
