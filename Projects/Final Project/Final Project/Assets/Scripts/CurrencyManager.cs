using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public int currentMoney = 0;

    public delegate void OnMoneyChanged(int newAmount);
    public event OnMoneyChanged onMoneyChanged;

    public void AddMoney(int amount)
    {
        currentMoney += amount;
        onMoneyChanged?.Invoke(currentMoney);
        Debug.Log($"Added ${amount}. Total: ${currentMoney}");
    }

    public bool SpendMoney(int amount)
    {
        if (currentMoney >= amount)
        {
            currentMoney -= amount;
            onMoneyChanged?.Invoke(currentMoney);
            Debug.Log($"Spent ${amount}. Remaining: ${currentMoney}");
            return true;
        }
        else
        {
            Debug.Log("Not enough money!");
            return false;
        }
    }

    public int GetMoney()
    {
        return currentMoney;
    }
}