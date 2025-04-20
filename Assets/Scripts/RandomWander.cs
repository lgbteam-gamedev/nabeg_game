using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class RandomWander : MonoBehaviour
{
    [Header("Settings")]
    public float wanderRadius = 10f;
    public float minWaitTime = 3f;
    public float maxWaitTime = 7f;

    private NavMeshAgent agent;
    private Coroutine wanderCoroutine;
    private Unit unit;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        unit = GetComponent<Unit>();
    }

    void OnEnable()
    {
        if (ShouldWander())
            StartWandering();
    }

    void OnDisable()
    {
        StopWandering();
    }

    public void StartWandering()
    {
        if (wanderCoroutine == null)
            wanderCoroutine = StartCoroutine(WanderRoutine());
    }

    public void StopWandering()
    {
        if (wanderCoroutine != null)
        {
            StopCoroutine(wanderCoroutine);
            wanderCoroutine = null;
        }
        agent.ResetPath();
    }

    private IEnumerator WanderRoutine()
    {
        while (true)
        {
            if (ShouldWander() && agent.isActiveAndEnabled)
            {
                Vector3 newPos = GetRandomNavMeshPosition();
                agent.SetDestination(newPos);
            }
            yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
        }
    }

    private bool ShouldWander()
    {
        // Не блуждаем если:
        // 1. Это юнит Fistun в режиме следования
        // 2. Агент выключен
        return !(unit != null && unit.IsFollowing) && agent.isActiveAndEnabled;
    }

    private Vector3 GetRandomNavMeshPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += transform.position;

        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, NavMesh.AllAreas);
        return hit.position;
    }
}