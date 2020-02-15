using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartingScene : MonoBehaviour
{

    [SerializeField] private int gameLevel = 3;
    [SerializeField] private GameObject[] actor = new GameObject[1];
    [SerializeField] private GameObject[] eyes = new GameObject[3];
    private int activeEyes = 0;
    [SerializeField] private GameObject dialogUI = null;
    [SerializeField] private Text txtDialogue = null;
    private string[] StoryLine = new string[6];
    [SerializeField] private GameObject missionUI = null;

    private bool isTalking = false;
    private bool isTyping = false;
    private bool cancelTyping = false;

    // fading screen variables :
    [SerializeField] private Image fadingScreen = null;
    private bool fadeIn = false, fadeOut = false;
    private Color fadingCol = Color.black;
    [SerializeField] private float fadingTime = 1f, fadeStep = 0.04f;

    // sound related :
    private AudioSource Sfx = null;
    [SerializeField] private AudioClip[] sfxClip = new AudioClip[5];

    void Awake()
    {
        StoryLine[0] = "Hello, is this Agent 135?\nThis is Agent Keanmew Fleas, from the Feline Secret Service.";
        StoryLine[1] = "I’m so glad I got a hold of you.The Feline Center for Disease Control has issued a nationwide warning.";
        StoryLine[2] = "The Feline Lukemia Virus is out of control.";
        StoryLine[3] = "Well, the good news is we found a cure. The bad news is we’ll need you to come out of retirement to help us out. One last time, I promise.";
        StoryLine[4] = "I wish there was anyone else I could ask, but you’re the only one who can pilot the time machine. Please, 135. You’re the world’s only hope.";
        StoryLine[5] = "Thank you! Thank you! I knew I could count on you. I'm gonna send Mission File over the phone. Good luck.";

        Sfx = GetComponent<AudioSource>();
        fadingScreen.gameObject.SetActive(true);
        fadingCol = fadingScreen.color;
        StartCoroutine("Blinking");
    }
    void Start()
    {
        StartCoroutine("PlayCutScene");
    }

    void FixedUpdate()
    {
        FadeScreen();
        if (isTalking)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (isTyping)
                {
                    cancelTyping = true;
                }
            }
        }
    }

    IEnumerator Blinking()
    {
        while(!isTalking)
        {
            SetActiveEye(2);
            yield return new WaitForSeconds(0.1f);
            SetActiveEye(activeEyes);
            yield return new WaitForSeconds(2.5f);
        }
    }

    void SetActiveEye (int eye)
    {
        for(int i =0 ; i < eyes.Length; i++)
        {
            if (i == eye)
                eyes[i].SetActive(true);
            else
                eyes[i].SetActive(false);
        }
    }

    IEnumerator PlayCutScene()
    {
        actor[0].GetComponent<Animation>().Play("sitting_chill");
        fadeIn = true;
        yield return new WaitForSeconds(fadingTime);
        fadeIn = false;
        fadingScreen.gameObject.SetActive(false);
        Vector3 start = Camera.main.transform.position;
        Vector3 end = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z + 8);
        float gap = Vector3.Distance(start, end);
        for (float t = 0; t <= gap; t += 0.05f)
        {
            Camera.main.transform.position = Vector3.MoveTowards(start, end, t);
            yield return new WaitForSeconds(0f);
        }
        yield return new WaitForSeconds(1f);
        // phone ringing :
        Sfx.PlayOneShot(sfxClip[0]);
        yield return new WaitForSeconds(2.5f);
        actor[0].GetComponent<Animation>().Play("sitting_pick_up_phone");
        yield return new WaitForSeconds(2f);
        Sfx.PlayOneShot(sfxClip[1]);
        yield return new WaitForSeconds(1f);

        dialogUI.SetActive(true);
        isTalking = true;
        actor[0].GetComponent<Animation>().Play("talking_on_phone_1");
        for (int i = 0; i <= 5; i++)
        {
            Sfx.Stop();
            int letter = 0;
            string lineOfText = StoryLine[i];
            txtDialogue.text = "";
            isTyping = true;
            cancelTyping = false;
            while (isTyping && !cancelTyping && (letter < lineOfText.Length - 1))
            {
                if (!Sfx.isPlaying) Sfx.PlayOneShot(sfxClip[2]);
                txtDialogue.text += lineOfText[letter];
                letter += 1;
                yield return new WaitForSeconds(0);
            }
            txtDialogue.text = lineOfText;
            yield return new WaitForSeconds(0);
            isTyping = false;
            cancelTyping = false;

            if (i == 3) activeEyes = 1;
            else activeEyes = 0;
            SetActiveEye(activeEyes);

            if (i != 1 && i != 5) yield return StartCoroutine(WaitForPlayerConfirm());
            else yield return StartCoroutine(WaitForPlayerPress());

            yield return new WaitForSeconds(0.2f);
        }

        dialogUI.SetActive(false);
        isTalking = false;
        Sfx.Stop();

        // Open Mission Briefing and play data processing sound
        missionUI.SetActive(true);
        yield return StartCoroutine(WaitForPlayerPress());
        missionUI.SetActive(false);

        Sfx.PlayOneShot(sfxClip[1]);
        actor[0].GetComponent<Animation>().Play("phone_end");
        StartCoroutine("Blinking");
        yield return new WaitForSeconds(2);
        activeEyes = 1;
        yield return new WaitForSeconds(3);
        
        // fade screen and go to time machine


        StartCoroutine("StartTheGame");
    }

    IEnumerator WaitForPlayerPress()
    {
        while (!Input.GetMouseButtonDown(0))
            yield return null;
    }

        IEnumerator WaitForPlayerConfirm()
    {
        while (!Input.GetMouseButtonDown(0))
            yield return null;
        txtDialogue.text = "...";
        actor[0].GetComponent<Animation>().Play("talking_on_phone_2");
        Sfx.PlayOneShot(sfxClip[3]);
        yield return new WaitForSeconds(1.5f);
        Sfx.Stop();
        actor[0].GetComponent<Animation>().Play("talking_on_phone_1");
    }

    IEnumerator StartTheGame()
    {
        fadingScreen.gameObject.SetActive(true);
        fadeOut = true;
        yield return new WaitForSeconds(fadingTime);
        fadeOut = false;
        SceneManager.LoadScene(gameLevel);
    }

    void FadeScreen()
    {
        if (fadeIn && fadingCol.a > 0)
        {
            fadingCol.a -= fadeStep;
        }
        if (fadeOut && fadingCol.a < 1)
        {
            fadingCol.a += fadeStep;
        }
        fadingScreen.color = fadingCol;
    }
}
