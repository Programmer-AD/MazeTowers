using UnityEngine;
using UnityEngine.Events;

public class MoneyAssistant : MonoBehaviour
{
    [SerializeField] private double startMoney;

    private double money;
    public double Money
    {
        get => money;
        private set
        {
            if (money != value)
            {
                money = value;
                OnMoneyAmountChanged();
            }
        }
    }

    public UnityEvent<double> MoneyAmountChanged;
    private void OnMoneyAmountChanged()
    {
        MoneyAmountChanged?.Invoke(money);
    }

    private void Start()
    {
        Money = startMoney;
    }

    public bool Withdraw(double amount)
    {
        if (Money >= amount)
        {
            Money -= amount;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void OnEnemyDead(Enemy enemy)
    {
        if (enemy.characteritics != null)
        {
            Money += enemy.characteritics.KillMoney;
        }
    }
}
