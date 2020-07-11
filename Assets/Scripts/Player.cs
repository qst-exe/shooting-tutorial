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
      var localPosition = transform.localPosition;
      localPosition += velocity;
      // プレイヤーが画面外に出ないように位置を制限する
      localPosition = Utils.ClampPosition( localPosition );
      transform.localPosition = localPosition;

      // プレイヤーのスクリーン座標を計算する
      var screenPos = Camera.main.WorldToScreenPoint(transform.position);

      // プレイヤーから見たマウスカーソルの方向を計算する
      var direction = Input.mousePosition - screenPos;

      // マウスカーソルが存在する方向の角度を取得する
      var angle = Utils.GetAngle(Vector3.zero, direction);

      // プレイヤーがマウスカーソルの方向を見るようにする
      var angles = transform.localEulerAngles;
      angles.z = angle - 90;
      transform.localEulerAngles = angles;

    }
}
