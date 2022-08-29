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
    public GameObject movesLabel;
    public GameObject timeLabel;
    public Text counter;
    public EndGameRequirements requirements;
    public int currentCounterValue;

    // Start is called before the first frame update
    void Start()
    {
        SetupGame();
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
            movesLabel.SetActive(false);
            timeLabel.SetActive(true);
        }
        counter.text = "" + currentCounterValue;
    }

    public void DecreaseCounterValue()
    {
        if(currentCounterValue > 1)
        {
            currentCounterValue--;
            counter.text = "" + currentCounterValue;
        }
        else
        {
            Debug.Log("You lose");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
