using UnityEngine;

public class CHOPWaypointBuilding : Building
{
    //[Header("CHOP Settings")]
    //public GameObject waypointMarker; // ���������� ������

    private void Start()
    {
        base.Start();
        unitLimitIncrease = 0; // �� ������ �� ����� ������
        //waypointMarker.SetActive(false); // ���� �������� ������
    }

    // ����� ������� ������ ��� ������ � Waypoint
}