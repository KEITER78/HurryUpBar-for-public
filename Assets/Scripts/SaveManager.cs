using UnityEngine;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class SaveManager : MonoBehaviour
{
    // 暗号化キー（16文字）
    private static readonly string encryptionKey = "t79gSMucaFBXSjgM";

    [System.Serializable]
    public class GameData
    {
        public int CurrentStageNumber;
        public float TotalRevenue;
    }

    // ファイルパスをpublicに変更
    public static string SaveFilePath => Path.Combine(Application.persistentDataPath, "saveData.json");

    public static void SaveGame(int currentStageNumber, float totalRevenue)
    {
        GameData data = new GameData
        {
            CurrentStageNumber = currentStageNumber,
            TotalRevenue = totalRevenue
        };

        string json = JsonUtility.ToJson(data, true);
        string encryptedJson = Encrypt(json);

        File.WriteAllText(SaveFilePath, encryptedJson);
        Debug.Log($"ゲームの状態が保存されました (暗号化)。ファイルパス: {SaveFilePath}");
    }

    public static (int, float) LoadGame()
    {
        if (!File.Exists(SaveFilePath))
        {
            Debug.LogWarning("セーブデータが存在しません。デフォルト値を返します。");
            return (1, 0f);
        }

        string encryptedJson = File.ReadAllText(SaveFilePath);
        string decryptedJson = Decrypt(encryptedJson);

        GameData data = JsonUtility.FromJson<GameData>(decryptedJson);

        Debug.Log("ゲームの状態がロードされました (復号化)。");
        return (data.CurrentStageNumber, data.TotalRevenue);
    }

    public static void ClearSave()
    {
        if (File.Exists(SaveFilePath))
        {
            File.Delete(SaveFilePath);
            Debug.Log("セーブデータが削除されました。");
        }
        else
        {
            Debug.LogWarning("削除するセーブデータが存在しません。");
        }
    }

    private static string Encrypt(string plainText)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(encryptionKey);
            aes.IV = new byte[16];

            using (MemoryStream ms = new MemoryStream())
            using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
            {
                using (StreamWriter sw = new StreamWriter(cs))
                {
                    sw.Write(plainText);
                }
                return System.Convert.ToBase64String(ms.ToArray());
            }
        }
    }

    private static string Decrypt(string cipherText)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(encryptionKey);
            aes.IV = new byte[16];

            using (MemoryStream ms = new MemoryStream(System.Convert.FromBase64String(cipherText)))
            using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read))
            using (StreamReader sr = new StreamReader(cs))
            {
                return sr.ReadToEnd();
            }
        }
    }
}
