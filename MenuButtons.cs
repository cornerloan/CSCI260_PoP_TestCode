using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    private Button button;
    [SerializeField] int buttonID;
    [SerializeField] RawImage titleScreen;
    [SerializeField] RawImage howToPlayScreen;
    [SerializeField] RawImage controlsScreen;
    [SerializeField] Button startButton;
    [SerializeField] Button controlsButton;
    [SerializeField] Button howToPlayButton;
    [SerializeField] Button returnButton;
    [SerializeField] Button quitButton;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        
        if(buttonID == 1)
        {
            button.onClick.AddListener(HowToPlay);
        }
        else if(buttonID == 2)
        {
            button.onClick.AddListener(LoadGame);
        }
        else if(buttonID == 3)
        {
            button.onClick.AddListener(Controls);
        }
        else if(buttonID == 4)
        {
            button.onClick.AddListener(ReturnTitle);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void HowToPlay()
    {
        titleScreen.gameObject.SetActive(false);
        startButton.gameObject.SetActive(false);
        controlsButton.gameObject.SetActive(false);
        howToPlayButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
        howToPlayScreen.gameObject.SetActive(true);
        returnButton.gameObject.SetActive(true);
    }

    private void LoadGame()
    {
        SceneManager.LoadScene("Game");
    }

    private void Controls()
    {
        titleScreen.gameObject.SetActive(false);
        startButton.gameObject.SetActive(false);
        controlsButton.gameObject.SetActive(false);
        howToPlayButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
        controlsScreen.gameObject.SetActive(true);
        returnButton.gameObject.SetActive(true);
    }

    private void ReturnTitle()
    {
        returnButton.gameObject.SetActive(false);
        controlsScreen.gameObject.SetActive(false);
        howToPlayScreen.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(true);
        titleScreen.gameObject.SetActive(true);
        startButton.gameObject.SetActive(true);
        controlsButton.gameObject.SetActive(true);
        howToPlayButton.gameObject.SetActive(true);
    }
}
