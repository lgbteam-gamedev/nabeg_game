using UnityEngine;
using System.Collections;

public class AddMoney : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int moneyAmount = 10;    // ���������� ����� �� ���
    [SerializeField] private float interval = 5f;     // �������� � ��������
    [SerializeField] ResourceManager manager;
    [Header("State")]
    [SerializeField] private bool isActive = true;    // ������� �� ���������

    private Coroutine moneyCoroutine;

    void Start()
    {
        manager = GameObject.Find("UnitLimitManager").GetComponent<ResourceManager>();
        StartMoneyGeneration();
    }

    void OnDisable()
    {
        StopMoneyGeneration();
    }

    public void StartMoneyGeneration()
    {
        if (moneyCoroutine == null)
        {
            moneyCoroutine = StartCoroutine(MoneyGenerationRoutine());
        }
    }

    public void StopMoneyGeneration()
    {
        if (moneyCoroutine != null)
        {
            StopCoroutine(moneyCoroutine);
            moneyCoroutine = null;
        }
    }

    private IEnumerator MoneyGenerationRoutine()
    {
        while (isActive)
        {
            yield return new WaitForSeconds(interval);

            if (manager != null)
            {
                manager.AddMoney(moneyAmount);
                Debug.Log($"Added {moneyAmount} money from {gameObject.name}");
            }
            else
            {
                Debug.LogError("ResourceManager not found!");
            }
        }
    }

    // ������ ��� ������������� ����������
    public void SetActive(bool state) => isActive = state;
    public void UpdateSettings(int newAmount, float newInterval)
    {
        moneyAmount = newAmount;
        interval = newInterval;
    }
}