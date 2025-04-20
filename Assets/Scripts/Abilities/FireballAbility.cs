using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballAbility : Ability
{
    public GameObject fireballPrefab;
    public float damage = 30f;
    public float explosionRadius = 5f;
    private void Awake()
    {
        requiresTarget = true;
    }

    protected override void Execute(Unit caster, Vector3 target)
    {
        GameObject fireball = Instantiate(fireballPrefab, caster.transform.position, Quaternion.identity);
        fireball.GetComponent<Fireball>().Initialize(target, damage, explosionRadius);
    }
}