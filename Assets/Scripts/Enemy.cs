using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum RESPAWN_TYPE
{
    UP,
    RIGHT,
    DOWN,
    LEFT,
    SIZEOF,
}

public class Enemy : MonoBehaviour
{
    public Vector2 m_respawnPosInside;
    public Vector2 m_respawnPosOutside;
    public float m_speed;
    public int m_hpMax;
    public int m_exp;
    public int m_damage;
    public Explosion m_explosionPrefab;

    private int m_hp;
    private Vector3 m_direction;

    // Start is called before the first frame update
    private void Start()
    {
        m_hp = m_hpMax;
    }

    // Update is called once per frame
    private void Update()
    {
        transform.localPosition += m_direction * m_speed;
    }

    public void Init(RESPAWN_TYPE respawnType)
    {
        var pos = Vector3.zero;

        switch (respawnType)
        {
            case RESPAWN_TYPE.UP:
                pos.x = Random.Range(-m_respawnPosInside.x, m_respawnPosInside.x);
                pos.y = m_respawnPosOutside.y;
                m_direction = Vector2.down;
                break;

            case RESPAWN_TYPE.RIGHT:
                pos.x = m_respawnPosOutside.x;
                pos.y = Random.Range(-m_respawnPosInside.y, m_respawnPosInside.y);
                m_direction = Vector2.left;
                break;

            case RESPAWN_TYPE.DOWN:
                pos.x = Random.Range(-m_respawnPosInside.x, m_respawnPosInside.x);
                pos.y = -m_respawnPosOutside.y;
                m_direction = Vector2.up;
                break;

            case RESPAWN_TYPE.LEFT:
                pos.x = -m_respawnPosOutside.x;
                pos.y = Random.Range(-m_respawnPosInside.y, m_respawnPosInside.y);
                m_direction = Vector2.right;
                break;

            case RESPAWN_TYPE.SIZEOF:
                break;

            default:
              throw new ArgumentOutOfRangeException(nameof(respawnType), respawnType, null);
        }

        transform.localPosition = pos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Contains("Player"))
        {
            var player = collision.GetComponent<Player>();
            player.Damage(m_damage);
            return;
        }

        if (collision.name.Contains("Shot"))
        {
            Instantiate(m_explosionPrefab, collision.transform.localPosition, Quaternion.identity);
            Destroy(collision.gameObject);

            m_hp--;

            if(0 < m_hp) return;

            Destroy(gameObject);
        }
    }
}
