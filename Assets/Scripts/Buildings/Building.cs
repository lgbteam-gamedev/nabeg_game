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

    [Header("»деологическое вли€ние")]
    public float sovyonokEffect;
    public float ussrEffect;
    public float eternalSummerEffect;

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

    public virtual void SetSelected(bool selected)
    {
        selectionIndicator.SetActive(selected);
    }
    public bool IsColliding()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position,
                                                GetComponent<Collider>().bounds.extents);
        return colliders.Length > 2; // 1 - собственный коллайдер
    }
}