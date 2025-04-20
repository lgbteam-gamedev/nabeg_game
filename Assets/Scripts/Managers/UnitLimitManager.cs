using TMPro;
using UnityEngine;

public class UnitLimitManager : MonoBehaviour
{
    public static UnitLimitManager Instance { get; private set; }

    [SerializeField] private int baseUnitLimit = 10;
    private int currentLimit;
    private int currentCount;
    public TMP_Text text;

    void Awake()
    {
        Instance = this;
        currentLimit = baseUnitLimit;
        currentCount = 0;
    }
    private void Update()
    {
        text.text = currentCount + "/" + currentLimit;
    }
    public bool CanSpawnUnit()
    {
        return currentCount < currentLimit;
    }

    public void IncreaseLimit(int amount)
    {
        currentLimit += amount;
    }

    public void RegisterUnit() => currentCount++;
    public void UnregisterUnit() => currentCount--;

    public int GetCurrentLimit() => currentLimit;
    public int GetCurrentCount() => currentCount;
}