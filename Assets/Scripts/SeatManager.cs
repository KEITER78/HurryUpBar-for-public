using System.Collections.Generic;
using UnityEngine;

public class SeatManager : MonoBehaviour
{
    public Transform[] seats;                     // 座席のTransformをInspectorから指定
    public bool[] seatOccupied;                   // 各座席が占有されているかどうかを管理
    public List<SeatWaypoints> waypointsPerSeat;  // 各座席に対応するウェイポイントリストを指定
    public List<float> seatRotations; // 座席ごとのY軸回転角度を格納するリスト（Inspectorで設定）

    // 座席ごとのウェイポイントを格納するカスタムクラス
    [System.Serializable]
    public class SeatWaypoints
    {
        public Transform[] waypoints;  // 各座席に対応するウェイポイントの配列
    }

    void Start()
    {
        // 全ての座席を空席にする
        //seatOccupied = new bool[seats.Length];
        //for (int i = 0; i < seatOccupied.Length; i++)
        //{
        //    seatOccupied[i] = false;
        //}
    }

    // 空いている座席があるか確認する関数
    public bool IsSeatAvailable(int seatIndex)
    {
        return !seatOccupied[seatIndex];  // 座席が空いているかを返す
    }

    // 座席を予約する関数
    public void ReserveSeat(int seatIndex)
    {
        seatOccupied[seatIndex] = true;
    }

    // 座席を解放する関数
    public void ReleaseSeat(int seatIndex)
    {
        seatOccupied[seatIndex] = false;

        // 席が空いたことを通知
        CustomerCallIcon.OnSeatAvailable();
    }

    // 空いている座席のリストを返す関数
    public List<int> GetAvailableSeats()
    {
        List<int> availableSeats = new List<int>();
        for (int i = 0; i < seatOccupied.Length; i++)
        {
            if (!seatOccupied[i])
            {
                availableSeats.Add(i);  // 空いている座席のインデックスを追加
            }
        }
        return availableSeats;
    }

    // 座席のTransformを取得する関数
    public Transform GetSeatTransform(int seatIndex)
    {
        return seats[seatIndex];
    }

    // 座席に対応するウェイポイントを取得する関数
    public Transform[] GetWaypointsForSeat(int seatIndex)
    {
        return waypointsPerSeat[seatIndex].waypoints;  // 指定された座席のウェイポイントを返す
    }
}