using System;
using System.IO;
using UnityEngine;

public static class LoadSystem
{
    public static SaveData LoadSaveData()
    {
        try
        {
            string filePath = Application.persistentDataPath + SaveSystem.SAVE_FILENAME;
            string content = File.ReadAllText(filePath);
            SaveData saveData = JsonUtility.FromJson<SaveData>(content);
            return saveData;

        }
        catch (Exception e)
        {
            Debug.Log(e);
            return null;
        }
    }
}
