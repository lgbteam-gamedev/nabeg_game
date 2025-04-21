using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Cosplayer : MonoBehaviour
{
    [Header("Base Settings")]
    [SerializeField] public int currentLevel = 1;
    [SerializeField] private float evolutionRadius = 5f;
    [SerializeField] private float checkInterval = 3f;
    [SerializeField] private float summonChance = 0.3f;
    [SerializeField] private float conversionChance = 0.2f;

    [Header("Prefabs")]
    [SerializeField] private GameObject fistunPrefab;
    [SerializeField] private GameObject simpPrefab;
    [SerializeField] private GameObject shizPrefab;

    private UnitLimitManager unitLimitManager;

    private void Start()
    {
        unitLimitManager = GameObject.Find("UnitLimitManager").GetComponent<UnitLimitManager>();
        StartCoroutine(BehaviorRoutine());
    }

    private IEnumerator BehaviorRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(checkInterval);

            switch (currentLevel)
            {
                case 1:
                    Level1Behavior();
                    break;
                case 2:
                    Level2Behavior();
                    break;
                case 3:
                    Level3Behavior();
                    break;
            }
        }
    }

    private void Level1Behavior()
    {
        if (Random.value < summonChance)
        {
            if (unitLimitManager.CanSpawnUnit()) { SummonUnit(fistunPrefab); }
            
        }
    }

    private void Level2Behavior()
    {
        // Summoning
        if (Random.value < summonChance)
        {
            if (unitLimitManager.CanSpawnUnit()) { SummonUnit(Random.value > 0.5f ? fistunPrefab : simpPrefab); }
            
        }

        ConvertUnitsInRadius("Fistun_Base", simpPrefab);
    }

    private void Level3Behavior()
    {
        // Summoning
        if (Random.value < summonChance)
        {
            GameObject[] options = { fistunPrefab, simpPrefab, shizPrefab };
            if (unitLimitManager.CanSpawnUnit()) { SummonUnit(options[Random.Range(0, 3)]); }
            
        }

        // Conversions
        ConvertUnitsInRadius("Fistun_Base", simpPrefab);
        ConvertUnitsInRadius("Simp", shizPrefab);
    }

    private void SummonUnit(GameObject unitPrefab)
    {
        Vector3 summonPosition = GetRandomNavMeshPosition(transform.position, 3f);
        Instantiate(unitPrefab, summonPosition, Quaternion.identity);
    }

    private void ConvertUnitsInRadius(string tag, GameObject targetPrefab)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, evolutionRadius);
        foreach (Collider col in colliders)
        {
            if (col.CompareTag(tag) && Random.value < conversionChance)
            {
                ConvertUnit(col.gameObject, targetPrefab);
            }
        }
    }

    private void ConvertUnit(GameObject original, GameObject targetPrefab)
    {
        Vector3 position = original.transform.position;
        Quaternion rotation = original.transform.rotation;
        Destroy(original);
        Instantiate(targetPrefab, position, rotation);
    }

    private Vector3 GetRandomNavMeshPosition(Vector3 center, float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += center;

        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, radius, NavMesh.AllAreas);
        return hit.position;
    }

    public void LevelUp()
    {
        currentLevel = Mathf.Clamp(currentLevel + 1, 1, 3);
        Debug.Log($"Cosplayer evolved to level {currentLevel}!");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, evolutionRadius);
    }
}