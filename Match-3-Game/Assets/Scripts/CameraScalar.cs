using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScalar : MonoBehaviour
{
    private Board board;
    public float cameraOffSet;
    public float aspectRario = 0.625f;
    public float padding = 2;


    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
        if(board != null)
        {
            RepositionCamera(board.width -1 , board.height-1);
        }
    }
    void RepositionCamera(float x, float y )
    {
        Vector3 tempPosition = new Vector3( (float)x / 2, (float)y / 2, cameraOffSet);
        transform.position = tempPosition;
        if(board.width >= board.height)
        {
            Camera.main.orthographicSize = (board.width / 2 + padding) / aspectRario;
        }
        else
        {
            Camera.main.orthographicSize = (board.height / 2 + padding);
        }
    }

    // Update is called once per frame  
    void Update()
    {
        
    }
}