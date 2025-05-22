using UnityEngine;
using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";
    private bool encryptData = false;
    private string codeWord = "judosucks";

    public FileDataHandler(string _dataDirPath, string _dataFileName,bool _encrypyData)
    {
        dataDirPath = _dataDirPath;
        dataFileName = _dataFileName;
        encryptData = _encrypyData;
    }

    public void Save(GameData _data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try
        {
           Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
           string dataToStore = JsonUtility.ToJson(_data,true);
           if (encryptData)
           {
               dataToStore = EncryptDecrypt(dataToStore);
           }
           using (FileStream stream = new FileStream(fullPath,FileMode.Create))
           {
               using (StreamWriter writer = new StreamWriter(stream))
               {
                   writer.Write(dataToStore);
               }
               
           }
        }
        catch(Exception e)
        {
            Debug.LogError("error on trying to save data to file"+fullPath+"\n"+e);
        }
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";

                using (FileStream stram = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stram))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                if (encryptData)
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }
                loadData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("fuck this shit"+e);
            }
            
        }

        return loadData;
    }

    public void Delete()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        if(File.Exists(fullPath))
            File.Delete(fullPath);
    }

    private string EncryptDecrypt(string _data)
    {
        string modifiedData = "";
        for (int i = 0; i < _data.Length; i++)
        {
            modifiedData += (char)(_data[i] ^ codeWord[i % codeWord.Length]);
        }

        return modifiedData;
    }
}
