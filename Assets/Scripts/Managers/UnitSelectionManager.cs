using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UnitSelectionManager : MonoBehaviour
{
    public static UnitSelectionManager Instance { get; private set; }

    [Header("Selection Settings")]
    public LayerMask selectableLayer;
    public RectTransform selectionBox;
    public Color selectionBoxColor = new Color(0.8f, 0.8f, 0.1f, 0.2f);

    private Vector2 startMousePosition;
    private List<Unit> selectedUnits = new List<Unit>();
    public List<Building> selectedBuildinds = new List<Building>();
    private Image selectionBoxImage;

    private bool isSelecting = false;
    public bool IsSelectingUnits() => isSelecting;

    [SerializeField] private UnitUI unitUI;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // ������������� selectionBox
        if (selectionBox != null)
        {
            selectionBoxImage = selectionBox.GetComponent<Image>();
            selectionBoxImage.color = selectionBoxColor;
            selectionBox.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        HandleSelectionInput();
        HandleMovementInput();
    }

    private void HandleMovementInput()
    {
        if (Input.GetMouseButtonDown(1)) // ������ ����
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                
                MoveSelectedUnits(hit.point, true);
            }
        }
    }
    private void MoveSelectedUnits(Vector3 center, bool clickMovement)
    {
        float radius = 2f; // ������ �����������
        int count = selectedUnits.Count;

        for (int i = 0; i < count; i++)
        {
            if (!selectedUnits[i].MoveAtWill) continue;
            if (clickMovement && !selectedUnits[i].mouseMove) continue;
            
            float angle = i * (360f / count);
            Vector3 pos = center + Quaternion.Euler(0, angle, 0) * Vector3.forward * radius;
            selectedUnits[i].SetDestination(pos);
        }
    }
    private void HandleSelectionInput()
    {
        
        // ������ ���������
        if (Input.GetMouseButtonDown(0))
        {
            isSelecting = true;
            startMousePosition = Input.mousePosition;
            selectionBox.gameObject.SetActive(true);
        }
        // ���������� ������� selectionBox
        if (Input.GetMouseButton(0))
        {
            UpdateSelectionBox();
        }

        // ���������� ���������
        if (Input.GetMouseButtonUp(0))
        {
            isSelecting = false;
            if (selectionBox.sizeDelta.magnitude < 10f)
            {
                SelectSingleUnit();
            }
            else
            {
                SelectUnitsInBox();
            }

            selectionBox.gameObject.SetActive(false);
        }
    }
    private void SelectSingleUnit()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, selectableLayer))
        {
            Unit unit = hit.collider.GetComponent<Unit>();
            if (unit != null)
            {
                // ���� ����� Shift/Ctrl - ��������� � ���������
                if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftControl))
                {
                    DeselectAll();
                }

                if (!selectedUnits.Contains(unit))
                {
                    DeselectBuildings();

                    selectedUnits.Add(unit);
                    unit.SetSelected(true);
                    unitUI.Initialize(unit);
                }
            }
            else
            {
                DeselectAll();
            }
        }
        else
        {
            DeselectAll();
        }
    }
    private void UpdateSelectionBox()
    {
        if (!selectionBox) return;

        // ������ ������� � ������� ��������������
        Vector2 currentMousePosition = Input.mousePosition;
        float width = currentMousePosition.x - startMousePosition.x;
        float height = currentMousePosition.y - startMousePosition.y;

        selectionBox.anchoredPosition = startMousePosition + new Vector2(width / 2, height / 2);
        selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
    }
    private void SelectUnitsInBox()
    {
        // ������� ����������� ���������
        DeselectAll();

        // �������� �������� ���������� ��������������
        Vector2 min = selectionBox.anchoredPosition - (selectionBox.sizeDelta / 2);
        Vector2 max = selectionBox.anchoredPosition + (selectionBox.sizeDelta / 2);
        Rect selectionRect = new Rect(min, max - min);

        // ������������ � viewport-����������
        Camera mainCamera = Camera.main;
        Vector3 viewportMin = mainCamera.ScreenToViewportPoint(min);
        Vector3 viewportMax = mainCamera.ScreenToViewportPoint(max);
        // ��������� ���� ������ �� �����
        foreach (Unit unit in FindObjectsOfType<Unit>())
        {
            if (unit.gameObject.layer != LayerMask.NameToLayer("Selectable")) continue;

            // �������� �������� ������� �����
            Vector3 screenPos = mainCamera.WorldToScreenPoint(unit.transform.position);

            // ���������, ��������� �� ����� ������ ��������������
            if (selectionRect.Contains(screenPos, true))
            {
                DeselectBuildings();

                selectedUnits.Add(unit);
                unit.SetSelected(true);
                unitUI.Initialize(unit);
            }
        }
    }

    public void DeselectAll()
    {
        
        foreach (Unit unit in selectedUnits)
        {
            unit.SetSelected(false);
            foreach (Transform child in unitUI.transform)
                Destroy(child.gameObject);
        }
        selectedUnits.Clear();
    }
    public void DeselectBuildings() {
        foreach (Building building in selectedBuildinds)
        {
            building.SetSelected(false);
        }
        selectedBuildinds.Clear();
    }
    // ��������� ����� ������ (TryCastAbility � TargetSelectionRoutine) ��� �����
    public void TryCastAbility(int abilityIndex)
    {
        // ��������: ���� �� ���������� �����
        if (selectedUnits.Count == 0)
        {
            Debug.Log("No units selected!");
            return;
        }

        // ����� ������� ����� (����� �������� ��� ������)
        Unit caster = selectedUnits[0];

        // ��������: ���������� �� �����������
        if (abilityIndex < 0 || abilityIndex >= caster.abilities.Length)
        {
            Debug.LogError($"Ability index {abilityIndex} is out of range!");
            return;
        }

        Ability ability = caster.abilities[abilityIndex];

        // ���� ����������� ������� ���� - ��������� �����
        if (ability.requiresTarget)
        {
            StartCoroutine(TargetSelectionCoroutine(caster, abilityIndex));
        }
        else // ���������� ��� ����
        {
            caster.UseAbility(abilityIndex, Vector3.zero);
        }
    }

    private IEnumerator TargetSelectionCoroutine(Unit caster, int abilityIndex)
    {
        Debug.Log("Select target...");
        bool targetChosen = false;
        Vector3 targetPosition = Vector3.zero;

        // ���� ����� �� ����� ��� �����
        while (!targetChosen)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    targetPosition = hit.point;
                    targetChosen = true;
                }
            }
            yield return null;
        }

        // ��������� �����������
        caster.UseAbility(abilityIndex, targetPosition);
    }
}