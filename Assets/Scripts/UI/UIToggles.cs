using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIToggles : MonoBehaviour
{
    // MANAGERS
    private GameManager GM;

    [Header("Game Phases")]
    [SerializeField]
    private int activeSincePhase;

    private int gamePhase;

    private void OnEnable()
    {
        // find the GameManager
        GameObject gameManagerGameObject = GameObject.Find("GameManager");
        GM = gameManagerGameObject.GetComponent<GameManager>();
    }

    void Update()
    {
        gamePhase = GM.GetGamePhase();

        if (gamePhase >= activeSincePhase)
        {
            this.GetComponent<Toggle>().interactable = true;
            this.GetComponentInChildren<Text>().enabled = true;
        }
        else
        {
            this.GetComponent<Toggle>().interactable = false;
            this.GetComponentInChildren<Text>().enabled = false;
        }
    }
}
