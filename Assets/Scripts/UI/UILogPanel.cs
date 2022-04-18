using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UILogPanel : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField]
    private WorldManager WM;

    [Header("Canvas")]
    [SerializeField]
    private Canvas pauseCanvas;

    [Header("Logs")]
    [SerializeField]
    private TextMeshProUGUI logText;
    [SerializeField]
    private string emptyLogText = "No Logs...";

    private float logTextHeight;
    private Vector2 logTextPos;
    private float logTextBaseY;
    private float logTextMaxY;
    private float maskHeight = 400;

    [Header("Scroll images")]
    [SerializeField]
    private Image upImage;
    [SerializeField]
    private Image stickImage;
    [SerializeField]
    private Image downImage;


    private float scrollSpeed = 10f;


    private int stickMovement;

    private void OnEnable()
    {
        //string[] logContent = WM.GetLogText();

        //logText.text = "";

        //// get first empty log
        //int firstEmpty = System.Array.IndexOf(logContent, "");


        //if (logContent[0] == "")
        //{
        //    logText.text = emptyLogText;
        //}
        //else
        //{
        //    for(int i = 0; i < firstEmpty; i++)
        //    {
        //        logText.text += "• " + logContent[i] + "\n";
        //    }
        //}

        string logContent = WM.GetLogText();

        if (logContent == "")
        {
            logText.text = emptyLogText;
        }
        else
        {
            logText.text = logContent;
        }

        Canvas.ForceUpdateCanvases();

        logTextHeight = logText.rectTransform.sizeDelta.y;
        logTextHeight = logText.rectTransform.rect.height;
        logTextPos = logText.rectTransform.localPosition;

        logTextBaseY = maskHeight;
        logTextMaxY = logTextHeight - maskHeight + logTextBaseY + 25;

        if (logTextHeight > maskHeight)
        {
            logText.rectTransform.localPosition = new Vector2(0, logTextMaxY);
        }
    }

    private void Update()
    {
        if (logTextHeight > maskHeight)
        {
            float newY = logText.rectTransform.localPosition.y;

            if (stickMovement > 0)
            {
                newY -= scrollSpeed;
            }

            if (stickMovement < 0)
            {
                newY += scrollSpeed;
            }

            newY = Mathf.Clamp(newY, logTextBaseY, logTextMaxY);

            logText.rectTransform.localPosition = new Vector2(0, newY);

            if (newY != logTextBaseY)
            {
                upImage.color = new Color(1, 1, 1, 1);
            }
            else
            {
                upImage.color = new Color(1, 1, 1, .5f);
            }

            stickImage.color = new Color(1, 1, 1, 1);

            if (newY != logTextMaxY)
            {
                downImage.color = new Color(1, 1, 1, 1);
            }
            else
            {
                downImage.color = new Color(1, 1, 1, .5f);
            }
        }
        else
        {
            upImage.color = new Color(1, 1, 1, .5f);
            stickImage.color = new Color(1, 1, 1, .5f);
            downImage.color = new Color(1, 1, 1, .5f);
        }
    }

    public void SetStickMovement(int movement)
    {
        stickMovement = movement;
    }
}
