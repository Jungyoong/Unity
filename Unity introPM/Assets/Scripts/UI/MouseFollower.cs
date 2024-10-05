using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollower : MonoBehaviour
{
    [SerializeField]
    Canvas canvas;
    
    Camera mainCam;

    [SerializeField]
    private InventoryItemUI item;

    public void Awake()
    {
        canvas = transform.root.GetComponent<Canvas>();
        mainCam = GameObject.Find("Instantiate Manager").GetComponent<InstantiateManager>().mainCam;
        item = GetComponentInChildren<InventoryItemUI>();
    }

    public void SetData(Sprite sprite, int quality)
    {
        item.SetData(sprite, quality);
    }

    void Update()
    {
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)canvas.transform,
        Input.mousePosition,
        canvas.worldCamera,
        out position);
        transform.position = canvas.transform.TransformPoint(position);
    }

    public void Toggle(bool val)
    {
        gameObject.SetActive(val);
    }
}
