using System;
using System.IO;
using ScoringSystem;
using UnityEngine;

public static class SaveSystem
{
    public const string SAVE_FILENAME = "/save.json";

    public static void SaveScore(ScoreModel score)
    {
        string filePath = Application.persistentDataPath + SAVE_FILENAME;
        SaveData saveData = new();
        saveData.highScore = score.HighScore.Value;
        string txt = JsonUtility.ToJson(saveData);
        File.WriteAllText(filePath, txt);
    }

}

[Serializable]
public class SaveData
{
    public int highScore;
}
