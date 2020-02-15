using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour {

    [SerializeField] private int startingMenu = 1;
    [SerializeField] private string teamName = "Pawesome";

    private Image fadingScreen = null;
	private bool fadeIn = false, fadeOut=false;
	private Color fadingCol = Color.black;
	[SerializeField] private float fadingTime = 1f, fadeStep = 0.04f, showScreenTime =1f;
	
	void Awake ()
	{
        fadingScreen = GameObject.Find("fading screen").GetComponent<Image>();
        GameObject.Find("Team Name").GetComponent<Text>().text = teamName;
        fadingCol= fadingScreen.color;
	}
	void Start () 
	{
		StartCoroutine("ShowSplashScreen");
	}

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            SceneManager.LoadScene(3);
        }
    }

    void FixedUpdate()
	{
		FadeScreen();
	}

	IEnumerator ShowSplashScreen()
	{
		fadeIn = true;
		yield return new WaitForSeconds(fadingTime);
		fadeIn=false; 
		yield return new WaitForSeconds(showScreenTime);

		fadeOut = true;
		yield return new WaitForSeconds(fadingTime);
		fadeOut = false;
		SceneManager.LoadScene(startingMenu);
	}
	void FadeScreen()
	{
		if(fadeIn && fadingCol.a>0)
		{
			fadingCol.a -=fadeStep;
		}
		if(fadeOut && fadingCol.a<1)
		{
			fadingCol.a +=fadeStep;
		}
		fadingScreen.color=fadingCol;
	}
}
