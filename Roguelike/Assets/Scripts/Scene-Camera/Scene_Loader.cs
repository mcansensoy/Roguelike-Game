using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Scene_Loader : MonoBehaviour
{
    public static Scene_Loader instance;

    [SerializeField] private CanvasGroup canvas_;
    [SerializeField] private Canvas death_restart;
    [SerializeField] private float fade_time;
    //[SerializeField] private int scene_index;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else { Destroy(gameObject); }
    }






    public void Load_Next_Level()
    {
        StartCoroutine(Load_Transition());
    }

    public void Load__Level_index(int sceneIndex_)
    {

        if (PlayerManager.instance.stat.isDead)
        {
            PlayerManager.instance.stat.isItOver = true;
            Debug.Log("hadi be olm");
        }
        StartCoroutine(Load_Transition_byIndex(sceneIndex_));
    }



    IEnumerator Load_Transition()
    {
        canvas_.alpha = 0;
        canvas_.DOFade(1, fade_time).SetUpdate(true);
        yield return new WaitForSeconds(fade_time);

        Next_Level();
        yield return new WaitForSeconds(fade_time);

        canvas_.alpha = 1;
        canvas_.DOFade(0, fade_time).SetUpdate(true);
    }

    IEnumerator Load_Transition_byIndex(int sceneIndex)
    {
        canvas_.alpha = 0;
        canvas_.DOFade(1, fade_time).SetUpdate(true);
        yield return new WaitForSeconds(fade_time);

        Load_level(sceneIndex);
        yield return new WaitForSeconds(fade_time);

        canvas_.alpha = 1;
        canvas_.DOFade(0, fade_time).SetUpdate(true);
        death_restart.gameObject.SetActive(false);
    }










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
