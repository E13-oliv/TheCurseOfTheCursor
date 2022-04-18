using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIInventoryButtons : Selectable
{
    // game manager
    private GameManager GM;

    private int[] inventory;
    private int[] inventoryDiscovered;

    [Header("Button")]
    [SerializeField]
    private Button itemButton;

    [Header("Sprites")]
    [SerializeField]
    private Sprite emptySprite;
    [SerializeField]
    private Sprite blackAndWhiteSprite;

    [Header("Item")]
    [SerializeField]
    private int itemID;

    [Header("Info")]
    [SerializeField]
    private GameObject infoImage;

    BaseEventData m_BaseEvent;

    private new void OnEnable()
    {
        // find the GameManager
        GameObject gameManagerGameObject = GameObject.Find("GameManager");
        GM = gameManagerGameObject.GetComponent<GameManager>();

        inventory = GM.GetInventory();
        inventoryDiscovered = GM.GetInventoryDiscovered();

        if (inventory[itemID] >= 1 && inventoryDiscovered[itemID] == 1)
        {
            itemButton.GetComponent<Image>().sprite = blackAndWhiteSprite;
            itemButton.gameObject.SetActive(true);
            itemButton.interactable = true;
        }
        else if (inventory[itemID] == 0 && inventoryDiscovered[itemID] == 1)
        {
            itemButton.GetComponent<Image>().sprite = emptySprite;
            itemButton.gameObject.SetActive(true);
            itemButton.interactable = true;
        }
        else
        {
            itemButton.gameObject.SetActive(false);
        }

        infoImage.SetActive(false);
    }

    private void Update()
    {
        if (IsHighlighted() == true)
        {
            if (itemButton.isActiveAndEnabled == true)
            {
                infoImage.SetActive(true);
            }
        }
        else
        {
            infoImage.SetActive(false);
        }
    }
}
