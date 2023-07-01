//https://github.com/GibsS/unity-pan-and-zoom
//Adapted from this script

using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Collections;

public class InputScript : MonoBehaviour
{
    //Instance of script
    public static InputScript instance;

    //public variabel to be set in inspector
    public GameObject gameManager;
    public bool controlType;//true for mobile, false for pc

    //Actions that the touch controls can make using event systems
    /// <summary> Called as soon as the player touches the screen. The argument is the screen position. </summary>
    public event Action<Vector2> onStartTouch;

    /// <summary> Called as soon as the player stops touching the screen. The argument is the screen position. </summary>
    public event Action<Vector2> onEndTouch;

    /// <summary> Called if the player completed a quick tap motion. The argument is the screen position. </summary>
    public event Action<Vector2> onTap;

    /// <summary> Called if the player swiped the screen. The argument is the screen movement delta. </summary>
    public event Action<Vector2> onSwipe;

    /// <summary> Called if the player pinched the screen. The arguments are the distance between the fingers before and after. </summary>
    public event Action<float, float> onPinch;

    /// <summary> Has the player at least one finger on the screen? </summary>
    public bool isTouching { get; private set; }

    /// <summary> The point of contact if it exists in Screen space. </summary>
    public Vector2 touchPosition { get { return touch0LastPosition; } }

    //Touch Variables
    //
    Vector3 touchStart;

        //Public so it can be checked in the radial script
    public float touch0StartTime;
    Vector2 touch0StartPosition;
    Vector2 touch0LastPosition;

        //Min/Max's for taps
    public float maxDistanceForTap;
    public float maxDurationForTap;

    //Variables used in script
    bool newRadial;
    bool newMarket;
    bool canMove;

        //Selection variables
    public int selectedID;
    public bool selecting = true;

    //Camera variables
    CameraScript cameraMovement;

        //Min/Max camera zoom
    public float zoomOutMin = 5;
    public float zoomOutMax = 17;

    //Allow Selecting
    public void AllowSelecting() { selecting = true;}

    //getters
    public bool GetAllowSelecting() { return selecting; }

    public void SetAllowSelecting(bool s) { selecting = s; }

    public void AllowSelectingAfterTime()
    {
        StartCoroutine(WaitToSelect());
    }
    public IEnumerator WaitToSelect()
    {
        float counter = 0;
        float waitTime = 0.5f;

        //Now, Wait until the current state is done playing
        while (counter < (waitTime))
        {
            counter += Time.deltaTime;
            yield return null;
        }

        //allow selecting again
        AllowSelecting();
    }

    public void SetCanMove(bool c) { canMove = c; }
    public bool GetCanMove() { return canMove; }
    public bool GetControlType() { return controlType; }
    public bool GetNewMarketplaceMenu() { return newMarket; }
    public bool GetNewRadialMenu() { return newRadial; }
    public int GetSelectedID() { return selectedID; }

    //setters
    public void SetNewMarketplaceMenu(bool m) { newMarket = m; }
    public void SetNewRadialMenu(bool r) { newRadial = r; }

    // Start is called before the first frame update
    private void Start()
    {
        //sets instance
        instance = this;

        //sets camera 
        cameraMovement = Camera.main.GetComponent<CameraScript>();

        //can move camera 
        canMove = true;
    }

    //get Input to be called in main game loop
    public void GetInput()
    {
        //checks control type
        if (controlType == false)
        {
            //checks if mouse button has been pressed
            if (Input.GetMouseButtonDown(0))
            {
                //calls select function to create a ray from camera to pointer location
                Select(Input.mousePosition);
            }

            //checks if player can move camera
            if (canMove == true)
            {
                //if player is poressing W, A, S, or D then the camera moves depending on function call
                if (Input.GetKey("w") && Camera.main.transform.position.z <= 41 && Camera.main.transform.position.x <= 58)
                {
                    cameraMovement.MoveUp(Time.deltaTime * 10.0f);
                }
                if (Input.GetKey("s") && Camera.main.transform.position.z >= -15 && Camera.main.transform.position.x >= -16)
                {
                    cameraMovement.MoveDown(Time.deltaTime * 10.0f);
                }
                if (Input.GetKey("d") && Camera.main.transform.position.z >= -15 && Camera.main.transform.position.x <= 58)
                {
                    cameraMovement.MoveLeft(Time.deltaTime * 10.0f);
                }
                if (Input.GetKey("a") && Camera.main.transform.position.z <= 41 && Camera.main.transform.position.x >= -16)
                {
                    cameraMovement.MoveRight(Time.deltaTime * 10.0f);
                }
            }

            //if 'i' or 'o' is pressed then the cameras orhtographic size is increased or decreased acting as a zoom
            if (Input.GetKey("i") && Camera.main.orthographicSize >= 5.0f)
            {
                Camera.main.orthographicSize -= .1f;
            }

            if (Input.GetKey("o") && Camera.main.orthographicSize <= 15.0f)
            {
                Camera.main.orthographicSize += .1f;
            }
        }
        else //control type is for mobile then call touch controls
        {
            UpdateWithTouch();
        }
    }

    //Touch controls
    void UpdateWithTouch()
    {
        //touch count to check how many fingers are on the screen
        int touchCount = Input.touches.Length;

        //if one finger is touching
        if (touchCount == 1)
        {
            //set new touch variable
            Touch touch = Input.touches[0];

            //checks which phase the touch controls are in 
            switch (touch.phase)
            {
                //once the player has touched the screen this is called to get start position and time and last position
                case TouchPhase.Began:
                {
                    //Gets starting touch, time and las touch position
                    touch0StartPosition = touch.position;
                    touch0StartTime = Time.time;
                    touch0LastPosition = touch0StartPosition;

                    //sets touching to true
                    isTouching = true;

                    //sets start touch to touch position
                    if (onStartTouch != null) onStartTouch(touch0StartPosition);

                    break;
                }
                //once the player has moved their finger on the screen whilst touching it then the moved phase is active
                case TouchPhase.Moved:
                {
                    //updates the last position touched
                    touch0LastPosition = touch.position;

                    //checks if last changed position is the same as last time and if not then calls swiping function
                    if (touch.deltaPosition != Vector2.zero && isTouching)
                    {
                        //passes last changed position to move camera to
                        OnSwipe(touch.deltaPosition);
                    }

                    break;
                }
                //once the player has removed their finger from the screen then the ended phase is active
                case TouchPhase.Ended:
                {
                    //checks if the time the touch took from touch to remove, is smaller than the max amount of time for tap to be considered. Same for the distance 
                    if (Time.time - touch0StartTime <= maxDurationForTap && Vector2.Distance(touch.position, touch0StartPosition) <= maxDistanceForTap && isTouching)
                    {
                        //if so then the click function is called to run the selected function that fires a ray from the screen to the touched position
                        OnClick(touch.position);
                    }

                    //sets end touch to touch position
                    if (onEndTouch != null) onEndTouch(touch.position);

                    //sets touching to false
                    isTouching = false;

                    break;
                }
            }
        }
        else if (touchCount == 2)//if 2 fingers are on the screen
        { //2 touch vraiables are set to each finger
            Touch touch0 = Input.touches[0];
            Touch touch1 = Input.touches[1];

            //checks if either finger is lifted off screen to return out of the function and not go ahead with the rest of the code
            if (touch0.phase == TouchPhase.Ended || touch1.phase == TouchPhase.Ended) return;

            //sets touching to true
            isTouching = true;

            //gets previous distance of the two fingers on screen
            float previousDistance = Vector2.Distance(touch0.position - touch0.deltaPosition, touch1.position - touch1.deltaPosition);

            //gets current distance of the two fingers on screen
            float currentDistance = Vector2.Distance(touch0.position, touch1.position);

            //if previous is no the same as current then call the pinch function
            if (previousDistance != currentDistance)
            {
                OnPinch((touch0.position + touch1.position) / 2, previousDistance, currentDistance, (touch1.position - touch0.position).normalized);
            }
        }
        else
        {
            //if touching is true then update last touch
            if (isTouching)
            {
                if (onEndTouch != null) onEndTouch(touch0LastPosition);

                //touching set to false
                isTouching = false;
            }
        }
    }

    void OnClick(Vector2 position)
    {
        //sets position if null
        if (onTap != null)
        {
            onTap(position);
        }

        //runs select function and passes through the touch position
        Select(position);
    }

    void OnSwipe(Vector2 deltaPosition)
    {
        //sets up camera clamp variables
        float XClamp, ZClamp;

        //if null sets position
        if (onSwipe != null)
        {
            onSwipe(deltaPosition);
        }

        //sets new x and z values using delta position of touch position
        float newX = Camera.main.ScreenToWorldPoint(deltaPosition).x - Camera.main.ScreenToWorldPoint(Vector2.zero).x;
        float newZ = Camera.main.ScreenToWorldPoint(deltaPosition).y - Camera.main.ScreenToWorldPoint(Vector2.zero).y;

        
        //checks if selecting is true and if the player can move to allow swiping
        if(selecting == true && canMove == true)
        {
            //checks boundaries of camera
            if (Camera.main.transform.position.x <= 58 && Camera.main.transform.position.x >= -15 || Camera.main.transform.position.z <= 41 && Camera.main.transform.position.z >= -16)
            {
                //moves camera 
                Camera.main.transform.position -= new Vector3(newX, 0.0f, newZ);

                //sets position of camera to clamp the camera into boundaries
                XClamp = Camera.main.transform.position.x;
                ZClamp = Camera.main.transform.position.z;
                XClamp = Mathf.Clamp(XClamp, -15, 58);
                ZClamp = Mathf.Clamp(ZClamp, -16, 41);

                //keeps y-position of the camera to 18
                Camera.main.transform.position = new Vector3(XClamp, 18.0f, ZClamp);
            }
        }
    }

    void OnPinch(Vector2 center, float oldDistance, float newDistance, Vector2 touchDelta)
    {
        //sets pinch values
        if (onPinch != null)
        {
            onPinch(oldDistance, newDistance);
        }
        
        //checks if selecting is true
        if (selecting == true)
        {
            //checks if the camera is in orthographic
            if (Camera.main.orthographic)
            {
                //sets centre of the two touch positions
                var currentPinchPosition = Camera.main.ScreenToWorldPoint(center);

                //changes orthographic size of the camera depending of distance between touch positions
                Camera.main.orthographicSize = Mathf.Max(0.1f, Camera.main.orthographicSize * oldDistance / newDistance);
                Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, zoomOutMin, zoomOutMax);

                //sets new position of the pinch
                var newPinchPosition = Camera.main.ScreenToWorldPoint(center);

                //changes position of the camera to put middle of the pinch as the position
                Camera.main.transform.position -= new Vector3(newPinchPosition.x - currentPinchPosition.x, 0.0f, newPinchPosition.y - currentPinchPosition.y);
            }
        }
    }

    //Finds what object is being selected
    void Select(Vector2 pos)
    {
        //Declare variables
        RaycastHit hit;

        //casts a ray from camera to mouse position/tap
        Ray ray = Camera.main.ScreenPointToRay(pos);

        //checks to see if position of click/touch is over any UI
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        //checks if selecting is true
        if (selecting == true)
        {
            //Checks if the ray connects with an object/asset
            if (Physics.Raycast(ray, out hit))
            {
                //if raycast interacts with a game object set selected ID
                selectedID = hit.collider.gameObject.GetComponent<ObjectInfo>().GetObjectID(); //object we are clicking's ID

                //checks what type of tile is selected
                if (hit.collider.gameObject.GetComponent<ObjectInfo>().GetObjectType() == ObjectInfo.ObjectType.EMPTY)
                {
                    gameManager.GetComponent<MarketplaceSpawner>().SpawnMenu();
                    selecting = false;
                }
                else if (hit.collider.gameObject.GetComponent<ObjectInfo>().GetObjectType() != ObjectInfo.ObjectType.EMPTY )
                {
                    //if the object has a radial pressable component
                    if (hit.collider.gameObject.TryGetComponent(out RadialPressable radial))
                    {
                        //triggers menu function to open a new raial
                        radial.TriggerMenu();

                        //sets selected tile
                        gameManager.GetComponent<GameLoop>().SetSelectedTile(hit.collider.gameObject);

                        //sets boolean values
                        newRadial = true;
                        selecting = false;
                    }
                    else
                    {
                        //returns out of function
                        return;
                    }
                }
            }
        }
    }

    public void AttemptBuild(ObjectInfo.ObjectType t, ObjectFill.FillType f)
    {
        //Gets data of target
        GameObject target = gameManager.GetComponent<GridScript>().GetGridTile(selectedID); //get Target
        ObjectData targetData = gameManager.GetComponent<GameInfo>().GetTypeInfo(t, f); //get data relating to target

        //check that user has enough money and that object is empty
        if (gameManager.GetComponent<Currency>().GetMoney() >= targetData.purchaseCost && target.GetComponent<ObjectInfo>().GetObjectType() == ObjectInfo.ObjectType.EMPTY)
        {
            //takes away cost from players money
            gameManager.GetComponent<Currency>().AddMoney(-targetData.purchaseCost);

            //builds type and fill at the selectedID location
            gameManager.GetComponent<AssetChange>().Build(selectedID, t, f);
        }
        else
        {
            //shows warning message about money
            gameManager.GetComponent<GameLoop>().GetMoneyWarning().gameObject.SetActive(true);
            gameManager.GetComponent<InputScript>().SetAllowSelecting(false);
        }
    }

    public void AttemptUpgrade(int id)
    {
        //Gets data of target
        GameObject target = gameManager.GetComponent<GridScript>().GetGridTile(id); //get Target
        ObjectData targetData = gameManager.GetComponent<GameInfo>().GetTypeInfo(target.GetComponent<ObjectInfo>().GetObjectType(), target.GetComponent<ObjectFill>().GetFillType()); //get data relating to target

        //sets up value to be cahnged to cost
        int levelCost;

        //get level upgrade cost
        if (target.GetComponent<ObjectInfo>().GetObjectLevel() == 1)
        {
            levelCost = targetData.level2Cost;
        }
        else
        {
            levelCost = targetData.level3Cost;
        }

        //Check that have enough money and that maxLevel of asset has not been reached
        if (gameManager.GetComponent<Currency>().GetMoney() >= levelCost && target.GetComponent<ObjectInfo>().GetObjectLevel() != targetData.levels)
        {
            //takes away cost from players money
            gameManager.GetComponent<Currency>().AddMoney(-levelCost);

            //upgrade selected tile
            gameManager.GetComponent<AssetChange>().Upgrade(id);
        }
        else
        {
            //shows warning message about money
            gameManager.GetComponent<GameLoop>().GetMoneyWarning().gameObject.SetActive(true);
            gameManager.GetComponent<InputScript>().SetAllowSelecting(false);
        }
    }

    public void AttemptDemolish(int id)
    {
        //demolishes the selected tile
        gameManager.GetComponent<AssetChange>().Demolish(id);
    }


}