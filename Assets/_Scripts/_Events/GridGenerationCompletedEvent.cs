using System.Collections.Generic;
using Events;
using Grid;
using UnityEngine;

public class GridGenerationCompletedEvent : IEvent
{
    public List<List<GridSlot>> gridSlotMatrix;
    public GridGenerationCompletedEvent(List<List<GridSlot>> gridSlotMatrix)
    {
        this.gridSlotMatrix = gridSlotMatrix;
    }
}
