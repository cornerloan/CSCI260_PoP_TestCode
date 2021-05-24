using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallEnemy : MonoBehaviour
{
    private Rigidbody ballRb;
    [SerializeField] GameObject player;
    private Vector3 offset;

    [SerializeField] GameObject projectile;

    private float health;
    private float maxHealth;
    [SerializeField] Slider healthBar;

    [SerializeField] float speed;
    private float distanceWanted;
    private float distance;

    [SerializeField] Terrain map;

    // Start is called before the first frame update
    void Start()
    {
        ballRb = GetComponent<Rigidbody>();

        player = GameObject.Find("Player");
        map = Terrain.activeTerrain;
        distanceWanted = 50;
        maxHealth = 10 + ((Time.timeSinceLevelLoad / 60) * 2);
        health = maxHealth;
        InvokeRepeating("Movement", 2f, 2f);
        InvokeRepeating("Shoot", 5f, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player.transform);
        MoveUp();
        UpdateHealth();
    }

    private void Movement()
    {
        distance = Vector3.Distance(transform.position, player.transform.position);
        if(distance > distanceWanted)
        {
            ballRb.AddForce((player.transform.position - transform.position) * speed);
        }
        else
        {
            ballRb.velocity = Vector3.zero;
            ballRb.angularVelocity = Vector3.zero;
        }
    }

    private void Shoot()
    {
        Vector3 projectilePosition = transform.position;
        Instantiate(projectile, projectilePosition, transform.rotation);
    }

    private void MoveUp()
    {
        offset = new Vector3(transform.position.x, map.SampleHeight(transform.position) + 15f, transform.position.z);
        transform.position = offset;
    }

    private void UpdateHealth()
    {
        healthBar.value = health / maxHealth;
        if (health < 1)
        {
            GameObject player = GameObject.Find("Player");
            PlayerController playerScript = player.GetComponent<PlayerController>();
            playerScript.money += 5;
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
            health -= fireballScript.damage;
            playerScript.currentHealth += (int)playerScript.plusLifesteal;
            GameObject gameManager = GameObject.Find("Game Manager");
            GameManager managerScript = gameManager.GetComponent<GameManager>();
            managerScript.damageDealt += (int)fireballScript.damage;
            Destroy(other.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Plasmaball")
        {
            GameObject plasmaball = GameObject.Find("Ability2Prefab(Clone)");
            PlasmaBall plasmaballScript = plasmaball.GetComponent<PlasmaBall>();
            GameObject player = GameObject.Find("Player");
            PlayerController playerScript = player.GetComponent<PlayerController>();
            if (health == maxHealth) health -= playerScript.plusFirstHit;
            health -= plasmaballScript.damage;
            playerScript.currentHealth += (int)playerScript.plusLifesteal;
            GameObject gameManager = GameObject.Find("Game Manager");
            GameManager managerScript = gameManager.GetComponent<GameManager>();
            managerScript.damageDealt += (int)plasmaballScript.damage;
            Destroy(collision.gameObject);
        }
    }
}
