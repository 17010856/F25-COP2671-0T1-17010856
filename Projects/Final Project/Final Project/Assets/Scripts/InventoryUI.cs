using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public InventorySystem inventorySystem;
    public GameObject itemSlotPrefab;
    public Transform itemSlotContainer;

    void Start()
    {
        if (inventorySystem != null)
        {
            inventorySystem.onInventoryChanged += RefreshUI;
        }

        RefreshUI();
    }

    void OnDestroy()
    {
        if (inventorySystem != null)
        {
            inventorySystem.onInventoryChanged -= RefreshUI;
        }
    }

    public void RefreshUI()
    {
        foreach (Transform child in itemSlotContainer)
        {
            Destroy(child.gameObject);
        }

        if (inventorySystem == null) return;

        foreach (InventorySlot slot in inventorySystem.inventory)
        {
            GameObject slotObj = Instantiate(itemSlotPrefab, itemSlotContainer);

            Image icon = slotObj.transform.Find("Icon").GetComponent<Image>();
            TextMeshProUGUI quantityText = slotObj.transform.Find("QuantityText").GetComponent<TextMeshProUGUI>();

            if (icon != null && slot.item.itemIcon != null)
            {
                icon.sprite = slot.item.itemIcon;
                icon.enabled = true;
            }

            if (quantityText != null)
            {
                quantityText.text = slot.quantity.ToString();
            }
        }
    }
}