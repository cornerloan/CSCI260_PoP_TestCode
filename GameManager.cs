using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timer;
    private Scene gameScene;
    [SerializeField] string minutes;
    [SerializeField] string seconds;
    [SerializeField] TextMeshProUGUI finalTime;
    private GameObject player;
    public int damageDealt;
    public int enemiesSlain;
    public int powerupsGained;
    private TextMeshProUGUI finalDamageDealt;
    private TextMeshProUGUI finalEnemiesSlain;
    private TextMeshProUGUI finalPowerupsGained;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        DontDestroyOnLoad(this);
        gameScene = SceneManager.GetActiveScene();
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene() == gameScene)
        {
            UpdateTimer();
            CheckDead();
        }
        else
        {
            finalTime = GameObject.Find("Life Time").GetComponent<TextMeshProUGUI>();
            finalDamageDealt = GameObject.Find("Damage Dealt").GetComponent<TextMeshProUGUI>();
            finalEnemiesSlain = GameObject.Find("Enemies Slain").GetComponent<TextMeshProUGUI>();
            finalPowerupsGained = GameObject.Find("Powerups Gained").GetComponent<TextMeshProUGUI>();
            FinalTime();
            FinalDamage();
            FinalEnemies();
            FinalPowerups();
        }
    }

    private void UpdateTimer()
    {
        float t = Time.timeSinceLevelLoad;
        minutes = ((int)t / 60).ToString();
        seconds = ((int)t % 60).ToString();

        if (minutes.Length == 1) timer.text = "0" + minutes + ":";
        else timer.text = minutes + ":"; 

        if (seconds.Length == 1) timer.text += "0" + seconds;
        else timer.text += seconds;
    }

    private void CheckDead()
    {
        player = GameObject.Find("Player");
        PlayerController playerScript = player.GetComponent<PlayerController>();
        if(playerScript.currentHealth <= 0)
        {
            SceneManager.LoadScene("Game Over");
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void FinalTime()
    {
        finalTime.text = "You survived for: ";

        if (minutes.Length == 1) finalTime.text += "0";
        finalTime.text += minutes + ":";

        if (seconds.Length == 1) finalTime.text += "0";
        finalTime.text += seconds;
    }

    private void FinalDamage()
    {
        finalDamageDealt.text = "Damage dealt: ";
        finalDamageDealt.text += damageDealt;
    }

    private void FinalEnemies()
    {
        finalEnemiesSlain.text = "Enemies slain: ";
        finalEnemiesSlain.text += enemiesSlain;
    }

    private void FinalPowerups()
    {
        finalPowerupsGained.text = "Powerups gained: ";
        finalPowerupsGained.text += powerupsGained;
    }
}
