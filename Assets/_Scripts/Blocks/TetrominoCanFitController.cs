using System.Collections;
using System.Collections.Generic;
using Block;
using EventMessage;
using Grid;
using UnityEngine;

namespace Block
{
    public class TetrominoCanFitController : Singleton<TetrominoCanFitController>
    {
        private List<List<GridSlot>> _gridSlotMatrix = new();
        private List<TetrominoController> _currentTetrominos = new();

        private bool _canCheck;
        private bool _isNewSet;

        private void OnEnable()
        {
            _canCheck = false;
            _isNewSet = false;
            EventSubscription();
        }

        private void EventSubscription()
        {
            EventMessenger.Default.Subscribe<TetrominoSetGeneratedEvent>(OnTetrominoSetGenerated, gameObject);
            EventMessenger.Default.Subscribe<GridGenerationCompletedEvent>(OnGridGenerated, gameObject);
            EventMessenger.Default.Subscribe<LineClearedEvent>(OnLineCleared, gameObject);
            EventMessenger.Default.Subscribe<PlaceBlockEvent>(OnBlockPlaced, gameObject);
        }

        private void OnLineCleared(LineClearedEvent @event)
        {
            _canCheck = true;
        }

        private void OnBlockPlaced(PlaceBlockEvent @event)
        {
            StartCoroutine(CanCheck(@event.tetrominoController));
        }

        private void OnGridGenerated(GridGenerationCompletedEvent @event)
        {
            _gridSlotMatrix = @event.gridSlotMatrix;
        }

        private void OnTetrominoSetGenerated(TetrominoSetGeneratedEvent @event)
        {
            _currentTetrominos = @event.tetrominoControllers;
        }

        private IEnumerator CanCheck(TetrominoController tetrominoController)
        {
            yield return new WaitUntil(() => _canCheck);
            // if (_isNewSet)
            // {
            //     _isNewSet = false;
            //     yield break;
            // }
            _canCheck = false;
            _currentTetrominos.Remove(tetrominoController);
            var canContinue = false;
            foreach (var tetromino in _currentTetrominos)
            {
                canContinue = AnyTetrominoFits(tetromino);
                if (canContinue) break;

            }

            if (!canContinue)
                EventMessenger.Default.Publish(new GameOverEvent());
        }


        private bool CanPlaceAt(List<List<GridSlot>> grid, Vector2Int[] shape, Vector2Int basePos)
        {
            bool canPlace = true;

            foreach (var offset in shape)
            {
                Vector2Int pos = basePos + offset;

                // Safety check
                if (pos.x < 0 || pos.x >= grid.Count || pos.y < 0 || pos.y >= grid[0].Count)
                {
                    canPlace = false;
                    DrawCell(pos, Color.red);
                    continue;
                }

                GridSlot slot = grid[pos.x][pos.y];

                if (slot.IsOccupied)
                {
                    canPlace = false;
                    DrawCell(pos, Color.yellow);
                }
                else
                {
                    DrawCell(pos, Color.green);
                }
            }

            return canPlace;
        }

        public bool AnyTetrominoFits(TetrominoController tetrominoController)
        {
            int gridWidth = _gridSlotMatrix.Count;
            int gridHeight = _gridSlotMatrix[0].Count;

            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    Vector2Int basePos = new Vector2Int(x, y);
                    bool canPlace = true;

                    foreach (var offset in tetrominoController.TetrominoShape.shape)
                    {
                        Vector2Int pos = basePos + offset;

                        // Check bounds
                        if (pos.x < 0 || pos.x >= gridWidth || pos.y < 0 || pos.y >= gridHeight)
                        {
                            canPlace = false;
#if UNITY_EDITOR
                            DrawCell(pos, Color.red);
#endif
                            continue;
                        }

                        var slot = _gridSlotMatrix[pos.x][pos.y];
                        if (slot.IsOccupied)
                        {
                            canPlace = false;
#if UNITY_EDITOR
                            DrawCell(pos, Color.red);
#endif
                        }
                        else
                        {
#if UNITY_EDITOR
                            DrawCell(pos, Color.green);
#endif
                        }
                    }

                    if (canPlace)
                    {
                        return true; // Found a valid placement
                    }
                }
            }

            return false; // No valid placement found

        }

#if UNITY_EDITOR
        float duration = 5f;
        private void DrawCell(Vector2Int gridPos, Color color)
        {
            if (!Application.isPlaying) return;

            Vector3 center = GridGenerator.Instance.GetWorldPosition(gridPos);

            // Get approximate cell size from a slot (assuming square)
            float size = 0.2f;

            Vector3 topLeft     = center + new Vector3(-size,  size);
            Vector3 topRight    = center + new Vector3( size,  size);
            Vector3 bottomRight = center + new Vector3( size, -size);
            Vector3 bottomLeft  = center + new Vector3(-size, -size);

            Debug.DrawLine(topLeft, topRight, color, duration);
            Debug.DrawLine(topRight, bottomRight, color, duration);
            Debug.DrawLine(bottomRight, bottomLeft, color, duration);
            Debug.DrawLine(bottomLeft, topLeft, color, duration);
        }
#endif
    }
}
