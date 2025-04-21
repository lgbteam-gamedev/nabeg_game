using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class FenceBuilding : Building
{
    [Header("Fence Settings")]
    public GameObject brokenModel;
    public GameObject repairedModel;
    private NavMeshObstacle obstacle;

    private bool isRepaired = false;

    private void Start()
    {
        base.Start();
        obstacle = gameObject.GetComponent<NavMeshObstacle>();
        UpdateFenceState();
    }

    public void TryRepair()
    {
        if (isRepaired || !ResourceManager.Instance.CanAfford(unlockCost)) return;

        obstacle.enabled = true;
        RepairFence();
    }

    private void RepairFence()
    {
        isRepaired = true;
        UpdateFenceState();
        OnUnlocked?.Invoke();
    }

    private void UpdateFenceState()
    {
        brokenModel.SetActive(!isRepaired);
        repairedModel.SetActive(isRepaired);
    }

    // Переопределяем базовое поведение
    public override void TryUnlock()
    {
        base.TryUnlock();
        if (isUnlocked)
        {
            TryRepair();
        }

    }
}