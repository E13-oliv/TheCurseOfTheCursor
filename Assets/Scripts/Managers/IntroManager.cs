using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    // MANAGERS
    private GameManager GM;
    private AudioManager AM;

    private void Start()
    {
        // find the GameManager
        GameObject gameManagerGameObject = GameObject.Find("GameManager");
        GM = gameManagerGameObject.GetComponent<GameManager>();
        AM = gameManagerGameObject.GetComponentInChildren<AudioManager>();

        GM.setPauseState(true);
        AM.SetInGame(false);
    }
}
