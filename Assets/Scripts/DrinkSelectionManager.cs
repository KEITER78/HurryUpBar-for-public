using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DrinkSelectionManager : MonoBehaviour
{
    //public SoundManager soundManager; // SoundManager の参照を持つ

    public GameObject drinkSelectionCanvas; // ドリンク選択画面のCanvas
    public Image[] drinkImages; // SakeA ~ SakeFのドリンク画像
    public GameObject[] drinkPrefabs; // ドリンクに対応するPrefabアセット
    public Transform player; // プレイヤー
    public GameObject[] currentDrinks = new GameObject[2]; // プレイヤーが持てるドリンクを2つにする
    public Transform kitchenLocation; // キッチンの位置
    public FirstPersonMovement playerMovement; // プレイヤーの移動と視点移動を管理するスクリプト
    public GameObject drinkSpotCanvas; // ドリンクスポットアイコンの画像

    public Button removeDrinksButton; // 削除ボタンをInspectorで指定
    //public Image removeDrinksButtonImage; // 削除ボタンをImageとしてInspectorで指定
    //public Sprite removeDrinksButtonSprite; // 削除ボタンのSourceImageをInspectorで指定
    //public Sprite removeDrinksButtonSprite_en; // 削除ボタンのSourceImageをInspectorで指定_en

    public Vector3 firstDrinkPositionOffset = new Vector3(0, 0, 2);
    public Vector3 firstDrinkRotationOffset = Vector3.zero;
    public Vector3 firstDrinkScale = Vector3.one;

    public Vector3 secondDrinkPositionOffset = new Vector3(0, 0, 2);
    public Vector3 secondDrinkRotationOffset = Vector3.zero;
    public Vector3 secondDrinkScale = Vector3.one;


    // Trigger DistanceをInspectorで設定可能に
    public float triggerDistance = 3f; // プレイヤーがキッチンに近づく距離の閾値

    // メッセージ表示に使用するSprite
    public Sprite openMenuSprite; // ドリンク選択画面を開くメッセージのSprite
    public Sprite closeMenuSprite; // ドリンク選択画面を閉じるメッセージのSprite
    public GameObject messageCanvas; // メッセージを表示するCanvas
    public Image messageImage; // メッセージ表示用のImage
    public TMP_Text messageText; // メッセージ表示用のText

    private bool isNearKitchen = false; // プレイヤーがキッチンに近いかどうか
    private bool isSelectionDisplayed = false; // ドリンク選択画面が表示されているかどうか

    // キー設定用の変数
    private KeyCode confirmKey;

    // 各種アイコンをInspectorで指定
    //public Image drinkSelection; // ドリンク選択画面の背景Image
    //public Sprite drinkSelectionSprite; // ドリンク選択画面の背景のSprite
    //public Sprite drinkSelectionSprite_en; // ドリンク選択画面の背景のSprite_en
    //public Image drinkSpotIcon; // ドリンク選択場所を示すImage
    //public Sprite drinkSpotIconSprite; // ドリンク選択場所を示す画像のSprite
    //public Sprite drinkSpotIconSprite_en; // ドリンク選択場所を示す画像のSprite_en

    // 言語設定のインデックス
    private int languageIndex;


    void Start()
    {
        // PlayerPrefsからキー設定を取得（デフォルト値は指定された値）
        confirmKey = (KeyCode)PlayerPrefs.GetInt("ConfirmKey", (int)KeyCode.E);

        // PlayerPrefsから言語設定のインデックスを取得
        languageIndex = PlayerPrefs.GetInt("Language", 0); // デフォルト値は0

        // 初期状態ではドリンク選択UIは非表示
        drinkSelectionCanvas.SetActive(false);

        // 初期状態ではメッセージ用Canvasも非表示
        messageCanvas.SetActive(false);

        // 各ドリンク画像にクリックイベントを追加
        for (int i = 0; i < drinkImages.Length; i++)
        {
            int index = i; // ローカル変数にindexを保存
            drinkImages[i].GetComponent<Button>().onClick.AddListener(() => SelectDrink(index));
        }

        //SoundManagerの設定がされているか確認
        //if (soundManager == null)
        //{
        //    Debug.LogError("SoundManagerが設定されていません。");
        //}

        // 削除ボタンにクリックイベントを追加
        removeDrinksButton.onClick.AddListener(RemoveDrinks);

        // 各種ImageにSpriteを設定 // enを追加実装すべし
        //removeDrinksButtonImage.sprite = removeDrinksButtonSprite;
        //drinkSelection.sprite = drinkSelectionSprite;
        //drinkSpotIcon.sprite = drinkSpotIconSprite;
    }

    void Update()
    {

        // DrinkSpotIconをプレイヤーの方向に向ける
        Vector3 directionToDrinkSpotIcon = player.position - drinkSpotCanvas.transform.position;
        directionToDrinkSpotIcon.y = 0; // Y軸の回転を防ぐ
        drinkSpotCanvas.transform.rotation = Quaternion.LookRotation(directionToDrinkSpotIcon);
        drinkSpotCanvas.transform.Rotate(0, 180, 0);

        // プレイヤーとキッチンの距離を測る
        if (Vector3.Distance(player.position, kitchenLocation.position) < triggerDistance)
        {
            isNearKitchen = true; // キッチンに近づいたことをフラグで管理

            // メッセージを表示する
            ShowMessage();
        }
        else
        {
            isNearKitchen = false; // 距離が遠ければフラグをfalseにする

            // メッセージを非表示にする
            HideMessage();
        }

        // キッチンに近づいてEボタンを押したとき
        if (isNearKitchen && Input.GetKeyDown(confirmKey))
        {
            if (isSelectionDisplayed)
            {
                // 効果音（決定音）を再生
                SoundManager.instance.PlayDecisionSound();

                // ドリンク選択画面を閉じる
                CloseDrinkSelection();
            }
            else
            {
                // 効果音（決定音）を再生
                SoundManager.instance.PlayDecisionSound();

                // ドリンク選択画面を開く
                ShowDrinkSelection();
            }
        }
    }

    private void ShowDrinkSelection()
    {
        // ドリンク選択画面を表示
        drinkSelectionCanvas.SetActive(true);
        isSelectionDisplayed = true;

        // プレイヤーの移動と視点移動を無効化
        playerMovement.DisableMovement();
        playerMovement.DisableLook();

        // メッセージをCloseMenuに切り替える
        //messageImage.sprite = closeMenuSprite;
        
        switch (languageIndex)
        {
            case 0: // 英語
                messageText.text = $"Press <size=90>{confirmKey}</size> to close";
                break;
            case 1: // 日本語
                messageText.text = $"<size=90>{confirmKey}</size> を押して閉じる";
                break;
        }
        messageCanvas.SetActive(true);

        // マウスカーソルを表示
        playerMovement.UnlockCursor();
    }

    private void CloseDrinkSelection()
    {
        // ドリンク選択画面を非表示にする
        drinkSelectionCanvas.SetActive(false);
        isSelectionDisplayed = false;

        // プレイヤーの移動と視点移動を有効化
        playerMovement.EnableMovement();
        playerMovement.EnableLook();

        // メッセージをOpenMenuに切り替える
        //messageImage.sprite = openMenuSprite;       
        switch (languageIndex)
        {
            case 0: // 英語
                messageText.text = $"Press <size=90>{confirmKey}</size> to display the menu";
                break;
            case 1: // 日本語
                messageText.text = $"<size=90>{confirmKey}</size> を押してメニューを表示";
                break;
        }
        messageCanvas.SetActive(true);

        // マウスカーソルを非表示
        playerMovement.LockCursor();
    }

    private void SelectDrink(int index)
    {
        // 効果音（クリック音）を再生
        SoundManager.instance.PlayClickSound();

        // すでに2つドリンクを持っている場合は1つ目を削除し、2つ目を1つ目に移す
        if (currentDrinks[0] != null && currentDrinks[1] != null)
        {
            // 1つ目のドリンクを削除
            Destroy(currentDrinks[0]);

            // 2つ目のドリンクも削除して、新たに1つ目の位置に生成し直す
            GameObject secondDrinkPrefab = currentDrinks[1]; // 2つ目のドリンクを一時保存
            Destroy(currentDrinks[1]); // 2つ目のドリンクを削除
            currentDrinks[1] = null; // 2つ目のスロットをクリア

            // 2つ目だったドリンクを1つ目のオフセット位置に再生成
            Vector3 firstDrinkPosition = player.position + player.TransformDirection(firstDrinkPositionOffset);
            Quaternion firstDrinkRotation = Quaternion.Euler(firstDrinkRotationOffset);
            GameObject newFirstDrink = Instantiate(secondDrinkPrefab, firstDrinkPosition, firstDrinkRotation);
            newFirstDrink.transform.localScale = firstDrinkScale; // スケールを適用
            newFirstDrink.transform.SetParent(player);
            currentDrinks[0] = newFirstDrink; // 1つ目に再設定
        }

        // 新しいドリンクを生成し、位置・回転・スケールを適用
        if (currentDrinks[0] == null)
        {
            // 1つ目のドリンク
            Vector3 spawnPosition = player.position + player.TransformDirection(firstDrinkPositionOffset); // 1つ目のドリンクの位置オフセットを使用
            Quaternion spawnRotation = Quaternion.Euler(firstDrinkRotationOffset); // 1つ目のドリンクの回転オフセットを使用
            GameObject newDrink = Instantiate(drinkPrefabs[index], spawnPosition, spawnRotation);
            newDrink.transform.localScale = firstDrinkScale; // 1つ目のドリンクのスケールを適用
            newDrink.transform.SetParent(player);
            currentDrinks[0] = newDrink;
        }
        else
        {
            // 2つ目のドリンク
            Vector3 spawnPosition = player.position + player.TransformDirection(secondDrinkPositionOffset); // 2つ目のドリンクの位置オフセットを使用
            Quaternion spawnRotation = Quaternion.Euler(secondDrinkRotationOffset); // 2つ目のドリンクの回転オフセットを使用
            GameObject newDrink = Instantiate(drinkPrefabs[index], spawnPosition, spawnRotation);
            newDrink.transform.localScale = secondDrinkScale; // 2つ目のドリンクのスケールを適用
            newDrink.transform.SetParent(player);
            currentDrinks[1] = newDrink;
        }

        // ドリンク選択画面を閉じる
        CloseDrinkSelection();
    }

    // ドリンクを削除するボタンに対応するメソッド
    public void RemoveDrinks()
    {
        // 効果音（クリック音）を再生
        SoundManager.instance.PlayClickSound();

        // ドリンクを全て削除
        if (currentDrinks[0] != null)
        {
            Destroy(currentDrinks[0]);
            currentDrinks[0] = null;
        }
        if (currentDrinks[1] != null)
        {
            Destroy(currentDrinks[1]);
            currentDrinks[1] = null;
        }

        // ドリンク選択画面を閉じる
        CloseDrinkSelection();
    }

    private void ShowMessage()
    {
        // ドリンク選択画面の状態に応じてメッセージを表示
        if (isSelectionDisplayed)
        {
            // CloseMenuメッセージを表示
            //messageImage.sprite = closeMenuSprite;
            switch (languageIndex)
            {
                case 0: // 英語
                    messageText.text = $"Press <size=90>{confirmKey}</size> to close";
                    break;
                case 1: // 日本語
                    messageText.text = $"<size=90>{confirmKey}</size> を押して閉じる";
                    break;
            }
        }
        else
        {
            // OpenMenuメッセージを表示
            //messageImage.sprite = openMenuSprite;
            switch (languageIndex)
            {
                case 0: // 英語
                    messageText.text = $"Press <size=90>{confirmKey}</size> to display the menu";
                    break;
                case 1: // 日本語
                    messageText.text = $"<size=90>{confirmKey}</size> を押してメニューを表示";
                    break;
            }
        }

        // メッセージを表示
        messageCanvas.SetActive(true);
    }

    private void HideMessage()
    {
        // メッセージを非表示にする
        messageCanvas.SetActive(false);
    }

    public void LoadKeySettings()
    {
        // PlayerPrefsからキー設定を取得（デフォルト値は指定された値）
        confirmKey = (KeyCode)PlayerPrefs.GetInt("ConfirmKey", (int)KeyCode.E);
    }
}
