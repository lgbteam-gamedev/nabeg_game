using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class BuildingUI : MonoBehaviour
{
    public static BuildingUI Instance { get; private set; }

    [Header("Settings")]
    public GameObject buttonPrefab;
    public GameObject canvas;
    public ResourceManager resourceManager;
    public float yOffset = 50f;

    private GameObject currentButton;
    private Building currentBuilding;

    void Awake()
    {
        Instance = this;
    }

    public void ShowForBuilding(Building building)
    {
        foreach (Transform child in canvas.transform)
        {
            Destroy(child.gameObject);
        }
        currentBuilding = building;

        // ������� ������ �����������
        currentButton = Instantiate(buttonPrefab, canvas.transform);

        // ����������� ������
        Button btn = currentButton.GetComponent<Button>();
        TMP_Text btnText = currentButton.GetComponentInChildren<TMP_Text>();

        btnText.text = $"Unlock ({building.unlockCost}$)";
        btn.onClick.AddListener(OnUnlockButtonClicked);

        // ��������� ���������
        btn.interactable = !building.isUnlocked && resourceManager.CanAfford(building.unlockCost);
    }
    private void OnUnlockButtonClicked()
    {
        currentBuilding.TryUnlock();
    }

}