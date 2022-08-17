using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{

    public int width;
    public int height;
    public GameObject tilePrefab;

    private BackgroundTile[,] allTiles;
    // Start is called before the first frame update
    void Start()
    {
        allTiles =  new BackgroundTile[width,height]; 
        Setup();
    }
    void Setup()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 tempPosition = new Vector2(x, y);
                Instantiate(tilePrefab, tempPosition, Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
