using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slime : MonoBehaviour
{
    private Rigidbody slimeRb;
    [SerializeField] float jumpHeight;
    [SerializeField] float jumpDistance;
    [SerializeField] GameObject player;
    private Vector3 jumpTarget;
    private Vector3 offset = new Vector3(0, 3, 0);
    private bool isOnGround;
    private float health;
    private float maxHealth;
    [SerializeField] Slider healthBar;
    private bool couldDamage;
    private int damageTimer;
    private int damage;


    // Start is called before the first frame update
    void Start()
    {
        slimeRb = GetComponent<Rigidbody>();

        player = GameObject.Find("Player");

        couldDamage = false;
        damageTimer = 0;
        damage = (int)(Time.timeSinceLevelLoad / 60) + 5;

        InvokeRepeating("Jump", 2f, 6f);
        InvokeRepeating("CanDamage", 0f, 1f);

        //health is 15, with 2 additional health per minute of the game
        maxHealth = 15 + ((Time.timeSinceLevelLoad / 60) * 3);
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player.transform);
        UpdateHealth();

    }

    private void Jump()
    {
        if (isOnGround)
        {
            isOnGround = false;
            jumpTarget = player.transform.position + offset;
            slimeRb.AddForce(Vector3.up * jumpHeight);
            slimeRb.AddForce((player.transform.position - transform.position) * jumpDistance);
        }
    }

    private void CanDamage()
    {
        if(damageTimer < 3)
        {
            damageTimer++;
        }
        if (damageTimer == 3)
        {
            couldDamage = true;
        }
    }

    private void UpdateHealth()
    {
        healthBar.value = health / maxHealth;
        if (health < 1)
        {
            GameObject player = GameObject.Find("Player");
            PlayerController playerScript = player.GetComponent<PlayerController>();
            playerScript.money += 8;
            GameObject gameManager = GameObject.Find("Game Manager");
            GameManager managerScript = gameManager.GetComponent<GameManager>();
            managerScript.enemiesSlain++;
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Fireball")
        {
            GameObject fireball = GameObject.Find("Fireball(Clone)");
            Fireball fireballScript = fireball.GetComponent<Fireball>();
            GameObject player = GameObject.Find("Player");
            PlayerController playerScript = player.GetComponent<PlayerController>();
            if (health == maxHealth) health -= playerScript.plusFirstHit;
            playerScript.currentHealth += (int)playerScript.plusLifesteal;
            health -= fireballScript.damage;
            GameObject gameManager = GameObject.Find("Game Manager");
            GameManager managerScript = gameManager.GetComponent<GameManager>();
            managerScript.damageDealt += (int)fireballScript.damage;
            Destroy(other.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
        }
        if(collision.gameObject.CompareTag("Plasmaball"))
        {
            GameObject plasmaball = GameObject.Find("Ability2Prefab(Clone)");
            PlasmaBall plasmaballScript = plasmaball.GetComponent<PlasmaBall>();
            GameObject player = GameObject.Find("Player");
            PlayerController playerScript = player.GetComponent<PlayerController>();
            if (health == maxHealth) health -= playerScript.plusFirstHit;
            playerScript.currentHealth += (int)playerScript.plusLifesteal;
            health -= plasmaballScript.damage;
            GameObject gameManager = GameObject.Find("Game Manager");
            GameManager managerScript = gameManager.GetComponent<GameManager>();
            managerScript.damageDealt += (int)plasmaballScript.damage;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            if (couldDamage)
            {
                GameObject player = GameObject.Find("Player");
                PlayerController playerScript = player.GetComponent<PlayerController>();

                damage -= (int)playerScript.plusReduceDamage;
                bool belowZero = false;
                if (damage <= 0)
                {
                    belowZero = true;
                    damage = 1;
                }

                playerScript.currentHealth -= damage;
                damage += (int)playerScript.plusReduceDamage;
                if (belowZero) damage--;

                couldDamage = false;
                playerScript.wasDamaged = true;
                playerScript.timeSinceDamaged = 0;
                damageTimer = 0;
            }
        }
    }
}
