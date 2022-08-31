using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintManager : MonoBehaviour
{

    private Board board;

    public float hintDelay;
    private float hintDelaySeconds;
    public GameObject hintParticle;
    public GameObject currentHint;


    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
        hintDelaySeconds = hintDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if(board.currentState == GameState.move)
        {
            hintDelaySeconds -= Time.deltaTime;
            if (hintDelaySeconds <= 0 && currentHint == null )
            {
                MarkHint();
                hintDelaySeconds = hintDelay;
            }
        }
        
    }
    // First, i want to find all possible matches on the board
    private List<GameObject> FindAllMatches()
    {
        List<GameObject> possibleMoves = new List<GameObject>();
        for (int x = 0; x < board.width; x++)
        {
            for (int y = 0; y < board.height; y++)
            {
                if (board.allDots[x, y] != null)
                {
                    if (x < board.width - 1)
                    {
                        // kiemm tra ben tren co cung kieu hay khong
                        if (board.SwitchAndCheck(x, y, Vector2.right))
                        {
                            possibleMoves.Add(board.allDots[x, y]);
                        }

                    }
                    if (y < board.height - 1)
                    {
                        if (board.SwitchAndCheck(x, y, Vector2.up))
                        {
                            possibleMoves.Add(board.allDots[x, y]);
                        }
                    }
                }
            }
        }
        return possibleMoves;
    }
    // pick one of those matches randomly
    private GameObject PickOneRandomly()
    {
        List<GameObject> possibleMoves = new List<GameObject>();
        possibleMoves = FindAllMatches();
        if (possibleMoves.Count > 0)
        {
            int pieceToUse = Random.Range(0, possibleMoves.Count);
            return possibleMoves[pieceToUse];
        }
        return null;
    }

    // create the hint behind the chosen match
    private void MarkHint()
    {
        GameObject move = PickOneRandomly();
        if(move != null)
        {
            currentHint = Instantiate(hintParticle, move.transform.position, Quaternion.identity);
        }
    }
    // destroy the hint
    public void DestroyHint()
    {
        if(currentHint != null)
        {
            Destroy(currentHint);
            currentHint = null;
            hintDelaySeconds = hintDelay;

        }
    }
}
