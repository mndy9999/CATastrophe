using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitGameButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(Exit);
    }

    private void Exit()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().Exit();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
