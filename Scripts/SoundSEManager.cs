using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UIコンポーネントを使用するために必要
using UnityEngine.SceneManagement; // シーン管理を使用するために必要

public class SoundSEManager : MonoBehaviour
{
    public static SoundSEManager Instance;
    public AudioSource audioSource;
    public AudioClip[] soundClickClips; // SEのクリップを格納する配列
    public AudioClip SoundNextStage; // 次のステージのSE
    public AudioClip SoundGameOver; // ゲームオーバーのSE
    public AudioClip SoundMakeEnemy;
    public AudioClip SoundUseItem; // アイテム使用のSE
    public AudioClip SoundGetMoney; // アイテム使用のSE



    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Button[] buttons = FindObjectsOfType<Button>();

        foreach (Button button in buttons)
        {
            if (button.name.Contains("Button")) // 名前に "Button" が含まれている場合
            {
                button.onClick.AddListener(() => SoundSEManager.Instance.PlayClickSound());
                Debug.Log("Buttonにクリック音を追加しました: " + button.name);
            }
        }

    }



    // Update is called once per frame
    void Update()
    {

    }

    public void PlayClickSound()
    {
        if (soundClickClips.Length > 0)
        {
            int randomIndex = Random.Range(0, soundClickClips.Length);
            audioSource.PlayOneShot(soundClickClips[randomIndex]);
        }
    }

    public void PlayNextStageSound()
    {
        audioSource.PlayOneShot(SoundNextStage);
    }
    public void PlayGameOverSound()
    {
        audioSource.PlayOneShot(SoundGameOver);
    }
    public void PlayMakeEnemySound()
    {
        audioSource.PlayOneShot(SoundMakeEnemy);
    }

    public void PlayUseItemSound()
    {
        audioSource.PlayOneShot(SoundUseItem);
    }
    public void PlayGetMoneySound()
    {
        audioSource.PlayOneShot(SoundGetMoney);
    }
}
