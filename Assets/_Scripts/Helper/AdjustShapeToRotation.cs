using UnityEngine;

public static class AdjustShapeToRotation
{
    private static Vector2Int RotateOffset(Vector2Int offset, int angle)
    {
        // angle must be 0, 90, 180, or 270
        switch (angle % 360)
        {
            case 90: return new Vector2Int(-offset.y, offset.x);
            case 180: return new Vector2Int(-offset.x, -offset.y);
            case 270: return new Vector2Int(offset.y, -offset.x);
            default: return offset; // 0 degrees
        }
    }

    public static Vector2Int[] GetRotatedShape(Vector2Int[] originalShape, float zRotation)
    {
        int angle = Mathf.RoundToInt(zRotation) % 360;
        Vector2Int[] rotated = new Vector2Int[originalShape.Length];
        for (int i = 0; i < originalShape.Length; i++)
        {
            rotated[i] = RotateOffset(originalShape[i], angle);
        }
        return rotated;
    }


}
