using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class InventoryPage : MonoBehaviour
{
    [SerializeField]
    InventoryItemUI itemPrefab;

    [SerializeField]
    RectTransform contentPanel;

    [SerializeField]
    InventoryDecriptionUI itemDescripton;
    
    [SerializeField]
    MouseFollower mouseFollower;

    List<InventoryItemUI> listOfUIItems = new List<InventoryItemUI>();

    public Sprite image, image2;
    public int quantity;
    public string title, description;

    int currentlyDraggedItemIndex = -1;

    internal CameraControl cameraControl;

    float tempYsensitivity;
    float tempXsensitivity;

    void Awake()
    {
        cameraControl = GameObject.Find("Instantiate Manager").GetComponent<InstantiateManager>().cameraControl;
        mouseFollower.Toggle(false);
    }

    public void InitializeInventoryUI(int inventorySize)
    {
        for (int i = 0; i < inventorySize; i++)
        {
            InventoryItemUI uiItem = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
            uiItem.transform.SetParent(contentPanel);
            listOfUIItems.Add(uiItem);
            uiItem.OnitemClicked += HandleItemSelection;
            uiItem.OnItemBeginDrag += HandleBeginDrag;
            uiItem.OnItemDroppedOn += HandleSwap;
            uiItem.OnItemEndDrag += HandleEndDrag;
            uiItem.OnRightMouseBtnClick += HandleShowItemActions;
        }
    }

    private void HandleShowItemActions(InventoryItemUI inventoryItemUI)
    {
        
    }

    private void HandleEndDrag(InventoryItemUI inventoryItemUI)
    {
        mouseFollower.Toggle(false);
    }

    private void HandleSwap(InventoryItemUI inventoryItemUI)
    {
        int index = listOfUIItems.IndexOf(inventoryItemUI);
        if (index == -1)
        {
            mouseFollower.Toggle(false);
            currentlyDraggedItemIndex = -1;
            return;
        }
        listOfUIItems[currentlyDraggedItemIndex].SetData(index == 0 ? image : image2, quantity);
        listOfUIItems[index].SetData(currentlyDraggedItemIndex == 0 ? image : image2, quantity);
        mouseFollower.Toggle(false);
        currentlyDraggedItemIndex = -1;
    }

    private void HandleBeginDrag(InventoryItemUI inventoryItemUI)
    {
        int index = listOfUIItems.IndexOf(inventoryItemUI);
        if (index == -1)
            return;
        currentlyDraggedItemIndex = index;
        mouseFollower.Toggle(true);
        mouseFollower.SetData(index == 0 ? image : image2, quantity);
    }

    private void HandleItemSelection(InventoryItemUI inventoryItemUI)
    {
        itemDescripton.SetDescription(image, title, description);

        listOfUIItems[0].Select();
    }
    public void Show()
    {
        gameObject.SetActive(true);
        itemDescripton.ResetDescription();

        listOfUIItems[0].SetData(image, quantity);
        listOfUIItems[1].SetData(image2, quantity);

        tempYsensitivity = cameraControl.Ysensitivity;
        tempXsensitivity = cameraControl.Xsensitivity;
        cameraControl.Ysensitivity = 0f;
        cameraControl.Xsensitivity = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        listOfUIItems[0].Deselect();

        cameraControl.Ysensitivity = tempYsensitivity;
        cameraControl.Xsensitivity = tempXsensitivity;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
