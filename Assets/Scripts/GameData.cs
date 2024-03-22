using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Security.Cryptography;
using Newtonsoft.Json;
using System.Text;

[Serializable]
public class SaveData
{
    public bool[] isActive;
    public int[] highScores;
    public int[] stars;
}

public class GameData : MonoBehaviour
{
    public static GameData gameData;
    public SaveData saveData;

    private byte[] secretKey = Encoding.ASCII.GetBytes("0987654321123456");
    private byte[] iv = Encoding.ASCII.GetBytes("1234567890123456");

    void Awake()
    {
        if (gameData == null)
        {
            DontDestroyOnLoad(this.gameObject);
            gameData = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        Load();
    }

    private void Start()
    {
        Debug.Log("Đường dẫn persistentDataPath: " + Application.persistentDataPath);
    }

    public void Save()
    {
        // Serialize dữ liệu thành chuỗi JSON
        string json = JsonConvert.SerializeObject(saveData, Formatting.Indented);

        // Mã hóa chuỗi JSON thành mảng byte
        byte[] encryptedBytes = EncryptString(json, secretKey, iv);

        // Lưu mảng byte vào file
        File.WriteAllBytes(Application.persistentDataPath + "/play1.json", encryptedBytes);
        Debug.Log("Saved");
    }

    public void Load()
    {
        string filePath = Application.persistentDataPath + "/play1.json";
        if (File.Exists(filePath))
        {
            // Đọc mảng byte từ file
            byte[] encryptedBytes = File.ReadAllBytes(filePath);

            // Giải mã mảng byte thành chuỗi JSON
            string decryptedJson = DecryptString(encryptedBytes, secretKey, iv);

            // Deserialize dữ liệu từ chuỗi JSON
            saveData = JsonConvert.DeserializeObject<SaveData>(decryptedJson);
            Debug.Log("Loaded");
        }
    }

    private void OnDisable()
    {
        Save();
    }

    private void Update()
    {

    }

    // Hàm mã hóa chuỗi sử dụng AES
    private byte[] EncryptString(string plainText, byte[] key, byte[] iv)
    {
        byte[] encrypted;
        //Tạo đối tượng Aes
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = key;
            aesAlg.IV = iv;

            //Tạo một đối tượng ICryptoTransform để thực hiện quá trình mã hóa
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
            // Sử dụng MemoryStream để lưu trữ dữ liệu đã được mã hóa.
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                // Sử dụng CryptoStream để thực hiện quá trình mã hóa.
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    // Sử dụng StreamWriter để ghi dữ liệu văn bản vào CryptoStream.
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }
                }

                encrypted = msEncrypt.ToArray();
            }
        }
        return encrypted;
    }

    // Hàm giải mã chuỗi sử dụng AES
    private string DecryptString(byte[] cipherText, byte[] key, byte[] iv)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = key;
            aesAlg.IV = iv;
            string plaintext = null;

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msDecrypt = new MemoryStream(cipherText))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }
            return plaintext;
        }
    }
}
