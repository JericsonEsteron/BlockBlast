using UnityEngine;

public class TetrominoTest : MonoBehaviour
{
    [SerializeField] TetrominoBuilder _builder;

    private void Start()
    {
        var oShape = new Vector2Int[]
        {
            new Vector2Int(0, 0),
            new Vector2Int(0, 1),
            new Vector2Int(0, 2),
            new Vector2Int(1, 0),
        };

        // Center of screen or desired position
        Vector3 worldPos = Vector3.zero;

    }
}
