using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float m_speed;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
      var h = Input.GetAxis("Horizontal");
      var v = Input.GetAxis("Vertical");

      var velocity = new Vector3(h, v) * m_speed;
      transform.localPosition += velocity;
    }
}
