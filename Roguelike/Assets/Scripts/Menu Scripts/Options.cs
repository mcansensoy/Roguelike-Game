using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

public class Options : MonoBehaviour
{
    //[SerializeField] GameObject[] game_objects;
    [Header("DoTween")]
    [SerializeField] RectTransform pause_rect;
    [SerializeField] RectTransform option_rect;
    [Space(10)]
    [SerializeField] GameObject pause_Menu;
    [SerializeField] CanvasGroup canvas_;
    [Space(10)]
    [SerializeField] float fade_pos;
    [SerializeField] float actual_pos, anim_duration;
    [Space(10)]
    public TMP_Dropdown graphicDropdown, resDropdown;
    [Space(1)]
    [Header("Sounds")]
    public Slider master_Vol;
    public Slider music_Vol, sfx_Vol;
    [Space(10)]
    public AudioMixer main_AudioMixer;


    private bool activated_ = false;


    Resolution[] All_resolutions;
    private int Selected_Res;




    private void Awake()
    {
        if (resDropdown)
            LoadSettings();
        Update_Sound();
    }


    private void Start()
    {
        if (resDropdown)
        {
            Res_Dropdown_();
        }
    }

   

    private async void Update()
    {
        SaveSettings();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            activated_ = !activated_;
            if (activated_ == true)
            {
                Time.timeScale = 0;
                pause_Menu.SetActive(activated_);
                Pause_Intro();
            }
            else
            {
                await Pause_Outro();
                pause_Menu.SetActive(activated_);
                Time.timeScale = 1;
            }
        }
    }









    #region Funcs



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




    public async void Resume_and_Pause()
    {
        activated_ = !activated_;
        await Pause_Outro();
        Time.timeScale = 1;
    }






    public void SaveSettings()
    {
        PlayerPrefs.SetInt("QualityLevel", graphicDropdown.value);
        if (resDropdown)
            PlayerPrefs.SetInt("SavedResolutionIndex", resDropdown.value);
        PlayerPrefs.SetFloat("MasterVolume", master_Vol.value);
        PlayerPrefs.SetFloat("MusicVolume", music_Vol.value);
        PlayerPrefs.SetFloat("SFXVolume", sfx_Vol.value);
        PlayerPrefs.Save();
    }


    public void LoadSettings()
    {
        if (PlayerPrefs.HasKey("QualityLevel"))
            graphicDropdown.value = PlayerPrefs.GetInt("QualityLevel");


        if (PlayerPrefs.HasKey("SavedResolutionIndex"))
        {
            Selected_Res = PlayerPrefs.GetInt("SavedResolutionIndex");
            resDropdown.value = Selected_Res;
            //Change_Resolution(Selected_Res);
        }


        if (PlayerPrefs.HasKey("MasterVolume"))
            master_Vol.value = PlayerPrefs.GetFloat("MasterVolume");
        if (PlayerPrefs.HasKey("MusicVolume"))
            music_Vol.value = PlayerPrefs.GetFloat("MusicVolume");
        if (PlayerPrefs.HasKey("SFXVolume"))
            sfx_Vol.value = PlayerPrefs.GetFloat("SFXVolume");
    }



    private void Res_Dropdown_()
    {
        All_resolutions = Screen.resolutions;
        resDropdown.ClearOptions();


        List<string> res_options = new();

        int current_Res_index = 0;

        for (int i = 0; i < All_resolutions.Length; i++)
        {
            string option_ = All_resolutions[i].width + " x " + All_resolutions[i].height;
            res_options.Add(option_);

            if (All_resolutions[i].width == Screen.currentResolution.width &&
                All_resolutions[i].height == Screen.currentResolution.height)
            {
                current_Res_index = i;
            }
        }


        resDropdown.AddOptions(res_options);
        resDropdown.value = current_Res_index;
        resDropdown.RefreshShownValue();
    }



    #endregion









    #region Ayarlar
    public void Change_Graphics()
    {
        QualitySettings.SetQualityLevel(graphicDropdown.value);
    }

    public void Change_Resolution(int res_index)
    {
        Resolution resolution_ = All_resolutions[res_index];
        Screen.SetResolution(resolution_.width, resolution_.height, Screen.fullScreen);
    }


    public void Update_Sound()
    {
        Change_Master_Volume();
        Change_Music_Volume();
        Change_Sfx_Volume();
    }


    public void Change_Master_Volume()
    {
        main_AudioMixer.SetFloat("Master Volume", master_Vol.value);
    }
    public void Change_Music_Volume()
    {
        main_AudioMixer.SetFloat("Music Volume", music_Vol.value);
    }
    public void Change_Sfx_Volume()
    {
        main_AudioMixer.SetFloat("SFX Volume", sfx_Vol.value);
    }



    #endregion











    #region DoTween

    void Pause_Intro()
    {
        canvas_.DOFade(1, anim_duration).SetUpdate(true);
        pause_rect.DOAnchorPosX(actual_pos, anim_duration).SetUpdate(true);
        option_rect.DOAnchorPosX(300, anim_duration).SetUpdate(true);
    }

    async Task Pause_Outro()
    {
        canvas_.DOFade(0, anim_duration).SetUpdate(true);
        option_rect.DOAnchorPosX(1200, anim_duration).SetUpdate(true);
        await pause_rect.DOAnchorPosX(fade_pos, anim_duration).SetUpdate(true).AsyncWaitForCompletion();
    }



    #endregion

}
