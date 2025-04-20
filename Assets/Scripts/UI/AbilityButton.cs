using UnityEngine;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour
{
    [Header("References")]
    public Image icon;
    public Image cooldownOverlay;
    public Text hotkeyText;

    private int abilityIndex; // ������ ����������� � ������� abilities �����

    // ������������� ������ (���������� ��� �������� UI)
    public void Setup(Ability ability, int index)
    {
        try {
            abilityIndex = index;
            icon.sprite = ability.icon;
            hotkeyText.text = (index + 1).ToString(); // ������� ������� 1,2,3...
        }
        catch { }
        // ������������� �� ������� ��������
        UpdateCooldown(ability.getCurrentCooldown());
        ability.OnCooldownUpdate.AddListener(UpdateCooldown);
    }

    // ���������� ����������� ��������
    private void UpdateCooldown(float progress)
    {
        Debug.Log(progress);
        cooldownOverlay.fillAmount = progress;
    }

    // ���������� ��� ����� �� ������
    public void OnClick()
    {
        UnitSelectionManager.Instance.TryCastAbility(abilityIndex);
    }
}