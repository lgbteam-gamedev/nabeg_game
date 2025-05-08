using UnityEngine;

public class AdministrativeBuilding : Building
{
    [SerializeField] BuildingShopUI shopUI;
    private void Start()
    {
        base.Start();
        unitLimitIncrease = 0;
    }

    public override void TryUnlock()
    {
        base.TryUnlock();
        shopUI.UnlockShop();
    }
    public override void SetSelected(bool selected)
    {
        base.SetSelected(selected);
        if (isUnlocked == true)
        {
            shopUI.UnlockShop();
        }
    }
}