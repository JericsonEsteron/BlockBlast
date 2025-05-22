using System.Collections.Generic;
using Block;
using Events;
using UnityEngine;

public class TetrominoSetGeneratedEvent : IEvent
{
    public List<TetrominoController> tetrominoControllers;

    public TetrominoSetGeneratedEvent(List<TetrominoController> tetrominoControllers)
    {
        this.tetrominoControllers = tetrominoControllers;
    }
}
