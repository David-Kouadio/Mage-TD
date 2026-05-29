using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PukekoSpawnController : MonoBehaviour
{

    public int initialPukekosPerWave = 5;
    public int currentPukekosPerWave;

    public float spawnDelay = 0.5f;

    public int currentWave = 0;
    public float waveCooldown = 10.0f;

    public bool inCooldown;
    public float cooldownCounter = 0;

    public List<Enemy> currentPukekoAlive;

    public GameObject pukekoPrefab;


    void Start()
    {
        currentPukekosPerWave = initialPukekosPerWave; 

        StartNextWave();
    }

    private void StartNextWave()
    {
        currentPukekoAlive.Clear();

        currentWave++;

        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        if (currentWave < 1)
        {
            for(int i = 0; i < currentPukekosPerWave; i++)
            {
                // gera a uma area de spawn
                Vector3 spawnOffset = new Vector3(Random.Range(-1f,1f), 0f, Random.Range(-1f,1f));
                Vector3 spawnPosition = transform.position + spawnOffset;

                // Instanciar o pukeko
                var pukeko = Instantiate(pukekoPrefab, spawnPosition, Quaternion.identity);

                // pegar o script do inimigo
                Enemy enemyScript = pukeko.GetComponent<Enemy>();

                currentPukekoAlive.Add(enemyScript);

                yield return new WaitForSeconds(spawnDelay);

            }
        }
    }


    void Update()
    {
        List<Enemy> pukekosToRemove = new List<Enemy>();
        foreach(Enemy pukeko in currentPukekoAlive)
        {
            if(pukeko.isDead)
            {
                pukekosToRemove.Add(pukeko);
            }
        }

        foreach (Enemy pukeko in pukekosToRemove)
        {
            currentPukekoAlive.Remove(pukeko);
        }

        pukekosToRemove.Clear();

        if(currentPukekoAlive.Count == 0 && inCooldown == false)
        {
            StartCoroutine(WaveCooldown());
        }

        if(inCooldown)
        {
            cooldownCounter -= Time.deltaTime;
        }
        else
        {
            cooldownCounter = waveCooldown;
        }
    }

    private IEnumerator WaveCooldown()
    {
        inCooldown = true;

        yield return new WaitForSeconds(waveCooldown);

        inCooldown = false;

        currentPukekosPerWave += 2;

        StartNextWave();
    }
}
