using UnityEngine;

public class BuildingSelectionHandler : MonoBehaviour
{
    private Building building;
    public BuildingUI buildingUI;
    [SerializeField] private UnitSelectionManager selectionManager;

    void Awake()
    {
        building = GetComponent<Building>();
    }

    void OnMouseDown()
    {
        if (!selectionManager.IsSelectingUnits())
        {
            Debug.Log("A");
            selectionManager.DeselectAll();
            building.SetSelected(true);
            selectionManager.selectedBuildinds.Add(building);
            Debug.Log(building);
            buildingUI.ShowForBuilding(building);
        }
    }
}