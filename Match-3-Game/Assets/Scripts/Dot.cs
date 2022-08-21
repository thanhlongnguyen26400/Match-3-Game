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

    [Header("Swipe Stuff")]
    public float swipeAngle = 0;
    public float swipeResist = 1f;

    [Header("Powerup Stuff")]
    public bool isColumnBomb;
    public bool isRowBomb;
    public bool isGeneratingBombs;
    public GameObject rowArrow;
    public GameObject columnArrow;

    // Start is called before the first frame update
    void Start()
    {
        isColumnBomb = false;
        isRowBomb = false;

        board = FindObjectOfType<Board>();
        findMatches = FindObjectOfType<FindMatches>();   
        /*        targetX = (int)transform.position.x;
                targetY = (int)transform.position.y;
                row = targetY;
                column = targetX;
                previousRow = row;
                previousColumn = column;*/
    }

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


    // Update is called once per frame
    void Update()
    {
        /*        FindMatches();*/
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
                yield return new WaitForSeconds(0.5f);
                board.currentDot = null;
                board.currentState = GameState.move;
            }
            else
            {
                board.DestroyMatches();
            }
            otherDot = null;
        }
        
    } 

    void OnMouseDown()
    {
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
            swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x) * 180 / Mathf.PI;
            MovePiece();
            board.currentState = GameState.wait; 
            board.currentDot = this;
        }
        else
        {
            board.currentState = GameState.move;
            
        }
    }


    //  vuot theo huong
    void MovePiece()
    {
        if(swipeAngle > - 45 && swipeAngle <= 45 && column < board.width -1 )
        {
            // Right Swipe
            otherDot = board.allDots[column + 1, row];
            previousRow = row;
            previousColumn = column;
            otherDot.GetComponent<Dot>().column -= 1;
            column += 1;
        }
        else if(swipeAngle > 45 && swipeAngle <= 135 && row < board.height -1)
        {
            // Up Swipe
            otherDot = board.allDots[column , row +1];
            previousRow = row;
            previousColumn = column; 
            otherDot.GetComponent<Dot>().row -= 1;
            row += 1;
        }
        else if ((swipeAngle > 135 || swipeAngle <= -135) && column > 0)
        {
            // Left Swipe
            otherDot = board.allDots[column - 1, row];
            previousRow = row;
            previousColumn = column; 
                otherDot.GetComponent<Dot>().column += 1;
            column -= 1;
        }
        else if (swipeAngle > -135 && swipeAngle <= -45 && row > 0)
        {
            // Down Swipe
            otherDot = board.allDots[column, row - 1];
            previousRow = row;
            previousColumn = column; 
            otherDot.GetComponent<Dot>().row += 1;
            row -= 1;
        }
        StartCoroutine(checkMoveCo());
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
        Debug.Log("is row bomb " + isColumnBomb);
        GameObject arrow = Instantiate(columnArrow, transform.position, Quaternion.identity);
        arrow.transform.parent = transform;
    }
}
