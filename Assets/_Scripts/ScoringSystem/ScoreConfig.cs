using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ScoreConfig", menuName = "Scriptable Objects/ScoreConfig")]
public class ScoreConfig : ScriptableObject
{
    [SerializeField] int _pointPerBlock = 1;
    [SerializeField] float _pointsPerLinesCleared = 10;
    [SerializeField] private ScoreStats[] _scoreStats;

    public int PointPerBlock => _pointPerBlock;
    public float PointsPerLinesCleared => _pointsPerLinesCleared;
    public ScoreStats[] ScoreStats => _scoreStats;

}

[Serializable]
public class ScoreStats
{
    public int numOfLines;
    public float scoreMultiplier;
}
