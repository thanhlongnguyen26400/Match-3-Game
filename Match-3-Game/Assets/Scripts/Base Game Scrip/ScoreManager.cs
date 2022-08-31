using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

    private Board board;
    public Text scoreText;
    public int score = 0;
    public Image scoreBar;

    private GameData gameData;  

    // Start is called before the first frame update
    void Start()
    {
        gameData = FindObjectOfType<GameData>();
        board = FindObjectOfType<Board>(); 
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "" + score;
        scoreBar.fillAmount = (float)score / (float)board.scoreGoals[board.scoreGoals.Length - 1];
    }

    public void IncreaseScore(int amountToIncrease)
    {
        score+= amountToIncrease;
        
    }
}
