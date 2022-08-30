using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadePanelController : MonoBehaviour
{
    public Animator panelAnim;

    public Animator gameInfoAnim;


    private Board board;


    private void Start()
    {
        board = GetComponent<Board>();
        if (panelAnim != null && gameInfoAnim != null)
        {
            panelAnim.SetBool("Out", false);
            gameInfoAnim.SetBool("Out", false);
        }
    }

    public void OK()
    {
        if(panelAnim != null && gameInfoAnim != null)
        {

            panelAnim.SetBool("Out", true);
            gameInfoAnim.SetBool("Out", true);
            Debug.Log("lick ok");
            StartCoroutine(GameStartCo());           
        }
        
    }
    public void GameOver()
    {
        panelAnim.SetBool("Out", false);
        panelAnim.SetBool("GameOver", true);
    }

    public void Check()
    {
        
        if (panelAnim != null)
        {
            gameInfoAnim.SetTrigger("Checkin");
            gameInfoAnim.SetBool("Out", false);
            Debug.Log("OK check in");
        }
    }

    IEnumerator GameStartCo()
    {
        Board board = FindObjectOfType<Board>();
        if(board.currentState == GameState.pause)
        {
            yield return new WaitForSeconds(1f);
            board.currentState = GameState.move;
        }
       
    }
}
