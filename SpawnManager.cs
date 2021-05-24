using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    public bool isGameActive;

    [SerializeField] GameObject ballEnemy;
    [SerializeField] GameObject slime;
    [SerializeField] GameObject bison;
    [SerializeField] GameObject chest;
    [SerializeField] TextMeshProUGUI chestText;
    [SerializeField] GameObject[] powerups;
    [SerializeField] TextMeshProUGUI powerUpTextUI;
    [SerializeField] TextMeshProUGUI plasmaballTextUI;
    [SerializeField] Terrain map;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnEnemies", 1f, 30f);
        InvokeRepeating("SpawnChest", 1f, 60f);
    }

    private void SpawnEnemies()
    {
        //CHANGE LATER MAYBE
        int enemiesToSpawn = 1 + ((int)(Time.timeSinceLevelLoad / 30));
        //CHANGE LATER MAYBE

        for (int a = 0; a < enemiesToSpawn; a++)
        {
            int randomEnemy = Random.Range(1, 4);
            int randomX = Random.Range(50, 460);
            int randomZ = Random.Range(50, 460);
            int notRandomY = 65;

            Vector3 spawnPos = new Vector3(randomX, notRandomY, randomZ);

            if(randomEnemy == 1)
            {
                Instantiate(ballEnemy, spawnPos, Quaternion.Euler(0,0,0));
            }
            if(randomEnemy == 2)
            {
                Instantiate(slime, spawnPos, Quaternion.Euler(0, 0, 0));
            }
            if(randomEnemy == 3)
            {
                Instantiate(bison, spawnPos, Quaternion.Euler(0, 0, 0));
            }
        }
    }

    private void SpawnChest()
    {
        int randomPosX = Random.Range(50, 460);
        int randomPosZ = Random.Range(50, 460);
        Vector3 getSpawnLocation = new Vector3(randomPosX, 65, randomPosZ);
        float notRandomY = map.SampleHeight(getSpawnLocation) + 1;
        int randomRotation = Random.Range(0, 360);


        Vector3 spawnPos = new Vector3(randomPosX, notRandomY, randomPosZ);

        GameObject createdChest = Instantiate(chest, spawnPos, Quaternion.Euler(0, randomRotation, 0));
        Chest chestScript = createdChest.GetComponent<Chest>();
        chestScript.SetVariables(chestText, powerups, powerUpTextUI, plasmaballTextUI);
    }
}
