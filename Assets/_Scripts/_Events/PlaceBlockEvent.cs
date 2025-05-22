using Block;
using Events;
using UnityEngine;

public class PlaceBlockEvent : IEvent
{
    public TetrominoController tetrominoController;
    public PlaceBlockEvent()
    {

    }

    public PlaceBlockEvent(TetrominoController tetrominoController)
    {
        this.tetrominoController = tetrominoController;
    }
    
}
