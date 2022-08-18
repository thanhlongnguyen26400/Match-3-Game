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
    private GameObject otherDot;

    Vector2 firstTouchPosition;
    Vector2 finalTouchPosition;
    Vector2 tempPosition;

    public float swipeAngle = 0;
    public float swipeResist = 1f;

    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
/*        targetX = (int)transform.position.x;
        targetY = (int)transform.position.y;
        row = targetY;
        column = targetX;
        previousRow = row;
        previousColumn = column;*/
     }

    // Update is called once per frame
    void Update()
    {
        FindMatches();
        if (isMatches)
        {
            SpriteRenderer mySprite = GetComponent<SpriteRenderer>();
            mySprite.color = new Color(1f, 1f, 1f, .2f);
        }


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
            if (!isMatches && !otherDot.GetComponent<Dot>().isMatches)
            {
                otherDot.GetComponent<Dot>().row = row;
                otherDot.GetComponent<Dot>().column = column;
                row = previousRow;
                column = previousColumn; 
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
        firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void OnMouseUp()
    {
        finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CalucateAngle();
    } 
    
    void CalucateAngle()
    { 
        
        if(Mathf.Abs(finalTouchPosition.y - firstTouchPosition.y) > swipeResist || Mathf.Abs(finalTouchPosition.x - firstTouchPosition.x) > swipeResist)
        {
            // check kéo th? chu?t theo góc (180 ??)
            swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x) * 180 / Mathf.PI;
        }
        

        MovePiece();
    }

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

    void FindMatches()
    {

        // check horizontal if left and right same type => ismatch
        if (column > 0 && column < board.width - 1 )
        {
            GameObject leftDot1 = board.allDots[column - 1, row];
            GameObject rightDot1 = board.allDots[column + 1, row];
            if(leftDot1 != null && rightDot1 != null)
            {
                if (leftDot1.tag == gameObject.tag && rightDot1.tag == gameObject.tag)
                {
                    leftDot1.GetComponent<Dot>().isMatches = true;
                    rightDot1.GetComponent<Dot>().isMatches = true;
                    isMatches = true;
                }
            }
             
        }


        // check vetical if up and down same type => ismatch
        if (row > 0 && row < board.height - 1)
        {
            GameObject upDot1 = board.allDots[column , row + 1];
            GameObject downDot1 = board.allDots[column , row -1 ] ;
            if(upDot1 != null && downDot1 != null)
            {
                if (upDot1.tag == gameObject.tag && downDot1.tag == gameObject.tag)
                {
                    upDot1.GetComponent<Dot>().isMatches = true;
                    downDot1.GetComponent<Dot>().isMatches = true;
                    isMatches = true;
                }
            }
            
        }
    } 
}
