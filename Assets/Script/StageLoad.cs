using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StageLoad : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void loadActiveStage()// 현재 실행중인 scene다시 호출
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    // scene이름으로 호출
    public void loadSceneName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
