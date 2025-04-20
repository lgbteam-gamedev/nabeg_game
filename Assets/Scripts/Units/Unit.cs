using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    public bool IsFollowing { get; set; }
    private RandomWander wander;

    public Ability[] abilities;
    public float currentEnergy = 100f;
    public float currentHealth = 100f;
    [Header("personal data")]
    public string Name; // Движется ли юнит?
    public string Descr; // Можно ли двигать юнит ПКМ?
    public Sprite Photo;
    private UnitLimitManager limitManager;
    [Header("Movement Settings")]
    public bool MoveAtWill = true; // Движется ли юнит?
    public bool mouseMove = true; // Можно ли двигать юнит ПКМ?

    [Header("Visuals")]
    public GameObject selectionCircle;
    private NavMeshAgent agent;

    void Start()
    {
        limitManager = GameObject.Find("UnitLimitManager").GetComponent<UnitLimitManager>();
        limitManager.RegisterUnit();
        agent = GetComponent<NavMeshAgent>();
        wander = GetComponent<RandomWander>();
    }
    public void SetDestination(Vector3 destination)
    {
        Debug.Log((!MoveAtWill || agent == null));
        if (!MoveAtWill || agent == null) return;
        Debug.Log(destination);
        agent.SetDestination(destination);
    }
    public void SetSelected(bool isSelected)
    {
        if (selectionCircle != null)
            selectionCircle.SetActive(isSelected);
    }

    public void ModifyEnergy(float amount)
    {
        currentEnergy = Mathf.Clamp(currentEnergy + amount, 0, 100f);
    }

    public void UseAbility(int abilityIndex, Vector3 target)
    {
        if (abilityIndex < 0 || abilityIndex >= abilities.Length) return;
        abilities[abilityIndex].Activate(this, target);
    }
    public void TakeDamage(float amount)
    {
        // Реализуйте логику здоровья
        // Например:
        currentHealth -= 0;
        if (currentHealth <= 0) Die();
    }

    private void Die()
    {
        // Логика смерти юнита
        Destroy(gameObject);
    }
    public void SetFollowingState(bool isFollowing)
    {
        IsFollowing = isFollowing;

        if (wander != null)
        {
            if (isFollowing)
                wander.StopWandering();
            else
                wander.StartWandering();
        }
    }
    private void OnDestroy()
    {
        limitManager.UnregisterUnit();
    }
}