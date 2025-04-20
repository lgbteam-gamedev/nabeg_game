using TMPro;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    [SerializeField] private int startingMoney = 1000;
    private int currentMoney;
    public TMP_Text text;

    void Awake()
    {
        Instance = this;
        currentMoney = startingMoney;
    }
    private void Update()
    {
        text.text = ""+currentMoney;
    }
    public bool CanAfford(int amount) => currentMoney >= amount;

    public void SpendMoney(int amount)
    {
        currentMoney = Mathf.Max(0, currentMoney - amount);
    }

    public void AddMoney(int amount)
    {
        currentMoney += amount;
    }

    public int GetCurrentMoney() => currentMoney;
}