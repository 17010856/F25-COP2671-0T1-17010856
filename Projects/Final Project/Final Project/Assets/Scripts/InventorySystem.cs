using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    public HarvestableItem item;
    public int quantity;

    public InventorySlot(HarvestableItem item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }
}

public class InventorySystem : MonoBehaviour
{
    public List<InventorySlot> inventory = new List<InventorySlot>();

    public delegate void OnInventoryChanged();
    public event OnInventoryChanged onInventoryChanged;

    public void AddItem(HarvestableItem item, int amount = 1)
    {
        InventorySlot existingSlot = inventory.Find(slot => slot.item == item);

        if (existingSlot != null)
        {
            existingSlot.quantity += amount;
            Debug.Log($"Added {amount} {item.itemName}. Total: {existingSlot.quantity}");
        }
        else
        {
            inventory.Add(new InventorySlot(item, amount));
            Debug.Log($"Added new item: {item.itemName} x{amount}");
        }

        onInventoryChanged?.Invoke();
    }

    public void RemoveItem(HarvestableItem item, int amount = 1)
    {
        InventorySlot existingSlot = inventory.Find(slot => slot.item == item);

        if (existingSlot != null)
        {
            existingSlot.quantity -= amount;

            if (existingSlot.quantity <= 0)
            {
                inventory.Remove(existingSlot);
            }

            onInventoryChanged?.Invoke();
        }
    }

    public int GetItemCount(HarvestableItem item)
    {
        InventorySlot existingSlot = inventory.Find(slot => slot.item == item);
        return existingSlot != null ? existingSlot.quantity : 0;
    }

    public void RefreshUI()
    {
        onInventoryChanged?.Invoke();
    }
}