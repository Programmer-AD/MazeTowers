using TMPro;
using UnityEngine;

public class MoneyShower : MonoBehaviour
{
    private TMP_Text text;

    void Awake()
    {
        text = GetComponent<TMP_Text>();
        text.text = string.Empty;
    }

    public void OnMoneyAmountChanged(double money)
    {
        text.text = $"{money}$";
    }
}
