using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class BuildingShopUI : MonoBehaviour
{
    public static BuildingShopUI Instance { get; private set; }

    [System.Serializable]
    public class ShopItem
    {
        public string name;
        public GameObject buildingPrefab;
        public int cost;
        public Sprite icon;
    }

    [Header("Settings")]
    public List<ShopItem> availableBuildings = new List<ShopItem>();

    [Header("UI Elements")]
    public GameObject shopPanel;
    public Transform buttonsParent;
    public GameObject buttonPrefab;

    [SerializeField]private BuildingPlacer placer;
    [SerializeField] private ResourceManager resourceManager;

    void Awake()
    {
        Instance = this;
        placer = FindObjectOfType<BuildingPlacer>();
        shopPanel.SetActive(false);
    }

    public void UnlockShop()
    {
        shopPanel.SetActive(true);
        InitializeShop();
    }

    private void InitializeShop() 
    {
        foreach (Transform child in buttonsParent)
            Destroy(child.gameObject);

        foreach (var item in availableBuildings)
        {
            GameObject button = Instantiate(buttonPrefab, buttonsParent);
            button.GetComponent<Image>().sprite = item.icon;
            button.GetComponent<Button>().onClick.AddListener(() => SelectBuilding(item));

            button.GetComponentInChildren<TMP_Text>().text =
                $"{item.name}\n{item.cost}$";
        }
    }

    private void SelectBuilding(ShopItem item)
    {
        if (resourceManager.CanAfford(item.cost))
        {
            placer.StartPlacingBuilding(item);
            shopPanel.SetActive(false);
        }
    }
}