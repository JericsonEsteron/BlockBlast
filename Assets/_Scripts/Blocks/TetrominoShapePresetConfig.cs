using System;
using UnityEngine;

[CreateAssetMenu(fileName = "TetrominoShapePresetConfig", menuName = "Scriptable Objects/TetrominoShapePresetConfig")]
public class TetrominoShapePresetConfig : ScriptableObject
{
    [Header("Element 0 Must Always be 0, 0")]
    [SerializeField] private TetrominoShape[] _tetrominoShapes;

    public TetrominoShape[] TetrominoShapes => _tetrominoShapes;
}

[Serializable]
public class TetrominoShape
{
    public string shapeName;
    public Vector2Int[] shape;
}
