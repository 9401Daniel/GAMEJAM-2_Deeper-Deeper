using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public struct EnemyConfig
{
    public GameObject prefab;
    public float spawnInterval;
    public float speed;
}

public class EnemySpawner : MonoBehaviour
{
    [Header("Configuración de Enemigos")]
    [SerializeField] private EnemyConfig mineConfig = new EnemyConfig { spawnInterval = 4f, speed = 2f };
    [SerializeField] private EnemyConfig eelConfig = new EnemyConfig { spawnInterval = 7f, speed = 6f };
    [SerializeField] private EnemyConfig urchinConfig = new EnemyConfig { spawnInterval = 5f, speed = 3.5f };
    [SerializeField] private EnemyConfig sharkConfig = new EnemyConfig { spawnInterval = 5f, speed = 3.5f };
    [SerializeField] private EnemyConfig tentacleConfig = new EnemyConfig { spawnInterval = 5f, speed = 3.5f };

    [Header("Configuración de Áreas")]
    [SerializeField] private Vector2 spawnRangeX = new Vector2(-6f, 6f);
    [SerializeField] private float spawnYBottom = -12f;
    [SerializeField] private float spawnYTop = 12f;

    // NUEVO: Margen extra para que el modelo 3D alcance a salir completamente de la pantalla
    [Tooltip("Distancia extra antes de destruir el objeto")]
    [SerializeField] private float destroyOffset = 3f;
    private bool startSpawn = false;
    private List<Coroutine> enemiesCoroutine = new List<Coroutine>();
    private void InitSpawn()
    {
        enemiesCoroutine.Add(StartCoroutine(SpawnRoutine(mineConfig, spawnYBottom, Vector3.up)));
        enemiesCoroutine.Add(StartCoroutine(SpawnRoutine(eelConfig, spawnYBottom, Vector3.up)));
        enemiesCoroutine.Add(StartCoroutine(SpawnRoutine(urchinConfig, spawnYTop, Vector3.down)));
        enemiesCoroutine.Add(StartCoroutine(SpawnRoutine(sharkConfig, spawnYBottom, Vector3.up)));
        enemiesCoroutine.Add(StartCoroutine(SpawnRoutine(tentacleConfig, spawnYBottom, Vector3.up)));
    }

    private IEnumerator SpawnRoutine(EnemyConfig config, float spawnY, Vector3 moveDirection)
    {
        if (config.prefab == null) yield break;

        while (startSpawn)
        {
            yield return new WaitForSeconds(config.spawnInterval);
            SpawnEnemy(config, spawnY, moveDirection);
        }
    }
    public void StartSpawn(bool startSpawn)
    {
        this.startSpawn = startSpawn;
        if (startSpawn)
        {
            if (enemiesCoroutine.Count == 0)
            {
                InitSpawn();
            }
        }
    }
    public void ResetSpawn()
    {
        startSpawn = false;
        foreach (var coroutine in enemiesCoroutine)
        {
            StopCoroutine(coroutine);
        }
        enemiesCoroutine.Clear();
    }

    private void SpawnEnemy(EnemyConfig config, float spawnY, Vector3 moveDirection)
    {

        float randomX = Random.Range(spawnRangeX.x, spawnRangeX.y);
        bool randomBool = Random.value > 0.5f;
        float xPos = randomBool ? -11 : 11;
        randomX = config.prefab.name == "Tentacle 1" ? xPos : randomX;
        Vector3 spawnPosition = new Vector3(randomX, spawnY, 0f);

        Quaternion spawnRotation = Quaternion.identity;
        if (config.prefab.name == "Tentacle 1" && xPos < 0)
        {
            spawnRotation = Quaternion.Euler(-90, 180, 0);
        }

        GameObject newEnemy = Instantiate(config.prefab, spawnPosition, spawnRotation);

        EnemyBase enemyScript = newEnemy.GetComponent<EnemyBase>();

        if (enemyScript != null)
        {
            // NUEVO: Lógica cruzada.
            // Si va hacia arriba, su límite de destrucción es el tope Y + el margen extra.
            // Si va hacia abajo, su límite es la base Y - el margen extra.
            float destroyLimit = (moveDirection.y > 0) ? (spawnYTop + destroyOffset) : (spawnYBottom - destroyOffset);

            // Pasamos el nuevo parámetro al inicializar
            enemyScript.Initialize(config.speed, moveDirection, destroyLimit);
        }
        else
        {
            Debug.LogError($"¡El prefab {config.prefab.name} necesita su script de enemigo!");
        }
    }
}