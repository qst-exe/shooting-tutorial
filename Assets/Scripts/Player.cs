﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float m_speed;
    public Shot m_shotPrefab;
    public float m_shotSpeed;
    public float m_shotAngleRange;
    public float m_shotTimer;
    public int m_shotCount;
    public float m_shotInterval;
    public int m_hpMax;
    public int m_hp;

    private void Awake()
    {
        m_hp = m_hpMax;
    }

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

        m_shotTimer += Time.deltaTime;

        if( m_shotTimer < m_shotInterval) return;

        m_shotTimer = 0;

        ShootNWay(angle, m_shotAngleRange, m_shotSpeed, m_shotCount);
    }

    private void ShootNWay(float angleBase, float angleRange, float speed, int count)
    {
        var pos = transform.localPosition;
        var rot = transform.localRotation;

        if (1 < count)
        {
            for (int i = 0; i < count; ++i)
            {
                var angle = angleBase + angleRange * ((float) i / (count - 1) - 0.5f);

                var shot = Instantiate(m_shotPrefab, pos, rot);

                shot.Init(angle, speed);
            }
        }
        else if (count == 1)
        {
            var shot = Instantiate(m_shotPrefab, pos, rot);


            shot.Init(angleBase, speed);

        }
    }

    public void Damage(int damage)
    {
        m_hp -= damage;

        if (0 < m_hp) return;

        gameObject.SetActive(false);
    }
}
