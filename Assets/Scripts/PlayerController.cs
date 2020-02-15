using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public enum WEAPON { NONE, GATTLING, GRENADE, RPG, SPEAR, BONE, BOSSPUNCH }
public class PlayerController : MonoBehaviour
{
    public NavMeshAgent agent;
    public LayerMask playerLayer;
    public LayerMask Ground;
    private const float MOVE_SPEED = 2f;
    private const float BULLET_SPEED = 50f;
    private const float GRENADE_SPEED = 15f;
    private const float RPG_SPEED = 35f;
    public Image HealthBar;
    public Image BulletBar;
    public Image GattlingText;
    public Image RocketText;
    public Text GrenadeAmount;
    public GameObject GattlingGun;
    public GameObject BulletPrefab;
    public GameObject FireLocation;
    public GameObject RPG;
    public GameObject RPGPrefab;
    public GameObject RPGFireLocation1;
    public GameObject RPGFireLocation2;
    public GameObject RPGFireLocation3;
    public GameObject RPGFireLocation4;
    public GameObject GrenadePrefab;
    public GameObject GrenadeLocation;
    public GameObject ActiveGoggles;
    public GameObject InactiveGoggles;
    public GameObject EyeLids;
    private bool GunCooldown;
    private bool shooting;
    private bool idle;
    private bool running;
    private bool hasGun;
    private bool Throwing;
    private bool hasRPG;
    private WEAPON _activeWeapon;
    private Queue<WEAPON> _inventory;
    private Animator _anim;
    private int _rpgBullets;
    private float gunTemperature;
    private float RPGCooldown;
    private int grenades;
    FMOD.Studio.EventInstance bulletSound;
    FMOD.Studio.EventInstance runningSound;

    void Start()
    {
        
        
        runningSound = FMODUnity.RuntimeManager.CreateInstance("event:/Running");
        bulletSound = FMODUnity.RuntimeManager.CreateInstance("event:/Gatling Gun");

        HealthBar = GameObject.Find("HealthBar").GetComponent<Image>();
        BulletBar = GameObject.Find("BulletBar").GetComponent<Image>();
        GattlingText = GameObject.Find("GattlingText").GetComponent<Image>();
        RocketText = GameObject.Find("RocketText").GetComponent<Image>();
        GrenadeAmount = GameObject.Find("GattlingText").GetComponent<Text>();
        RPGCooldown = 0;
        GunCooldown = false;
        gunTemperature = 0;
        grenades = 5;
        _inventory = new Queue<WEAPON>();
        _inventory.Enqueue(WEAPON.GATTLING);
        _inventory.Enqueue(WEAPON.RPG);
        _activeWeapon = _inventory.Dequeue();
        _anim = gameObject.GetComponent<Animator>();
        hasGun = true;
    }

    /// <summary>
    /// Swaps the active weapon with the weapon at the top of the Inventory Queue, and puts the previously active weapon at the bottom of the queue.
    /// </summary>
    void CycleWeapons()
    {
        _inventory.Enqueue(_activeWeapon);
        _activeWeapon = _inventory.Dequeue();
    }
    IEnumerator StartCooldown()
    {
        GunCooldown = true;
        shooting = false;
        yield return new WaitForSeconds(3f);
        GunCooldown = false;
    }
    IEnumerator Shoot()
    {
        FMOD.Studio.PLAYBACK_STATE state;
        if(_activeWeapon == WEAPON.GATTLING && Input.GetMouseButton(0))
        {
            bulletSound.getPlaybackState(out state);
            if (state != FMOD.Studio.PLAYBACK_STATE.PLAYING)
                bulletSound.start();
        }
        else
        {
            bulletSound.getPlaybackState(out state);
            if (state == FMOD.Studio.PLAYBACK_STATE.PLAYING)
                bulletSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            shooting = false;

        }
        yield return new WaitForSeconds(0.2f);

        if (_activeWeapon == WEAPON.GATTLING && Input.GetMouseButton(0))
        {
            
            shooting = true;
            gunTemperature += 0.005f;
            if (gunTemperature > 1f)
                StartCoroutine(StartCooldown());
            GameObject bullet = GameObject.Instantiate(BulletPrefab);
            bullet.transform.position = FireLocation.transform.position;
            bullet.GetComponent<Rigidbody>().velocity = transform.forward * BULLET_SPEED;
        }else if(_activeWeapon == WEAPON.RPG && Input.GetMouseButton(0) && RPGCooldown <= 0) 
        {
            Quaternion rot = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, 1);
            RPGCooldown = 5f;
            GameObject[] projectiles = new GameObject[4];
            projectiles[0] = Instantiate(RPGPrefab, RPGFireLocation1.transform.position, rot);
            projectiles[1] = Instantiate(RPGPrefab, RPGFireLocation2.transform.position, rot);
            projectiles[2] = Instantiate(RPGPrefab, RPGFireLocation3.transform.position, rot);
            projectiles[3] = Instantiate(RPGPrefab, RPGFireLocation4.transform.position, rot);
            foreach (GameObject p in projectiles)
            {
                p.GetComponent<Rigidbody>().velocity = transform.forward * RPG_SPEED;
            }
        }
       
    }

    void DissipateHeat()
    {
        gunTemperature -= 0.002f;
        if (gunTemperature < 0)
            gunTemperature = 0;
    }
    /// <summary>
    /// Function to determine how to move the player for a given frame.
    /// </summary>
    void MovePlayer()
    {
        idle = true;
        running = false;
        float x = 0;
        float z = 0;
        if (!Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
        {
            running = true;
            idle = false;
            x = 1 * MOVE_SPEED;
        }
        else if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            running = true;
            idle = false;
            x = -1 * MOVE_SPEED;
        }

        if (!Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S))
        {
            running = true;
            idle = false;
            z = -1 * MOVE_SPEED;
        }
        else if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
        {
            running = true;
            idle = false;
            z = 1 * MOVE_SPEED;
        }
        gameObject.GetComponent<Rigidbody>().velocity = new Vector3(x, 0, z);
        if (idle)
        {
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            runningSound.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }
        else
        {
            FMOD.Studio.PLAYBACK_STATE state;
            bulletSound.getPlaybackState(out state);
            if (state == FMOD.Studio.PLAYBACK_STATE.PLAYING)
                runningSound.start();
        }
            
    }

    IEnumerator ThrowGrenade()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().ThrowGrenade();
        Throwing = true;
        yield return new WaitForSeconds(0.6f);
        GameObject grenade = GameObject.Instantiate(GrenadePrefab);
        grenade.transform.position = GrenadeLocation.transform.position;
        grenade.transform.position += new Vector3(0,0.4f,0);
        grenade.GetComponent<Rigidbody>().velocity = transform.forward * GRENADE_SPEED;
        yield return new WaitForSeconds(0.4f);
        Throwing = false;
    }
    /// <summary>
    /// Function to rotate the player towards the mouse position.
    /// </summary>
    void RotateToMouse()
    {

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 5.23f;

        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;

        float angle = -(Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg);
        transform.rotation = Quaternion.Euler(new Vector3(0, angle + 90, 0));
    }

    void PlayerInput()
    {
        MovePlayer();
        if (gunTemperature < 1f && !GunCooldown && !Throwing)
            StartCoroutine(Shoot());
        else
            shooting = false;
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetMouseButtonDown(1)) 
            CycleWeapons();
        if (Input.GetKeyDown(KeyCode.Space) && GameObject.Find("GameManager").GetComponent<GameManager>().CanThrowGrenade())
            StartCoroutine(ThrowGrenade());
        if (Input.GetKeyDown(KeyCode.F))
            StartCoroutine(Shoot());
    }

    void UpdateUI()
    {
        float hPct = GetComponent<PlayerStats>().Health / 100;
        float bPct = 0;
        if(_activeWeapon == WEAPON.GATTLING)
        {
            bPct = gunTemperature;
        } else if(_activeWeapon == WEAPON.RPG)
        {
            bPct = RPGCooldown / 5f;
        }
        GameObject.Find("GameManager").GetComponent<GameManager>().UpdateUI(hPct, bPct, _activeWeapon);
    }
    void GenerateAnimationState()
    {
        _anim.SetBool("Idle", idle);
        _anim.SetBool("Running", running);
        _anim.SetBool("Throwing", Throwing);

        _anim.SetBool("Shooting", shooting);
        if (shooting)
        {
            InactiveGoggles.GetComponent<SkinnedMeshRenderer>().enabled = false;
            ActiveGoggles.GetComponent<SkinnedMeshRenderer>().enabled = true;
        }
        else
        {
            InactiveGoggles.GetComponent<SkinnedMeshRenderer>().enabled = true;
            ActiveGoggles.GetComponent<SkinnedMeshRenderer>().enabled = false;
        }
        if (_activeWeapon == WEAPON.GATTLING)
        {
            _anim.SetBool("HasGun", true);
            _anim.SetBool("Has RPG", false);
            GattlingGun.gameObject.GetComponent<SkinnedMeshRenderer>().enabled = true;
            RPG.gameObject.GetComponent<MeshRenderer>().enabled = false;
            //RocketText.GetComponent<Image>().enabled = true;
            //GattlingText.GetComponent<Image>().enabled = false;
        }
        else if (_activeWeapon == WEAPON.RPG)
        {
            _anim.SetBool("Has RPG", true);
            _anim.SetBool("HasGun", false);
            GattlingGun.gameObject.GetComponent<SkinnedMeshRenderer>().enabled = false;
            RPG.gameObject.GetComponent<MeshRenderer>().enabled = true;
            //RocketText.GetComponent<Image>().enabled = true;
            //GattlingText.GetComponent<Image>().enabled = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (_anim.GetBool("IsDead"))
        {
            bulletSound.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            GattlingGun.gameObject.GetComponent<SkinnedMeshRenderer>().enabled = false;
            RPG.gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionX |RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
        }
        else
        {
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            if(!GameObject.Find("GameManager").GetComponent<GameManager>().Paused)
                RotateToMouse();
            PlayerInput();
            DissipateHeat();
            UpdateUI();
            GenerateAnimationState();
            if(RPGCooldown > 0)
            {
                RPGCooldown -= Time.deltaTime;
            }
        }
        agent.velocity = GetComponent<Rigidbody>().velocity;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bone"))
        {
            FMOD.Studio.EventInstance cavemanAttackSound;
            cavemanAttackSound = FMODUnity.RuntimeManager.CreateInstance("event:/Melee and Spear hits main char");
            cavemanAttackSound.start();

            gameObject.GetComponent<PlayerStats>().TakeDamage(WEAPON.BONE);
        }
            

        if (other.gameObject.CompareTag("Spear"))
        {
            FMOD.Studio.EventInstance cavemanAttackSound;
            cavemanAttackSound = FMODUnity.RuntimeManager.CreateInstance("event:/Melee and Spear hits main char");
            cavemanAttackSound.start();
            gameObject.GetComponent<PlayerStats>().TakeDamage(WEAPON.SPEAR);
        }
            

        if(other.gameObject.CompareTag("GrenadePowerup"))
        {
            FMOD.Studio.EventInstance powerupSound;
            powerupSound = FMODUnity.RuntimeManager.CreateInstance("event:/Powerup");
            powerupSound.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject.transform, GetComponent<Rigidbody>()));
            powerupSound.start();
            GameObject.Find("GameManager").GetComponent<GameManager>().AddGrenade();
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("HealthPowerup"))
        {
            FMOD.Studio.EventInstance powerupSound;
            powerupSound = FMODUnity.RuntimeManager.CreateInstance("event:/Powerup");
            powerupSound.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject.transform, GetComponent<Rigidbody>()));
            powerupSound.start();
            GetComponent<PlayerStats>().HealthPack();
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("Boss Punch"))
        {
            FMOD.Studio.EventInstance cavemanAttackSound;
            cavemanAttackSound = FMODUnity.RuntimeManager.CreateInstance("event:/Melee and Spear hits main char");
            cavemanAttackSound.start();

            gameObject.GetComponent<PlayerStats>().TakeDamage(WEAPON.BOSSPUNCH);
        }
    }
}
