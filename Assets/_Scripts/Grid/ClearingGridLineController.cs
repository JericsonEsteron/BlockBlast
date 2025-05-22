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
            List<int> fullRows;
            List<int> fullCols;

            FindFullRowsAndColumns(out fullRows, out fullCols);
            ClearLines(fullRows, fullCols);

            var numOfLines = fullRows.Count + fullCols.Count;
            EventMessenger.Default.Publish(new LineClearedEvent(numOfLines));
        }

        void FindFullRowsAndColumns(out List<int> fullRows, out List<int> fullCols)
        {
            fullRows = new();
            fullCols = new();

            // Check full rows
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
                    fullRows.Add(row);
            }

            // Check full columns
            int columnCount = _gridSlotMatrix.Count > 0 ? _gridSlotMatrix[0].Count : 0;

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
                    fullCols.Add(col);
            }
        }

        void ClearLines(List<int> fullRows, List<int> fullCols)
        {
            foreach (int row in fullRows)
            {
                for (int col = 0; col < _gridSlotMatrix[row].Count; col++)
                {
                    _gridSlotMatrix[row][col].RemoveBlock();
                }
            }

            foreach (int col in fullCols)
            {
                for (int row = 0; row < _gridSlotMatrix.Count; row++)
                {
                    _gridSlotMatrix[row][col].RemoveBlock();
                }
            }
        }



    }

}
