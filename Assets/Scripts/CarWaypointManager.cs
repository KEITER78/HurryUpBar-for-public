using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Route
{
    public List<Transform> waypoints;
}

public class CarWaypointManager : MonoBehaviour
{
    // Singleton的にアクセスするためのインスタンス
    public static CarWaypointManager Instance;

    // 複数のルートをまとめて保持
    [Header("ルートごとのWaypoint一覧")]
    public List<Route> waypointRoutes;

    private void Awake()
    {
        // Singletonパターン（単一インスタンス）
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
    /// 指定した index のルートを取得
    /// </summary>
    public List<Transform> GetWaypoints(int routeIndex)
    {
        if (waypointRoutes == null || waypointRoutes.Count == 0)
        {
            Debug.LogError("waypointRoutesが設定されていません");
            return null;
        }

        if (routeIndex < 0 || routeIndex >= waypointRoutes.Count)
        {
            Debug.LogError("無効なルートIndexが指定されました: " + routeIndex);
            return null;
        }

        return waypointRoutes[routeIndex].waypoints;
    }
}
