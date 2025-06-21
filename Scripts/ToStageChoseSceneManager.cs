using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToStageChoseSceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void ToStageChoseScene()
    {
        // シーンをStageChoseに切り替える
        UnityEngine.SceneManagement.SceneManager.LoadScene("StageChoseScene");
    }
}
