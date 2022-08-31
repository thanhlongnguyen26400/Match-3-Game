using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ConfirmPanel : MonoBehaviour
{
    [Header("Level Info")]
    public string levelToLoad;
    private int startsActive;
    public int level;
    private GameData gameData;
    private int highScore;

    [Header ("UI stuff")]
    public Image[] stars;
    public Text hightScoreText;
    public Text StarText;
    



    // Start is called before the first frame update
    void Start()
    {
        gameData = FindObjectOfType<GameData>();
        LoadData();
        ActivateStars();
        SetText();
    }

    void LoadData()
    {
        if(gameData != null)
        {
            startsActive = gameData.saveData.stars[level - 1];
            highScore = gameData.saveData.highScores[level - 1];
        }
    }

    void SetText()
    {
        hightScoreText.text = "" + highScore;
        StarText.text = "" + startsActive + "/3";

    }

    void ActivateStars()
    {
        for (int i = 0; i < startsActive; i++)
        {
            stars[i].enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Cancel()
    {
        this.gameObject.SetActive(false);

    }

    public void Play()
    {
        PlayerPrefs.SetInt("Current Level", level - 1);
        SceneManager.LoadScene(levelToLoad);
    }
}
