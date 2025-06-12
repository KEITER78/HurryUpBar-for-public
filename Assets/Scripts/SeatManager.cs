using System.Collections.Generic;
using UnityEngine;

public class SeatManager : MonoBehaviour
{
    public Transform[] seats;                     // ���Ȃ�Transform��Inspector����w��
    public bool[] seatOccupied;                   // �e���Ȃ���L����Ă��邩�ǂ������Ǘ�
    public List<SeatWaypoints> waypointsPerSeat;  // �e���ȂɑΉ�����E�F�C�|�C���g���X�g���w��
    public List<float> seatRotations; // ���Ȃ��Ƃ�Y����]�p�x���i�[���郊�X�g�iInspector�Őݒ�j

    // ���Ȃ��Ƃ̃E�F�C�|�C���g���i�[����J�X�^���N���X
    [System.Serializable]
    public class SeatWaypoints
    {
        public Transform[] waypoints;  // �e���ȂɑΉ�����E�F�C�|�C���g�̔z��
    }

    void Start()
    {
        // �S�Ă̍��Ȃ���Ȃɂ���
        //seatOccupied = new bool[seats.Length];
        //for (int i = 0; i < seatOccupied.Length; i++)
        //{
        //    seatOccupied[i] = false;
        //}
    }

    // �󂢂Ă�����Ȃ����邩�m�F����֐�
    public bool IsSeatAvailable(int seatIndex)
    {
        return !seatOccupied[seatIndex];  // ���Ȃ��󂢂Ă��邩��Ԃ�
    }

    // ���Ȃ�\�񂷂�֐�
    public void ReserveSeat(int seatIndex)
    {
        seatOccupied[seatIndex] = true;
    }

    // ���Ȃ��������֐�
    public void ReleaseSeat(int seatIndex)
    {
        seatOccupied[seatIndex] = false;

        // �Ȃ��󂢂����Ƃ�ʒm
        CustomerCallIcon.OnSeatAvailable();
    }

    // �󂢂Ă�����Ȃ̃��X�g��Ԃ��֐�
    public List<int> GetAvailableSeats()
    {
        List<int> availableSeats = new List<int>();
        for (int i = 0; i < seatOccupied.Length; i++)
        {
            if (!seatOccupied[i])
            {
                availableSeats.Add(i);  // �󂢂Ă�����Ȃ̃C���f�b�N�X��ǉ�
            }
        }
        return availableSeats;
    }

    // ���Ȃ�Transform���擾����֐�
    public Transform GetSeatTransform(int seatIndex)
    {
        return seats[seatIndex];
    }

    // ���ȂɑΉ�����E�F�C�|�C���g���擾����֐�
    public Transform[] GetWaypointsForSeat(int seatIndex)
    {
        return waypointsPerSeat[seatIndex].waypoints;  // �w�肳�ꂽ���Ȃ̃E�F�C�|�C���g��Ԃ�
    }
}