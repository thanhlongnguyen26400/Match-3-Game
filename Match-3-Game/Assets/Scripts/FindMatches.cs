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
<<<<<<< HEAD
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
=======

                                // check hang co isRowBomb hay khong
                                if (currentDot.GetComponent<Dot>().isRowBomb
                                    || leftDot.GetComponent<Dot>().isRowBomb
                                    || rightDot.GetComponent<Dot>().isRowBomb)
                                {
                                    currentMatches.Union(GetRowPieces(y)); // b? sung t?ng ??i t??ng vào list currentMatches
                                }

                                // check hang co isAdjacentBomb hay khong
                                if (currentDot.GetComponent<Dot>().isAdjacentBomb
                                    || leftDot.GetComponent<Dot>().isAdjacentBomb
                                    || rightDot.GetComponent<Dot>().isAdjacentBomb)
                                {
                                    currentMatches.Union(GetAdjacentPiece(x,y));
                                }

                                // check hang co isColumnBomb hay khong
                                if (currentDot.GetComponent<Dot>().isColumnBomb) currentMatches.Union(GetColumnPieces(x));
                                if (leftDot.GetComponent<Dot>().isColumnBomb) currentMatches.Union(GetColumnPieces(x - 1));
                                if (rightDot.GetComponent<Dot>().isColumnBomb) currentMatches.Union(GetColumnPieces(x + 1));


                                if (!currentMatches.Contains(leftDot))
>>>>>>> Check
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

                        GameObject upDot = board.allDots[x, y + 1];  // tham chieu doi tuong 

                        GameObject downDot = board.allDots[x, y - 1];
                        if (upDot != null && downDot != null)
                            if (upDot.tag == currentDot.tag && downDot.tag == currentDot.tag)
                            {
<<<<<<< HEAD
=======

>>>>>>> Check
                                // check cot co isColumnBomb hay khong
                                if (currentDot.GetComponent<Dot>().isColumnBomb
                                    || upDot.GetComponent<Dot>().isColumnBomb
                                    || downDot.GetComponent<Dot>().isColumnBomb)
                                {
                                    currentMatches.Union(GetColumnPieces(x)); //
                                }
<<<<<<< HEAD
                                // check cot co isRowBomb hay khong
                                if (currentDot.GetComponent<Dot>().isRowBomb) currentMatches.Union(GetRowPieces(y));
                                if (upDot.GetComponent<Dot>().isRowBomb) currentMatches.Union(GetRowPieces(y+1));
                                if (downDot.GetComponent<Dot>().isRowBomb) currentMatches.Union(GetRowPieces(y-1));


                                // check cot co isGeneratingBombs hay khong
                                if (currentDot.GetComponent<Dot>().isGeneratingBombs) currentMatches.Union(GetRowColumnPieces(y));
                                if (upDot.GetComponent<Dot>().isGeneratingBombs) currentMatches.Union(GetRowColumnPieces(y - 1));
                                if (downDot.GetComponent<Dot>().isGeneratingBombs) currentMatches.Union(GetRowColumnPieces(y + 1));

=======
                                // check cot co isAdjacentBomb hay khong
                                if (currentDot.GetComponent<Dot>().isAdjacentBomb
                                    || upDot.GetComponent<Dot>().isAdjacentBomb
                                    || downDot.GetComponent<Dot>().isAdjacentBomb)
                                {
                                    currentMatches.Union(GetAdjacentPiece(x,y)); //
                                }


                                // check cot co isRowBomb hay khong
                                if (currentDot.GetComponent<Dot>().isRowBomb) currentMatches.Union(GetRowPieces(y));
                                if (upDot.GetComponent<Dot>().isRowBomb) currentMatches.Union(GetRowPieces(y + 1));
                                if (downDot.GetComponent<Dot>().isRowBomb) currentMatches.Union(GetRowPieces(y - 1));


>>>>>>> Check

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

<<<<<<< HEAD
    List<GameObject> GetColumnPieces(int x)
=======

    // lay tat ca trong pham vi 1 o
    List<GameObject> GetAdjacentPiece(int column, int row)
    {
        List<GameObject> dots = new List<GameObject>();
        for (int x = column - 1; x <= column + 1; x++)
        {
            for (int y = row -1; y <= row + 1; y++)
            {
                if (x >= 0 && x < board.width && y >= 0 && y < board.height && board.allDots[x,y] != null)
                {
                    dots.Add(board.allDots[x, y]);
                    board.allDots[x, y].GetComponent<Dot>().isMatches = true;
                }
            }
        }
        return dots;
    }

    // lay tat ca Dot cung tag 
    public void MatchPieceOfColor(string color)
    {
        for (int x = 0; x < board.width; x++)
        {
            for (int y = 0; y < board.height; y++)
            {
                // check if that piece exits
                if (board.allDots[x,y] != null)
                {
                    if (board.allDots[x,y].tag == color && board.allDots[x, y] != null)
                    {
                        board.allDots[x, y].GetComponent<Dot>().isMatches = true;
                         
                    }
                }
            }
        }  
    }


    // lay tat ca cot
    List<GameObject> GetColumnPieces(int column)
>>>>>>> Check
    {
        List<GameObject> dots = new List<GameObject>();
        for (int y = 0; y < board.height; y++)
        {
            if (board.allDots[x, y] != null)
            {
<<<<<<< HEAD
                dots.Add(board.allDots[x, y]);
                board.allDots[x, y].GetComponent<Dot>().isMatches = true;
=======
                Dot dot = board.allDots[column, y].GetComponent<Dot>();
                if (dot.isRowBomb)
                {
                    dots.Union(GetRowPieces(y)).ToList();
                }
                if (dot.isAdjacentBomb)
                {
                    dots.Union(GetAdjacentPiece(column,y)).ToList();
                }

                dots.Add(board.allDots[column, y]);
                dot.isMatches = true;
>>>>>>> Check

            }
        }

        return dots;
    }

<<<<<<< HEAD

    List<GameObject> GetRowPieces(int y)
=======
   
    // lay tat hang
    List<GameObject> GetRowPieces(int row)
>>>>>>> Check
    {
        List<GameObject> dots = new List<GameObject>();
        for (int x = 0; x < board.width; x++)
        {
            if (board.allDots[x, y] != null)
            {
<<<<<<< HEAD
                dots.Add(board.allDots[x, y]);
                board.allDots[x, y].GetComponent<Dot>().isMatches = true;
=======
                Dot dot = board.allDots[x, row].GetComponent<Dot>();
                if (dot.isColumnBomb)
                {
                    dots.Union(GetColumnPieces(x)).ToList();
                }
                if (dot.isAdjacentBomb)
                {
                    dots.Union(GetAdjacentPiece(x, row)).ToList();
                }

                dots.Add(board.allDots[x, row]);
                dot.isMatches = true;
>>>>>>> Check

            }
        }

        return dots;
    }

<<<<<<< HEAD
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
=======
    // check bomb theo huong di chuyen khi keo chuot
    public void CheckBomb()
    {
        // did the player move somgthing?
        if (board.currentDot != null)
>>>>>>> Check
        {
          
            // is the piece they move matches
            if (board.currentDot.isMatches)
            {
                // make it unmatched
                board.currentDot.isMatches = false;
<<<<<<< HEAD
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
=======
                if ((board.currentDot.swipeAngle > -45 && board.currentDot.swipeAngle <= 45)
                   || (board.currentDot.swipeAngle < -135 || board.currentDot.swipeAngle >= 135))
                {
                    board.currentDot.MakeRowBomb();
                }
                else
                {
>>>>>>> Check
                    board.currentDot.MakeColumnBomb();
                }
            }
            // is the other piece matches
            else
            {
<<<<<<< HEAD
                 
            }
        }
    }

=======
                Dot otherDot = board.currentDot.otherDot.GetComponent<Dot>();
                if (board.currentDot.otherDot != null && otherDot.isMatches)
                {
                    // is the other Dot matched?
                    otherDot.isMatches = false;
                    if ((otherDot.swipeAngle > -45 && otherDot.swipeAngle <= 45)
                    || (otherDot.swipeAngle < -135 || otherDot.swipeAngle >= 135))
                    {
                        otherDot.MakeRowBomb();
                    }
                    else
                    {
                        otherDot.MakeColumnBomb();
                    }
                }
            }
        }
        
    }
>>>>>>> Check

}
