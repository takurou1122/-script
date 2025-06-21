using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UIコンポーネントを使用するために必要


using System.Text.RegularExpressions; // 正規表現を使用するために必要

public class SoundBGMManager : MonoBehaviour
{
    public static SoundBGMManager Instance;
    public AudioSource audioSource;
    public AudioClip TitleAndMenuBGM;
    public AudioClip[] GameBGM; // ゲーム中のBGMを格納する配列
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

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


    // Update is called once per frame
    void Update()
    {

    }

    public void PlayTitleAndMenuBGM()
    {
        audioSource.clip = TitleAndMenuBGM;
        audioSource.Play();
    }

    public void PlayGameBGM(string Stage)
    {
        if (GameBGM.Length > 0)
        {
            string number = Regex.Match(Stage, @"\d+").Value; // 数字のみ抽出
            audioSource.clip = GameBGM[int.Parse(number)];
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("GameBGM配列が空です。BGMを再生できません。");
        }
    }
    public void StopBGM()
    {
        audioSource.Stop();
    }
    
    public void PauseBGM()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Pause(); // 中断（再生位置を保持）
        }
    }

    public void ResumeBGM()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.UnPause(); // 中断位置から再開
        }
    }
}
