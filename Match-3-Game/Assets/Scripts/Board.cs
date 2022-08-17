using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public GameObject[] dots;

    public int width;
    public int height;
    public GameObject tilePrefab;
    public GameObject[,] allDots;

    private BackgroundTile[,] allTiles;

    // Start is called before the first frame update
    void Start()
    {
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
                Vector2 tempPosition = new Vector2(x, y);
                GameObject backgroundTile =   Instantiate(tilePrefab, tempPosition, Quaternion.identity);
                backgroundTile.transform.parent = transform;  
                backgroundTile.name = "( " + x + "_" + y + " )";


                int RandomId = Random.Range(0, dots.Length);
                GameObject dot = Instantiate(dots[RandomId], tempPosition, Quaternion.identity);
                dot.transform.parent = transform;
                dot.name = "( " + x + "_" + y + " )";
                allDots[x,y] = dot;

            }
        } 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
