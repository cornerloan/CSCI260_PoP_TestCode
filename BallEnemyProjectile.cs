using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallEnemyProjectile : MonoBehaviour
{
    private GameObject player;
    private float speed;
    public int damage;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        Vector3 target = player.transform.position + new Vector3(0, 0.5f, 0);
        transform.LookAt(target);
        speed = 45;
        damage = (int)(Time.timeSinceLevelLoad / 60) + 3;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
        DestroyObject();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            GameObject player1 = GameObject.Find("Player");
            PlayerController playerScript = player1.GetComponent<PlayerController>();

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

            playerScript.wasDamaged = true;
            playerScript.timeSinceDamaged = 0;
        }
    }
    

    private void DestroyObject()
    {
        if(transform.position.x < -50 || transform.position.x > 520)
        {
            Destroy(gameObject);
        }
        if(transform.position.y < -10 || transform.position.y > 90)
        {
            Destroy(gameObject);
        }
        if(transform.position.z < -50 || transform.position.z > 520)
        {
            Destroy(gameObject);
        }
    }
}
