using UnityEngine;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour
{
    [Header("References")]
    public Image icon;
    public Image cooldownOverlay;
    public Text hotkeyText;

    private int abilityIndex; // Индекс способности в массиве abilities юнита

    // Инициализация кнопки (вызывается при создании UI)
    public void Setup(Ability ability, int index)
    {
        try {
            abilityIndex = index;
            icon.sprite = ability.icon;
            hotkeyText.text = (index + 1).ToString(); // Горячие клавиши 1,2,3...
        }
        catch { }
        // Подписываемся на событие кулдауна
        UpdateCooldown(ability.getCurrentCooldown());
        ability.OnCooldownUpdate.AddListener(UpdateCooldown);
    }

    // Обновление отображения кулдауна
    private void UpdateCooldown(float progress)
    {
        Debug.Log(progress);
        cooldownOverlay.fillAmount = progress;
    }

    // Вызывается при клике на кнопку
    public void OnClick()
    {
        UnitSelectionManager.Instance.TryCastAbility(abilityIndex);
    }
}