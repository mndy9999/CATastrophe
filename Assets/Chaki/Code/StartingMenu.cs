using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartingMenu : MonoBehaviour
{
    [SerializeField] private int cutSceneLevel = 2;

    // fading screen variables :
    [SerializeField] private Image fadingScreen = null;
    private bool fadeIn = false, fadeOut = false;
    private Color fadingCol = Color.black;
    [SerializeField] private float fadingTime = 1f, fadeStep = 0.04f;
    [SerializeField] private float moveSpeed = 0.25f;

    // sound related :
    private AudioSource Sfx = null;
    [SerializeField] private AudioClip sfxClip = null;

    // for credit page :
    [SerializeField] private GameObject backgroundImage = null;
    [SerializeField] private GameObject menuButtonUI = null;
    [SerializeField] private GameObject[] actor = new GameObject[8];

    [SerializeField] private GameObject[] eyes = new GameObject[3];
    [SerializeField] private GameObject[] catWeapon = new GameObject[2];
    [SerializeField] private GameObject[] keanmweWeapon = new GameObject[2];
    [SerializeField] private GameObject meteorRock = null;

    [SerializeField] private float distanceToRun = 5.0f;
    private bool run = true;

    void Awake()
    {
        fadingScreen.gameObject.SetActive(true);
        Sfx = GetComponent<AudioSource>();
        fadingCol = fadingScreen.color;
        menuButtonUI.SetActive(true);
        backgroundImage.SetActive(true);
    }
    void Start()
    {
        StartCoroutine("CharParade");
        StartCoroutine("ScreenFadeIn");
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
    void SetActiveWeaponCat(int weapon)
    {
        for (int i = 0; i < catWeapon.Length; i++)
        {
            if (i == weapon)
                catWeapon[i].SetActive(true);
            else
                catWeapon[i].SetActive(false);
        }
    }


    void SetActiveWeaponKeane(int weapon)
    {
        for (int i = 0; i < keanmweWeapon.Length; i++)
        {
            if (i == weapon)
                keanmweWeapon[i].SetActive(true);
            else
                keanmweWeapon[i].SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            SceneManager.LoadScene(3);
        }
    }


    IEnumerator CharParade()
    {
        while(run)
        {
            // 0 cat, 1 keanmew, 2-3 alien shooter, 4-5 cavemen, 6 alienboss, 7 meteoron
            // everybody run :

            // cat -> 2 aliens
            SetDirectionChar(0);
            SetActiveEye(1);
            SetActiveWeaponCat(0);
            actor[0].GetComponent<Animation>().Play("run_shoot");
            actor[2].GetComponent<Animation>().Play("run_shoot");
            actor[3].GetComponent<Animation>().Play("run_shoot");

            Vector3 start = actor[0].transform.position;
            Vector3 end = new Vector3(actor[0].transform.position.x + distanceToRun, actor[0].transform.position.y, actor[0].transform.position.z);
            Vector3 start1 = actor[2].transform.position;
            Vector3 end1 = new Vector3(actor[2].transform.position.x + distanceToRun, actor[2].transform.position.y, actor[2].transform.position.z);
            Vector3 start2 = actor[3].transform.position;
            Vector3 end2 = new Vector3(actor[3].transform.position.x + distanceToRun, actor[3].transform.position.y, actor[3].transform.position.z);
            float gap = Vector3.Distance(start, end);
            for (float t = 0; t <= gap; t += moveSpeed)
            {
                actor[0].transform.position = Vector3.MoveTowards(start, end, t);
                actor[2].transform.position = Vector3.MoveTowards(start1, end1, t);
                actor[3].transform.position = Vector3.MoveTowards(start2, end2, t);

                yield return new WaitForSeconds(0f);
            }

            yield return new WaitForSeconds(2f);

            // cat <- alien boss + 2 aliens
            SetDirectionChar(1);
            SetActiveEye(0);
            actor[0].GetComponent<Animation>().Play("run_with_gun");
            actor[2].GetComponent<Animation>().Play("run_shoot");
            actor[3].GetComponent<Animation>().Play("run_shoot");
            actor[6].GetComponent<Animation>().Play("go_forward");
            Vector3 start3 = actor[6].transform.position;
            Vector3 end3 = new Vector3(actor[6].transform.position.x - distanceToRun, actor[6].transform.position.y, actor[6].transform.position.z);

            for (float t = 0; t <= gap; t += moveSpeed)
            {
                actor[0].transform.position = Vector3.MoveTowards(end, start, t);
                actor[2].transform.position = Vector3.MoveTowards(end1, start1, t);
                actor[3].transform.position = Vector3.MoveTowards(end2, start2, t);
                actor[6].transform.position = Vector3.MoveTowards(start3, end3, t);

                yield return new WaitForSeconds(0f);
            }

            yield return new WaitForSeconds(2f);

            // cat + keanmew -> 2 aliens + alien boss
            SetActiveEye(1);
            SetActiveWeaponKeane(0);
            SetDirectionChar(0);
            actor[0].GetComponent<Animation>().Play("run_shoot");
            actor[1].GetComponent<Animation>().Play("run_shoot");
            actor[2].GetComponent<Animation>().Play("run_shoot");
            actor[3].GetComponent<Animation>().Play("run_shoot");
            actor[6].GetComponent<Animation>().Play("float_idle");

            Vector3 start4 = actor[1].transform.position;
            Vector3 end4 = new Vector3(actor[1].transform.position.x + distanceToRun, actor[1].transform.position.y, actor[1].transform.position.z);

            for (float t = 0; t <= gap; t += moveSpeed)
            {
                actor[0].transform.position = Vector3.MoveTowards(start, end, t);
                actor[1].transform.position = Vector3.MoveTowards(start4, end4, t);
                actor[2].transform.position = Vector3.MoveTowards(start1, end1, t);
                actor[3].transform.position = Vector3.MoveTowards(start2, end2, t);
                actor[6].transform.position = Vector3.MoveTowards(end3, start3, t);

                yield return new WaitForSeconds(0f);
            }

            yield return new WaitForSeconds(2f);

            // cat + keanue <- meteoron boss + 2 aliens + 2 cavemen
            SetDirectionChar(1);
            SetActiveEye(0);
            meteorRock.SetActive(true);
            actor[0].GetComponent<Animation>().Play("run_with_gun");
            actor[1].GetComponent<Animation>().Play("run_with_gun");
            actor[2].GetComponent<Animation>().Play("run_shoot");
            actor[3].GetComponent<Animation>().Play("run_shoot");
            actor[6].GetComponent<Animation>().Play("go_forward");
            actor[4].GetComponent<Animation>().Play("run");
            actor[5].GetComponent<Animation>().Play("run");
            actor[7].GetComponent<Animation>().Play("run_with_meteor");
            Vector3 start5 = actor[4].transform.position;
            Vector3 end5 = new Vector3(actor[4].transform.position.x - distanceToRun, actor[4].transform.position.y, actor[4].transform.position.z);
            Vector3 start6 = actor[5].transform.position;
            Vector3 end6 = new Vector3(actor[5].transform.position.x - distanceToRun, actor[5].transform.position.y, actor[5].transform.position.z);
            Vector3 start7 = actor[7].transform.position;
            Vector3 end7 = new Vector3(actor[7].transform.position.x - distanceToRun, actor[7].transform.position.y, actor[7].transform.position.z);

            for (float t = 0; t <= gap; t += moveSpeed)
            {
                actor[0].transform.position = Vector3.MoveTowards(end, start, t);
                actor[1].transform.position = Vector3.MoveTowards(end4, start4, t);
                actor[2].transform.position = Vector3.MoveTowards(end1, start1, t);
                actor[3].transform.position = Vector3.MoveTowards(end2, start2, t);
                actor[4].transform.position = Vector3.MoveTowards(start5, end5, t);
                actor[5].transform.position = Vector3.MoveTowards(start6, end6, t);
                actor[7].transform.position = Vector3.MoveTowards(start7, end7, t);

                yield return new WaitForSeconds(0f);
            }

            yield return new WaitForSeconds(2f);

            // cat -> meteoron boss + 2 aliens + 2 cavemen
            SetDirectionChar(0);
            SetActiveEye(1);
            SetActiveWeaponCat(1);
            meteorRock.SetActive(false);
            actor[0].GetComponent<Animation>().Play("run_shoot_RPG");
            actor[2].GetComponent<Animation>().Play("run_shoot");
            actor[3].GetComponent<Animation>().Play("run_shoot");
            actor[4].GetComponent<Animation>().Play("run");
            actor[5].GetComponent<Animation>().Play("run");
            actor[7].GetComponent<Animation>().Play("run");
            
            for (float t = 0; t <= gap; t += moveSpeed)
            {
                actor[0].transform.position = Vector3.MoveTowards(start, end, t);
                actor[2].transform.position = Vector3.MoveTowards(start1, end1, t);
                actor[3].transform.position = Vector3.MoveTowards(start2, end2, t);
                actor[4].transform.position = Vector3.MoveTowards(end5, start5, t);
                actor[5].transform.position = Vector3.MoveTowards(end6, start6, t);
                actor[7].transform.position = Vector3.MoveTowards(end7, start7, t);

                yield return new WaitForSeconds(0f);
            }

            yield return new WaitForSeconds(2f);

            // cat <- 2 aliens
            SetDirectionChar(1);
            SetActiveEye(0);
            SetActiveWeaponCat(2);
            actor[0].GetComponent<Animation>().Play("run");
            actor[2].GetComponent<Animation>().Play("run_shoot");
            actor[3].GetComponent<Animation>().Play("run_shoot");

            for (float t = 0; t <= gap; t += moveSpeed)
            {
                actor[0].transform.position = Vector3.MoveTowards(end,start, t);
                actor[2].transform.position = Vector3.MoveTowards(end1,start1, t);
                actor[3].transform.position = Vector3.MoveTowards(end2,start2, t);

                yield return new WaitForSeconds(0f);
            }

            yield return new WaitForSeconds(2f);
        }
    }

    void SetDirectionChar(int dir)
    {
        if (dir == 0)
        {
            for (int i = 0; i < actor.Length; i ++)
            {
                actor[i].transform.rotation = Quaternion.Euler(0, 90, 0);
            }
        }
        else
        {
            for (int i = 0; i < actor.Length; i++)
            {
                actor[i].transform.rotation = Quaternion.Euler(0, -90, 0);
            }
        }
    }

    void FixedUpdate()
    {
        FadeScreen();
    }

    public void buttonPress(int button)
    {
        Sfx.PlayOneShot(sfxClip);
       if(button == 0)
       {
            run = false;
            StartCoroutine("GoToStartingScene");
       }
       else if(button == 1)
       {
            backgroundImage.SetActive(false);
            menuButtonUI.SetActive(false);
       }
       else if(button == 2)
       {
            run = false;
            Application.Quit();
       }
       else if(button == 3)
       {
            backgroundImage.SetActive(true);
            menuButtonUI.SetActive(true);
       }
    }

    IEnumerator GoToStartingScene()
    {
        fadingScreen.gameObject.SetActive(true);
        fadeOut = true;
        yield return new WaitForSeconds(fadingTime);
        fadeOut = false;
        SceneManager.LoadScene(cutSceneLevel);
    }

    IEnumerator ScreenFadeIn()
    {
        fadeIn = true;
        yield return new WaitForSeconds(fadingTime);
        fadeIn = false;
        fadingScreen.gameObject.SetActive(false);
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
