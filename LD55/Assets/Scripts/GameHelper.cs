using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameHelper
{
    public static Vector2 MouseToWorldPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    public static bool IsWithinThreshold(Vector2 pos1, Vector2 pos2, float threshold)
    {
        return (pos1 - pos2).magnitude < threshold;
    }
}
