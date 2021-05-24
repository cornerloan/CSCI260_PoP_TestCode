using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaBall : MonoBehaviour
{
    //particle effects for the projectile
    public ParticleSystem travellingParticle;
    //public ParticleSystem explosionParticle;

    //how fast the object moves
    public float velocity;
    //how much the x,y, and z scale is increased for every half second charging the shot
    public float increaseSizePerTick;
    //size to be created at
    private Vector3 objectSize;

    //used to keep the projectile in front of the player while charging
    //public GameObject player1;
    public float offset = 3;

    //determines if the projectile is being charged up
    public bool isCharging;
    //holds the time that the projectile us being charged for
    public float TimeCharging;
    //contains how many half seconds the projectile has charged for
    private int amountCharged;

    //determines where the projectile will fly when it is fired
    private GameObject shootLocation;

    public float damage;

    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.Find("Player");
        PlayerController playerScript = player.GetComponent<PlayerController>();
        
        travellingParticle.Play();
        isCharging = true;
        objectSize = new Vector3(0.75f, 0.75f, 0.75f);
        IncreaseSizeObject();
        TimeCharging = 0;
        amountCharged = 0;
        shootLocation = GameObject.Find("Player Facing");

        damage = 10 + playerScript.plusDamage;

        int doesCrit = Random.Range(0, 100);
        if(doesCrit < playerScript.plusCrit)
        {
            damage *= 2;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if the user is currently charging up the ability, then the object's size will increase
        if (isCharging && amountCharged <= 6)
        {
            IncreaseSizeObject();
        }
        //if the object is not charging, then the object has been launched and will move forward
        else
        {
            TimeCharging = 0;
            transform.Translate(Vector3.forward * velocity * Time.deltaTime);
        }
        
        //restart the particles if the end mid flight
        if (travellingParticle.isStopped)
        {
            travellingParticle.Play();
        }

        ShouldDelete();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //stop the traveling particle effect, and begin the explosion effect
        if (collision.gameObject.tag != "Player")
        {
            travellingParticle.Stop();
            //explosionParticle.Play();
            Destroy(gameObject);
        }
    }

    private void IncreaseSizeObject()
    {
        GameObject player = GameObject.Find("Player");
        //if the user is currently holding the right mouse button, then holdtoincrease() will run
        if (Input.GetMouseButton(1) && amountCharged < 6)
        {
            HoldToIncrease();
            transform.rotation = new Quaternion(0.0f, transform.rotation.y, 0.0f, transform.rotation.w);
            transform.position = player.transform.position + player.transform.forward * offset;
        }
        //if the user is no longer holding the right mouse, then they are no longer charging the projectile
        else
        {
            isCharging = false;
            TravelDirection();
            PlayerController playerScript = player.GetComponent<PlayerController>();
            damage += playerScript.plusDamage;
        }
    }

    private void HoldToIncrease()
    {
        //every half a second, the size of the projectile is increased by the increaseSizePerTick variable
        TimeCharging += Time.deltaTime;
        if(TimeCharging >= 0.5)
        {
            transform.localScale += new Vector3(increaseSizePerTick, increaseSizePerTick, increaseSizePerTick);
            TimeCharging = 0;
            amountCharged++;
            damage += 2;
        }
    }

    private void ShouldDelete()
    {
        if (transform.position.y > 90)
        {
            Destroy(gameObject);
        }
    }

    private void TravelDirection()
    {
        transform.LookAt(shootLocation.transform.position);
    }
}
