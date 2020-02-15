using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndingScene : MonoBehaviour
{

    [SerializeField] private int levelToLoad = 1;
    [SerializeField] private GameObject[] actor = new GameObject[6];
    [SerializeField] private GameObject[] eyes = new GameObject[3];
    private int activeEyes = 0;
    [SerializeField] private GameObject dialogUI = null;
    [SerializeField] private Text txtDialogue = null;
    private string[] StoryLine = new string[6];

    [SerializeField] private Transform[] cameraPos = new Transform[5];
    [SerializeField] private float distanceToRun = 25.0f;
    [SerializeField] private GameObject timeMachine = null;
    [SerializeField] private GameObject[] actorOnTimeMachine = new GameObject[2];

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
        actorOnTimeMachine[0].SetActive(false);
        actorOnTimeMachine[1].SetActive(false);
        timeMachine.GetComponent<Animation>().Play("stand");

        // actor 0 : cat, 1 : keanmew , 2 - 5 : enemy

        StoryLine[0] = "Well done, Agent 135. You managed to secure the meteorite.";
        StoryLine[1] = "I followed you to ensure your success. Now, let's go home and save the world.";
        
        Sfx = GetComponent<AudioSource>();
        fadingScreen.gameObject.SetActive(true);
        fadingCol = fadingScreen.color;
        StartCoroutine("FadeScreenIn");
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
        while (!isTalking)
        {
            SetActiveEye(2);
            yield return new WaitForSeconds(0.1f);
            SetActiveEye(activeEyes);
            yield return new WaitForSeconds(2.5f);
        }
    }

    void SetActiveEye(int eye)
    {
        for (int i = 0; i < eyes.Length; i++)
        {
            if (i == eye)
                eyes[i].SetActive(true);
            else
                eyes[i].SetActive(false);
        }
    }

    IEnumerator FadeScreenIn()
    {
        fadeIn = true;
        yield return new WaitForSeconds(fadingTime);
        fadeIn = false;
        fadingScreen.gameObject.SetActive(false);
    }

    IEnumerator PlayCutScene()
    {
        Camera.main.transform.position = cameraPos[0].position;
        Camera.main.transform.rotation = cameraPos[0].rotation;

        // everybody run :
        actor[0].GetComponent<Animation>().Play("run_with_gun");
        actor[2].GetComponent<Animation>().Play("run_shoot");
        actor[3].GetComponent<Animation>().Play("run_shoot");
        actor[4].GetComponent<Animation>().Play("run");
        actor[5].GetComponent<Animation>().Play("run");

        Vector3 start = actor[0].transform.position;
        Vector3 end = new Vector3(actor[0].transform.position.x, actor[0].transform.position.y, actor[0].transform.position.z - distanceToRun);
        Vector3 start1 = actor[2].transform.position;
        Vector3 end1 = new Vector3(actor[2].transform.position.x, actor[2].transform.position.y, actor[2].transform.position.z - distanceToRun);
        Vector3 start2 = actor[3].transform.position;
        Vector3 end2 = new Vector3(actor[3].transform.position.x, actor[3].transform.position.y, actor[3].transform.position.z - distanceToRun);
        Vector3 start3 = actor[4].transform.position;
        Vector3 end3 = new Vector3(actor[4].transform.position.x, actor[4].transform.position.y, actor[4].transform.position.z - distanceToRun);
        Vector3 start4 = actor[5].transform.position;
        Vector3 end4 = new Vector3(actor[5].transform.position.x, actor[5].transform.position.y, actor[5].transform.position.z - distanceToRun);
        Vector3 startCam = Camera.main.transform.position;
        Vector3 endCam = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z - distanceToRun);
        float gap = Vector3.Distance(start, end);
        for (float t = 0; t <= gap; t += 0.5f)
        {
            actor[0].transform.position = Vector3.MoveTowards(start, end, t);
            actor[2].transform.position = Vector3.MoveTowards(start1, end1, t);
            actor[3].transform.position = Vector3.MoveTowards(start2, end2, t);
            actor[4].transform.position = Vector3.MoveTowards(start3, end3, t);
            actor[5].transform.position = Vector3.MoveTowards(start4, end4, t);
            Camera.main.transform.position = Vector3.MoveTowards(startCam, endCam, t);

            yield return new WaitForSeconds(0f);
        }
        actor[2].GetComponent<Animation>().Play("shoot");
        actor[3].GetComponent<Animation>().Play("shoot");
        actor[4].GetComponent<Animation>().Play("throw");
        actor[5].GetComponent<Animation>().Play("attack");

        activeEyes = 1;
        SetActiveEye(activeEyes);
        actor[0].transform.rotation = Quaternion.Euler(0, 0, 0);
        actor[0].GetComponent<Animation>().Play("shoot");

        startCam = Camera.main.transform.position;
        endCam = cameraPos[1].transform.position;
        gap = Vector3.Distance(startCam, endCam);
        for (float t = 0; t <= gap; t += 0.75f)
        {
            Camera.main.transform.position = Vector3.MoveTowards(startCam, endCam, t);
            Camera.main.transform.rotation = Quaternion.RotateTowards(Camera.main.transform.rotation, cameraPos[1].rotation, t);
            yield return new WaitForSeconds(0f);
        }
        
        yield return new WaitForSeconds(2f);

        startCam = Camera.main.transform.position;
        endCam = cameraPos[2].transform.position;
        gap = Vector3.Distance(startCam, endCam);
        for (float t = 0; t <= gap; t += 1f)
        {
            Camera.main.transform.position = Vector3.MoveTowards(startCam, endCam, t);
            Camera.main.transform.rotation = Quaternion.RotateTowards(Camera.main.transform.rotation, cameraPos[2].rotation, t);
            yield return new WaitForSeconds(0f);
        }
        

        actor[0].transform.rotation = Quaternion.Euler(0, 180, 0);
        actor[0].GetComponent<Animation>().Play("run_with_gun");

        // run towards agaim
        start = actor[0].transform.position;
        end = new Vector3(actor[0].transform.position.x, actor[0].transform.position.y, actor[0].transform.position.z - 80);
        gap = Vector3.Distance(start, end);
        startCam = Camera.main.transform.position;
        endCam = cameraPos[3].transform.position;

        for (float t = 0; t <= gap; t += 0.5f)
        {
            actor[0].transform.position = Vector3.MoveTowards(start, end, t);
            Camera.main.transform.position = Vector3.MoveTowards(startCam, endCam, t);
            yield return new WaitForSeconds(0f);
        }

        actor[0].transform.rotation = Quaternion.Euler(0, 0, 0);
        actor[0].GetComponent<Animation>().Play("shoot");
        actor[1].GetComponent<Animation>().Play("shoot_RPG");
        yield return new WaitForSeconds(2f);

        actor[1].transform.LookAt(new Vector3(actor[0].transform.position.x, actor[1].transform.position.y, actor[0].transform.position.z));
        actor[0].GetComponent<Animation>().Play("idle");
        actor[1].GetComponent<Animation>().Play("idle");


        dialogUI.SetActive(true);
        isTalking = true;
        for (int i = 0; i <= 1; i++)
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

            activeEyes = 0;
            SetActiveEye(activeEyes);

            if (i == 0 ) actor[0].transform.LookAt(new Vector3(actor[1].transform.position.x, actor[0].transform.position.y, actor[1].transform.position.z));
            yield return StartCoroutine(WaitForPlayerConfirm());
            yield return new WaitForSeconds(0.2f);
        }

        dialogUI.SetActive(false);
        isTalking = false;
        Sfx.Stop();

        Camera.main.transform.position = cameraPos[4].position;
        Camera.main.transform.rotation = cameraPos[4].rotation;

        // go to time machine
        for (int i =0; i < actor.Length; i ++)
        {
            actor[i].SetActive(false);
        }

        actorOnTimeMachine[0].SetActive(true);
        actorOnTimeMachine[1].SetActive(true);
        timeMachine.GetComponent<Animation>().Play("float");
        yield return new WaitForSeconds(4f);
        timeMachine.GetComponent<Animation>().Play("vanish");
        yield return new WaitForSeconds(1f);

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
        Sfx.PlayOneShot(sfxClip[3]);
        //yield return new WaitForSeconds(1.5f);
        Sfx.Stop();
    }

    IEnumerator StartTheGame()
    {
        fadingScreen.gameObject.SetActive(true);
        fadeOut = true;
        yield return new WaitForSeconds(fadingTime);
        fadeOut = false;
        SceneManager.LoadScene(levelToLoad);
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
