using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class Wave_Manager : MonoBehaviourPun
{
    public int MaxWaves = 5;
    public int CurrentWave = 1;

    public GameObject[] enemyPrefabs;
    public int enemiesPerWave = 4;
    public int enemyIncrease = 2;
    public float SpawnRate = 1f;
    public int enemyType = 1;
    public int spawnPlaces = 1;
    public Transform[] spawnPoints;

    private int spawnedEnemies = 0;
    public int killedEnemies = 0;
    public bool victory = false;

    BaseEnemy_SM newBaseEnemy;
    AttackEnemy_SM newAttackEnemy;

    public static Wave_Manager Instance;

    public TMP_Text textoRonda;
    public GameObject canvasVictoria;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        newBaseEnemy = enemyPrefabs[0].GetComponent<BaseEnemy_SM>();
        newAttackEnemy = enemyPrefabs[1].GetComponent<AttackEnemy_SM>();
        canvasVictoria.SetActive(false);
        
        StartCoroutine(CRT_SpawnEnemies());
    }
    private void Update()
    {
        if (textoRonda == null)
        {
            textoRonda = GameObject.FindGameObjectWithTag("RoundTag").GetComponent<TMP_Text>();
        }
        textoRonda.text = "Round " + CurrentWave;
        if (victory == true)
        {
            canvasVictoria.SetActive(true);
        }
    }

    void SpawnEnemy()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            int _spawnIndex = Random.Range(0, spawnPlaces);
            int _enemyIndex = Random.Range(0, enemyType);
            PhotonNetwork.Instantiate(enemyPrefabs[_enemyIndex].name, spawnPoints[_spawnIndex].position, Quaternion.identity);
            spawnedEnemies++;
        }
        

    }

    IEnumerator CRT_SpawnEnemies()
    {
        while (spawnedEnemies < enemiesPerWave)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(SpawnRate);
        }
    }
    
    public void EnemyKilled()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC(nameof(RPC_EnemyKilled), RpcTarget.All);

        }
    }

    [PunRPC]
    public void RPC_EnemyKilled()
    {
        FunctionEnemyKilled();
    }

    public void FunctionEnemyKilled()
    {
        killedEnemies++;
        Debug.Log("Hemos matado un enemigo: "+killedEnemies);
        if (killedEnemies >= enemiesPerWave)
        {
            Debug.Log("Debería pasar de wave");
            NextWave();

        }
    }

    void NextWave()
    {
        CurrentWave++;
        textoRonda.text = "Round " + CurrentWave;
        killedEnemies = 0;
        spawnedEnemies = 0;
        enemiesPerWave += enemyIncrease;

        if (CurrentWave == 2)
        {
            newBaseEnemy.health = 180;
        }

        if (CurrentWave == 3)
        {
            newBaseEnemy.health = 210;
            
            enemyType++;
            spawnPlaces++;
        }
        if (CurrentWave == 4)
        {
            newBaseEnemy.health = 260;
            newAttackEnemy.health = 300;
        }
        if (CurrentWave == 5)
        {
            newBaseEnemy.health = 300;
            newAttackEnemy.health = 400;
        }

        if (CurrentWave > MaxWaves)
        {
            victory = true;
            return;
        }
        else
        {
            StartCoroutine(CRT_SpawnEnemies());
        }
        
    }
}
