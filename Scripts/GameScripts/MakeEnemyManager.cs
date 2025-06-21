using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MakeEnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab; // 敵のプレハブ
    public Transform spawnPoint;   // 生成基準地点
    public float spawnInterval = 0.5f;               // 生成間隔（秒）
    public float minX = -3f;                         // X軸の最小位置
    public float maxX = 3f;                          // X軸の最大位置
    public float distance = 15f;
    public Transform Bow;

    private PlayerManager PlayerManager; // PlayerManagerの参照
    

    void Start()
    {

        StartCoroutine(SpawnEnemiesCoroutine());
        

        PlayerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        int hardMode = PlayerPrefs.GetInt("hardMode", 0); // PlayerPrefsからハードモードを取得
        float RealInterval = spawnInterval;
        spawnInterval -= (PlayerManager.AroundNum * 0.07f) + (hardMode * 0.1f); // 難易度に応じて生成間隔を調整
        if (spawnInterval < RealInterval - 0.35f) // 最小生成間隔を設定
        {
            spawnInterval = RealInterval - 0.35f;
        }


    }

    IEnumerator SpawnEnemiesCoroutine()
    {
        while (!PlayerManager.isGameOver && !PlayerManager.isGameClear) // ゲームオーバーまたはクリアでない場合
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval); // 指定間隔ごとに実行
        }
    }

    void SpawnEnemy()
    {
        if (PlayerManager.TorchEnemy)
        {
            SoundSEManager.Instance.PlayMakeEnemySound();
        }
        
        float randomX = Random.Range(minX, maxX); // ランダムなX軸の値を取得
        Vector3 spawnPosition = new Vector3(randomX, spawnPoint.position.y, spawnPoint.position.z + distance);
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "Stage1" || sceneName == "Stage4")
        {
            Bow.position = new Vector3(randomX - 2f, 2f, spawnPoint.position.z + distance); // Bowの位置を更新
            Instantiate(enemyPrefab, new Vector3(randomX, 2f, spawnPoint.position.z + distance), Quaternion.Euler(90, 180, 0)); // 敵を生成
        }
        else if (sceneName == "Stage2")
        {

            Instantiate(enemyPrefab, new Vector3(randomX, 2f, spawnPoint.position.z + distance), Quaternion.Euler(90, 180, 0)); // 敵を生成
        }
        else if (sceneName == "Stage3")
        {

            Instantiate(enemyPrefab, new Vector3(randomX, 10.5f, spawnPoint.position.z + distance), Quaternion.Euler(-60, 0, -90)); // 敵を生成
        }
        else if (sceneName == "Stage5")
        {
            Instantiate(enemyPrefab, new Vector3(randomX, -1.3f, spawnPoint.position.z + distance), Quaternion.Euler(0, 180, 0)); // 敵を生成
        }
        else if (sceneName == "Stage6")
        {
            Instantiate(enemyPrefab, new Vector3(randomX, 0.65f, spawnPoint.position.z + distance), Quaternion.Euler(0,90, 0)); // 敵を生成
        }
        else
        {
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
        
    }
}
