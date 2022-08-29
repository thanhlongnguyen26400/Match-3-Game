using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BlankGoal 
{
    public int numberNeeded;
    public int numberCollected;
    public Sprite goalSprite;
    public string matchValue;

}


public class GoalManager : MonoBehaviour
{
    public BlankGoal[] levelGoals;
    public GameObject goalPrefab;
    public GameObject goalIntroParent;
    public GameObject goalGameParent;
    public GameObject[] goalGame;

    public List<GoalPanel> currentGoals = new List<GoalPanel>();

    // Start is called before the first frame update
    void Start()
    {
        SetupIntroGoals();
    }

    void SetupIntroGoals()
    {
        for (int i = 0; i < levelGoals.Length; i++)
        {
            GameObject goal = Instantiate(goalPrefab, goalIntroParent.transform.position, Quaternion.identity);
            goal.transform.SetParent(goalIntroParent.transform);

            GoalPanel panel = goal.GetComponent<GoalPanel>();
            panel.thisSprite = levelGoals[i].goalSprite;
            panel.thisString = "0/" + levelGoals[i].numberNeeded;


            currentGoals.Add(panel);

/*            GameObject goalGame = Instantiate(goalPrefab, goalGameParent.transform.position, Quaternion.identity);
            goalGame.transform.SetParent(goalGameParent.transform);

            panel = goalGame.GetComponent<GoalPanel>();
            panel.thisSprite = levelGoals[i].goalSprite;
            panel.thisString = "0/" + levelGoals[i].numberNeeded;*/

            


        }

    }
        
    public void UpdateGoals()
    {
        int goalsComplete = 0;
        for (int i = 0; i < levelGoals.Length; i++)
        {
            currentGoals[i].thisText.text = "" + levelGoals[i].numberCollected + "/" + levelGoals[i].numberNeeded;
            if (levelGoals[i].numberCollected >= levelGoals[i].numberNeeded)
            {
                goalsComplete++;
                currentGoals[i].thisText.text = "" + levelGoals[i].numberNeeded + "/" + levelGoals[i].numberNeeded;
            }
        }
        if(goalsComplete >= levelGoals.Length)
        {
            Debug.Log("You win");
        }
    } 

    public void CompareGoal(string goalToCompare)
    {
        for (int i = 0; i < levelGoals.Length; i++)
        {
            if(goalToCompare == levelGoals[i].matchValue)
            {
                levelGoals[i].numberCollected++;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGoals();
    }
}
