using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour
{
    [Header("Board Vairables")]
    
    public int column;
    public int row;
    public int previousColumn;
    public int previousRow;
    public int targetX;
    public int targetY;

    public bool isMatches = false;

    private Board board;
    public GameObject otherDot;
    private FindMatches findMatches;
    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;
    private Vector2 tempPosition;
    private HintManager hintManager;

    [Header("Swipe Stuff")]
    public float swipeAngle = 0;
    public float swipeResist = 1f;

    [Header("Powerup Stuff")]
    public bool isColorBomb;
    public bool isColumnBomb;
    public bool isRowBomb;
<<<<<<< HEAD
    public bool isGeneratingBombs;
=======
    public bool isAdjacentBomb;
    public GameObject adjacentMarker;
>>>>>>> Check
    public GameObject rowArrow;
    public GameObject columnArrow;
    public GameObject colorBomb;
    

    // Start is called before the first frame update
    void Start()
    {
        isColumnBomb = false;
        isRowBomb = false;
        isColorBomb = false;
        isAdjacentBomb = false;

        board = FindObjectOfType<Board>();
        findMatches = FindObjectOfType<FindMatches>();
        hintManager = FindObjectOfType<HintManager>();
        /*        targetX = (int)transform.position.x;
                targetY = (int)transform.position.y;
                row = targetY;
                column = targetX;
                previousRow = row;
                previousColumn = column;*/
    }

<<<<<<< HEAD
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isColumnBomb = true;
            Debug.Log("is row bomb " + isColumnBomb);
            GameObject arrow = Instantiate(columnArrow, transform.position, Quaternion.identity);
            arrow.transform.parent = transform;
        }
    }

=======
>>>>>>> Check

    // Update is called once per frame
    void Update()
    {
<<<<<<< HEAD
        /*        FindMatches();*/
=======

>>>>>>> Check
        targetX = column;
        targetY = row;
        move();
        

    }       

    void move()
    {
        if (Mathf.Abs(targetX - transform.position.x) > .1)
        {
/*            Debug.Log(targetX + " - " + transform.position.x);*/
            // Move Towards the target
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .6f);
            if (board.allDots[column, row] != gameObject)
            {
                board.allDots[column, row] = gameObject;
            }
            findMatches.FindAllMatches();
        }
        else
        {

            // directly set the position
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = tempPosition;
           
        }


        if (Mathf.Abs(targetY - transform.position.y) > .1)
        {
            // Move Towards the target
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .6f);
            if (board.allDots[column, row] != gameObject)
            {
                board.allDots[column, row] = gameObject;
                
            }

            findMatches.FindAllMatches();

        }
        else
        {
            // directly set the position
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = tempPosition;
            board.allDots[column, row] = gameObject;
        }
    }

    public IEnumerator checkMoveCo()
    {
        if (isColorBomb)
        {
            //this piece is a color bomb and the otehr piece is the color the destroy
            findMatches.MatchPieceOfColor(otherDot.tag);
            isMatches = true;
        }
        else if (otherDot.GetComponent<Dot>().isColorBomb)
        {
            // the other piece is a color bomb and thes piece has the color to destroy 
            findMatches.MatchPieceOfColor(this.gameObject.tag);
            otherDot.GetComponent<Dot>().isMatches = true;
        }
        yield return new WaitForSeconds(0.5f);
        if(otherDot != null)
        {

            // check khi khong tim thay matches thi se tra lai vi tri ban dau
            if (!isMatches && !otherDot.GetComponent<Dot>().isMatches)
            {
                otherDot.GetComponent<Dot>().row = row;
                otherDot.GetComponent<Dot>().column = column;
                row = previousRow;
                column = previousColumn;
                board.currentState = GameState.wait;
                yield return new WaitForSeconds(0.5f);
                board.currentDot = null;
                board.currentState = GameState.move;
            }
            else
            {
                board.DestroyMatches();
                board.currentState = GameState.wait;
            }
            //otherDot = null;
        }
        
    }

    void OnMouseDown()
    {
        // Destroy the hint
        if(hintManager != null)
            hintManager.DestroyHint();
        if(board.currentState == GameState.move)
            firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void OnMouseUp()
    {
        if(board.currentState == GameState.move)
        {
            finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CalucateAngle();
        }
        
    } 
    
    void CalucateAngle()
    { 
        
        if(Mathf.Abs(finalTouchPosition.y - firstTouchPosition.y) > swipeResist || Mathf.Abs(finalTouchPosition.x - firstTouchPosition.x) > swipeResist)
        {
            // check kéo th? chu?t theo góc (180 ??)
            board.currentState = GameState.wait;
            swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x) * 180 / Mathf.PI;
            MovePiece();
<<<<<<< HEAD
            board.currentState = GameState.wait; 
=======
            
>>>>>>> Check
            board.currentDot = this;
        }
        else
        {
            board.currentState = GameState.move;
            
        }
    }
    void MovePieceActual(Vector2 direction)
    {
        otherDot = board.allDots[column + (int)direction.x, row + (int)direction.y];
        previousRow = row;
        previousColumn = column;
        if(otherDot != null)
        {
            otherDot.GetComponent<Dot>().column += -1 * (int)direction.x;
            otherDot.GetComponent<Dot>().row += -1 * (int)direction.y;
            column += (int)direction.x;
            row += (int)direction.y;
            StartCoroutine(checkMoveCo());
        }
        else
        {
            board.currentState = GameState.move;
        }
        
    }


    //  vuot theo huong
    void MovePiece()
    {
        if(swipeAngle > - 45 && swipeAngle <= 45 && column < board.width -1)
        {
            // Right Swipe
            MovePieceActual(Vector2.right);

        }
        else if(swipeAngle > 45 && swipeAngle <= 135 && row < board.height -1)
        {
            // Up Swipe
            MovePieceActual(Vector2.up);

        }
        else if ((swipeAngle > 135 || swipeAngle <= -135) && column > 0)
        {
            // Left Swipe
            MovePieceActual(Vector2.left);

        }
        else if (swipeAngle > -135 && swipeAngle <= -45 && row > 0)
        {
            // Down Swipe
            /*otherDot = board.allDots[column, row - 1];
            previousRow = row;
            previousColumn = column;
            otherDot.GetComponent<Dot>().row += 1;
            row -= 1;
            StartCoroutine(checkMoveCo());*/
            MovePieceActual(Vector2.down);

        }
        else
        {
            board.currentState = GameState.move;
        }

    }
    public void MakeRowBomb()
    {
        isRowBomb = true;
        Debug.Log("is row bomb " + isRowBomb);
        GameObject arrow = Instantiate(rowArrow, transform.position, Quaternion.identity);
        arrow.transform.parent = transform;
    }
    public void MakeColumnBomb()
    {
        isColumnBomb = true;
        Debug.Log("is ColumnBomb bomb " + isColumnBomb);
        GameObject arrow = Instantiate(columnArrow, transform.position, Quaternion.identity);
        arrow.transform.parent = transform;
    }
    public void MakeColorBomb()
    {
        isColorBomb = true;
        Debug.Log("is isColorBomb bomb " + isColorBomb);
        GameObject color = Instantiate(colorBomb, transform.position, Quaternion.identity);
        color.transform.parent = transform;
    }

<<<<<<< HEAD
    public void MakeRowBomb()
    {
        isRowBomb = true;
        Debug.Log("is row bomb " + isRowBomb);
        GameObject arrow = Instantiate(rowArrow, transform.position, Quaternion.identity);
        arrow.transform.parent = transform;
    }
    public void MakeColumnBomb()
    {
        isColumnBomb = true;
        Debug.Log("is row bomb " + isColumnBomb);
        GameObject arrow = Instantiate(columnArrow, transform.position, Quaternion.identity);
        arrow.transform.parent = transform;
    }
=======
    public void MakeAdjacentBomb()
    {
        isAdjacentBomb = true;
        Debug.Log("adjacentMarker");
        GameObject marker = Instantiate(adjacentMarker, transform.position, Quaternion.identity);
        marker.transform.parent = transform;
    }


>>>>>>> Check
}
