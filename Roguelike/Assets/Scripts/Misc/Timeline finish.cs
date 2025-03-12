using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Timelinefinish : MonoBehaviour
{
    public void Next_Level()
    {
        int next_sceneIndex = (SceneManager.GetActiveScene().buildIndex + 1)
            % SceneManager.sceneCountInBuildSettings;

        Load_level(next_sceneIndex);
    }



    public void Load_level(int levelIndex)
    {
        Time.timeScale = 1;

        if (levelIndex >= 0 &&
            levelIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(levelIndex);
        }
    }
}
