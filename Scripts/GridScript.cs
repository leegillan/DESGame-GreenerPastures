using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GridScript : MonoBehaviour
{
    //Declare variables for grid dimensions and layout
    public float xStart, yStart;
    public int columnLength, rowLength;
    public int xSpacing, zSpacing;

    //initial tile to be placed on grid
    public GameObject gridSquare;
    public Object lockLvl2;
    public Object lockLvl3;

    //list if tiles on grid
    List<GameObject> gridSquares = new List<GameObject>();
    public List<GameObject> GetGrid() { return gridSquares; }

    //removes tile from the grid list
    public void RemoveGridTile(GameObject ob) { gridSquares.Remove(ob); }

    //adds tile from the grid list
    public void AddGridTile(GameObject ob) { gridSquares.Add(ob); }

    //Gets grid tile from ID of tile
    public GameObject GetGridTile(int id)
    {
        //Decalre variables
        bool found = false;
        int count = 0;

        //looks throught the list to find the one being used
        while (found == false)
        {
            if (gridSquares[count].GetComponent<ObjectInfo>().GetObjectID() == id)
            {
                found = true;
            }

            count += 1;
        }

        //returns the grid when found otherwise it returns nothing
        if (found)
        {
            return gridSquares[count - 1];
        }
        else
        {
            return null;
        }
    }

    //Creates the grid with the tiles and spacing betwen them
    public void CreateGrid(bool tutorial)
    {
        //Declares variable
        int id = 0;

        //Creates minimum spacing if the default is below 1
        if (xSpacing < 1)
        {
            xSpacing = 1;
        }

        if (zSpacing < 1)
        {
            zSpacing = 1;
        }

        //spawns outside ground
        Instantiate(Resources.Load("Grid"), new Vector3(36.0f, 1.0f, 25.9f), Quaternion.identity);
        Instantiate(Resources.Load("GridFill"), new Vector3(36.0f, 1.0f, 25.9f), Quaternion.identity);
        Instantiate(Resources.Load("OutsideArea"), new Vector3(36.0f, 1.0f, 25.9f), Quaternion.identity);

        Instantiate(Resources.Load("Tractor"), new Vector3(64.0f, 2.0f, 33.0f), new Quaternion(0.0f, 0.225f, 0.0f, 0.974f));

        //Spawns locked area blockers
        lockLvl2 = Instantiate(lockLvl2, new Vector3(76.64f, 13.5f, -23.25f), Quaternion.identity);

        lockLvl3 = Instantiate(lockLvl3, new Vector3(-13.25f, 13.5f, 86.23f), new Quaternion(0.0f, 0.7071f, 0.0f, 0.7071f));

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("TutorialScene"))
        {
            Instantiate(Resources.Load("TutorialResources/TutorialBlockers"), new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        }

        //creates and instantiates each tile, giving them a unique ID
        for (int i = 0; i < columnLength; i++)
        {
            for (int j = 0; j < rowLength; j++)
            {
                //Calls Create Square function to place a unique tile
                CreateSquare(new Vector3((xSpacing * (i % columnLength)), 1.0f, (zSpacing * (j % rowLength))), id, tutorial);

                id++;
            }
        }
    }

    //creates individual tiles, setting ID and types
    void CreateSquare(Vector3 pos, int ID, bool tutorial)
    {
        //Sets default grids components and locations of assets
        if (ID == 10)
        {
            gridSquares.Add((GameObject)Instantiate(Resources.Load("Barn"), pos, Quaternion.identity));
            gridSquares[ID].GetComponent<ObjectInfo>().SetObjectID(ID);
            gridSquares[ID].GetComponent<ObjectInfo>().SetObjectType(ObjectInfo.ObjectType.BARN);
            gridSquares[ID].GetComponent<ObjectInfo>().SetObjectLevel(1);
        }
        else if (ID == 23)
        {

            gridSquares.Add((GameObject)Instantiate(Resources.Load("Farmhouse"), pos, Quaternion.identity));
            gridSquares[ID].GetComponent<ObjectInfo>().SetObjectID(ID);
            gridSquares[ID].GetComponent<ObjectInfo>().SetObjectType(ObjectInfo.ObjectType.FARMHOUSE);
            gridSquares[ID].GetComponent<ObjectInfo>().SetObjectLevel(1);

        }
        else if (ID == 9)
        {
            gridSquares.Add((GameObject)Instantiate(Resources.Load("ResearchLab"), pos, Quaternion.identity));
            gridSquares[ID].GetComponent<ObjectInfo>().SetObjectID(ID);
            gridSquares[ID].GetComponent<ObjectInfo>().SetObjectType(ObjectInfo.ObjectType.RESEARCH);
            gridSquares[ID].GetComponent<ObjectInfo>().SetObjectLevel(1);
        }
        else if (tutorial == true)
        {
            CreateTutSquare(pos, ID);
        }
        else if (ID == 18)
        {
            gridSquares.Add((GameObject)Instantiate(Resources.Load("ChickenCoop"), pos, Quaternion.identity));
            gridSquares[ID].GetComponent<ObjectInfo>().SetObjectID(ID);
            gridSquares[ID].GetComponent<ObjectInfo>().SetObjectType(ObjectInfo.ObjectType.CHICKEN_COOP);
            gridSquares[ID].GetComponent<ObjectInfo>().SetObjectLevel(1);
            Instantiate(Resources.Load("Chicken"), new Vector3(pos.x + 2.818f, pos.y, pos.z - 1.299f), new Quaternion(0.0f, -0.625f, 0.0f, 0.780f), gridSquares[ID].transform);
            gridSquares[ID].GetComponent<ObjectFill>().SetFillType(ObjectFill.FillType.CHICKEN);
        }
        else
        {   
            gridSquare.GetComponent<ObjectInfo>().SetObjectID(ID);
            gridSquare.GetComponent<ObjectInfo>().SetObjectType(ObjectInfo.ObjectType.EMPTY);
            gridSquares.Add((GameObject)Instantiate(gridSquare, pos, Quaternion.identity));
        }
    }

    //creates individual tiles, setting ID and types
    void CreateTutSquare(Vector3 pos, int ID)
    {
        if (ID == 3)
        {
            gridSquares.Add((GameObject)Instantiate(Resources.Load("VerticalFarm_lvl2"), pos, Quaternion.identity));
            gridSquares[ID].GetComponent<ObjectInfo>().SetObjectID(ID);
            gridSquares[ID].GetComponent<ObjectInfo>().SetObjectType(ObjectInfo.ObjectType.VERTICAL_FARM);
            gridSquares[ID].GetComponent<ObjectInfo>().SetObjectLevel(2);
            Instantiate(Resources.Load("Sphere"), pos, Quaternion.identity, gridSquares[ID].transform);
            gridSquares[ID].GetComponent<ObjectFill>().SetFillType(ObjectFill.FillType.NONE);
        }
        else if (ID == 5)
        {
            gridSquares.Add((GameObject)Instantiate(Resources.Load("ChickenCoop"), pos, Quaternion.identity));
            gridSquares[ID].GetComponent<ObjectInfo>().SetObjectID(ID);
            gridSquares[ID].GetComponent<ObjectInfo>().SetObjectType(ObjectInfo.ObjectType.CHICKEN_COOP);
            gridSquares[ID].GetComponent<ObjectInfo>().SetObjectLevel(1);
            Instantiate(Resources.Load("Chicken"), new Vector3(pos.x + 2.818f, pos.y, pos.z - 1.299f), new Quaternion(0.0f, -0.625f, 0.0f, 0.780f), gridSquares[ID].transform);
            gridSquares[ID].GetComponent<ObjectFill>().SetFillType(ObjectFill.FillType.CHICKEN);
        }
        else if (ID == 7)
        {
            gridSquares.Add((GameObject)Instantiate(Resources.Load("ChickenCoop_lvl3"), pos, Quaternion.identity));
            gridSquares[ID].GetComponent<ObjectInfo>().SetObjectID(ID);
            gridSquares[ID].GetComponent<ObjectInfo>().SetObjectType(ObjectInfo.ObjectType.CHICKEN_COOP);
            gridSquares[ID].GetComponent<ObjectInfo>().SetObjectLevel(3);
            Instantiate(Resources.Load("Chicken"), new Vector3(pos.x + -4.5f, pos.y, pos.z + 0.02f), new Quaternion(0.0f, 0.287f, 0.0f, 0.958f), gridSquares[ID].transform);
            Instantiate(Resources.Load("Chicken"), new Vector3(pos.x + 0.12f, pos.y, pos.z + 1.88f), new Quaternion(0.0f, 0.988f, 0.0f, -0.157f), gridSquares[ID].transform);
            Instantiate(Resources.Load("Chicken"), new Vector3(pos.x + 2.818f, pos.y, pos.z - 1.299f), new Quaternion(0.0f, -0.625f, 0.0f, 0.780f), gridSquares[ID].transform);
            gridSquares[ID].GetComponent<ObjectFill>().SetFillType(ObjectFill.FillType.CHICKEN);
        }
        else if (ID == 8)
        {
            gridSquares.Add((GameObject)Instantiate(Resources.Load("Field_lvl3"), pos, Quaternion.identity));
            gridSquares[ID].GetComponent<ObjectInfo>().SetObjectID(ID);
            gridSquares[ID].GetComponent<ObjectInfo>().SetObjectType(ObjectInfo.ObjectType.FIELD);
            gridSquares[ID].GetComponent<ObjectInfo>().SetObjectLevel(3);
            Instantiate(Resources.Load("Sugarcane"), pos, Quaternion.identity, gridSquares[ID].transform);
            gridSquares[ID].GetComponent<ObjectFill>().SetFillType(ObjectFill.FillType.SUGARCANE);
        }
        else if(ID == 11)
        {
            gridSquares.Add((GameObject)Instantiate(Resources.Load("CowField_lvl2"), pos, Quaternion.identity));
            gridSquares[ID].GetComponent<ObjectInfo>().SetObjectID(ID);
            gridSquares[ID].GetComponent<ObjectInfo>().SetObjectType(ObjectInfo.ObjectType.COW_FIELD);
            gridSquares[ID].GetComponent<ObjectInfo>().SetObjectLevel(2);
            Instantiate(Resources.Load("Cow"), new Vector3(pos.x - 2.86f, pos.y, pos.z + -1.41f), new Quaternion(0.0f, 0.335f, 0.0f, 0.942f), gridSquares[ID].transform);
            Instantiate(Resources.Load("Cow"), new Vector3(pos.x + -1.52f, pos.y, pos.z + 0.93f), new Quaternion(0.0f, 0.990f, 0.0f, 0.138f), gridSquares[ID].transform);
            gridSquares[ID].GetComponent<ObjectFill>().SetFillType(ObjectFill.FillType.COW);
        }
        else if (ID == 14)
        {
            gridSquares.Add((GameObject)Instantiate(Resources.Load("MeatLab"), pos, Quaternion.identity));
            gridSquares[ID].GetComponent<ObjectInfo>().SetObjectID(ID);
            gridSquares[ID].GetComponent<ObjectInfo>().SetObjectType(ObjectInfo.ObjectType.MEAT_LAB);
            gridSquares[ID].GetComponent<ObjectInfo>().SetObjectLevel(1);
            Instantiate(Resources.Load("Sphere"), pos, Quaternion.identity, gridSquares[ID].transform);
            gridSquares[ID].GetComponent<ObjectFill>().SetFillType(ObjectFill.FillType.NONE);
        }
        else if(ID == 15)
        {
            gridSquares.Add((GameObject)Instantiate(Resources.Load("PigField_lvl3"), pos, Quaternion.identity));
            gridSquares[ID].GetComponent<ObjectInfo>().SetObjectID(ID);
            gridSquares[ID].GetComponent<ObjectInfo>().SetObjectType(ObjectInfo.ObjectType.PIG_PEN);
            gridSquares[ID].GetComponent<ObjectInfo>().SetObjectLevel(3);
            Instantiate(Resources.Load("Pig"), new Vector3(pos.x - 5.75f, pos.y, pos.z + 2.38f), new Quaternion(0.0f, -0.827f, 0.0f, 0.563f), gridSquares[ID].transform);
            Instantiate(Resources.Load("Pig"), new Vector3(pos.x + 2.44f, pos.y, pos.z - 1.97f), new Quaternion(0.0f, 0.294f, 0.0f, 0.956f), gridSquares[ID].transform);
            Instantiate(Resources.Load("Pig"), new Vector3(pos.x - 2.19f, pos.y, pos.z + 1.24f), new Quaternion(0.0f, 0.999f, 0.0f, 0.041f), gridSquares[ID].transform);
            gridSquares[ID].GetComponent<ObjectFill>().SetFillType(ObjectFill.FillType.PIG);
        }
        else if(ID == 16)
        {
            gridSquares.Add((GameObject)Instantiate(Resources.Load("PigField_lvl2"), pos, Quaternion.identity));
            gridSquares[ID].GetComponent<ObjectInfo>().SetObjectID(ID);
            gridSquares[ID].GetComponent<ObjectInfo>().SetObjectType(ObjectInfo.ObjectType.PIG_PEN);
            gridSquares[ID].GetComponent<ObjectInfo>().SetObjectLevel(2);
            Instantiate(Resources.Load("Pig"), new Vector3(pos.x - 5.75f, pos.y, pos.z + 2.38f), new Quaternion(0.0f, 0.294f, 0.0f, 0.956f), gridSquares[ID].transform);
            Instantiate(Resources.Load("Pig"), new Vector3(pos.x + 2.44f, pos.y, pos.z - 1.97f), new Quaternion(0.0f, 0.294f, 0.0f, 0.956f), gridSquares[ID].transform);
            gridSquares[ID].GetComponent<ObjectFill>().SetFillType(ObjectFill.FillType.PIG);
        }
        else if(ID == 19)
        {
            gridSquares.Add((GameObject)Instantiate(Resources.Load("Field"), pos, Quaternion.identity));
            gridSquares[ID].GetComponent<ObjectInfo>().SetObjectID(ID);
            gridSquares[ID].GetComponent<ObjectInfo>().SetObjectType(ObjectInfo.ObjectType.FIELD);
            gridSquares[ID].GetComponent<ObjectInfo>().SetObjectLevel(1);
            Instantiate(Resources.Load("Corn"), pos, Quaternion.identity, gridSquares[ID].transform);
            gridSquares[ID].GetComponent<ObjectFill>().SetFillType(ObjectFill.FillType.CORN);
        }
        else if(ID == 22)
        {
            gridSquares.Add((GameObject)Instantiate(Resources.Load("Field_lvl2"), pos, Quaternion.identity));
            gridSquares[ID].GetComponent<ObjectInfo>().SetObjectID(ID);
            gridSquares[ID].GetComponent<ObjectInfo>().SetObjectType(ObjectInfo.ObjectType.FIELD);
            gridSquares[ID].GetComponent<ObjectInfo>().SetObjectLevel(2);
            Instantiate(Resources.Load("Sunflower"), pos, Quaternion.identity, gridSquares[ID].transform);
            gridSquares[ID].GetComponent<ObjectFill>().SetFillType(ObjectFill.FillType.SUNFLOWER);
        }
        else if(ID == 24)
        {
            gridSquares.Add((GameObject)Instantiate(Resources.Load("Field_lvl2"), pos, Quaternion.identity));
            gridSquares[ID].GetComponent<ObjectInfo>().SetObjectID(ID);
            gridSquares[ID].GetComponent<ObjectInfo>().SetObjectType(ObjectInfo.ObjectType.FIELD);
            gridSquares[ID].GetComponent<ObjectInfo>().SetObjectLevel(2);
            Instantiate(Resources.Load("Sunflower"), pos, Quaternion.identity, gridSquares[ID].transform);
            gridSquares[ID].GetComponent<ObjectFill>().SetFillType(ObjectFill.FillType.SUNFLOWER);
        }
        else
        {
            gridSquare.GetComponent<ObjectInfo>().SetObjectID(ID);
            gridSquare.GetComponent<ObjectInfo>().SetObjectType(ObjectInfo.ObjectType.EMPTY);
            gridSquares.Add((GameObject)Instantiate(gridSquare, pos, Quaternion.identity));
        }
    }

    //creates individual tiles, setting ID and types
    public void LoadGrid(int[] ID, Vector3[] pos, string[] type, int[] lvl, string[] fill)
    {
        ObjectFill.FillType[] gridFill;
        ObjectInfo.ObjectType[] gridType;

        gridFill = new ObjectFill.FillType[25];
        gridType = new ObjectInfo.ObjectType[25];

        AssetChange gridLoad = gameObject.GetComponent<AssetChange>();

        //creates and instantiates each tile, giving them a unique ID
        for (int i = 0; i < columnLength * rowLength; i++)
        { 
            gridFill[i] = (ObjectFill.FillType)System.Enum.Parse(typeof(ObjectFill.FillType), fill[i]);
            gridType[i] = (ObjectInfo.ObjectType)System.Enum.Parse(typeof(ObjectInfo.ObjectType), type[i]);

            //Checks if the farmhouse is being upgraded and if so what level the farmhouse is being upgraded to
            if (gridType[i] == ObjectInfo.ObjectType.FARMHOUSE && lvl[i] < 2 && !lockLvl2)
            {
                lockLvl2 = Instantiate(Resources.Load("Locked_lvl2"), new Vector3(76.64f, 12.0f, -14.213f), Quaternion.identity);
            }

            if (gridType[i] == ObjectInfo.ObjectType.FARMHOUSE && lvl[i] < 3 && !lockLvl3)
            {
                lockLvl3 = Instantiate(Resources.Load("Locked_lvl3"), new Vector3(-11.89f, 12.0f, 87.5f), new Quaternion(0.0f, 0.7071f, 0.0f, 0.7071f));
            }

            gridLoad.ChangeAsset(ID[i], lvl[i], gridType[i], gridFill[i]);
        }
    }
}