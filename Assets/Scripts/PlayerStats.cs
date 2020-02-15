using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    class PlayerStats : MonoBehaviour
    {
        private float _health;
        public Animator anim;
        public float Health
        {
            get
            {
                return _health;
            }
        }

        private void Start()
        {
            _health = 100f;
        }

        public void TakeDamage(WEAPON wpn)
        {
            if (wpn == WEAPON.SPEAR)
                _health -= 7;

            if (wpn == WEAPON.BONE)
                _health -= 4;

            if (wpn == WEAPON.BOSSPUNCH)
                _health -= 6;
        }

        IEnumerator DeathGUI()
        {
            yield return new WaitForSeconds(3f);
            GameObject.Find("GameManager").GetComponent<GameManager>().PlayerDied();
        }
        private void Update()
        {   
            if(_health <= 0)
            {
                GetComponent<BoxCollider>().enabled = false;
                StartCoroutine(DeathGUI());
                //Partlces here
                anim.SetBool("IsDead", true);
            }
        }
        public void HealthPack()
        {
            _health += 20;
            if(_health > 100)
            {
                _health = 100;
            }
        }
    }
}
