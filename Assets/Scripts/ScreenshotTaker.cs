using UnityEngine;
using System.IO; // ファイルシステムを扱うために必要

public class ScreenshotTaker : MonoBehaviour
{
    public string screenshotBaseName = "screenshot"; // 基本のファイル名
    public string fileExtension = ".png"; // ファイル拡張子
    public int resolutionMultiplier = 1; // 解像度の倍率
    public string folderName = "Screenshots"; // スクリーンショットを保存するフォルダ名

    void Update()
    {
        // Pキーを押した時にスクリーンショットを撮る
        if (Input.GetKeyDown(KeyCode.P))
        {
            TakeScreenshot();
        }
    }

    void TakeScreenshot()
    {
        // ファイル名を作成する（既存のファイルがある場合は番号を付加）
        string path = GetUniqueFilePath();
        ScreenCapture.CaptureScreenshot(path, resolutionMultiplier);
        Debug.Log("Screenshot saved to: " + path);
    }

    // ユニークなファイルパスを生成するメソッド
    string GetUniqueFilePath()
    {
        // 保存先のディレクトリパスを取得（Assets/Screenshots フォルダ）
        string directoryPath = Path.Combine(Application.dataPath, folderName);

        // フォルダが存在しない場合は作成する
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
            Debug.Log("Directory created at: " + directoryPath);
        }

        // 最初のファイル名を作成
        string fileName = screenshotBaseName + fileExtension;
        string filePath = Path.Combine(directoryPath, fileName);

        int fileCounter = 1; // 番号を付けるためのカウンタ

        // 同じファイル名が存在するか確認し、存在する場合は番号を追加
        while (File.Exists(filePath))
        {
            fileName = screenshotBaseName + "_" + fileCounter + fileExtension;
            filePath = Path.Combine(directoryPath, fileName);
            fileCounter++;
        }

        return filePath;
    }
}
