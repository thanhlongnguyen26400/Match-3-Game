using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BackToSplash : MonoBehaviour
{

    public string sceneToLoad;
    private GameData gameData;
    private Board board;
    private ScoreManager scoreManager;



    public void WinOK()
    {
        if (gameData != null)
        {
            gameData.saveData.isActive[board.level + 1] = true;
          
            int highScore = gameData.saveData.highScores[board.level];
            if (scoreManager.score > highScore)
            {
                gameData.saveData.highScores[board.level] = scoreManager.score;
            }
            if (gameData.saveData.stars[board.level] >= 1)
            {
                gameData.saveData.stars[board.level] = 3;
            }
            gameData.saveData.stars[board.level] = Random.Range(1, 4);

            gameData.Save();
        }
        
        SceneManager.LoadScene(sceneToLoad);
    }

    public void LoseOK()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
    // Start is called before the first frame update
    void Start()
    {
        gameData = FindObjectOfType<GameData>();
        board = FindObjectOfType<Board>();
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
