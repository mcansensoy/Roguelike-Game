using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private GameObject Shop_UI;
    [SerializeField] CanvasGroup shopCanvas;
    [Space(10)]
    [SerializeField] float anim_duration;





    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Shop_UI.SetActive(true);

            Fade_OpenShop();
        }

        Time.timeScale = 0;
    }

    public async void Close_Shop()
    {
        await Fade_CloseShop();
        Shop_UI.SetActive(false);
        Time.timeScale = 1;
    }


    #region DoTween
    void Fade_OpenShop()
    {
        shopCanvas.DOFade(1, anim_duration).SetUpdate(true);
    }

    async Task Fade_CloseShop()
    {
        await shopCanvas.DOFade(0, anim_duration).SetUpdate(true).AsyncWaitForCompletion();
    }



    #endregion
}
