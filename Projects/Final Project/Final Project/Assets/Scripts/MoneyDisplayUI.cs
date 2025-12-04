using UnityEngine;
using TMPro;

public class MoneyDisplayUI : MonoBehaviour
{
    public CurrencyManager currencyManager;
    public TextMeshProUGUI moneyText;

    void Start()
    {
        if (currencyManager != null)
        {
            currencyManager.onMoneyChanged += UpdateDisplay;
            UpdateDisplay(currencyManager.GetMoney());
        }
    }

    void OnDestroy()
    {
        if (currencyManager != null)
        {
            currencyManager.onMoneyChanged -= UpdateDisplay;
        }
    }

    void UpdateDisplay(int amount)
    {
        if (moneyText != null)
        {
            moneyText.text = "$" + amount.ToString();
        }
    }
}