using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButtons : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Text buttonText;

    private Color textColor = new Color(1, 1, 1, .75f);
    private Color hoverTextColor = new Color(1, 1, 1, 1);

    private void OnEnable()
    {
        buttonText = GetComponentInChildren<Text>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonText.color = hoverTextColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonText.color = textColor;
    }
}