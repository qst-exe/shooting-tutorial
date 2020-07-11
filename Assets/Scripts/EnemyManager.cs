using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public Enemy[] m_enemyPrefabs;
    public float m_interval;

    private float m_timer;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        m_timer += Time.deltaTime;

        if (m_timer < m_interval) return;

        m_timer = 0;

        var enemyIndex = Random.Range(0, m_enemyPrefabs.Length);

        var enemyPrefab = m_enemyPrefabs[enemyIndex];

        var enemy = Instantiate(enemyPrefab);

        var respawnType = (RESPAWN_TYPE) Random.Range(0, (int) RESPAWN_TYPE.SIZEOF);

        enemy.Init(respawnType);
    }
}
