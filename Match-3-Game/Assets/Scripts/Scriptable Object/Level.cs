using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "World", menuName = "Level")]
public class Level : ScriptableObject
{
    [Header("Board Dimansions ")]
    public int width;
    public int height;

    [Header("Starting Tiles")]
    public TileType[] boardLayout;

    [Header("Avaiable Dots")]
    public GameObject[] Dots;

    [Header("Score Goals")]
    public int[] scoreGoals;


    public EndGameRequirements endGameRequirements;
    public BlankGoal[] levelGoals;
}
