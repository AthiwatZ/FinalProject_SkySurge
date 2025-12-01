using UnityEngine;

[System.Serializable]
public class SpawnEntry
{
    public GameObject enemyPrefab;
    public int count = 10;
    public float interval = 0.5f;
}

[CreateAssetMenu(menuName = "SkySurge/WavePreset")]
public class WavePreset : ScriptableObject
{
    public float duration = 30f;
    public SpawnEntry[] composition;
}
