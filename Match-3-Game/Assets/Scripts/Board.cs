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
    public GameObject breakableTilePrefab;
    public GameObject[,] allDots;
    public GameObject destroyEffect;
    public Dot currentDot;
    public float refillDelay = 0.5f;
    public int[] scoreGoals;
    

    public int width;
    public int height;
    public int offSet;

    public TileType[] boardLayout;

    private bool[,] blankSpaces;

    private FindMatches findMatches;

    private BackgroundTile[,] breakableTiles;

    private ScoreManager scoreManager;

    private HintManager hintManager;

    private AudioController audioController;

    private GoalManager goalManager;



    // Start is called before the first frame update
    void Start()
    {
        goalManager = FindObjectOfType<GoalManager>();
        findMatches = FindObjectOfType<FindMatches>();
        scoreManager = FindObjectOfType<ScoreManager>();
        hintManager = FindObjectOfType<HintManager>();
        audioController = FindObjectOfType<AudioController>();
        blankSpaces =  new bool[width, height];
        allDots = new GameObject[width,height];
        breakableTiles = new BackgroundTile[width,height];
        Setup();
    }

    // blankSpaces check khong gian co gia tri khong ko
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

    public void GenerateBreakableTiles()
    {
        // look at all the tiles in the layout
        for (int i = 0; i < boardLayout.Length; i++)
        {
            // if a tiles is a "Jelly" tile
            if (boardLayout[i].tileKind == TileKind.Breakable)
            {
                // Ceate a "Jelly" ile at that position
                Vector2 tempPosition = new Vector2(boardLayout[i].x, boardLayout[i].y);
                GameObject tile = Instantiate(breakableTilePrefab, tempPosition, Quaternion.identity);
                breakableTiles[boardLayout[i].x, boardLayout[i].y] = tile.GetComponent<BackgroundTile>();
            }
        }
    }


    void Setup()
    {
        GenarateBlankSpaces();
        GenerateBreakableTiles();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {

                if (!blankSpaces[x, y]) // Neu x, y truyen vao bang voi blankSpaces la true => bo qua Dot vi tri x,y
                {
                    Vector2 tempPosition = new Vector2(x, y + offSet);
                    Vector2 tilePosition = new Vector2(x, y);
                    GameObject backgroundTile = Instantiate(tilePrefab, tilePosition, Quaternion.identity);
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
        // check trong board co match hay khong


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
        // check v? trí 1-1 -> 0-0
        else if (column <= 1 || row <= 1)
        {
            if (row > 1)
                if (allDots[column, row - 1] != null && allDots[column, row - 2] != null)
                    if (allDots[column, row - 1].tag == piece.tag && allDots[column, row - 2].tag == piece.tag) return true;

            if (column > 1)
                if (allDots[column -1, row] != null && allDots[column -2, row] != null)
                    if (allDots[column - 1, row].tag == piece.tag && allDots[column - 2, row].tag == piece.tag) return true;

        }
        return false;
    }

    bool ColumnOrRow()
    {
        int horizontal = 0;
        int vertical = 0;
        Dot firstPiece = findMatches.currentMatches[0].GetComponent<Dot>();
        if(firstPiece != null)
        {
            foreach (GameObject currentPiece in findMatches.currentMatches)
            {
                Dot dot = currentPiece.GetComponent<Dot>();
                if(dot.row == firstPiece.row )
                {
                    horizontal++;
                }
                if(dot.column == firstPiece.column )
                {
                    vertical++;
                }
            }
        }
        return horizontal >= 5 || vertical >= 5;


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
                {
                    Dot otherDot = currentDot.otherDot.GetComponent<Dot>();
                    if (currentDot.isMatches)
                    {
                        if (!currentDot.isColorBomb)
                        {
                            currentDot.isMatches = false;
                            currentDot.MakeColorBomb();
                        }
                    }
                    else if(otherDot.isMatches)
                    {
                        if (currentDot.otherDot != null)
                        {
                            if (!otherDot.isColorBomb)
                            {
                                otherDot.isMatches = false;
                                otherDot.MakeColorBomb();
                            }
                        }
                    }
                }
               

            }
            else
            {
                // make a adjacent bomb
                if (currentDot != null) 
                {
                    Dot otherDot = currentDot.otherDot.GetComponent<Dot>();
                    if (currentDot.isMatches)
                    {
                        if (!currentDot.isAdjacentBomb)
                        {
                            currentDot.isMatches = false;
                            currentDot.MakeAdjacentBomb();
                        }
                    }
                    else if(otherDot.isMatches)
                    {
                        if (currentDot.otherDot != null)
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
        if (allDots[column, row].GetComponent<Dot>().isMatches )
        {
            // at the same time destroy allDot[x,y] at Board and remove allDot[x,y] at list<obj>
            if (findMatches.currentMatches.Count >= 4 )
            {
                CheckToMakeBomb();
            }
            if(breakableTiles[column, row] != null)
            {
                // if it does, give one damage
                breakableTiles[column, row].TakeDamage(1);
                if (breakableTiles[column, row].hitPoints <= 0)
                {
                    breakableTiles[column, row] = null; 
                }
            }

            if(goalManager != null)
            {
                goalManager.CompareGoal(allDots[column, row].tag.ToString());
                goalManager.UpdateGoals();
            }

            
            GameObject partice = Instantiate(destroyEffect, allDots[column, row].transform.position, Quaternion.identity); // hieu ung
            Destroy(partice, 1f); // destroy sau 1s

            Destroy(allDots[column, row]);
            if (audioController)
            {
                audioController.PlaySound(audioController.isMatch);
            }
            allDots[column, row] = null;
            scoreManager.score += 10;
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
        findMatches.currentMatches.Clear();
        StartCoroutine(DecreaseRowCo2());
        // Destroy the hint
        if (hintManager != null)
            hintManager.DestroyHint();
    }

    private IEnumerator DecreaseRowCo2()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // if the current spot isn't blank or empty
                if (!blankSpaces[x,y] && allDots[x,y] == null)
                {
                    // loop from the space above to the top of the column
                    for (int k = y + 1; k < height; k++)
                    {
                        if (allDots[x,k] != null)
                        {
                            // move that dot to this empty space
                            allDots[x, k].GetComponent<Dot>().row = y;
                            //set that spot to be null
                            allDots[x, k] = null;
                            break;
                        }
                    }
                }
            }
        }
        yield return new WaitForSeconds(refillDelay);
        StartCoroutine(FillBoardCo());
    }

    private void RefillBoard()
    {

        // khi destroy kh?i t?o dots m?i
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (allDots[x,y] == null && !blankSpaces[x,y])
                {
                    Vector2 tempPosition = new Vector2(x, y + offSet);
                    int RandomId = Random.Range(0, dots.Length);

                    while (MatchesAt(x, y, dots[RandomId]))
                    {
                        RandomId = Random.Range(0, dots.Length);
                    }

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
        yield return new WaitForSeconds(refillDelay);

        // n?u có matches thì ti?p t?c l?p
        while (MatchesOnBoard())
        {
            currentState = GameState.wait;
            yield return new WaitForSeconds(refillDelay);
            DestroyMatches(); // destroy khi có matches
            currentState = GameState.wait;
        }
        findMatches.currentMatches.Clear();
        currentDot = null;
        yield return new WaitForSeconds(refillDelay);

        if (IsDeadlocked())
        {
            ShuffleBoard();
            Debug.Log("IsDeadlocked");
        }
        currentState = GameState.move;
    }


    private void SwitchPieces(int column, int row, Vector2 direction)
    {
        // take the second piece and save it in a holder
        GameObject holder = allDots[column + (int)direction.x, row + (int)direction.y]; 
        if(holder != null)
        {
            // switching the first dot to be the second position
            allDots[column + (int)direction.x, row + (int)direction.y] = allDots[column, row];
            //Set the first dot to be the second dot
            allDots[column, row] = holder;
        }
        
    }

    private bool CheckForMatches()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (allDots[x,y]!= null)
                {
                    if(x < width - 2)
                    {
                        // Check if the dots to the right and two to the right exist
                        if (allDots[x + 1, y] != null && allDots[x + 2, y] != null)
                        {
                            if (allDots[x + 1, y].tag == allDots[x, y].tag
                                && allDots[x + 2, y].tag == allDots[x, y].tag)
                            {
                                return true;
                            }
                        }
                    }
                    if (y < height - 2)
                    {
                        // Check if the dots above  exist
                        if (allDots[x, y + 1] != null && allDots[x, y + 2] != null)
                        {
                            if (allDots[x, y + 1].tag == allDots[x, y].tag
                                && allDots[x, y + 2].tag == allDots[x, y].tag)
                            {
                                return true;
                            }
                        }
                    }
                    
                }
            }
        }
        return false;
    }
    // check theo huong, kiem tra direction co 2 Dot cung Tag hay khong. Muc dich tim xem tren Board co matches hay khong
    public bool SwitchAndCheck(int column, int row, Vector2 direction)
    {
        SwitchPieces(column, row, direction); // hoan doi Dot theo huong ( vd: up or right)
        if (CheckForMatches()) // khi dc hoan doi thi check xem 2 Dot Right or Up co cung Tag hay khong
        {
            SwitchPieces(column, row, direction);
            return true;
           
        }
        SwitchPieces(column, row, direction);
        return false;
        
    }

    private bool IsDeadlocked()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (allDots[x,y] != null)
                {
                    if(x < width - 1)
                    {
                        // kiemm tra ben tren co cung kieu hay khong
                        if(SwitchAndCheck(x, y, Vector2.right))
                        {
                            return false;
                        }
                        
                    }
                    if(y < height - 1)
                    {
                        if(SwitchAndCheck(x,y, Vector2.up))
                        {
                            return false;
                        }
                    }
                }
            }
        }
        return true;
    }

    private void ShuffleBoard()
    {
        // Create a list Game object
        List<GameObject> newBoard = new List<GameObject>();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (allDots[x,y] != null)
                {
                    newBoard.Add(allDots[x,y]);
                }
            }
        }
        // for every spot on the board
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // if this spot shouldn't be blank
                if (!blankSpaces[x, y])
                {
                    // pick a random number
                    int pieceToUse = Random.Range(0, newBoard.Count);
                    
                    while (MatchesAt(x, y, newBoard[pieceToUse]))
                    {
                        pieceToUse = Random.Range(0, newBoard.Count);
                    }
                    Dot piece = newBoard[pieceToUse].GetComponent<Dot>(); // make a container for the piece

                    piece.column = x; 
                    piece.row = y; // gan lai column and row cho piece
                    allDots[x, y] = newBoard[pieceToUse]; // allDots duoc gan lai voi nhung object da dc random
                    newBoard.Remove(newBoard[pieceToUse]); // xoa List object 
                }
            }
        }
        // check if is deadlocked
        if (IsDeadlocked())
        {
            ShuffleBoard();
        }
    }

}
