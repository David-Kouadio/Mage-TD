using UnityEngine;

public class SpawnerTargets : MonoBehaviour
{
    [Header("Prefab do alvo")]
    public GameObject targetPrefab;

    [Header("Área de spawn")]
    public Vector3 center = Vector3.zero;
    public Vector3 range = new Vector3(10f, 5f, 10f);

    [Header("Tempo para nascer outro alvo")]
    public float respawnDelay = 3f;

    private GameObject currentTarget;

    void Start()
    {
        SpawnTarget();
    }

    void Update()
    {
        // Se o alvo foi destruído
        if (currentTarget == null)
        {
            Invoke(nameof(SpawnTarget), respawnDelay);
            enabled = false;
        }
    }

    void SpawnTarget()
    {
        // Coordenada aleatória dentro do range
        Vector3 randomPosition = new Vector3(
            Random.Range(center.x - range.x, center.x + range.x),
            Random.Range(center.y - range.y, center.y + range.y),
            Random.Range(center.z - range.z, center.z + range.z)
        );

        // Cria o alvo
        currentTarget = Instantiate(targetPrefab, randomPosition, Quaternion.identity);

        enabled = true;
    }
}
