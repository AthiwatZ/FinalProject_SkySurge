using UnityEngine;

[System.Serializable]
public class DropEntry { public GameObject prefab; [Range(0, 1)] public float chance = 0.3f; }

public class ItemSpawner : MonoBehaviour
{
    public DropEntry[] dropTable;

    public void RollDrop(Vector2 pos)
    {
        foreach (var e in dropTable)
        {
            if (Random.value <= e.chance)
            {
                Instantiate(e.prefab, pos, Quaternion.identity);
                break;
            }
        }
    }
}