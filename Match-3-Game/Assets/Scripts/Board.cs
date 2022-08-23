using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    wait,
    move,

}
public enum TileKind
{
    Breakable,
    Blank,
    Normal,
}



[System.Serializable]
public class TileType 
{
    public int x;
    public int y;
    public TileKind tileKind;
}


public class Board : MonoBehaviour
{

    public GameState currentState = GameState.move;
    public GameObject[] dots;
    public GameObject tilePrefab;
    public GameObject[,] allDots;
    public GameObject destroyEffect;
    public Dot currentDot;

    

    public int width;
    public int height;
    public int offSet;

    public TileType[] boardLayout;

    private bool[,] blankSpaces;

    private FindMatches findMatches;

    // Start is called before the first frame update
    void Start()
    {
        findMatches = FindObjectOfType<FindMatches>();
        blankSpaces =  new bool[width, height];
        allDots = new GameObject[width,height]; 
        Setup();
    }

    // ????????????????????????
    public void GenarateBlankSpaces()
    {
        for (int i = 0; i < boardLayout.Length; i++)
        {
            if (boardLayout[i].tileKind == TileKind.Blank)
            {
                blankSpaces[boardLayout[i].x, boardLayout[i].y] = true;
            }
        }
    }

    void Setup()
    {
        GenarateBlankSpaces();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {

                if (!blankSpaces[x, y]) // ????????????
                {
                    Vector2 tempPosition = new Vector2(x, y + offSet);
                    GameObject backgroundTile = Instantiate(tilePrefab, tempPosition, Quaternion.identity);
                    backgroundTile.transform.parent = transform;
                    backgroundTile.name = "( " + x + "_" + y + " )";


                    int RandomId = Random.Range(0, dots.Length);
                    while (MatchesAt(x, y, dots[RandomId]))
                    {
                        RandomId = Random.Range(0, dots.Length);
                    }

                    GameObject dot = Instantiate(dots[RandomId], tempPosition, Quaternion.identity);
                    dot.GetComponent<Dot>().row = y;
                    dot.GetComponent<Dot>().column = x; // b? sung compoment Dot vào GameObject Dot


                    dot.transform.parent = transform;
                    dot.name = "( " + x + "_" + y + " )";
                    allDots[x, y] = dot;
                }
                

            }
        } 
    }
    private bool MatchesAt(int column, int row, GameObject piece)
    {
        // kiêm tra khi b?t ??u game nh?ng dot ko ???c trùng nhau


        // check t? v? trí 2-2 -> h?t trong board
        if(column > 1 && row > 1)
        {
            if(allDots[column - 1, row] != null && allDots[column-2,row] != null)
            {
                if (allDots[column - 1, row].tag == piece.tag && allDots[column - 2, row].tag == piece.tag)
                {
                    return true;
                }
            }

            if (allDots[column, row -1]!= null && allDots[column, row -2] != null)
            {
                if (allDots[column, row - 1].tag == piece.tag && allDots[column, row - 2].tag == piece.tag)
                {
                    return true;
                }
            }
            // -----------------------------------------------------------------//
        }
        // check v? trí 0-0 -> 1-1
        else if (column <= 1 || row <= 1)
        {
            if (row > 1)
                if (allDots[column, row - 1].tag == piece.tag && allDots[column, row - 2].tag == piece.tag) return true;

            if (column > 1)
                if (allDots[column - 1, row].tag == piece.tag && allDots[column - 2, row].tag == piece.tag) return true;

        }
        return false;
    }

    bool ColumnOrRow()
    {
        int numberHozizontal = 0;
        int numberVertical = 0;
        Dot firstPiece = findMatches.currentMatches[0].GetComponent<Dot>();
        if(firstPiece != null)
        {
            foreach (GameObject currentPiece in findMatches.currentMatches)
            {
                Dot dot = currentPiece.GetComponent<Dot>();
                if(dot.row == firstPiece.row && firstPiece.gameObject.tag == currentPiece.tag)
                {
                    numberHozizontal++;
                }
                if(dot.column == firstPiece.column && firstPiece.gameObject.tag == currentPiece.tag)
                {
                    numberVertical++;
                }
            }
        }
        return (numberVertical == 5 || numberHozizontal == 5);
        
    }

    void CheckToMakeBomb()
    {
        if (findMatches.currentMatches.Count == 4 || findMatches.currentMatches.Count == 7)
            findMatches.CheckBomb();
        else if(findMatches.currentMatches.Count == 5 || findMatches.currentMatches.Count == 8)
        {
            if (ColumnOrRow())
            {
                // make a color bomb
                
                if (currentDot != null)
                    if (currentDot.isMatches)
                    {
                        if (!currentDot.isColorBomb)
                        {
                            currentDot.isMatches = false;
                            currentDot.MakeColorBomb();
                        }
                    }
                    else
                    {
                        if(currentDot.otherDot != null)
                        {
                            Dot otherDot = currentDot.otherDot.GetComponent<Dot>();
                            if (otherDot.isMatches)
                            {
                                if (!otherDot.isColorBomb)
                                {
                                    otherDot.isMatches = false;
                                    otherDot.MakeColorBomb();
                                    Debug.Log("Color bomb");
                                }
                            }
                        }
                    }         
            }
            else
            {
                // make a adjacent bomb
                if (currentDot != null)
                    if (currentDot.isMatches)
                    {
                        if (!currentDot.isAdjacentBomb)
                        {
                            currentDot.isMatches = false;
                            currentDot.MakeAdjacentBomb();
                        }
                    }
                    else
                    {
                        if (currentDot.otherDot != null)
                        {
                            Dot otherDot = currentDot.otherDot.GetComponent<Dot>();
                            if (otherDot.isMatches)
                            {
                                if (!otherDot.isAdjacentBomb)
                                {
                                    otherDot.isMatches = false;
                                    otherDot.MakeAdjacentBomb();
                                }
                            }
                        }
                    }
            }
        }
            
    }

    void DestroyMatchesAt(int column, int row)
    {
        if (allDots[column, row].GetComponent<Dot>().isMatches)
        {
            // at the same time destroy allDot[x,y] at Board and remove allDot[x,y] at list<obj>
            if (findMatches.currentMatches.Count >= 4 )
            {
                currentState = GameState.wait;
                CheckToMakeBomb();
                
            }
            findMatches.currentMatches.Remove(allDots[column, row]);
            GameObject partice = Instantiate(destroyEffect, allDots[column, row].transform.position, Quaternion.identity); // hieu ung
            Destroy(partice, 1f); // destroy sau 1s

            Destroy(allDots[column, row]);
            allDots[column, row] = null;
        }
    }

    public void DestroyMatches()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (allDots[x, y] != null)
                {
                    DestroyMatchesAt(x, y);
                }
            }
        }
        StartCoroutine(DecreaseRowCo());
    }

    private IEnumerator DecreaseRowCo()
    {
        // s? ?? v? c?a dots
        int nullCount = 0;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (allDots[x,y] == null) // check dots ?ã b? destroy
                {
                    nullCount++;
                }
                else if( nullCount > 0)
                {
                    allDots[x, y].GetComponent<Dot>().row -= nullCount; // ??
                    
                    allDots[x, y] = null;
                }
            }
            nullCount = 0;
        }
        yield return new WaitForSeconds(0.4f);
        StartCoroutine(FillBoardCo());
    }

    private void RefillBoard()
    {

        // khi destroy kh?i t?o dots m?i
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (allDots[x,y] == null)
                {
                    Vector2 tempPosition = new Vector2(x, y + offSet);
                    int RandomId = Random.Range(0, dots.Length);
                    GameObject piece = Instantiate(dots[RandomId], tempPosition, Quaternion.identity);
                    allDots[x, y] = piece;
                    piece.GetComponent<Dot>().row = y;
                    piece.GetComponent<Dot>().column = x;
                }
            }
        }
        
    }

    private bool MatchesOnBoard()
    {
        // ki?m tra xem có matchs trên board hay không
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (allDots[x,y] != null)
                {
                    if (allDots[x, y].GetComponent<Dot>().isMatches)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
    private IEnumerator FillBoardCo()
    {
        RefillBoard();
        currentState = GameState.wait;
        yield return new WaitForSeconds(0.5f);

        // n?u có matches thì ti?p t?c l?p
        while (MatchesOnBoard())
        {
            yield return new WaitForSeconds(0.5f);
            DestroyMatches(); // destroy khi có matches
            currentState = GameState.wait;
        }
        findMatches.currentMatches.Clear();
        currentDot = null;
        yield return new WaitForSeconds(0.5f);
        currentState = GameState.move;
    }
}
