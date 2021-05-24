using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bison : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] float speed;
    private float health;
    private float maxHealth;
    [SerializeField] Slider healthBar;
    private Rigidbody bisonRb;
    [SerializeField] Terrain map;

    private bool couldDamage;
    private int damageTimer;
    private int damage;

    // Start is called before the first frame update
    void Start()
    {
        bisonRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        map = Terrain.activeTerrain;

        maxHealth = 20 + ((Time.timeSinceLevelLoad / 60) * 4);
        health = maxHealth;

        couldDamage = false;
        damageTimer = 0;
        damage = (int)(Time.timeSinceLevelLoad / 60) + 15;

        InvokeRepeating("Charge", 8f, 8f);
        InvokeRepeating("CanDamage", 0f, 1f);

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 whereToLook = new Vector3(player.transform.position.x, map.SampleHeight(player.transform.position), player.transform.position.z);
        transform.LookAt(whereToLook);

        StayOnGround();
        UpdateHealth();
    }

    private void Charge()
    {
        Vector3 chargeTarget = player.transform.position - transform.position;
        bisonRb.AddForce(chargeTarget * speed);
    }

    private void CanDamage()
    {
        if (damageTimer < 3)
        {
            damageTimer++;
        }
        if (damageTimer == 3)
        {
            couldDamage = true;
        }
    }

    private void StayOnGround()
    {
        Vector3 ground = new Vector3(transform.position.x, map.SampleHeight(transform.position) + 2f, transform.position.z);
        transform.position = ground;
    }

    private void UpdateHealth()
    {
        healthBar.value = health / maxHealth;
        if (health < 1)
        {
            GameObject player = GameObject.Find("Player");
            PlayerController playerScript = player.GetComponent<PlayerController>();
            playerScript.money += 13;
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
            managerScript.damageDealt+= (int)fireballScript.damage;
            Destroy(other.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject player = GameObject.Find("Player");
        PlayerController playerScript = player.GetComponent<PlayerController>();

        if (collision.gameObject.CompareTag("Plasmaball"))
        {
            GameObject plasmaball = GameObject.Find("Ability2Prefab(Clone)");
            PlasmaBall plasmaballScript = plasmaball.GetComponent<PlasmaBall>();
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
