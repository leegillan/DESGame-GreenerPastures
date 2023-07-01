/*
Asset change function

Lee Gillan, David Ireland
16/01/2020 
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetChange : MonoBehaviour
{
    //Declare variables
    public GameObject gameManager;
    GameObject newAsset;
    GameObject newFill;

    public void Upgrade(int id) //upgrade level and then change asset
    {
       // int oldID;
        GameObject asset;
        asset = gameManager.GetComponent<GridScript>().GetGridTile(id);

        //get level and upgrade
        int level = asset.GetComponent<ObjectInfo>().GetObjectLevel() + 1;

        //get type
        ObjectInfo.ObjectType type = asset.GetComponent<ObjectInfo>().GetObjectType();

        //set fill
        ObjectFill.FillType fill;

        //checks whatr type
        if (type == ObjectInfo.ObjectType.FIELD || type == ObjectInfo.ObjectType.CHICKEN_COOP || type == ObjectInfo.ObjectType.COW_FIELD || type == ObjectInfo.ObjectType.PIG_PEN)
        {
            //fill type assosiated so has then check what fill it is
            fill = asset.GetComponent<ObjectFill>().GetFillType();
        }
        else
        {
            fill = ObjectFill.FillType.NONE;
        }

        //pass change asset the required data to change the asset
        ChangeAsset(id, level, type, fill);
    }

    public void Demolish(int id) //change to empty
    { 
        //same id, level 0, typee empty, no fill
        ChangeAsset(id, 0, ObjectInfo.ObjectType.EMPTY, 0);
    }

    public void Build(int id, ObjectInfo.ObjectType type, ObjectFill.FillType fill)//build new tile
    {
        //same id, level 1, type, fill
        ChangeAsset(id, 1, type, fill);
    }


    public GameObject LoadAsset(ObjectInfo.ObjectType type, Transform transform, int level)//load asset based on type
    { 
        string lvlExtension;

        if(level == 2)//finds correct extension for name of resource
        {
            lvlExtension = "_lvl2";
        }
        else if (level == 3)
        {
            lvlExtension = "_lvl3";
        }
        else
        {
            lvlExtension = ""; //no extension needed.
        }

        //load using name and extension, extension being _lvl2 or _lvl3
        switch (type)
        {
            case ObjectInfo.ObjectType.EMPTY:
                return (GameObject)Instantiate(Resources.Load("Empty"), transform.position, Quaternion.identity);

            case ObjectInfo.ObjectType.BARN:
                return (GameObject)Instantiate(Resources.Load("Barn" + lvlExtension), transform.position, Quaternion.identity);

            case ObjectInfo.ObjectType.FARMHOUSE:
                return (GameObject)Instantiate(Resources.Load("Farmhouse" + lvlExtension), transform.position, Quaternion.identity);

            case ObjectInfo.ObjectType.FIELD:
                return (GameObject)Instantiate(Resources.Load("Field" + lvlExtension), transform.position, Quaternion.identity);

            case ObjectInfo.ObjectType.RICE:
                return (GameObject)Instantiate(Resources.Load("RiceField" + lvlExtension), transform.position, Quaternion.identity);

            case ObjectInfo.ObjectType.CHICKEN_COOP:
                return (GameObject)Instantiate(Resources.Load("ChickenCoop" + lvlExtension), transform.position, Quaternion.identity);

            case ObjectInfo.ObjectType.COW_FIELD:
                return (GameObject)Instantiate(Resources.Load("CowField" + lvlExtension), transform.position, Quaternion.identity);

            case ObjectInfo.ObjectType.PIG_PEN:
                return (GameObject)Instantiate(Resources.Load("PigField" + lvlExtension), transform.position, Quaternion.identity);

            case ObjectInfo.ObjectType.RESEARCH:
                return (GameObject)Instantiate(Resources.Load("ResearchLab" + lvlExtension), transform.position, Quaternion.identity);

            case ObjectInfo.ObjectType.VERTICAL_FARM:
                return (GameObject)Instantiate(Resources.Load("VerticalFarm" + lvlExtension), transform.position, Quaternion.identity);

            case ObjectInfo.ObjectType.MEAT_LAB:
                return (GameObject)Instantiate(Resources.Load("MeatLab" + lvlExtension), transform.position, Quaternion.identity);

            case ObjectInfo.ObjectType.BATTERY:
                return (GameObject)Instantiate(Resources.Load("BatteryFarm" + lvlExtension), transform.position, Quaternion.identity);

            default:
                return null;
        }    
    }

    //code to unload old and load new asset
    public void ChangeAsset(int id, int level, ObjectInfo.ObjectType type, ObjectFill.FillType fill)
    {
        //find asset and keep transform
        GameObject asset;
        asset = gameManager.GetComponent<GridScript>().GetGridTile(id);

        //store transform
        Transform transform = asset.transform;
        Object locked;

        Animator anim;

        //load correct asset based on type
        newAsset = LoadAsset(type, transform, level);

        //Checks if the farmhouse is being upgraded and if so what level the farmhouse is being upgraded to
        if (type == ObjectInfo.ObjectType.FARMHOUSE && level >= 2)
        {
            locked = gameManager.GetComponent<GridScript>().lockLvl2;
            DestroyImmediate(locked);
        }
        if (type == ObjectInfo.ObjectType.FARMHOUSE && level == 3)
        {
            locked = gameManager.GetComponent<GridScript>().lockLvl3;
            Destroy(locked);
        }

        //Checks if the object being changed is a field or not to set the fill of the grid
        if (fill != ObjectFill.FillType.NONE)
        {
            //load correct fill based on whats chosen 
            newFill = LoadFill(fill, transform, level);
        }

        if (asset.TryGetComponent<Animator>(out anim))
        {
            anim.Play("DestroyAnim");
            StartCoroutine(UpdateClipLength(anim, asset));
        }
        else
        {
            //remove from list
            gameManager.GetComponent<GridScript>().RemoveGridTile(asset);

            //Destroy shape to be replaced
            GameObject.Destroy(asset);
        }

        //set objectID and level and fill
        newAsset.GetComponent<ObjectInfo>().SetObjectID(id);
        newAsset.GetComponent<ObjectInfo>().SetObjectLevel(level);
        newAsset.GetComponent<ObjectFill>().SetFillType(fill);

        //Add from list
        gameManager.GetComponent<GridScript>().AddGridTile(newAsset);

        if (newAsset.TryGetComponent<Animator>(out anim))
        {
            anim.Play("BuildAnim");
        }

        //sust has changed so update.
        gameManager.GetComponent<SustainabilityScript>().CheckPollution();
    }

    IEnumerator DestroyAnimation(Animator anim, GameObject asset)
    {
        Destroy(asset.GetComponent<BoxCollider>());
        
        float counter = 0;
        float waitTime = anim.GetCurrentAnimatorStateInfo(0).length;

        //Now, Wait until the current state is done playing
        while (counter < (waitTime))
        {
            counter += Time.deltaTime;
            yield return null;
        }

        //remove from list
        gameManager.GetComponent<GridScript>().RemoveGridTile(asset);

        //Destroy shape to be replaced
        GameObject.Destroy(asset);
    }

    private IEnumerator UpdateClipLength(Animator anim, GameObject asset)
    {
        bool newFrame = false;

        while (!newFrame)
        {
            newFrame = true;
            yield return new WaitForEndOfFrame();
        }

        print("current clip length = " + anim.GetCurrentAnimatorStateInfo(0).length);

        StartCoroutine(DestroyAnimation(anim, asset));
    }

    GameObject LoadFill(ObjectFill.FillType fill, Transform transform, int level)
    {
        //load fill based on name
        switch (fill)
        {
            case ObjectFill.FillType.NONE:
                return (GameObject)Instantiate(Resources.Load("Sphere"), new Vector3(100.0f, 1000.0f, 0.0f), Quaternion.identity, newAsset.transform);

            case ObjectFill.FillType.WHEAT:
                return (GameObject)Instantiate(Resources.Load("Wheat"), transform.position, Quaternion.identity, newAsset.transform);

            case ObjectFill.FillType.CORN:
                return (GameObject)Instantiate(Resources.Load("Corn"), transform.position, Quaternion.identity, newAsset.transform);

            case ObjectFill.FillType.CARROT:
                return (GameObject)Instantiate(Resources.Load("Carrots"), transform.position, Quaternion.identity, newAsset.transform);

            case ObjectFill.FillType.POTATO:
                return (GameObject)Instantiate(Resources.Load("Cabbages"), transform.position, Quaternion.identity, newAsset.transform);

            case ObjectFill.FillType.TURNIP:
                return (GameObject)Instantiate(Resources.Load("Turnips"), transform.position, Quaternion.identity, newAsset.transform);

            //position of animals based on level
            case ObjectFill.FillType.COW:

                if (level == 3)
                { 
                    Instantiate(Resources.Load("Cow"), new Vector3(transform.position.x + -1.52f, transform.position.y, transform.position.z + 0.93f), new Quaternion(0.0f, 0.990f, 0.0f, 0.138f), newAsset.transform);
                }

                if (level >= 2)
                {
                    Instantiate(Resources.Load("Cow"), new Vector3(transform.position.x - 2.86f, transform.position.y, transform.position.z + -1.41f), new Quaternion(0.0f, 0.335f, 0.0f, 0.942f), newAsset.transform);
                }

                return (GameObject)Instantiate(Resources.Load("Cow"), new Vector3(transform.position.x + 1.93f, transform.position.y, transform.position.z - 1.33f), new Quaternion(0.0f, -0.827f, 0.0f, 0.563f), newAsset.transform);
                
            case ObjectFill.FillType.PIG:

                if (level == 3)
                {
                    Instantiate(Resources.Load("Pig"), new Vector3(transform.position.x - 5.75f, transform.position.y, transform.position.z + 2.38f), new Quaternion(0.0f, -0.827f, 0.0f, 0.563f), newAsset.transform);
                }

                if (level >= 2)
                {
                    Instantiate(Resources.Load("Pig"), new Vector3(transform.position.x + 2.44f, transform.position.y, transform.position.z - 1.97f), new Quaternion(0.0f, 0.294f, 0.0f, 0.956f), newAsset.transform);
                }

                return (GameObject)Instantiate(Resources.Load("Pig"), new Vector3(transform.position.x - 2.19f, transform.position.y, transform.position.z + 1.24f), new Quaternion(0.0f, 0.999f, 0.0f, 0.041f), newAsset.transform);

            case ObjectFill.FillType.CHICKEN:

                if (level == 3)
                {
                    Instantiate(Resources.Load("Chicken"), new Vector3(transform.position.x + -4.5f, transform.position.y, transform.position.z + 0.02f), new Quaternion(0.0f, 0.287f, 0.0f, 0.958f), newAsset.transform);
                }

                if (level >= 2)
                {
                    Instantiate(Resources.Load("Chicken"), new Vector3(transform.position.x + 0.12f, transform.position.y, transform.position.z + 1.88f), new Quaternion(0.0f, 0.988f, 0.0f, -0.157f), newAsset.transform);
                }

                return (GameObject)Instantiate(Resources.Load("Chicken"), new Vector3(transform.position.x + 2.818f, transform.position.y, transform.position.z - 1.299f), new Quaternion(0.0f, -0.625f, 0.0f, 0.780f), newAsset.transform);

            case ObjectFill.FillType.SUNFLOWER:
                return (GameObject)Instantiate(Resources.Load("Sunflower"), transform.position, Quaternion.identity, newAsset.transform);
               
            case ObjectFill.FillType.SUGARCANE:
                return (GameObject)Instantiate(Resources.Load("Sugarcane"), transform.position, Quaternion.identity, newAsset.transform);

            case ObjectFill.FillType.COCCOA:
                return (GameObject)Instantiate(Resources.Load("Cocoa"), transform.position, Quaternion.identity, newAsset.transform);

            default:
                return null;
        }
    }
}