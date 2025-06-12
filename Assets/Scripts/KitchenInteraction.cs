using UnityEngine;

public class KitchenInteraction : MonoBehaviour
{
    public GameObject drinkSelectionCanvas; // ドリンク選択画面のCanvas
    public Transform player; // プレイヤー

    private bool isNearKitchen = false; // プレイヤーがキッチンに近いかどうか

    void Start()
    {
        // 初期状態ではドリンク選択UIは非表示
        drinkSelectionCanvas.SetActive(false);
    }

    void Update()
    {
        // プレイヤーとキッチンの距離を測る
        if (Vector3.Distance(player.position, transform.position) < 3f)
        {
            isNearKitchen = true; // キッチンに近づいたことをフラグで管理
        }
        else
        {
            isNearKitchen = false; // 距離が遠ければフラグをfalseにする
        }

        // キッチンに近づいてEボタンを押したとき
        if (isNearKitchen && Input.GetKeyDown(KeyCode.E))
        {
            // ドリンク選択UIを表示
            ShowDrinkSelection();
        }
    }

    private void ShowDrinkSelection()
    {
        // ドリンク選択画面を表示
        drinkSelectionCanvas.SetActive(true);
    }
}
