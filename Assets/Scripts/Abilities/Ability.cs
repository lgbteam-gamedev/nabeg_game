using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Ability : MonoBehaviour
{
    [Header("Base Settings")]
    public string abilityName;
    public Sprite icon;
    public float cooldown = 5f;
    public float energyCost = 10f;
    public bool requiresTarget = false;

    protected bool isOnCooldown = false;
    protected float currentCooldown = 0;
    private Coroutine cooldownCoroutine;

    // События для UI
    public UnityEvent<float> OnCooldownUpdate; // Передает текущий кулдаун

    public virtual bool CanActivate(Unit unit)
    {
        return !isOnCooldown && unit.currentEnergy >= energyCost;
    }

    public virtual void Activate(Unit unit, Vector3 target = default)
    {
        if (!CanActivate(unit))
        {
            Debug.Log("Способность не может быть активирована!");
            return;
        }

        // Остановить предыдущий кулдаун (если был)
        if (cooldownCoroutine != null)
            StopCoroutine(cooldownCoroutine);

        cooldownCoroutine = StartCoroutine(CooldownRoutine());
        Execute(unit, target);
    }

    protected abstract void Execute(Unit unit, Vector3 target);

    private IEnumerator CooldownRoutine()
    {
        isOnCooldown = true;
        currentCooldown = cooldown;


        while (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
            OnCooldownUpdate?.Invoke(currentCooldown / cooldown);
            yield return null;
        }

        isOnCooldown = false;
    }
    public float getCurrentCooldown()
    {
        return currentCooldown;
    }
}