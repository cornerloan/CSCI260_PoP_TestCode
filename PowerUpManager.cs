using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PowerUpManager : MonoBehaviour
{
    [SerializeField] int[] powerUpCount;
    [SerializeField] RawImage[] powerUpIcons;
    [SerializeField] TextMeshProUGUI[] powerUpText;
    private int[] powerUpPosition;
    private int powerUpPositionIndex;

    // Start is called before the first frame update
    void Start()
    {
        for (int a = 0; a < 10; a++)
        {
            powerUpCount[a] = 0;
        }
        powerUpPosition = new int[10];
        for (int b = 0; b < 10; b++)
        {
            powerUpPosition[b] = -690 + (100 * b);
        }
        powerUpPositionIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateUI(int powerUpID)
    {
        powerUpCount[powerUpID]++;
        if(powerUpCount[powerUpID] == 1)
        {
            powerUpIcons[powerUpID].gameObject.SetActive(true);
            powerUpIcons[powerUpID].gameObject.transform.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f,1);
            powerUpIcons[powerUpID].gameObject.transform.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 1);
            if (powerUpID == 8) powerUpIcons[powerUpID].gameObject.transform.localPosition = new Vector2(powerUpPosition[powerUpPositionIndex], 535);
            else powerUpIcons[powerUpID].gameObject.transform.localPosition = new Vector2(powerUpPosition[powerUpPositionIndex], 490);
            powerUpPositionIndex++;
        }
        powerUpText[powerUpID].text = powerUpCount[powerUpID].ToString();
    }
    
}
