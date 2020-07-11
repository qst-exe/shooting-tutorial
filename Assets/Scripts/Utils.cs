﻿using System.Collections;
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

}