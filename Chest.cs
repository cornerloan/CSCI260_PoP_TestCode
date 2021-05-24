using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Chest : MonoBehaviour
{
    public TextMeshProUGUI chestText;
    private GameObject closedChest;
    private GameObject openedChest;
    private Vector3 openedChestLocation;
    private GameObject powerupSpawnLocation;
    public GameObject[] powerups;
    private bool shouldDestroy;
    private float destroyTimer;
    public TextMeshProUGUI powerUpTextUI;
    public TextMeshProUGUI plasmaballTextUI;
    private Terrain map;

    // Start is called before the first frame update
    void Start()
    {
        closedChest = transform.GetChild(0).gameObject;
        openedChest = transform.GetChild(1).gameObject;
        powerupSpawnLocation = transform.GetChild(2).gameObject;
        closedChest.SetActive(true);
        openedChest.SetActive(false);
        shouldDestroy = false;
        destroyTimer = 0;
        map = Terrain.activeTerrain;
    }

    // Update is called once per frame
    void Update()
    {
        //KeepChildrenInside();
        if (shouldDestroy)
        {
            destroyTimer += Time.deltaTime;
        }
        if(destroyTimer > 3)
        {
            chestText.gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            chestText.gameObject.SetActive(true);

            GameObject player = GameObject.Find("Player");
            PlayerController playerScript = player.GetComponent<PlayerController>();

            if (Input.GetKeyDown(KeyCode.E) && playerScript.money >= 0 && !shouldDestroy)
            {
                //playerScript.money -= 50;
                openedChestLocation = closedChest.transform.position;
                chestText.gameObject.SetActive(false);
                closedChest.SetActive(false);
                openedChest.SetActive(true);
                openedChest.transform.position = openedChestLocation;

                int randomPowerUp = Random.Range(0, 9);

                // FIX THIS SHIT
                Vector3 yoloplswork = new Vector3(powerupSpawnLocation.transform.position.x, powerupSpawnLocation.transform.position.y + 15, powerupSpawnLocation.transform.position.z);
                float yPos = map.SampleHeight(yoloplswork);
                Vector3 spawnLocation = new Vector3(powerupSpawnLocation.transform.position.x, yPos + 5, powerupSpawnLocation.transform.position.z);
                //FIX THIS SHIT

                Quaternion spawnRotation = new Quaternion(0, 0, 0, 0);
                GameObject createdPowerup = Instantiate(powerups[randomPowerUp], spawnLocation, spawnRotation);
                PowerUpCommon powerUpScript = createdPowerup.GetComponent<PowerUpCommon>();
                powerUpScript.SetTextVariables(powerUpTextUI, plasmaballTextUI);
                shouldDestroy = true;
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        chestText.gameObject.SetActive(false);
    }
    
    public void SetVariables(TextMeshProUGUI textChest, GameObject[] powerup, TextMeshProUGUI powerUpText, TextMeshProUGUI plasmaballText)
    {
        chestText = textChest;
        for(int a = 0; a < powerup.Length; a++)
        {
            powerups[a] = powerup[a];
        }
        powerUpTextUI = powerUpText;
        plasmaballTextUI = plasmaballText;
    }

    private void KeepChildrenInside()
    {
        transform.GetChild(0).transform.localPosition = new Vector3(0, 0, 0);
    }
}
