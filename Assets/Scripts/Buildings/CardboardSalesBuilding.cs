using UnityEngine;

public class CardboardSalesBuilding : Building
{
    public static int MoneyBonus { get; private set; } = 0;

    private void Start()
    {
        base.Start();
        unitLimitIncrease = 0; // Не влияет на лимит юнитов
    }

    public override void TryUnlock()
    {
        base.TryUnlock();
        if (isUnlocked || !ResourceManager.Instance.CanAfford(unlockCost)) return;

        ResourceManager.Instance.SpendMoney(unlockCost);
        MoneyBonus++;
        isUnlocked = true;
        
        ApplyBonusToAllUnits();
    }

    private void ApplyBonusToAllUnits()
    {
        foreach (var unit in FindObjectsOfType<Unit>())
        {
            //if (unit.TryGetComponent<Simp>(out var simp))
            //    simp.UpdateMoneyGeneration();

            //if (unit.TryGetComponent<Shiz>(out var shiz))
            //    shiz.UpdateMoneyGeneration();
        }
    }
}