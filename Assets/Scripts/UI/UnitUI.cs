using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UnitUI : MonoBehaviour
{
    [SerializeField] private Transform abilitiesPanel;
    [SerializeField] private Transform name_and_decrPanel;
    [SerializeField] private Transform photoPanel;
    [SerializeField] private GameObject abilityButtonPrefab;

    public void Initialize(Unit unit)
    {
        // ������� ������ ������
        foreach (Transform child in abilitiesPanel)
            Destroy(child.gameObject);

        name_and_decrPanel.GetComponent<TMP_Text>().text = unit.Descr;
        photoPanel.GetComponent<Image>().sprite = unit.Photo;
        
        // �������� ������ ��� ������ �����������
        for (int i = 0; i < unit.abilities.Length; i++)
        {
            Ability ability = unit.abilities[i];

            // ������� ������
            GameObject buttonObj = Instantiate(abilityButtonPrefab, abilitiesPanel);
            buttonObj.GetComponentInChildren<TMP_Text>().text = ability.name;
            AbilityButton button = buttonObj.GetComponent<AbilityButton>();

            // �������������� ������ � �������� �����������
            button.Setup(ability, i);
        }
    }
}