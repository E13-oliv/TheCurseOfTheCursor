using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{
    // MANAGERS
    private GameManager GM;

    [Header("Info Text")]
    [SerializeField]
    private Text infoTextZone;

    private int gamePhase;

    // special for tetris pieces
    private int maxPieces = 12;
    private int numOfPiecesInInventory;

    private void OnEnable()
    {
        GameObject gameManagerGameObject = GameObject.Find("GameManager");
        GM = gameManagerGameObject.GetComponent<GameManager>();

        gamePhase = GM.GetGamePhase();

        // 2 = tetris pieces
        numOfPiecesInInventory = GM.GetInventory()[2];

        infoTextZone.text = "";
    }

    public void SetInfoText(string infoText)
    {
        infoTextZone.text = infoText;
    }

    // special for tetris pieces
    public void SetInfoTextTetrisPieces(string infoText)
    {
        if (gamePhase < 71)
        {
            infoTextZone.text = infoText;
        }
        else
        {
            infoTextZone.text = "[" + numOfPiecesInInventory + "/" + maxPieces + "]" + infoText;
        }
    }
}
