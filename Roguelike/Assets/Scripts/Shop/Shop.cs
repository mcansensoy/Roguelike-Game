using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public TextMeshProUGUI[] stat_UI_texts;
    public TextMeshProUGUI currency_text;
    public Character CurrentCharacter;
    [Header("Boost")]
    public int boost_health;
    public int boost_armor, boost_damage, boost_speed;



    private PlayerStat player_Stat;
    private Stats stats_;

    private int health_, armor_, damage_, speed_;

    [Header("ücretler")]
    public int currency;
    public int healthUpgradeCost = 10;
    public int armorUpgradeCost = 15;
    public int DamageUpgradeCost = 20;
    public int speedUpgradeCost = 35;


    private void Start()
    {
        player_Stat = PlayerManager.instance.stat;

        Take_Stat();

        currency_text.text = currency.ToString();
    }






    #region Funcs




    private void Take_Stat()
    {
        if (CurrentCharacter != null)
        {
            health_ = (int)player_Stat.CurrentHealth;
            armor_ = (int)player_Stat.CurrentArmor;
            damage_ = (int)player_Stat.CurrentDamage;
            speed_ = (int)player_Stat.CurrentMoveSpeed;

            ShowStat();
        }
    }

    public void ShowStat()
    {

        stat_UI_texts[0].text = health_.ToString();
        stat_UI_texts[1].text = armor_.ToString();
        stat_UI_texts[2].text = damage_.ToString();
        //stat_UI_texts[3].text = nameof(rang_dmg_);
        stat_UI_texts[3].text = speed_.ToString();
        currency_text.text = currency.ToString();
    }




    public void UpgradeStat(int statIndex)
    {
        if (CurrentCharacter == null) return;

        switch (statIndex)
        {
            case 0:

                if (currency >= healthUpgradeCost)
                {
                    player_Stat.UpdatePlayerHealth(boost_health);
                    health_ += boost_health;
                    currency -= healthUpgradeCost;
                }
                else { return; }
                break;
            case 1:
                if (currency >= armorUpgradeCost)
                {
                    player_Stat.UpdatePlayerArmor(boost_armor);
                    stat_UI_texts[1].text = nameof(stats_.CurrentArmor);
                    armor_ += boost_armor;
                    currency -= armorUpgradeCost;
                }
                else { return; }
                break;
            case 2:
                if (currency >= DamageUpgradeCost)
                {
                    player_Stat.UpdatePlayerDamage(boost_damage);
                    stat_UI_texts[2].text = nameof(stats_.CurrentDamage);
                    damage_ += boost_damage;
                    currency -= DamageUpgradeCost;
                }
                else { return; }
                break;
            case 3:
                if (currency >= speedUpgradeCost)
                {
                    player_Stat.UpdatePlayerMoveSpeed(boost_speed);
                    stat_UI_texts[3].text = nameof(stats_.CurrentMoveSpeed);
                    speed_ += boost_speed;
                    currency -= speedUpgradeCost;
                }
                else { return; }
                break;

            default:
                Debug.LogError("Invalid stat index for upgrade!");
                break;
        }

        ShowStat();
        //}
        //else
        //{
        //    Debug.Log("Insufficient currency for upgrade!");
        //}
    }

    #endregion


}
