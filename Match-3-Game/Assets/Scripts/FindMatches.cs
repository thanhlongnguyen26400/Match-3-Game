using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FindMatches : MonoBehaviour
{

    public Board board;
    public List<GameObject> currentMatches = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>(); 

    }

    public void FindAllMatches()
    {
        StartCoroutine(FindAllMatchesCo());
    } 

    IEnumerator FindAllMatchesCo()
    {
        yield return new WaitForSeconds(0.2f);
        for (int x = 0; x < board.width; x++)
        {
            for (int y = 0; y < board.height; y++)
            {
                GameObject currentDot = board.allDots[x, y];
                if (currentDot != null)
                {
                    // check horizontal
                    if( x > 0 && x < board.width - 1)
                    {
                        GameObject leftDot = board.allDots[x -1, y];
                        GameObject rightDot = board.allDots[x + 1, y];
                        if (leftDot != null && rightDot != null)
                            if (leftDot.tag == currentDot.tag && rightDot.tag == currentDot.tag)
                            {
                                // check hang co isRowBomb hay khong
                                if (currentDot.GetComponent<Dot>().isRowBomb 
                                    || leftDot.GetComponent<Dot>().isRowBomb 
                                    || rightDot.GetComponent<Dot>().isRowBomb)  
                                {
                                    currentMatches.Union(GetRowPieces(y)); // b? sung t?ng ??i t??ng vào list currentMatches
                                }
                                // check hang co isColumnBomb hay khong
                                if (currentDot.GetComponent<Dot>().isColumnBomb) currentMatches.Union(GetColumnPieces(x));
                                if (leftDot.GetComponent<Dot>().isColumnBomb) currentMatches.Union(GetColumnPieces(x-1));
                                if (rightDot.GetComponent<Dot>().isColumnBomb) currentMatches.Union(GetColumnPieces(x+1));


                                // check hang co isGeneratingBombs hay khong
                                if (currentDot.GetComponent<Dot>().isGeneratingBombs) currentMatches.Union(GetRowColumnPieces(x));
                                if (leftDot.GetComponent<Dot>().isGeneratingBombs) currentMatches.Union(GetRowColumnPieces(x - 1));
                                if (rightDot.GetComponent<Dot>().isGeneratingBombs) currentMatches.Union(GetRowColumnPieces(x + 1));



                                if (!currentMatches.Contains(leftDot)) 
                                {
                                    currentMatches.Add(leftDot);
                                }
                                leftDot.GetComponent<Dot>().isMatches = true;
                                if (!currentMatches.Contains(rightDot))
                                {
                                    currentMatches.Add(rightDot);
                                }
                                rightDot.GetComponent<Dot>().isMatches = true;
                                if (!currentMatches.Contains(currentDot))
                                {
                                    currentMatches.Add(currentDot); 
                                }
                                currentDot.GetComponent<Dot>().isMatches = true;
                            }


                               

                    }
                    // check vertical
                    if (y > 0 && y < board.height - 1)
                    {

                        GameObject upDot = board.allDots[x, y + 1];
                        GameObject downDot = board.allDots[x, y - 1];
                        if (upDot != null && downDot != null)
                            if (upDot.tag == currentDot.tag && downDot.tag == currentDot.tag)
                            {
                                // check cot co isColumnBomb hay khong
                                if (currentDot.GetComponent<Dot>().isColumnBomb
                                    || upDot.GetComponent<Dot>().isColumnBomb
                                    || downDot.GetComponent<Dot>().isColumnBomb)
                                {
                                    currentMatches.Union(GetColumnPieces(x)); //
                                }
                                // check cot co isRowBomb hay khong
                                if (currentDot.GetComponent<Dot>().isRowBomb) currentMatches.Union(GetRowPieces(y));
                                if (upDot.GetComponent<Dot>().isRowBomb) currentMatches.Union(GetRowPieces(y+1));
                                if (downDot.GetComponent<Dot>().isRowBomb) currentMatches.Union(GetRowPieces(y-1));


                                // check cot co isGeneratingBombs hay khong
                                if (currentDot.GetComponent<Dot>().isGeneratingBombs) currentMatches.Union(GetRowColumnPieces(y));
                                if (upDot.GetComponent<Dot>().isGeneratingBombs) currentMatches.Union(GetRowColumnPieces(y - 1));
                                if (downDot.GetComponent<Dot>().isGeneratingBombs) currentMatches.Union(GetRowColumnPieces(y + 1));


                                if (!currentMatches.Contains(upDot))
                                {
                                    currentMatches.Add(upDot);
                                }
                                upDot.GetComponent<Dot>().isMatches = true;
                                if (!currentMatches.Contains(downDot))
                                {
                                    currentMatches.Add(downDot);
                                }
                                downDot.GetComponent<Dot>().isMatches = true;
                                if (!currentMatches.Contains(currentDot))
                                {
                                    currentMatches.Add(currentDot);
                                }
                                currentDot.GetComponent<Dot>().isMatches = true;
                            }
                    }
                }
                
            }
        }
    }

    List<GameObject> GetColumnPieces(int x)
    {
        List<GameObject> dots = new List<GameObject>();
        for (int y = 0; y < board.height; y++)
        {
            if (board.allDots[x, y] != null)
            {
                dots.Add(board.allDots[x, y]);
                board.allDots[x, y].GetComponent<Dot>().isMatches = true;

            }
        }

        return dots;
    }


    List<GameObject> GetRowPieces(int y)
    {
        List<GameObject> dots = new List<GameObject>();
        for (int x = 0; x < board.width; x++)
        {
            if (board.allDots[x, y] != null)
            {
                dots.Add(board.allDots[x, y]);
                board.allDots[x, y].GetComponent<Dot>().isMatches = true;

            }
        }

        return dots;
    }

    List<GameObject> GetRowColumnPieces(int xy)
    {
        List<GameObject> dots = new List<GameObject>();
        for (int x = 0; x < board.width; x++)
        {
            if (board.allDots[x, xy] != null)
            {
                dots.Add(board.allDots[x, xy]);
                board.allDots[x, xy].GetComponent<Dot>().isMatches = true;

            }
        }
        for (int y = 0; y < board.height; y++)
        {
            if (board.allDots[xy, y] != null)
            {
                dots.Add(board.allDots[xy, y]);
                board.allDots[xy, y].GetComponent<Dot>().isMatches = true;
            }
        }
        return dots;
    }

    public void CheckBomb()
    {
        // did the player move somgthing?
        if(board.currentDot != null)
        {
            // is the piece they move matches
            if (board.currentDot.isMatches)
            {
                // make it unmatched
                board.currentDot.isMatches = false;
                // decide what kind of bomb to make
                int typeOfBomb = Random.Range(0, 100);
                if(typeOfBomb < 50)
                {
                    // make a row bomb
                    board.currentDot.MakeRowBomb();
                }
                else if(typeOfBomb >= 50)
                {
                    // make a column bomb
                    board.currentDot.MakeColumnBomb();
                }
            }
            // is the other piece matches
            else if (board.currentDot.otherDot != null)
            {
                 
            }
        }
    }


}
