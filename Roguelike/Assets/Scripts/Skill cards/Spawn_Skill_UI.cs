using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

public class Spawn_Skill_UI : MonoBehaviour
{

    [SerializeField] Card_Manager cardManager;

    [SerializeField] private GameObject UpgradeUI;
    [Space(10)]
    [SerializeField] CanvasGroup cardCanvas;
    [Space(10)]
    [SerializeField] float anim_duration;

     public BoxCollider thisBox_collider;


    private void Start()
    {
        cardManager = cardManager.GetComponent<Card_Manager>();
        thisBox_collider = GetComponent<BoxCollider>();
    }







    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            UpgradeUI.SetActive(true);
            cardManager.RespawnCards();
            Fade_OpenCard();
        }
        Time.timeScale = 0;
    }







    public void Disable_Collider()
    {
        thisBox_collider.enabled = false;
    }





    #region DoTween
    void Fade_OpenCard()
    {
        cardCanvas.DOFade(1, anim_duration).SetUpdate(true);
    }






    public async void Close_Skill()
    {
        await Fade_CloseSkill();
        Time.timeScale = 1;
    }




    async Task Fade_CloseSkill()
    {
        await cardCanvas.DOFade(0, anim_duration).SetUpdate(true).AsyncWaitForCompletion();
    }



    #endregion
}
