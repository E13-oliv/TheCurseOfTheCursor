using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFacing : MonoBehaviour
{
    // MANAGERS
    private GameManager GM;

    private GameObject player;

    private string colorsMode;

    [Header("Settings")]
    [SerializeField]
    private bool faceTheCamera = true;

    private void OnEnable()
    {
        // find the GameManager
        GameObject gameManagerGameObject = GameObject.Find("GameManager");
        GM = gameManagerGameObject.GetComponent<GameManager>();

        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        string currentColorsMode = GM.GetColorsMode();

        if (colorsMode != currentColorsMode)
        {
            colorsMode = currentColorsMode;

            // deactivate all colors sprites except for the right one
            for (int i = 1; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);

                if (transform.GetChild(i).tag == colorsMode)
                {
                    transform.GetChild(i).gameObject.SetActive(true);
                }
            }
        }

        if (faceTheCamera == true)
        {
            transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
        }
    }
}
