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

    public static bool HasLineOfSight(GameObject from, GameObject to)
    {
        Vector2 fromPosition2D = from.transform.position;
        Vector2 toPosition2D = to.transform.position;
        Vector2 direction = toPosition2D - fromPosition2D;

        ContactFilter2D filter = new ContactFilter2D();
        RaycastHit2D[] results = new RaycastHit2D[25];
        var raycastHit = Physics2D.Raycast(from.transform.position, direction, filter, results);

        foreach (var hit in results)
        {
            if (hit.collider == null)
            {
                break;
            }

            if (hit.collider.gameObject.layer == 7)
            {
                return false;
            }

            return true;
        }

        return true;
    }
}
