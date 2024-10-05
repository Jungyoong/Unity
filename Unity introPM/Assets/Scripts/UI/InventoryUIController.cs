using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUIController : MonoBehaviour
{
    [SerializeField]
    InventoryPage inventoryUI;

    public int inventorySize = 10;

    void Start()
    {
        inventoryUI.InitializeInventoryUI(inventorySize);
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (inventoryUI.isActiveAndEnabled == false)
            {
                inventoryUI.Show();
            }
            else
            {
                inventoryUI.Hide();
            }
        }
    }
}
