using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PowerUpCommon : MonoBehaviour
{
    private float speed;
    private float height;
    public int powerUpID;
    public TextMeshProUGUI powerUpTextUI;
    public string powerUpText;
    public TextMeshProUGUI plasmaBallCharges;

    // Start is called before the first frame update
    void Start()
    {
        speed = 3;
        height = 1;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = transform.position;
        float newY = Mathf.Sin(Time.time * speed);
        transform.position = new Vector3(position.x, newY + 4, position.z) * height;
        transform.Rotate(0, 0.5f, 0, Space.Self);
    }

    private void OnDestroy()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        powerUpTextUI.text = "E: " + powerUpText;
        powerUpTextUI.gameObject.SetActive(true);

        if (Input.GetKeyDown(KeyCode.E))
        {
            GameObject powerUpManager = GameObject.Find("Power Up Manager");
            PowerUpManager powerUpManagerScript = powerUpManager.GetComponent<PowerUpManager>();
            powerUpManagerScript.UpdateUI(powerUpID);
            GrantPowers();
            powerUpTextUI.gameObject.SetActive(false);
            GameObject gameManager = GameObject.Find("Game Manager");
            GameManager managerScript = gameManager.GetComponent<GameManager>();
            managerScript.powerupsGained++;
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        powerUpTextUI.gameObject.SetActive(false);
    }

    private void GrantPowers()
    {
        GameObject player = GameObject.Find("Player");
        PlayerController playerScript = player.GetComponent<PlayerController>();
        if(powerUpID == 0)
        {
            playerScript.plusDamage += 5;
        }
        else if(powerUpID == 1)
        {
            playerScript.speed += 3;
        }
        else if(powerUpID == 2)
        {
            playerScript.plusJump += 1;
        }
        else if (powerUpID == 3)
        {
            playerScript.plusCharge += 1;
            if (playerScript.plusCharge == 2) plasmaBallCharges.gameObject.SetActive(true);
        }
        else if(powerUpID == 4)
        {
            playerScript.plusCrit += 10;
        }
        else if(powerUpID == 5)
        {
            playerScript.plusReduceDamage += 3;
        }
        else if(powerUpID == 6)
        {
            playerScript.plusFirstHit += 10;
        }
        else if(powerUpID == 7)
        {
            playerScript.maxHealth += 20;
            playerScript.currentHealth += 20;
        }
        else if(powerUpID == 8)
        {
            playerScript.plusHealthRegen += 1;
        }
        else
        {
            playerScript.plusLifesteal += 1;
        }
    }

    public void SetTextVariables(TextMeshProUGUI poweruptext, TextMeshProUGUI plasmaballtext)
    {
        powerUpTextUI = poweruptext;
        plasmaBallCharges = plasmaballtext;
    }
}