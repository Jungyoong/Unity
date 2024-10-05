using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDecriptionUI : MonoBehaviour
{
    [SerializeField]
    Image itemImage;
    [SerializeField]
    TMP_Text title;
    [SerializeField]
    private TMP_Text description;

    public void Awake()
    {
        ResetDescription();
    }

    public void ResetDescription()
    {
        this.itemImage.gameObject.SetActive(false);
        this.title.text = "";
        this.description.text = "";
    }

    public void SetDescription(Sprite sprite, string itemName, string ItemDescription)
    {
        this.itemImage.gameObject.SetActive(true);
        this.itemImage.sprite = sprite;
        this.title.text = itemName;
        this.description.text = ItemDescription;
    }
}
