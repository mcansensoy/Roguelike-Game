using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;
using Unity.VisualScripting;

public class Menu : MonoBehaviour
{
    [SerializeField] Options options_;
    [Space(10)]
    [Header("DoTween")]
    [SerializeField] RectTransform option_rect;
    [SerializeField] RectTransform credit_rect;
    //[SerializeField] RectTransform main_rect;
    [Space(10)]
    [SerializeField] GameObject option_menu;
    [SerializeField] GameObject credit_menu;
    [SerializeField] GameObject main_menu;
    [SerializeField] CanvasGroup canvas_;
    [Space(10)]
    [SerializeField] float anim_duration;



    private void Awake()
    {
        options_ = GetComponent<Options>();
        if (options_ != null)
        {
            options_.LoadSettings();
            options_.Update_Sound();
        }
    }

    private void Start()
    {
        canvas_.DOFade(1, .2f).SetUpdate(true);
        if (options_ != null)
            options_.LoadSettings();
    }









    #region Functs

    public void Intro_option()
    {
        main_menu.SetActive(false);
        canvas_.DOFade(0, anim_duration).SetUpdate(true);
        option_rect.DOAnchorPosX(0, anim_duration).SetUpdate(true);
        option_menu.SetActive(true);
    }
    public void Intro_credit()
    {
        main_menu.SetActive(false);
        canvas_.DOFade(0, anim_duration).SetUpdate(true);
        credit_rect.DOAnchorPosX(100, anim_duration).SetUpdate(true);
        credit_menu.SetActive(true);
    }

    public void Intro_main()
    {
        canvas_.DOFade(0, anim_duration).SetUpdate(true);
        //main_rect.DOAnchorPosX(100, anim_duration).SetUpdate(true);
        main_menu.SetActive(true);
    }



    public async void Outro()
    {
        if (options_ != null)
            options_.SaveSettings();


        await Outro_menu();
        option_menu.SetActive(false);
        credit_menu.SetActive(false);
        canvas_.DOFade(1, anim_duration).SetUpdate(true);
        main_menu.SetActive(true);
    }

    async Task Outro_menu()
    {
        //canvas_.DOFade(0, anim_duration).SetUpdate(true);
        await option_rect.DOAnchorPosX(1000, anim_duration).SetUpdate(true).AsyncWaitForCompletion();
        await credit_rect.DOAnchorPosX(900, anim_duration).SetUpdate(true).AsyncWaitForCompletion();
        // await main_rect.DOAnchorPosX(-450, anim_duration).SetUpdate(true).AsyncWaitForCompletion();
    }

    #endregion



    public void OnApplicationQuit()
    {
        Application.Quit();
    }

}
