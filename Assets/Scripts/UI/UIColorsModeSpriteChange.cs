using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIColorsModeSpriteChange : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField]
    private WorldManager WM;

    [Header("Sprites")]
    [SerializeField]
    private Sprite BWSprite;
    [SerializeField]
    private Sprite colors16Sprite;
    [SerializeField]
    private Sprite colors256Sprite;
    [SerializeField]
    private Sprite colorsFullSprite;

    private string colorsMode;

    private void Start()
    {
        colorsMode = WM.GetColorsMode();

        ChangeSprite();
    }

    private void Update()
    {
        if (colorsMode != WM.GetColorsMode())
        {
            colorsMode = WM.GetColorsMode();

            ChangeSprite();
        }
    }

    private void ChangeSprite()
    {
        switch (colorsMode)
        {
            case "Colors16":
                GetComponent<Image>().sprite = colors16Sprite;
                break;
            case "Colors256":
                GetComponent<Image>().sprite = colors256Sprite;
                break;
            case "ColorsFull":
                GetComponent<Image>().sprite = colorsFullSprite;
                break;
            // case "BlackAndWhite"
            default:
                GetComponent<Image>().sprite = BWSprite;
                break;
        }
    }
}
