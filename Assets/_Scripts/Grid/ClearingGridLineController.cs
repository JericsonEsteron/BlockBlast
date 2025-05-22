using System;
using System.Collections.Generic;
using EventMessage;
using UnityEngine;

namespace Grid
{
    public class ClearingGridLineController : MonoBehaviour
    {
        private List<List<GridSlot>> _gridSlotMatrix = new();

        private void OnEnable()
        {
            EventSubscription();    
        }

        private void EventSubscription()
        {
            EventMessenger.Default.Subscribe<PlaceBlockEvent>(OnLineCheck, gameObject);
            EventMessenger.Default.Subscribe<GridGenerationCompletedEvent>(OnGridCompleted, gameObject);
        }
        
        private void OnLineCheck(PlaceBlockEvent @event)
        {
            Invoke(nameof(CheckLines), .5f);
        }

        private void OnGridCompleted(GridGenerationCompletedEvent @event)
        {
            _gridSlotMatrix = @event.gridSlotMatrix;
        }
        
        private void CheckLines()
        {
            CheckAndClearFullRows();
            CheckAndClearFullColumns();
        }

        void CheckAndClearFullRows()
        {
            for (int row = 0; row < _gridSlotMatrix.Count; row++)
            {
                bool isFull = true;

                for (int col = 0; col < _gridSlotMatrix[row].Count; col++)
                {
                    if (!_gridSlotMatrix[row][col].IsOccupied)
                    {
                        isFull = false;
                        break;
                    }
                }

                if (isFull)
                {
                    for (int col = 0; col < _gridSlotMatrix[row].Count; col++)
                    {
                        _gridSlotMatrix[row][col].RemoveBlock();
                    }

                    Debug.Log($"Row {row} cleared!");
                }
            }
        }

        void CheckAndClearFullColumns()
        {
            if (_gridSlotMatrix.Count == 0) return;
            int columnCount = _gridSlotMatrix[0].Count;

            for (int col = 0; col < columnCount; col++)
            {
                bool isFull = true;

                for (int row = 0; row < _gridSlotMatrix.Count; row++)
                {
                    if (!_gridSlotMatrix[row][col].IsOccupied)
                    {
                        isFull = false;
                        break;
                    }
                }

                if (isFull)
                {
                    for (int row = 0; row < _gridSlotMatrix.Count; row++)
                    {
                        _gridSlotMatrix[row][col].RemoveBlock();
                    }

                    Debug.Log($"Column {col} cleared!");
                }
            }
        }



    }

}
