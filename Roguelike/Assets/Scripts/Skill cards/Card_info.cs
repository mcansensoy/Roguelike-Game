using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Threading.Tasks;

public class Card_info : MonoBehaviour
{
    [Space(10)]
    [SerializeField] float anim_duration;
    [Space(10)]
    //[SerializeField] private TextMeshProUGUI skill;
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI subClass;


    private Card_Manager cardManager;
    private PlayerStat player_Stat;
    private BasicSkill basicSkill;
    private UltimateSkill ultimateSkill;
    private Spawn_Skill_UI spawnKill_UI;


    private Card card;
    private GameObject UpgradeUI;
    CanvasGroup canvas_;








    public void Initialize(Card cardData)
    {
        card = cardData;
        //skill.text = card.skill;
        skillName.text = card.skillName;
        description.text = card.description;
        subClass.text = card.subclass;



        UpgradeUI = GameObject.Find("Skill UI");
        canvas_ = UpgradeUI.GetComponent<CanvasGroup>();
        cardManager = FindObjectOfType<Card_Manager>();
        player_Stat = PlayerManager.instance.stat;
        basicSkill = SkillManager.instance.basicSkill;
        ultimateSkill = SkillManager.instance.ultimateSkill;
        spawnKill_UI = FindObjectOfType<Spawn_Skill_UI>();
    }





    public async void Selected_Upgrade()
    {
        await Fade_Close();
        UpgradeUI.SetActive(false);
        Time.timeScale = 1;

        cardManager.DestroyAllCards();

        spawnKill_UI.Disable_Collider();


        switch (card.category) // saðlýk, zýrh, atak, atak hýz, hýz
        {
            case CardCategory.Dagger:
                basicSkill.currentBasic = (int)card.category;
                break;
            case CardCategory.Trap:
                basicSkill.currentBasic = (int)card.category;
                break;
            case CardCategory.Ultimate:
                basicSkill.currentBasic = -1;
                break;

                //case CardCategory.Speed:
                //    player_Stat.UpdatePlayerMoveSpeed(card.boost);
                //    break;
        }

    }



    async Task Fade_Close()
    {
        await canvas_.DOFade(0, .2f).SetUpdate(true).AsyncWaitForCompletion();
    }

}
