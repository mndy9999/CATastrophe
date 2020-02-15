using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoritePickup : MonoBehaviour
{
    private GameManager GM;

    private void Start()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        GM.LevelWon();
    }
}
