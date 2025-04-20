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
        // Очистка старых кнопок
        foreach (Transform child in abilitiesPanel)
            Destroy(child.gameObject);

        name_and_decrPanel.GetComponent<TMP_Text>().text = unit.Descr;
        photoPanel.GetComponent<Image>().sprite = unit.Photo;
        
        // Создание кнопок для каждой способности
        for (int i = 0; i < unit.abilities.Length; i++)
        {
            Ability ability = unit.abilities[i];

            // Создаем кнопку
            GameObject buttonObj = Instantiate(abilityButtonPrefab, abilitiesPanel);
            buttonObj.GetComponentInChildren<TMP_Text>().text = ability.name;
            AbilityButton button = buttonObj.GetComponent<AbilityButton>();

            // Инициализируем кнопку с индексом способности
            button.Setup(ability, i);
        }
    }
}