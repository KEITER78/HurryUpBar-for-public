using UnityEngine;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class SaveManager : MonoBehaviour
{
    // �Í����L�[�i16�����j
    private static readonly string encryptionKey = "t79gSMucaFBXSjgM";

    [System.Serializable]
    public class GameData
    {
        public int CurrentStageNumber;
        public float TotalRevenue;
    }

    // �t�@�C���p�X��public�ɕύX
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
        Debug.Log($"�Q�[���̏�Ԃ��ۑ�����܂��� (�Í���)�B�t�@�C���p�X: {SaveFilePath}");
    }

    public static (int, float) LoadGame()
    {
        if (!File.Exists(SaveFilePath))
        {
            Debug.LogWarning("�Z�[�u�f�[�^�����݂��܂���B�f�t�H���g�l��Ԃ��܂��B");
            return (1, 0f);
        }

        string encryptedJson = File.ReadAllText(SaveFilePath);
        string decryptedJson = Decrypt(encryptedJson);

        GameData data = JsonUtility.FromJson<GameData>(decryptedJson);

        Debug.Log("�Q�[���̏�Ԃ����[�h����܂��� (������)�B");
        return (data.CurrentStageNumber, data.TotalRevenue);
    }

    public static void ClearSave()
    {
        if (File.Exists(SaveFilePath))
        {
            File.Delete(SaveFilePath);
            Debug.Log("�Z�[�u�f�[�^���폜����܂����B");
        }
        else
        {
            Debug.LogWarning("�폜����Z�[�u�f�[�^�����݂��܂���B");
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
