using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevelStorer
{

    //public variable to be set in inspector
    public GameObject gameManager;

    //sets up gameobjects to be set
    GameObject farmhouse;
    GameObject barn;
    GameObject research;

    //creates levels variabel to be stored
    int[] levels = new int[3];

    //getter
    public int[] GetLvls() { return levels; }

    // Start is called before the first frame update
    public void GetLevels()
    {
        //finds game manager in scene
        gameManager = GameObject.FindGameObjectWithTag("GameController");

        //gets grid tiles with default buildings
        farmhouse = gameManager.GetComponent<GridScript>().GetGridTile(23);
        barn = gameManager.GetComponent<GridScript>().GetGridTile(10);
        research = gameManager.GetComponent<GridScript>().GetGridTile(9);

        //Sets level of each default
        levels[0] = farmhouse.GetComponent<ObjectInfo>().GetObjectLevel();
        levels[1] = barn.GetComponent<ObjectInfo>().GetObjectLevel();
        levels[2] = research.GetComponent<ObjectInfo>().GetObjectLevel();
    }
}
