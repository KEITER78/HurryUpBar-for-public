using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Route
{
    public List<Transform> waypoints;
}

public class CarWaypointManager : MonoBehaviour
{
    // Singleton�I�ɃA�N�Z�X���邽�߂̃C���X�^���X
    public static CarWaypointManager Instance;

    // �����̃��[�g���܂Ƃ߂ĕێ�
    [Header("���[�g���Ƃ�Waypoint�ꗗ")]
    public List<Route> waypointRoutes;

    private void Awake()
    {
        // Singleton�p�^�[���i�P��C���X�^���X�j
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// �w�肵�� index �̃��[�g���擾
    /// </summary>
    public List<Transform> GetWaypoints(int routeIndex)
    {
        if (waypointRoutes == null || waypointRoutes.Count == 0)
        {
            Debug.LogError("waypointRoutes���ݒ肳��Ă��܂���");
            return null;
        }

        if (routeIndex < 0 || routeIndex >= waypointRoutes.Count)
        {
            Debug.LogError("�����ȃ��[�gIndex���w�肳��܂���: " + routeIndex);
            return null;
        }

        return waypointRoutes[routeIndex].waypoints;
    }
}
