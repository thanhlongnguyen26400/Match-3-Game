using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    wait,
    move,

}


public class Board : MonoBehaviour
{

    public GameState currentState = GameState.move;
    public GameObject[] dots;
    public GameObject tilePrefab;
    public GameObject[,] allDots;
    public GameObject destroyEffect;

    public int width;
    public int height;
    public int offSet;
    
    private BackgroundTile[,] allTiles;

    private FindMatches findMatches;

    // Start is called before the first frame update
    void Start()
    {
        findMatches = FindObjectOfType<FindMatches>();
        allTiles =  new BackgroundTile[width,height]; 
        allDots = new GameObject[width,height]; 
        Setup();
    }
    void Setup()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 tempPosition = new Vector2(x, y + offSet);
                GameObject backgroundTile =   Instantiate(tilePrefab, tempPosition, Quaternion.identity);
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
                allDots[x,y] = dot;

            }
        } 
    }
    private bool MatchesAt(int column, int row, GameObject piece)
    {
        // kiêm tra khi b?t ??u game nh?ng dot ko ???c trùng nhau


        // check t? v? trí 2-2 -> h?t trong board
        if(column > 1 && row > 1)
        {
            if (allDots[column - 1, row].tag == piece.tag && allDots[column -2, row].tag == piece.tag)
            {
                return true;
            }
            if (allDots[column , row -1].tag == piece.tag && allDots[column , row -2].tag == piece.tag)
            {
                return true;
            }
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

    void DestroyMatchesAt(int column, int row)
    {
        if (allDots[column, row].GetComponent<Dot>().isMatches)
        {
            // at the same time destroy allDot[x,y] at Board and remove allDot[x,y] at list<obj>
            findMatches.currentMatches.Remove(allDots[column, row]);
            GameObject partice = Instantiate(destroyEffect, allDots[column, row].transform.position, Quaternion.identity);
            Destroy(partice, 1f);

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
        yield return new WaitForSeconds(0.5f);

        // n?u có matches thì ti?p t?c l?p
        while (MatchesOnBoard())
        {
            yield return new WaitForSeconds(0.5f);
            DestroyMatches(); // destroy khi có matches
        }

        yield return new WaitForSeconds(0.5f);
        currentState = GameState.move;
    }
}
