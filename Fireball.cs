using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public ParticleSystem fireParticle;

    public GameObject shootLocation;

    public float velocity;

    public float damage;


    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.Find("Player");
        PlayerController playerScript = player.GetComponent<PlayerController>();
        damage = 10 + playerScript.plusDamage;

        int doesCrit = Random.Range(0, 100);
        if (doesCrit < playerScript.plusCrit)
        {
            damage *= 2;
        }

        shootLocation = GameObject.Find("Player Facing");
        fireParticle.Play();
        FireballDirection();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * velocity * Time.deltaTime);
        
        if (fireParticle.isStopped)
        {
            fireParticle.Play();
        }

        ShouldDelete();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            fireParticle.Stop();
            Destroy(gameObject);
        }
    }

    private void ShouldDelete()
    {
        if(transform.position.y > 90)
        {
            Destroy(gameObject);
        }
    }
    
    private void FireballDirection()
    {
        transform.LookAt(shootLocation.transform.position);
    }
}
