using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class RallyCircleAbility : Ability
{
    [Header("Settings")]
    public float searchRadius = 10f;
    public float formationRadius = 3f;
    public float followDuration = 15f;
    public LayerMask unitLayer;

    [Header("Movement")]
    public float updateInterval = 0.5f;
    public float positionOffset = 0.05f;

    private Dictionary<Unit, Coroutine> activeFormations = new Dictionary<Unit, Coroutine>();

    protected override void Execute(Unit caster, Vector3 target)
    {
        Debug.Log("Execution_started");
        Collider[] colliders = Physics.OverlapSphere(caster.transform.position, searchRadius, unitLayer);
        List<Unit> followers = GetValidFollowers(colliders);

        // Запускаем корутину следования
        if (activeFormations.ContainsKey(caster))
        {
            StopCoroutine(activeFormations[caster]);
        }
        activeFormations[caster] = StartCoroutine(FollowCasterRoutine(caster, followers));
    }

    private List<Unit> GetValidFollowers(Collider[] colliders)
    {
        List<Unit> followers = new List<Unit>();
        foreach (Collider col in colliders)
        {
            if ((col.CompareTag("Fistun_Base") || col.CompareTag("Simp") || col.CompareTag("Shiz")) &&
               col.TryGetComponent<Unit>(out Unit unit) &&
               col.TryGetComponent<NavMeshAgent>(out NavMeshAgent agent))
            {
                followers.Add(unit);
            }
        }
        return followers;
    }

    private IEnumerator FollowCasterRoutine(Unit caster, List<Unit> followers)
    {
        foreach (Unit follower in followers)
        {
            follower.SetFollowingState(true);
        }
        float endTime = Time.time + followDuration;
        Vector3 lastCasterPosition = caster.transform.position;
        
        // Первоначальное построение
        UpdateFormationPositions(caster.transform.position, followers);

        while (Time.time < endTime && caster != null)
        {
            // Обновляем позиции только если кастер сдвинулся
            if (Vector3.Distance(lastCasterPosition, caster.transform.position) > positionOffset)
            {
                UpdateFormationPositions(caster.transform.position, followers);
                lastCasterPosition = caster.transform.position;
            }
            yield return new WaitForSeconds(updateInterval);
        }

        // Очистка
        foreach (Unit follower in followers)
        {
            if (follower != null)
            {
                follower.SetFollowingState(false);
                follower.GetComponent<NavMeshAgent>().ResetPath();
            }
        }
        activeFormations.Remove(caster);
    }

    private void UpdateFormationPositions(Vector3 center, List<Unit> followers)
    {
        int count = followers.Count;
        float angleStep = 360f / count;

        for (int i = 0; i < count; i++)
        {
            Unit follower = followers[i];
            if (follower == null) continue;

            Vector3 targetPos = CalculateFormationPosition(center, angleStep * i);
            MoveToPosition(follower, targetPos);
        }
    }

    private Vector3 CalculateFormationPosition(Vector3 center, float angle)
    {
        float rad = angle * Mathf.Deg2Rad;
        return center + new Vector3(
            Mathf.Cos(rad) * formationRadius,
            0,
            Mathf.Sin(rad) * formationRadius
        );
    }

    private void MoveToPosition(Unit unit, Vector3 target)
    {
        NavMeshAgent agent = unit.GetComponent<NavMeshAgent>();
        if (agent != null && agent.enabled)
        {
            agent.SetDestination(target);
        }
    }

    private void OnDestroy()
    {
        // Остановка всех корутин при уничтожении
        foreach (var pair in activeFormations)
        {
            StopCoroutine(pair.Value);
        }
        activeFormations.Clear();
    }
}