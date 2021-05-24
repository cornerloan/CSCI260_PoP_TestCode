using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    public float horizontalInput;
    public float forwardInput;
    [SerializeField] Terrain map;
    private Vector3 spawnPos;

    public ParticleSystem jetpackFlames;
    public ParticleSystem ability3Explosion;
    public float jetpackFlamesTimer;

    //healthbar variables
    [SerializeField] Image healthBar;
    [SerializeField] TextMeshProUGUI healthBarNumber;
    public int maxHealth;
    public int currentHealth;
    private bool canRegen;
    public bool wasDamaged;
    public float timeSinceDamaged;
    private float regenDelay;
    
    //movement variables
    public float jumpHeight;
    public float speed;
    public float turnSpeed;
    public float hoverForce;
    public float ability3Height;
    private bool isOnGround;

    //ability variables
    [SerializeField] Image ability1Icon;
    [SerializeField] Image ability2Icon;
    [SerializeField] Image ability3Icon;
    [SerializeField] TextMeshProUGUI fireballsNumber;
    [SerializeField] TextMeshProUGUI plasmaballsNumber;
    public float ability1CD;
    public float ability2CD;
    public float ability3CD;
    private float real1CD;
    private float real2CD;
    private float real3CD;

    public GameObject fireballPrefab;
    public int fireballsStored = 0;
    public int maxFireballs = 4;
    [SerializeField] int plasmaballStored = 0;
    
    public GameObject plasmaBallPrefab;

    private GameObject lookingAt;

    [SerializeField] TextMeshProUGUI moneyNumber;
    public int money;

    [SerializeField] float extraJumps;

    //powerup variables
    public float plusDamage = 0;
    public float plusJump = 0;
    public float plusCharge = 1;
    public float plusCrit = 0;
    public float plusReduceDamage = 0;
    public float plusFirstHit = 0;
    public float plusHealthRegen = 0;
    public float plusLifesteal = 0;

    // Start is called before the first frame update
    void Start()
    {
        //set the player's location to a random position in the map
        int randomX = Random.Range(50, 460);
        int randomZ = Random.Range(50, 460);
        int nonRandomY = 40;
        spawnPos = new Vector3(randomX, nonRandomY, randomZ);
        transform.position = spawnPos;

        //allows me to edit the rigidbody of the player in this script
        playerRb = GetComponent<Rigidbody>();

        //I want to change the ability cooldowns later, so I can show the time before the player can press them again
        //the real cooldown variables are used to store the total seconds of cooldown for each ability
        real1CD = ability1CD;
        real2CD = ability2CD;
        real3CD = ability3CD;

        //set lookingAt to an empty object that represents where the player is facing at any time
        lookingAt = GameObject.Find("Player Facing");

        //set the max hp to 100, and the current hp to the maximum hp
        maxHealth = 100;
        currentHealth = maxHealth;
        timeSinceDamaged = 0;
        regenDelay = 0;

        //set the money to 0 to start out
        money = 0;

        map = Terrain.activeTerrain;
    }

    // Update is called once per frame
    void Update()
    {
        LookAtMouse();
        UpdateHealthbar();
        RegenHealth();
        UpdateMoney();
        AbilityCooldownTimer();
        UpdateFireballs();
        UpdatePlasmaballs();

        //check player movement
        Movement();
        Jump();
        BackOnMap();
        
        if (Input.GetMouseButtonDown(0) && fireballsStored > 0)
        {
            Ability1();
        }

        if(Input.GetMouseButtonDown(1) && plasmaballStored > 0)
        {
            Ability2();
        }
        
        if (Input.GetKeyDown(KeyCode.R) && ability3CD == 0)
        {
            Ability3();
        }

        
    }

    private void LookAtMouse()
    {
        Vector3 LookPosition = new Vector3(lookingAt.transform.position.x, transform.position.y, lookingAt.transform.position.z);
        transform.LookAt(LookPosition);
    }

    private void Movement()
    {
        //allows for WASD inputs, and moves the player
        horizontalInput = Input.GetAxis("Horizontal");
        forwardInput = Input.GetAxis("Vertical");

        //checks if the player is running
        if (Input.GetKey(KeyCode.LeftShift))
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed * forwardInput * 2);
            transform.Translate(Vector3.right * Time.deltaTime * speed * horizontalInput * 2);
        }
        else
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed * forwardInput);
            transform.Translate(Vector3.right * Time.deltaTime * speed * horizontalInput);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            jetpackFlames.Stop();
            extraJumps = plusJump;
        }
    }

    //holding jump is an ability in this game, so jump is not included in the movement() function. however the jump ability has no cooldown
    private void Jump()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            playerRb.drag = hoverForce;
            jetpackFlames.Play();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isOnGround)
            {
                playerRb.AddForce(Vector3.up * jumpHeight);
                isOnGround = false;
            }
            else if(transform.position.y > map.SampleHeight(transform.position) + 1)
            {
                if (extraJumps > 0) ExtraJumps();
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            playerRb.drag = 0;
            jetpackFlames.Stop();
        }
    }

    private void ExtraJumps()
    {
        playerRb.AddForce(Vector3.up * jumpHeight);
        extraJumps -= 1;
    }

    private void Ability1()
    {
        Vector3 fireballPosition = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
        Instantiate(fireballPrefab, fireballPosition, transform.rotation);
        fireballsStored--;
    }
    
    private void Ability2()
    {
        Vector3 plasmaBallPosition = new Vector3(transform.position.x, transform.position.y +1.5f, transform.position.z);
        GameObject setBallSpawn = Instantiate(plasmaBallPrefab, plasmaBallPosition, transform.rotation) as GameObject;
        setBallSpawn.transform.position = plasmaBallPosition;
        setBallSpawn.transform.rotation = transform.rotation;
        plasmaballStored--;
    }

    private void Ability3()
    {
        playerRb.AddForce(Vector3.up * jumpHeight * ability3Height);
        ability3Explosion.Play();
        jetpackFlames.Play();

        StartCoroutine(StopAbility3Effects(1.5f));

        ability3CD = real3CD;
    }

    private void AbilityCooldownTimer()
    {
        if(ability1CD > 0 && fireballsStored < maxFireballs)
        {
            ability1CD -= Time.deltaTime;
            if (ability1CD < 0) ability1CD = 0;

            if(ability1CD == 0)
            {
                ability1CD = real1CD;
                fireballsStored++;
            }
        }
        if(ability2CD > 0 && plasmaballStored < plusCharge)
        {
            ability2CD -= Time.deltaTime;
            if (ability2CD < 0) ability2CD = 0;
            float ability2IconPercent = ability2CD / real2CD;
            ability2Icon.fillAmount = 1 - ability2IconPercent;

            if (ability2CD == 0)
            {
                ability2CD = real2CD;
                plasmaballStored++;
            }

        }
        if(ability3CD > 0)
        {
            ability3CD -= Time.deltaTime;
            if (ability3CD < 0) ability3CD = 0;
            float ability3IconPercent = ability3CD / real3CD;
            ability3Icon.fillAmount = 1 - ability3IconPercent;
        }
    }
    
    private IEnumerator StopAbility3Effects(float interval)
    {
        yield return new WaitForSeconds(interval);
        ability3Explosion.Stop();
        yield return new WaitForSeconds(interval);
        jetpackFlames.Stop();
    }

    private void UpdateHealthbar()
    {
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        float hpBarPercentage = (float) currentHealth / maxHealth;
        healthBar.fillAmount = hpBarPercentage;
        healthBarNumber.text = currentHealth + " / " + maxHealth;
    }
    
    private void RegenHealth()
    {
        if (wasDamaged && timeSinceDamaged < 7)
        {
            timeSinceDamaged += Time.deltaTime;
        }

        if(timeSinceDamaged >= 7)
        {
            wasDamaged = false;
        }

        if (!wasDamaged && currentHealth < maxHealth)
        {
            regenDelay += Time.deltaTime;
            if(regenDelay >= 1)
            {
                currentHealth += (int)(1 + plusHealthRegen);
                regenDelay = 0;
            }
        }
    }
    
    private void UpdateMoney()
    {
        moneyNumber.text = "$" + money;
    }

    private void UpdateFireballs()
    {
        fireballsNumber.text = fireballsStored.ToString();
    }

    private void UpdatePlasmaballs()
    {
        if(plasmaballsNumber.gameObject.activeSelf) plasmaballsNumber.text = plasmaballStored.ToString();
    }

    private void BackOnMap()
    {
        if(transform.position.y < -10)
        {
            transform.position = spawnPos;
        }
    }
}