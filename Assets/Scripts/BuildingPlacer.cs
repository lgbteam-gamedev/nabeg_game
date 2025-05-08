using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingPlacer : MonoBehaviour
{
    [Header("Settings")]
    public LayerMask placementMask;
    public Material validMaterial;
    public Material invalidMaterial;

    [SerializeField]private GameObject currentPreview;
    private BuildingShopUI.ShopItem currentItem;

    [SerializeField] ResourceManager resourceManager;
    [SerializeField] BuildingShopUI buildingShopUI;

    void Update()
    {
        if (currentPreview != null)
        {
            Debug.Log("Has currentPreview");
            UpdatePreviewPosition();
            UpdatePreviewColors();
            HandlePlacementInput();
        }
    }

    public void StartPlacingBuilding(BuildingShopUI.ShopItem item)
    {
        currentItem = item;
        currentPreview = Instantiate(item.buildingPrefab);
        currentPreview.GetComponent<Building>().resourceManager = GameObject.Find("UnitLimitManager").GetComponent<ResourceManager>();
        currentPreview.GetComponent<Building>().unitLimitManager = GameObject.Find("UnitLimitManager").GetComponent<UnitLimitManager>();
        currentPreview.layer = 7;
        foreach (Transform child in currentPreview.transform)
        {
            child.gameObject.layer = 7;
        }
        SetPreviewMaterials(currentPreview, validMaterial);
    }
    private void UpdatePreviewColors()
    {
        if (IsPositionValid())
        {
            SetPreviewMaterials(currentPreview, validMaterial);
        }
        else
        {
            SetPreviewMaterials(currentPreview, invalidMaterial);
        }
    }
    private void UpdatePreviewPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, placementMask))
        {
            currentPreview.transform.position = hit.point;
            currentPreview.transform.rotation = Quaternion.identity;
        }
    }

    private void HandlePlacementInput()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (IsPositionValid())
            {
                PlaceBuilding();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CancelPlacement();
        }
    }

    private bool IsPositionValid()
    {
        // ѕроверка коллайдеров и навигационной сетки
        
        return !currentPreview.GetComponent<Building>().IsColliding();
    }

    private void PlaceBuilding()
    {
        resourceManager.SpendMoney(currentItem.cost);
        Instantiate(currentItem.buildingPrefab,
                 currentPreview.transform.position,
                 Quaternion.identity);
        Destroy(currentPreview);
        currentItem = null;
        buildingShopUI.shopPanel.SetActive(true);
    }

    private void CancelPlacement()
    {
        Destroy(currentPreview);
        currentItem = null;
        buildingShopUI.shopPanel.SetActive(true);
    }

    private void SetPreviewMaterials(GameObject obj, Material material)
    {
        foreach (var renderer in obj.GetComponentsInChildren<Renderer>())
        {
            renderer.material = material;
        }
    }
}