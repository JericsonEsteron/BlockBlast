using UnityEngine;

public class LineClearedEvent : MonoBehaviour
{
    public int numOfLinesCleared;

    public LineClearedEvent(int numOfLinesCleared)
    {
        this.numOfLinesCleared = numOfLinesCleared;
    }
}
