using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingBehaviour : MonoBehaviour
{
    public RectTransform icon;
    public Text levelNumber;
    public Text levelName;
    // Start is called before the first frame update
    void Start()
    {
        Color temp = GetComponent<Image>().color;
        temp.a = 0;
        GetComponent<Image>().color = temp;
        temp = icon.GetComponent<Image>().color;
        temp.a = 0;
        icon.GetComponent<Image>().color = temp;
        temp = levelNumber.GetComponent<Text>().color;
        temp.a = 0;
        levelNumber.GetComponent<Text>().color = temp;
        temp = levelName.GetComponent<Text>().color;
        temp.a = 0;
        levelName.GetComponent<Text>().color = temp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator RotateIcon()
    {
        icon.rotation = Quaternion.Euler(0, 0, 0);
        while (true){
            icon.Rotate(0, 5, 0);
            yield return new WaitForSecondsRealtime(.01f);
        }
    }
    public void StartLoading(LEVEL level)
    {
        Color temp = GetComponent<Image>().color;
        temp.a = 1;
        GetComponent<Image>().color = temp;
        temp = icon.GetComponent<Image>().color;
        temp.a = 1;
        icon.GetComponent<Image>().color = temp;
        temp = levelNumber.GetComponent<Text>().color;
        temp.a = 1;
        levelNumber.GetComponent<Text>().color = temp;
        temp = levelName.GetComponent<Text>().color;
        temp.a = 1;
        levelName.GetComponent<Text>().color = temp;
        if (level == LEVEL.LEVEL1)
        {
            levelNumber.text = "LEVEL 1";
            levelName.text = "JUNGLE";
        }
        if (level == LEVEL.LEVEL2)
        {
            levelNumber.text = "LEVEL 2";
            levelName.text = "CAVE";
        }
        StartCoroutine(RotateIcon());
    }

    public void StopLoading()
    {
        Color temp = GetComponent<Image>().color;
        temp.a = 0;
        GetComponent<Image>().color = temp;
        temp = icon.GetComponent<Image>().color;
        temp.a = 0;
        icon.GetComponent<Image>().color = temp;
        temp = levelNumber.GetComponent<Text>().color;
        temp.a = 0;
        levelNumber.GetComponent<Text>().color = temp;
        temp = levelName.GetComponent<Text>().color;
        temp.a = 0;
        levelName.GetComponent<Text>().color = temp;
        StopAllCoroutines();
    }
}
