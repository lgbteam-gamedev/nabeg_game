using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Fireball : MonoBehaviour
{
    [Header("Settings")]
    public float speed = 15f;
    public float damage = 30f;
    public float explosionRadius = 5f;
    public GameObject explosionEffect;

    private Vector3 targetPosition;
    private bool hasExploded = false;

    // Инициализация при выстреле
    public void Initialize(Vector3 target, float dmg, float radius)
    {
        targetPosition = target;
        damage = dmg;
        explosionRadius = radius;
    }

    void Update()
    {
        if (hasExploded) return;

        // Движение к цели
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            speed * Time.deltaTime
        );

        // Если достигли цели - взрыв
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            Explode();
        }
    }

    void Explode()
    {
        hasExploded = true;

        // Визуальный эффект взрыва
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        // Поиск целей в радиусе
        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hit in hits)
        {
            Unit unit = hit.GetComponent<Unit>();
            if (unit != null)
            {
                unit.TakeDamage(damage);
            }
        }

        Destroy(gameObject);
    }

    // Визуализация радиуса в редакторе
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}