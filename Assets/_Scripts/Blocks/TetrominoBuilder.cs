using System.Collections.Generic;
using Block;
using UnityEngine;
using UnityEngine.Pool;

public class TetrominoBuilder : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private BoxCollider2D _collider;
    [SerializeField] private float _gridPadding = 0.01f;
    [SerializeField] private Vector2 _slotSize = Vector2.one; // Same as GridSlot prefab's localScale

    private List<BlockController> _blockControllers = new();

    /// <summary>
    /// Builds a Tetromino shape centered at world position.
    /// The returned GameObject is the pivot-centered Tetromino root.
    /// </summary>
    public List<BlockController> BuildTetromino(Vector2Int[] shape, GameObject tetrominoRoot, Vector3 worldPosition, IObjectPool<BlockController> pool)
    {
        // Calculate center offset
        Vector3 offset = CalculateShapeCenterOffset(shape);
        _blockControllers.Clear();

        foreach (var cell in shape)
        {
            BlockController block = pool.Get();
            block.transform.parent = tetrominoRoot.transform;

            Vector3 localPos = new Vector3(
                cell.x * (_slotSize.x + _gridPadding),
                cell.y * (_slotSize.y + _gridPadding),
                0f
            );

            _blockControllers.Add(block);

            block.transform.localPosition = localPos - offset;
        }

        tetrominoRoot.transform.position = worldPosition;
        return _blockControllers;
    }

    // Calculates center offset for the tetromino shape (to center its pivot).
    private Vector3 CalculateShapeCenterOffset(Vector2Int[] shape)
    {
        int minX = int.MaxValue, maxX = int.MinValue;
        int minY = int.MaxValue, maxY = int.MinValue;

        foreach (var offset in shape)
        {
            if (offset.x < minX) minX = offset.x;
            if (offset.x > maxX) maxX = offset.x;
            if (offset.y < minY) minY = offset.y;
            if (offset.y > maxY) maxY = offset.y;
        }

        float width = (maxX - minX + 1) * (_slotSize.x + _gridPadding);
        float height = (maxY - minY + 1) * (_slotSize.y + _gridPadding);

        FitCollider(width, height);

        return new Vector3(width / 2f - _slotSize.x / 2f, height / 2f - _slotSize.y / 2f, 0f);
    }

    private void FitCollider(float width, float height)
    {
        Vector2 colliderSize = new Vector2( width, height );
        _collider.size = colliderSize * 2;
        _collider.offset = Vector2.zero;
    }
}
