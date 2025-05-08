using UnityEngine;

public class CHOPWaypointBuilding : Building
{
    //[Header("CHOP Settings")]
    //public GameObject waypointMarker; // Визуальный маркер

    private void Start()
    {
        base.Start();
        unitLimitIncrease = 0; // Не влияет на лимит юнитов
        //waypointMarker.SetActive(false); // Пока скрываем маркер
    }

    // Позже добавим методы для работы с Waypoint
}