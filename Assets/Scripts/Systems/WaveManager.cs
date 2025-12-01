using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    public int WaveIndex { get; private set; } = 0;
    public WavePreset[] presets;
    public Transform[] spawnPoints;
    public ItemSpawner itemSpawner;

    bool spawning = false;
    int enemyAlive = 0;

    public void StartNextWave()
    {
        if (spawning) return;
        WaveIndex++;
        GameManager.I.ui.UpdateHUD(GameManager.I.player.hp, GameManager.I.player.maxHp, 
            GameManager.I.player.lv, GameManager.I.player.exp, 
            GameManager.I.player.ExpToNextLevel(), WaveIndex, GameManager.I.score);
        StartCoroutine(SpawnWaveRoutine(GetRandomPreset()));
        StartCoroutine(CheckWaveClear());
    }

    public void StopWave() { StopAllCoroutines(); spawning = false; }

    WavePreset GetRandomPreset()
    {
        return presets[Random.Range(0, presets.Length)];
    }

    IEnumerator SpawnWaveRoutine(WavePreset preset)
    {
        spawning = true;
        foreach (var entry in preset.composition)
        {
            for (int i = 0; i < entry.count; i++)
            {
                var pt = spawnPoints[Random.Range(0, spawnPoints.Length)];
                var go = Instantiate(entry.enemyPrefab, pt.position, Quaternion.identity);
                var e = go.GetComponent<Enemy>();
                e.spawner = itemSpawner;
                e.waveManager = this;
                e.InitFromScale(GameManager.I.player.lv, WaveIndex);
                enemyAlive++;
                yield return new WaitForSeconds(entry.interval);
            }
        }
        spawning = false;

    }

    public void OnEnemyKilled()
    {
        enemyAlive--;
    }

    IEnumerator CheckWaveClear()
    {
        while (true)
        {
            if (!spawning && enemyAlive <= 0)
            {
                yield return new WaitForSeconds(1.0f);
                StartNextWave();
                yield break;
            }

            yield return null;
        }
    }
}

