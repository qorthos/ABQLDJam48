using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitioner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void GotoGameScene()
    {
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }

    public void GotoTitleScene()
    {
        SceneManager.LoadScene("TitleScene", LoadSceneMode.Single);
    }

    public void GotoScoreScene()
    {
        SceneManager.LoadScene("ScoreScene", LoadSceneMode.Single);
    }
}
