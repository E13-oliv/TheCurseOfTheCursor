using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ViewCubeManager : MonoBehaviour
{
    // Movements manager
    [Header("Movements and World Managers")]
    [SerializeField]
    private MovementsManager MM;
    [SerializeField]
    private WorldManager WM;

    [Header("Player")]
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private Camera playerCamera;

    [Header("UI")]
    [SerializeField]
    private GameObject NorthUIObjects;
    [SerializeField]
    private GameObject EastUIObjects;
    [SerializeField]
    private GameObject SouthUIObjects;
    [SerializeField]
    private GameObject WestUIObjects;
    [SerializeField]
    private GameObject UI_Canvas;

    [Header("Cube Images")]
    [SerializeField]
    private GameObject[] underPlayerFloorImages;
    [SerializeField]
    private GameObject[] floorImages;
    [SerializeField]
    private GameObject[] skyImages;

    private Sprite sprite;
    private GameObject prefab;

    private string colorMode;

    private float viewDistance = 30;

    private float canvasWidth;

    private Vector3 objectPosition;
    private Vector3 objectScale;

    private int caseNumber;
    private float caseSize;

    private float level0Y = -233f;
    private float level1Y = -94f;
    private float level2Y = 20f;

    private float level0Scale = 1f;
    private float level1Scale = .50f;
    private float level2Scale = .25f;

    private float level0XMax = 350f;
    private float level1XMax = 400f;
    private float level2XMax = 450f;

    private float levelXMax;

    private float floorColorsAlpha = 1f;

    // 0 : water (grey)
    // 1 : others (white)
    private Color[] floorColorsBlackAndWhite = new Color[] {
            new Color(.5f, .5f, .5f, 1),
            new Color(1f, 1f, 1f, 1),
            new Color(.25f, .25f, .25f, 1)
        };
    private Color skyColorBalckANdWhite = new Color(.25f, .25f, .25f, 1);

    // 0 : water (blue)
    // 1 : grass (green)
    // 2 : hole in grass (darkgreen)
    // 3 : red rock (red)
    // 4 : sand (light yeloow)
    // 5 : brown rock (brown)
    // 6 : blue swamp (blue)
    // 7 : onyx (black)
    // 8 : salt (white)
    // 9 : stone bridges (gray)
    private Color[] floorColors16 = new Color[] {
            new Color(0, 0, 1, 1),
            new Color(.25f, 1, 0, 1),
            new Color(.3f, .3f, .3f, 1),
            new Color(1, 0, 0, 1),
            new Color(1, 1, 0, 1),
            new Color(1, .5f, 0, 1),
            new Color(0, .5f, 1, 1),
            new Color(0, 0, 0, 1),
            new Color(1, 1, 1, 1),
            new Color(.66f, .66f, .66f, 1)
        };
    private Color skyColor16 = new Color(0, 1, 1, 1);

    private Color[] floorColors256 = new Color[] {
            new Color(.2f, .2f, 1, 1),
            new Color(.2f, .6f, 0, 1),
            new Color(0, .4f, 0, 1),
            new Color(.6f, .2f, 0, 1),
            new Color(1, .8f, .4f, 1),
            new Color(.6f, .4f, .2f, 1),
            new Color(0, .2f, .8f, 1),
            new Color(0, 0, 0, 1),
            new Color(1, 1, 1, 1),
            new Color(.6f, .6f, .6f, 1)
        };
    private Color skyColor256 = new Color(0, 1, 1, 1);

    private Color[] floorColorsFull = new Color[] {
            new Color(.15f, .2f, 1, 1),
            new Color(.16f, .6f, .15f, 1),
            new Color(0, .34f, 0, 1),
            new Color(.7f, .4f, .25f, 1),
            new Color(.9f, .85f, .65f, 1),
            new Color(.5f, .4f, .1f, 1),
            new Color(0, .12f, .86f, 1),
            new Color(0, 0, 0, 1),
            new Color(1, 1, 1, 1),
            new Color(.63f, .61f, .61f, 1)
        };
    private Color skyColorFull = new Color(.2f, .5f, 1, 1);

    private float darkenGap = .33f;

    private void OnEnable()
    {
        // get active colorMode
        colorMode = WM.GetColorsMode();

        canvasWidth = GetComponent<RectTransform>().rect.width;
        caseSize = MM.GetCaseSIze();

        for (int i = 0; i < floorColorsFull.Length; i++)
        {
            floorColorsFull[i].a = floorColorsAlpha;
        }

        viewDistance = Mathf.Sqrt(Mathf.Pow(caseSize, 2) * 2) * 2; // 28.28 is caseSize = 10
    }

    private void Update()
    {
        // get active colorMode
        colorMode = WM.GetColorsMode();

        // to keep the globe around the player
        transform.position = new Vector3(player.transform.position.x, 0, player.transform.position.z + 10);

        caseNumber = MM.GetCaseNumber();

        int caseType;
        int cardinalPoint;
        int distance;
        int gap;

        // apply color under the player
        int playerCaseType = MM.GetPlayerCaseType();

        for (int i = 0; i < underPlayerFloorImages.Length; i++)
        {
            ApplyColor(underPlayerFloorImages[i], playerCaseType);
        }

        // apply color to the sky
        for (int i = 0; i < skyImages.Length; i++)
        {
            ApplySkyColor(skyImages[i], playerCaseType);
        }

        // get case type and apply color to distant cases display
        for (int i = 0; i < floorImages.Length; i++)
        {
            cardinalPoint = floorImages[i].GetComponent<OverlayFloorImages>().cardinalPoint;
            distance = floorImages[i].GetComponent<OverlayFloorImages>().distance;
            gap = floorImages[i].GetComponent<OverlayFloorImages>().rightGap;

            caseType = MM.GetCaseType(cardinalPoint, caseNumber, distance, gap);

            ApplyColor(floorImages[i], caseType);
        }
    }

    private void LateUpdate()
    {
        // count and sort (on z axis) all UIObjects in an array
        GameObject[] UIObjects = GameObject.FindGameObjectsWithTag("UIObjects");
        GameObject[] UIObjectsOrdered = UIObjects.OrderByDescending(go => go.transform.localPosition.z).ToArray();

        // change the sibling index according to previous sort
        for (int i = 0; i < UIObjectsOrdered.Length; i++)
        {
            UIObjectsOrdered[i].transform.SetSiblingIndex(i*10);
        }
    }

    public GameObject BecameVisible(GameObject sceneObject)
    {
        float angle = GetAngle(sceneObject);
        int objectCardinalPoint = GetObjectCardinalPoint(angle);

        GameObject UIObjects;

        // if animation
        bool isAnimated = sceneObject.GetComponent<IsVisibleCube>().GetIsAnimated();

        string prefabName;

        if (isAnimated == true)
        {
            prefabName = sceneObject.GetComponent<IsVisibleCube>().GetAnimationPrefab().name;
        }
        else
        {
            prefabName = "UIObject";
        }

        switch (objectCardinalPoint)
        {
            case 1:
                UIObjects = EastUIObjects;
                break;
            case 2:
                UIObjects = SouthUIObjects;
                break;
            case 3:
                UIObjects = WestUIObjects;
                break;
            // north
            default:
                UIObjects = NorthUIObjects;
                break;
        }

        // set UI object position and scale
        SetObjectPositionAndScale(sceneObject);

        // get the prefab
        prefab = Resources.Load<GameObject>("Prefabs/" + prefabName);

        // instantiate, scale and position the UI object
        GameObject UIObject = Instantiate(prefab, objectPosition, Quaternion.Euler(0, 0, 0));

        UIObject.name = sceneObject.name;
        UIObject.transform.SetParent(UIObjects.transform);
        UIObject.transform.localScale = objectScale;
        UIObject.transform.localRotation = Quaternion.Euler(0, 0, 0);

        string colorsFolder;

        switch (colorMode)
        {
            case "BlackAndWhite":
                colorsFolder = "BlackAndWhite/";
                break;
            case "Colors16":
                colorsFolder = "Colors16/";
                break;
            case "Colors256":
                colorsFolder = "Colors256/";
                break;
            // case "ColorsFull"
            default:
                colorsFolder = "ColorsFull/";
                break;
        }

        // get the right sprite
        sprite = Resources.Load<Sprite>("Images/" + colorsFolder + sceneObject.name);
        UIObject.GetComponent<Image>().sprite = sprite;

        if (isAnimated == true)
        {
            sceneObject.GetComponent<Cutscenes>().SetFlat2DObject(UIObject);
        }

        return UIObject;
    }

    public void StillVisible(GameObject sceneObject, GameObject UIObject)
    {
        float angle = GetAngle(sceneObject);
        int objectCardinalPoint = GetObjectCardinalPoint(angle);

        GameObject UIObjects;

        switch (objectCardinalPoint)
        {
            case 1:
                UIObjects = EastUIObjects;
                break;
            case 2:
                UIObjects = SouthUIObjects;
                break;
            case 3:
                UIObjects = WestUIObjects;
                break;
            // case 4 (north)
            default:
                UIObjects = NorthUIObjects;
                break;
        }

        // set UI object position and scale
        SetObjectPositionAndScale(sceneObject);

        SetObjectParent(UIObject, UIObjects);

        UIObject.transform.localScale = objectScale;
        UIObject.transform.localPosition = objectPosition;
        UIObject.transform.localRotation = Quaternion.Euler(0 ,0 ,0);
    }

    public void BecameInvisible(GameObject UIObject)
    {
        // destroy the UI object
        GameObject.Destroy(UIObject);
    }

    private float GetAngle(GameObject sceneObject)
    {
        float adjacent = sceneObject.transform.position.z - player.transform.position.z;
        float opposite = sceneObject.transform.position.x - player.transform.position.x;

        float angle = Mathf.Atan2(opposite, adjacent) * Mathf.Rad2Deg;

        return angle;
    }

    private float GetDistance(GameObject sceneObject)
    {
        float adjacent = sceneObject.transform.position.z - player.transform.position.z;
        float opposite = sceneObject.transform.position.x - player.transform.position.x;

        float distance = Mathf.Sqrt(Mathf.Pow(adjacent, 2) + Mathf.Pow(opposite, 2));

        return distance;
    }

    private void SetObjectParent(GameObject UIObject, GameObject parentUIObject)
    {
        UIObject.transform.SetParent(parentUIObject.transform);
    }

    private int GetObjectCardinalPoint(float angle)
    {
        int objectCardinalPoint = 0;

        if (angle > -135 && angle < -45 )
        {
            objectCardinalPoint = 3;
        }
        else if (angle >= -45 && angle <= 45)
        {
            objectCardinalPoint = 0;
        }
        else if (angle > 45 && angle < 135)
        {
            objectCardinalPoint = 1;
        }
        else
        {
            objectCardinalPoint = 2;
        }

        return objectCardinalPoint;
    }

    private void SetObjectPositionAndScale(GameObject sceneObject) {
        float distance = GetDistance(sceneObject);

        float angle = GetAngle(sceneObject);

        int objectCardinalPoint = GetObjectCardinalPoint(angle);

        caseSize = 10;

        //float hypCaseMax = caseSize / Mathf.Cos(45);

        float hypAngle = Mathf.Abs(angle);

        if (hypAngle > 90)
        {
            hypAngle = hypAngle - 90;
        }

        if (hypAngle > 45)
        {
            hypAngle = 90 - hypAngle;
        }

        float hypCase = caseSize / Mathf.Cos(hypAngle * Mathf.Deg2Rad);

        float hypHalfCase = (caseSize / 2) / Mathf.Cos(hypAngle * Mathf.Deg2Rad);

        int levelDisplay;

        if (distance < hypHalfCase)
        {
            levelDisplay = 0;
        }
        else
        {
            levelDisplay = Mathf.FloorToInt((distance - hypHalfCase) / hypCase) + 1;
        }

        // to prevent level to bu to high
        levelDisplay = Mathf.Clamp(levelDisplay, 0, 2);

        float posY = 0;
        float scale = 1;

        switch(levelDisplay)
        {
            case 0:
                posY = level0Y;
                scale = level0Scale;
                levelXMax = level0XMax;
                break;
            case 1:
                posY = level1Y;
                scale = level1Scale;
                levelXMax = level1XMax;
                break;
            case 2:
                posY = level2Y;
                scale = level2Scale;
                levelXMax = level2XMax;
                break;
        }

        // position
        //float posX = angle / 180 * canvasWidth / 2;

        switch(objectCardinalPoint)
        {
            case 0:
                angle += 45;
                break;
            case 1:
                angle -= 45;
                break;
            case 2:
                if (angle >= 135)
                {
                    angle -= 135;
                }
                else
                {
                    angle += 225;
                }
                break;
            case 3:
                angle += 135;
                break;
        }

        float posX = Mathf.Lerp(levelXMax * -1, levelXMax, angle / 90);

        objectPosition = new Vector3(posX, posY, distance);

        objectScale = new Vector3(scale, scale, scale);
    }

    private void ApplyColor(GameObject imgGO, int caseType)
    {
        int lastInt = int.Parse(caseType.ToString().Substring(caseType.ToString().Length - 1, 1));

        switch (WM.GetColorsMode())
        {
            case "BlackAndWhite":
                // 1000 -> water mega jump
                if (caseType <= 0 || caseType == 1000)
                {
                    imgGO.GetComponent<Image>().color = floorColorsBlackAndWhite[0];
                }
                else if (Mathf.Abs(caseType) >= 100)
                {
                    imgGO.GetComponent<Image>().color = floorColorsBlackAndWhite[2];
                }
                else
                {
                    imgGO.GetComponent<Image>().color = floorColorsBlackAndWhite[1];
                }
                break;
            case "Colors16":
                if (caseType >= 100)
                {
                    Color darkenColor = new Color(0,0,0,1);

                    darkenColor.r = floorColors16[Mathf.Abs(lastInt)].r - darkenGap;
                    darkenColor.g = floorColors16[Mathf.Abs(lastInt)].g - darkenGap;
                    darkenColor.b = floorColors16[Mathf.Abs(lastInt)].b - darkenGap;

                    imgGO.GetComponent<Image>().color = darkenColor;
                }
                else
                {
                    imgGO.GetComponent<Image>().color = floorColors16[Mathf.Abs(lastInt)];
                }
                break;
            case "Colors256":
                if (caseType >= 100)
                {
                    Color darkenColor = new Color(0, 0, 0, 1);

                    darkenColor.r = floorColors256[Mathf.Abs(lastInt)].r - darkenGap;
                    darkenColor.g = floorColors256[Mathf.Abs(lastInt)].g - darkenGap;
                    darkenColor.b = floorColors256[Mathf.Abs(lastInt)].b - darkenGap;

                    imgGO.GetComponent<Image>().color = darkenColor;
                }
                else
                {
                    imgGO.GetComponent<Image>().color = floorColors256[Mathf.Abs(lastInt)];
                }
                break;
            // case "ColorsFull"
            default:
                if (caseType >= 100)
                {
                    Color darkenColor = new Color(0, 0, 0, 1);

                    darkenColor.r = floorColorsFull[Mathf.Abs(lastInt)].r - darkenGap;
                    darkenColor.g = floorColorsFull[Mathf.Abs(lastInt)].g - darkenGap;
                    darkenColor.b = floorColorsFull[Mathf.Abs(lastInt)].b - darkenGap;

                    imgGO.GetComponent<Image>().color = darkenColor;
                }
                else
                {
                    imgGO.GetComponent<Image>().color = floorColorsFull[Mathf.Abs(lastInt)];
                }
                break;
        }
    }

    private void ApplySkyColor(GameObject imgGO, int caseType)
    {
        switch (WM.GetColorsMode())
        {
            case "BlackAndWhite":
                imgGO.GetComponent<Image>().color = skyColorBalckANdWhite;
                break;
            case "Colors16":
                imgGO.GetComponent<Image>().color = skyColor16;
                break;
            case "Colors256":
                imgGO.GetComponent<Image>().color = skyColor256;
                break;
            // case "ColorsFull"
            default:
                imgGO.GetComponent<Image>().color = skyColorFull;
                break;
        }
    }

    // PUBLIC METHODS
    public Camera GetCamera()
    {
        return playerCamera;
    }

    public GameObject GetPlayer()
    {
        return player;
    }

    public float GetViewDistance()
    {
        return viewDistance;
    }

    public string GetColorsMode()
    {
        return WM.GetColorsMode();
    }
}
