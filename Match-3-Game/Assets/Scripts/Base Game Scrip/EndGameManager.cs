using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum GameType 
{
    Moves,
    Time,
}

[System.Serializable]
public class EndGameRequirements 
{
    public GameType gameType;
    public int counterValue;


}



public class EndGameManager : MonoBehaviour
{
    public World world;
    int level;

    public GameObject movesLabel;
    public GameObject timeLabel;
    public GameObject youWinPanel;
    public GameObject tryAgianPanel;
    public Text counter;
    public EndGameRequirements requirements;
    public int currentCounterValue;
    private Board board;
    private float timerSeconds;
    private FadePanelController fadePanelController;


    private void Awake()
    {
        /* currentCounterValue = world.level[board.level].endGameRequirements.counterValue;*/
/*        currentCounterValue = requirements.counterValue;*/

    }
    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
        fadePanelController = FindObjectOfType<FadePanelController>();
        SetGameType();
        SetupGame();


    }
    void SetGameType()
    {
        if (board.world != null)
        {
            if(board.level < board.world.level.Length)
            {
                if (board.world.level[board.level] != null)
                {
                    requirements = board.world.level[board.level].endGameRequirements;
                }
            }
            
        }
    }

    void SetupGame()
    {
        if(requirements.gameType == GameType.Moves)
        {
            movesLabel.SetActive(true);
            timeLabel.SetActive(false);
        }
        else
        {
            timerSeconds = 1;
            movesLabel.SetActive(false);
            timeLabel.SetActive(true);
        }
        counter.text = "" + currentCounterValue;
    }

    public void DecreaseCounterValue()
    {
        if(board.currentState != GameState.pause)
        {
            currentCounterValue--;
            counter.text = "" + currentCounterValue;
            if (currentCounterValue <= 0)
            {
                LoseGame();
            }
        }
        
        
    }

    public void WinGame()
    {
        youWinPanel.SetActive(true);
        board.currentState = GameState.win;
        currentCounterValue = 0;
        counter.text = "" + currentCounterValue;
        FadePanelController fade = FindObjectOfType<FadePanelController>();
        fade.GameOver();

        Debug.Log("You win");
    }

    public void LoseGame()
    {
        tryAgianPanel.SetActive(true);
        board.currentState = GameState.lose;
        currentCounterValue = 0;
        counter.text = "" + currentCounterValue;
        FadePanelController fade = FindObjectOfType<FadePanelController>();
        fade.GameOver();
        Debug.Log("You lose");
    }

    // Update is called once per frame
    void Update()
    {
        if(requirements.gameType == GameType.Time && currentCounterValue > 0)
        {
            timerSeconds -= Time.deltaTime;
            if(timerSeconds <= 0)
            {
                DecreaseCounterValue();
                timerSeconds = 1;
            }
        }
    }
}
