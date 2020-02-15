using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum LEVEL { LEVEL1, LEVEL2, LEVEL3 }
public class GameManager : MonoBehaviour
{
    LEVEL _level;
    public GameObject Meteorite;
    public GameObject EnvironmentPrefab;
    private GameObject activeEnv;
    public RectTransform loadingScreen;
    public GameObject BossTriggerPrefab;
    private Vector3 BossTilePosition;
    public RectTransform HealthBar;
    public RectTransform BulletBar;
    public RectTransform GattlingText;
    public RectTransform RocketText;
    public Text GrenadeText;
    public RectTransform statsContainer;
    public RectTransform pauseMenu;
    public RectTransform deathMenu;
    public RectTransform m1;
    public RectTransform m2;
    public RectTransform m3;
    public GameObject boss;
    public float healthPercent;
    public float bulletPercent;
    public int grenadeAmount;
    public bool Paused;
    private bool plrDead;
    private GameObject currentBoss;
    FMOD.Studio.EventInstance music;
    // Start is called before the first frame updates
    void Start()
    {
        currentBoss = null;
        music = FMODUnity.RuntimeManager.CreateInstance("event:/Jungle Music");
        Paused = false;
        grenadeAmount = 5;
        _level = LEVEL.LEVEL1;

        activeEnv = Instantiate(EnvironmentPrefab);
        loadingScreen.GetComponent<LoadingBehaviour>().StartLoading(_level);
        BossTilePosition = activeEnv.GetComponent<LevelGenerator>().BuildMap(ENVIRONMENT.JUNGLE);
        music.start();
        SpawnBoss();
        loadingScreen.GetComponent<LoadingBehaviour>().StopLoading();
    }
    private void Update()
    {
        if (currentBoss != null && currentBoss.GetComponent<EnemyStats>().health <= 0)
            BossDefeated();
        if (!plrDead)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                Paused = !Paused;

            if (Paused)
            {
                statsContainer.gameObject.SetActive(false);
                pauseMenu.gameObject.SetActive(true);
                Time.timeScale = 0;
            }
            else
            {
                Resume();
            }
        }
        else
        {
            music.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }
    public void Resume()
    {
        statsContainer.gameObject.SetActive(true);
        pauseMenu.gameObject.SetActive(false);
        deathMenu.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void RestartLevel()
    {
        FMOD.Studio.PLAYBACK_STATE state;
        music.getPlaybackState(out state);
        if (state == FMOD.Studio.PLAYBACK_STATE.PLAYING)
            music.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        music.start();
        if (plrDead == true)
            plrDead = false;
        Paused = false;
        Resume();
        if (activeEnv != null)
            Destroy(activeEnv);
        activeEnv = Instantiate(EnvironmentPrefab);
        loadingScreen.GetComponent<LoadingBehaviour>().StartLoading(_level);
        if (_level == LEVEL.LEVEL1)
        {
            music.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            music = FMODUnity.RuntimeManager.CreateInstance("event:/Jungle Music");
            music.start();
            BossTilePosition = activeEnv.GetComponent<LevelGenerator>().BuildMap(ENVIRONMENT.JUNGLE);
            BossDefeated();
        }
        else if (_level == LEVEL.LEVEL2)
        {
            music.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            music = FMODUnity.RuntimeManager.CreateInstance("event:/Desert Music");
            music.start();
            BossTilePosition = activeEnv.GetComponent<LevelGenerator>().BuildMap(ENVIRONMENT.DESERT);
            BossDefeated();
        }
        else if (_level == LEVEL.LEVEL3)
        {
            music.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            music = FMODUnity.RuntimeManager.CreateInstance("event:/Cave Music");
            music.start();
            BossTilePosition = activeEnv.GetComponent<LevelGenerator>().BuildMap(ENVIRONMENT.CAVE);//change after
            BossDefeated();
        }
        Instantiate(BossTriggerPrefab, BossTilePosition, Quaternion.identity);
        loadingScreen.GetComponent<LoadingBehaviour>().StopLoading();
    }

    public void SpawnBoss()
    {
        currentBoss = Instantiate(boss, BossTilePosition + new Vector3(0, 1, 0), Quaternion.identity, activeEnv.transform);
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void PlayerDied()
    {
        Paused = true;
        plrDead = true;
        deathMenu.gameObject.SetActive(true);
    }
    public void UpdateUI(float hPct, float bPct, WEAPON wpn)
    {
        float HealthBarPercentage = Mathf.Lerp(0, 161.86f, hPct);
        HealthBar.sizeDelta = new Vector2(HealthBarPercentage, 17.885f);
        bPct = 1f - bPct;
        float BulletBarPercentage = Mathf.Lerp(0, 120.39f, bPct);
        BulletBar.sizeDelta = new Vector2(BulletBarPercentage, 13.73f);
        if (wpn == WEAPON.GATTLING)
        {
            GattlingText.GetComponent<Image>().enabled = true;
            RocketText.GetComponent<Image>().enabled = false;
        }
        else if (wpn == WEAPON.RPG)
        {
            GattlingText.GetComponent<Image>().enabled = false;
            RocketText.GetComponent<Image>().enabled = true;
        }
        GrenadeText.text = grenadeAmount.ToString();
    }
    public void AddGrenade()
    {
        grenadeAmount++;
    }
    public bool CanThrowGrenade()
    {
        if (grenadeAmount <= 0)
            return false;
        return true;
    }
    public void ThrowGrenade()
    {
        grenadeAmount--;
    }
    void SpawnLevel(ENVIRONMENT env)
    {
        if (activeEnv != null)
            Destroy(activeEnv);
        activeEnv = Instantiate(EnvironmentPrefab);
        BossTilePosition = activeEnv.GetComponent<LevelGenerator>().BuildMap(env);
        if (env != ENVIRONMENT.CAVE)
            Instantiate(BossTriggerPrefab, BossTilePosition, Quaternion.identity);
    }


    public void TriggerBossFight()
    {
        //Todo
    }

    public void BossDefeated()
    {
        Instantiate(Meteorite, currentBoss.transform.position, Quaternion.identity, activeEnv.transform);
    }

    public void LevelWon()
    {
        FMOD.Studio.PLAYBACK_STATE state;
        music.getPlaybackState(out state);
        if (state == FMOD.Studio.PLAYBACK_STATE.PLAYING)
            music.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        SceneManager.LoadScene(4);
    }
}
