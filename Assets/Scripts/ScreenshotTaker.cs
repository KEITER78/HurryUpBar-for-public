using UnityEngine;
using System.IO; // �t�@�C���V�X�e�����������߂ɕK�v

public class ScreenshotTaker : MonoBehaviour
{
    public string screenshotBaseName = "screenshot"; // ��{�̃t�@�C����
    public string fileExtension = ".png"; // �t�@�C���g���q
    public int resolutionMultiplier = 1; // �𑜓x�̔{��
    public string folderName = "Screenshots"; // �X�N���[���V���b�g��ۑ�����t�H���_��

    void Update()
    {
        // P�L�[�����������ɃX�N���[���V���b�g���B��
        if (Input.GetKeyDown(KeyCode.P))
        {
            TakeScreenshot();
        }
    }

    void TakeScreenshot()
    {
        // �t�@�C�������쐬����i�����̃t�@�C��������ꍇ�͔ԍ���t���j
        string path = GetUniqueFilePath();
        ScreenCapture.CaptureScreenshot(path, resolutionMultiplier);
        Debug.Log("Screenshot saved to: " + path);
    }

    // ���j�[�N�ȃt�@�C���p�X�𐶐����郁�\�b�h
    string GetUniqueFilePath()
    {
        // �ۑ���̃f�B���N�g���p�X���擾�iAssets/Screenshots �t�H���_�j
        string directoryPath = Path.Combine(Application.dataPath, folderName);

        // �t�H���_�����݂��Ȃ��ꍇ�͍쐬����
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
            Debug.Log("Directory created at: " + directoryPath);
        }

        // �ŏ��̃t�@�C�������쐬
        string fileName = screenshotBaseName + fileExtension;
        string filePath = Path.Combine(directoryPath, fileName);

        int fileCounter = 1; // �ԍ���t���邽�߂̃J�E���^

        // �����t�@�C���������݂��邩�m�F���A���݂���ꍇ�͔ԍ���ǉ�
        while (File.Exists(filePath))
        {
            fileName = screenshotBaseName + "_" + fileCounter + fileExtension;
            filePath = Path.Combine(directoryPath, fileName);
            fileCounter++;
        }

        return filePath;
    }
}
