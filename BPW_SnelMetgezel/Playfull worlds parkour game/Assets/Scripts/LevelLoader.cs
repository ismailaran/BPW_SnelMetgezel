using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LevelLoader : MonoBehaviour
{
    public Text finishTime;
    public Text text;
    public GameObject restartButton;
    public GameObject luigiImage;

    void Start()
    {
        finishTime.enabled = false;
        TimeScript.finished = false;
        text.enabled = true;
        restartButton.SetActive(false);
        luigiImage.SetActive(false);
        Cursor.visible = false;
        Debug.Log("ok");
    }
    public void level1Load()
    {
        SceneManager.LoadScene("TutorialLevel");
    }
    public void level1TimeScreenLoad()
    {
        SceneManager.LoadScene("TimeScreen");
    }
}
