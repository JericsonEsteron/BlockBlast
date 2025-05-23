using UnityEngine;

public interface IInputReader 
{
    Vector2 PointerDelta{ get; }
    void EnableInputControl();
    void DisableInputControl();
}
