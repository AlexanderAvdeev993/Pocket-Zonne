
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{      
    [SerializeField] private Enemy _enemyPrefab;

    public Vector2 GetSpawnPosition()
    {
        return transform.position;
    }
    public Enemy GetEnemyPrefab()
    {
        return _enemyPrefab;
    }
}
