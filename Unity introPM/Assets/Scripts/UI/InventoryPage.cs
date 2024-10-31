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

    int currentlyDraggedItemIndex = -1;

    public event Action<int> OnDescriptionRequested,
        //OnItemActionRequested,
        OnStartDragging;

    public event Action<int, int> OnSwapItems;

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

    public void UpdateData(int itemIndex, Sprite itemImage, int itemQuantity)
    {
        if (listOfUIItems.Count > itemIndex)
        {
            listOfUIItems[itemIndex].SetData(itemImage, itemQuantity);
        }
    }

    private void HandleShowItemActions(InventoryItemUI inventoryItemUI)
    {
        
    }

    private void HandleEndDrag(InventoryItemUI inventoryItemUI)
    {
        ResetDraggedItem();
    }

    private void HandleSwap(InventoryItemUI inventoryItemUI)
    {
        int index = listOfUIItems.IndexOf(inventoryItemUI);
        if (index == -1)
        {
            return;
        }
        OnSwapItems?.Invoke(currentlyDraggedItemIndex, index);
    }

    private void HandleBeginDrag(InventoryItemUI inventoryItemUI)
    {
        int index = listOfUIItems.IndexOf(inventoryItemUI);
        if (index == -1)
            return;
        currentlyDraggedItemIndex = index;
        HandleItemSelection(inventoryItemUI);
        OnStartDragging?.Invoke(index);
    }

    public void CreateDraggedItem(Sprite sprite, int quantity)
    {
        mouseFollower.Toggle(true);
        mouseFollower.SetData(sprite, quantity);
    }

    private void HandleItemSelection(InventoryItemUI inventoryItemUI)
    {
        int index = listOfUIItems.IndexOf(inventoryItemUI);
        if (index == -1)
        {
            return;
        }
        OnDescriptionRequested?.Invoke(index);
    }
    public void Show()
    {
        if (Time.timeScale == 1)
        {
            gameObject.SetActive(true);
            ResetSelection();

            tempYsensitivity = cameraControl.Ysensitivity;
            tempXsensitivity = cameraControl.Xsensitivity;
            cameraControl.Ysensitivity = 0f;
            cameraControl.Xsensitivity = 0f;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void ResetSelection()
    {
        itemDescripton.ResetDescription();
        DeselectAllItems();
    }

    private void DeselectAllItems()
    {
        foreach (InventoryItemUI item in listOfUIItems)
        {
            item.Deselect();
        }
    }

    public void Hide()
    {
        if (Time.timeScale == 1)
        {
            gameObject.SetActive(false);
            ResetDraggedItem();

            cameraControl.Ysensitivity = tempYsensitivity;
            cameraControl.Xsensitivity = tempXsensitivity;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        
    }

    public void ResetDraggedItem()
    {
        mouseFollower.Toggle(false);
        currentlyDraggedItemIndex = -1;
    }
}
