using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TimeScript : MonoBehaviour
{

    Text text;
    public static bool canCount = false;
    public static bool finished = false;
    public static float finishedPlace = 0;
    public Text finishTime;
    public GameObject restartButton;
    public GameObject luigiImage;
    float count = 0f;
    void Start()
    {
        text = GetComponent<Text>();
        text.text = count.ToString();
    }

    void Update()
    {
        if (canCount)
        {
            count += Time.deltaTime;
        }
        text.text = "time " + count.ToString();

        if (finished == true)
        {
            finishTime.enabled = true;
            Cursor.visible = true;
            finishTime.text = "Congratulations" + "\n" + "end Time: " + count.ToString();
            text.enabled = false;
            restartButton.SetActive(true);
            luigiImage.SetActive(true);
            if (count < 85)
            {
                finishedPlace = 1;
            }
            else if (count < 105)
            {
                finishedPlace = 2;
            }
            else if (count < 160)
            {
                finishedPlace = 3;
            }
            else if (count > 160)
            {
                finishedPlace = 4;
            }
        }
        //Debug.Log(canCount);
    }
}
