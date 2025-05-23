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

        private void OnEnable()
        {
            _canCheck = false;
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
                            continue;
                        }

                        var slot = _gridSlotMatrix[pos.x][pos.y];
                        if (slot.IsOccupied)
                        {
                            canPlace = false;
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
    }
}
