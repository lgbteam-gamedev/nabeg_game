using UnityEngine;

public class LevelUpAbility : Ability
{
    [System.Serializable]
    private class LevelUpCost
    {
        public int fromLevel;
        public int cost;
    }

    [Header("Level Up Settings")]
    [SerializeField]
    private LevelUpCost[] levelCosts = new LevelUpCost[]
    {
        new LevelUpCost { fromLevel = 1, cost = 300 },
        new LevelUpCost { fromLevel = 2, cost = 700 }
    };

    private Cosplayer targetCosplayer;

    public override bool CanActivate(Unit unit)
    {
        // Проверяем что юнит имеет компонент Cosplayer
        targetCosplayer = unit.GetComponent<Cosplayer>();
        if (targetCosplayer == null) return false;

        // Получаем текущий уровень и стоимость апгрейда
        var currentLevel = targetCosplayer.currentLevel;
        var upgradeCost = GetCostForLevel(currentLevel);

        // Проверяем возможность апгрейда
        return base.CanActivate(unit) &&
               upgradeCost.HasValue &&
               ResourceManager.Instance.CanAfford(upgradeCost.Value);
    }

    protected override void Execute(Unit unit, Vector3 target = default)
    {
        var currentLevel = targetCosplayer.currentLevel;
        var upgradeCost = GetCostForLevel(currentLevel);

        if (upgradeCost.HasValue)
        {
            ResourceManager.Instance.SpendMoney(upgradeCost.Value);
            targetCosplayer.LevelUp();
            Debug.Log($"{unit.name} upgraded to level {targetCosplayer.currentLevel}!");
        }
    }

    private int? GetCostForLevel(int currentLevel)
    {
        foreach (var levelCost in levelCosts)
        {
            if (levelCost.fromLevel == currentLevel)
            {
                return levelCost.cost;
            }
        }
        return null;
    }
}