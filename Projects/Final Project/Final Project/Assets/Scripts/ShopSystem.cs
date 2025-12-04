using UnityEngine;

// handles selling items to merchant boat
public class ShopSystem : MonoBehaviour
{
    public InventorySystem playerInventory;
    public CurrencyManager currencyManager;
    public Transform playerTransform;
    public MerchantBoatManager boatManager;
    public float sellRange = 3f; // how close to boat you need to be
    public GameObject sellParticles;

    public void SellItem(HarvestableItem item, int quantity)
    {
        if (!CanSell()) return;
        if (playerInventory == null || currencyManager == null || item == null) return;

        int playerHas = playerInventory.GetItemCount(item);

        if (playerHas >= quantity)
        {
            int totalValue = item.sellPrice * quantity;
            playerInventory.RemoveItem(item, quantity);
            currencyManager.AddMoney(totalValue);
            PlaySellEffect();
        }
    }

    public void SellAllOfItem(HarvestableItem item)
    {
        if (!CanSell()) return;
        if (playerInventory == null || item == null) return;

        int quantity = playerInventory.GetItemCount(item);
        if (quantity > 0)
        {
            SellItem(item, quantity);
        }
    }

    public void SellEntireInventory()
    {
        if (!CanSell())
        {
            Debug.Log("You need to be near the merchant boat to sell!");
            return;
        }

        if (playerInventory == null || currencyManager == null) return;

        int totalValue = 0;

        for (int i = playerInventory.inventory.Count - 1; i >= 0; i--) // calculate total value
        {
            InventorySlot slot = playerInventory.inventory[i];
            int value = slot.item.sellPrice * slot.quantity;
            totalValue += value;
        }

        if (totalValue > 0)
        {
            playerInventory.inventory.Clear();
            currencyManager.AddMoney(totalValue);
            playerInventory.RefreshUI();
            PlaySellEffect();
        }
    }

    private bool CanSell() // checks if player is near boat
    {
        if (boatManager == null || playerTransform == null) return true;

        GameObject boat = boatManager.GetCurrentBoat();
        if (boat == null)
        {
            Debug.Log("The merchant boat isn't here yet!");
            return false;
        }

        float distance = Vector2.Distance(playerTransform.position, boat.transform.position);
        if (distance > sellRange)
        {
            Debug.Log("You're too far from the merchant boat!");
            return false;
        }

        return true;
    }

    private void PlaySellEffect()
    {
        if (sellParticles != null && playerTransform != null)
        {
            GameObject particles = Instantiate(sellParticles, playerTransform.position, Quaternion.identity);
            Destroy(particles, 2f);
        }
    }
}