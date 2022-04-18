using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMap : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField]
    private MovementsManager MM;
    private GameManager GM;

    [Header("Maps")]
    [SerializeField]
    private Sprite lvl1Map;
    [SerializeField]
    private Sprite lvl2Map;
    [SerializeField]
    private Sprite lvl3Map;
    [SerializeField]
    private Sprite lvl4Map;

    [Header("Pointer")]
    [SerializeField]
    private GameObject pointer;

    private int mapColums = 42;
    private int mapLines = 35;
    private int mapNumOfCases;

    private int playerCase;
    private Vector3 playerRotation;

    private int mapPointerScale = 10;

    private int mapPointerGap = 5;

    private int gamePhase;

    private void OnEnable()
    {
        GameObject gameManagerGameObject = GameObject.Find("GameManager");
        GM = gameManagerGameObject.GetComponent<GameManager>();

        mapNumOfCases = mapColums * mapLines;
        playerRotation = MM.GetPlayerRotation();

        playerCase = MM.GetCaseNumber() + 1;

        int playerLine = Mathf.FloorToInt(playerCase / mapColums);
        int playerColumn = playerCase - playerLine * mapColums;

        // don't know why...
        int strangeBugX = -15;
        int strangeBugY = 140;

        strangeBugX = -210;
        strangeBugY = 175;

        float pointerX = playerColumn * mapPointerScale + strangeBugX - mapPointerGap;
        float pointerY = playerLine * -mapPointerScale + strangeBugY - mapPointerGap;

        //pointer.GetComponent<CurvedUI.CurvedUIVertexEffect>().enabled = false;
        //pointer.GetComponent<CurvedUI.CurvedUIVertexEffect>().SetDirty();

        pointer.transform.localPosition = new Vector3(pointerX, pointerY, 0);
        pointer.transform.localRotation = Quaternion.Euler(0, 0, playerRotation.y * -1);

        //pointer.GetComponent<CurvedUI.CurvedUIVertexEffect>().enabled = true;
    }

    private void Update()
    {
        gamePhase = GM.GetGamePhase();

        if (gamePhase <= 30)
        {
            GetComponent<Image>().sprite = lvl1Map;
        }
        else if (gamePhase <= 50)
        {
            GetComponent<Image>().sprite = lvl2Map;
        }
        else if (gamePhase <= 70)
        {
            GetComponent<Image>().sprite = lvl3Map;
        }
        else
        {
            GetComponent<Image>().sprite = lvl4Map;
        }
    }
}
