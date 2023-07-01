using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFill : MonoBehaviour
{
    public FillType fillType;

    // Start is called before the first frame update
    public enum FillType
    {
        NONE = 0,
        WHEAT = 1,
        CORN = 2,
        CARROT = 3,
        POTATO = 4,
        TURNIP = 5,
        COW = 6,
        PIG = 7,
        CHICKEN = 8,
        SUNFLOWER = 9,
        SUGARCANE = 10,
        COCCOA = 11
    };


    //getters and setters for object
    public FillType GetFillType() { return fillType; }

    public void SetFillType(FillType t) { fillType = t; }
}
