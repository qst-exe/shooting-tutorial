using System;
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
    public static Player m_instance;
    public float m_magnetDistance;
    public int m_nextExpBase;
    public int m_nextExpInterval;
    public int m_level;
    public int m_exp;
    public int m_prevNeedExp;
    public int m_needExp;
    public AudioClip m_levelUpClip;
    public AudioClip m_damageClip;
    public int m_levelMax;
    public int m_shotCountFrom;
    public int m_shotCountTo;
    public float m_shotIntervalFrom;
    public float m_shotIntervalTo;
    public float m_magnetDistanceFrom;
    public float m_magnetDistanceTo;

    private void Awake()
    {
        m_instance = this;
        m_hp = m_hpMax;
        m_level = 1;
        m_needExp = GetNeedExp(1);
        m_shotCount = m_shotCountFrom;
        m_shotInterval = m_shotIntervalFrom;
        m_magnetDistance = m_magnetDistanceFrom;
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

        var audioSource = FindObjectOfType<AudioSource>();
        audioSource.PlayOneShot(m_damageClip);
    }

    public void AddExp(int exp)
    {
        m_exp += exp;

        if (m_exp < m_needExp) return;

        m_level++;

        m_prevNeedExp = m_needExp;

        m_needExp = GetNeedExp(m_level);

        var angleBase = 0;
        var angleRange = 360;
        var count = 28;
        ShootNWay(angleBase, angleRange, 0.15f, count);
        ShootNWay(angleBase, angleRange, 0.2f, count);
        ShootNWay(angleBase, angleRange, 0.25f, count);

        var audioSource = FindObjectOfType<AudioSource>();
        audioSource.PlayOneShot(m_levelUpClip);

        var t = (float) (m_level - 1) / (m_levelMax - 1);
        m_shotCount = Mathf.RoundToInt(Mathf.Lerp(m_shotCountFrom, m_shotCountTo, t));
        m_shotInterval = Mathf.Lerp(m_shotCountFrom, m_shotIntervalTo, t);
        m_magnetDistance = Mathf.Lerp(m_magnetDistanceFrom, m_magnetDistanceTo, t);
    }

    private int GetNeedExp(int level)
    {
        return m_nextExpBase + m_nextExpInterval * ((level - 1) * (level - 1));
    }
}
