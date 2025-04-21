using UnityEngine;
using UnityEngine.Events;

public class Building : MonoBehaviour
{
    [Header("Settings")]
    public int unlockCost = 500;
    public int unitLimitIncrease = 5;
    public bool isUnlocked = false;

    [Header("Visuals")]
    public GameObject lockedModel;
    public GameObject unlockedModel;
    public GameObject selectionIndicator;
    public ResourceManager resourceManager;
    public UnitLimitManager unitLimitManager;
    [Header("Events")]
    public UnityEvent OnUnlocked;

    public void Start()
    {
        UpdateVisualState();
        selectionIndicator.SetActive(false);
    }

    public virtual void TryUnlock()
    {
        if (isUnlocked) return;

        if (resourceManager.CanAfford(unlockCost))
        {
            resourceManager.SpendMoney(unlockCost);
            unitLimitManager.IncreaseLimit(unitLimitIncrease);
            isUnlocked = true;
            UpdateVisualState();
            OnUnlocked?.Invoke();
        }
    }

    private void UpdateVisualState()
    {
        lockedModel.SetActive(!isUnlocked);
        unlockedModel.SetActive(isUnlocked);
    }

    public void SetSelected(bool selected)
    {
        selectionIndicator.SetActive(selected);
    }
}