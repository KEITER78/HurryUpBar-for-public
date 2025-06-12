using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMover : MonoBehaviour
{
    [Header("���s���[�g�ݒ�")]
    [SerializeField] private int routeIndex;   // �ǂ̃��[�g�𑖂邩
    [SerializeField] private float speed = 10f; // �Ԃ̃X�s�[�h
    [SerializeField] private float rotationDuration = 0.2f; // ��]�ɂ����鎞��
    [SerializeField] private float startDelay = 0f; // �J�n���̒x�����ԁi�b�j

    private List<Transform> waypoints;
    private int currentWaypointIndex = 0;

    // ���݂̉�]�R���[�`����ێ����邽�߂̕ϐ�
    private Coroutine rotationCoroutine;

    // ��x�����x����K�p���邽�߂̃t���O
    private bool isFirstLap = true;

    // �ړ���������t���O
    private bool canMove = false;

    private void Start()
    {
        // CarWaypointManager ����Y�����[�g���擾
        waypoints = CarWaypointManager.Instance.GetWaypoints(routeIndex);

        if (waypoints == null || waypoints.Count == 0)
        {
            Debug.LogError("�w�肳�ꂽ���[�g��Waypoint�����݂��܂���B");
            return;
        }

        // �J�n���ɐ擪��Waypoint�֏u�Ԉړ�
        transform.position = waypoints[0].position;
        currentWaypointIndex = 0;

        // ����Waypoint������
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
        if (!canMove) return; // �ړ���������Ă��Ȃ��ꍇ�͏����𒆒f

        if (waypoints == null || waypoints.Count == 0) return;

        // ���݌������ׂ�Waypoint
        Transform targetWaypoint = waypoints[currentWaypointIndex];

        // �t���[��������̈ړ�����
        float step = speed * Time.deltaTime;

        // ����Waypoint�܂ł̋���
        float distanceToTarget = Vector3.Distance(transform.position, targetWaypoint.position);

        if (distanceToTarget <= step)
        {
            // �ړI�n(���݂�Waypoint)�܂ň�C�ɃX�i�b�v
            transform.position = targetWaypoint.position;

            // ����Waypoint�֐i��
            currentWaypointIndex++;

            // �Ō��Waypoint�𒴂����ꍇ�͍ŏ��ɖ߂� (D��A �̏u�Ԉړ�)
            if (currentWaypointIndex >= waypoints.Count)
            {
                currentWaypointIndex = 0;
                transform.position = waypoints[0].position;
            }

            // ����Waypoint�̕�������
            LookAtNextWaypoint();
        }
        else
        {
            // �ړI��Waypoint�܂� step �������ړ�
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, step);
        }
    }

    /// <summary>
    /// ����Waypoint�̕��� Y����]�Ō���
    /// </summary>
    private void LookAtNextWaypoint()
    {
        Vector3 direction = waypoints[currentWaypointIndex].position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 90, 0);

            // �����̉�]�R���[�`�������쒆�Ȃ��~
            if (rotationCoroutine != null)
            {
                StopCoroutine(rotationCoroutine);
            }

            // �V������]�R���[�`�����J�n
            rotationCoroutine = StartCoroutine(RotateToTarget(targetRotation));
        }
    }

    // ��]�����Ԃ������čs���R���[�`��
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

    // �J�n���̒x������������R���[�`��
    private IEnumerator StartInitialDelay()
    {
        yield return new WaitForSeconds(startDelay);
        canMove = true; // �ړ�������
        isFirstLap = false;
    }


}
