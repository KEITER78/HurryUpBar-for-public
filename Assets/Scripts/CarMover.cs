using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMover : MonoBehaviour
{
    [Header("走行ルート設定")]
    [SerializeField] private int routeIndex;   // どのルートを走るか
    [SerializeField] private float speed = 10f; // 車のスピード
    [SerializeField] private float rotationDuration = 0.2f; // 回転にかかる時間
    [SerializeField] private float startDelay = 0f; // 開始時の遅延時間（秒）

    private List<Transform> waypoints;
    private int currentWaypointIndex = 0;

    // 現在の回転コルーチンを保持するための変数
    private Coroutine rotationCoroutine;

    // 一度だけ遅延を適用するためのフラグ
    private bool isFirstLap = true;

    // 移動を許可するフラグ
    private bool canMove = false;

    private void Start()
    {
        // CarWaypointManager から該当ルートを取得
        waypoints = CarWaypointManager.Instance.GetWaypoints(routeIndex);

        if (waypoints == null || waypoints.Count == 0)
        {
            Debug.LogError("指定されたルートにWaypointが存在しません。");
            return;
        }

        // 開始時に先頭のWaypointへ瞬間移動
        transform.position = waypoints[0].position;
        currentWaypointIndex = 0;

        // 次のWaypointを向く
        LookAtNextWaypoint();

        if (isFirstLap)
        {
            StartCoroutine(StartInitialDelay());
        }
        else
        {
            canMove = true;
        }

    }

    private void Update()
    {
        if (!canMove) return; // 移動が許可されていない場合は処理を中断

        if (waypoints == null || waypoints.Count == 0) return;

        // 現在向かうべきWaypoint
        Transform targetWaypoint = waypoints[currentWaypointIndex];

        // フレームあたりの移動距離
        float step = speed * Time.deltaTime;

        // 次のWaypointまでの距離
        float distanceToTarget = Vector3.Distance(transform.position, targetWaypoint.position);

        if (distanceToTarget <= step)
        {
            // 目的地(現在のWaypoint)まで一気にスナップ
            transform.position = targetWaypoint.position;

            // 次のWaypointへ進む
            currentWaypointIndex++;

            // 最後のWaypointを超えた場合は最初に戻す (D→A の瞬間移動)
            if (currentWaypointIndex >= waypoints.Count)
            {
                currentWaypointIndex = 0;
                transform.position = waypoints[0].position;
            }

            // 次のWaypointの方を向く
            LookAtNextWaypoint();
        }
        else
        {
            // 目的のWaypointまで step 分だけ移動
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, step);
        }
    }

    /// <summary>
    /// 次のWaypointの方を Y軸回転で向く
    /// </summary>
    private void LookAtNextWaypoint()
    {
        Vector3 direction = waypoints[currentWaypointIndex].position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 90, 0);

            // 既存の回転コルーチンが動作中なら停止
            if (rotationCoroutine != null)
            {
                StopCoroutine(rotationCoroutine);
            }

            // 新しい回転コルーチンを開始
            rotationCoroutine = StartCoroutine(RotateToTarget(targetRotation));
        }
    }

    // 回転を時間をかけて行うコルーチン
    private IEnumerator RotateToTarget(Quaternion targetRot)
    {
        Quaternion initialRot = transform.rotation;
        float elapsed = 0f;

        while (elapsed < rotationDuration)
        {
            transform.rotation = Quaternion.Slerp(initialRot, targetRot, elapsed / rotationDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRot;
        rotationCoroutine = null;
    }

    // 開始時の遅延を処理するコルーチン
    private IEnumerator StartInitialDelay()
    {
        yield return new WaitForSeconds(startDelay);
        canMove = true; // 移動を許可
        isFirstLap = false;
    }


}
