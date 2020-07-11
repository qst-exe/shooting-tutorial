using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    public static Vector2 m_moveList = new Vector2(4.7f, 3.4f);

    public static Vector3 ClampPosition(Vector3 position)
    {
        return new Vector3(
          Mathf.Clamp(position.x, -m_moveList.x, m_moveList.x),
          Mathf.Clamp(position.y, -m_moveList.y, m_moveList.y),
          0
          );
    }

    public static float GetAngle(Vector2 from, Vector2 to)
    {
        var dx = to.x - from.x;
        var dy = to.y - from.y;
        var rad = Mathf.Atan2(dy, dx);
        return rad * Mathf.Rad2Deg;
    }

    public static Vector3 GetDirection(float angle)
    {
        return new Vector3
        (
            Mathf.Cos(angle * Mathf.Deg2Rad),
            Mathf.Sin(angle * Mathf.Deg2Rad),
            0
        );
    }

}
