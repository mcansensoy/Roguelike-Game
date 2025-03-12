using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DuloGames.UI;

public class HUD : MonoBehaviour
{
    private Death_Fade death_Fade_;
    [SerializeField] private Canvas death_screen, death_, hud_;

    [Header("Health")]
    public UIProgressBar healthSlider;
    public TextMeshProUGUI healthText;

    private Stats player_Stat;
    private bool _isDead = false;




    void Start()
    {
        player_Stat = PlayerManager.instance.stat;
        death_Fade_ = Death_Fade.ins_;

        if (player_Stat == null || death_Fade_ == null)
        {
            Debug.LogError("PlayerStats component not found!");
            return;
        }
    }

    void Update()
    {
        //Debug.Log("can " + player_Stat.CurrentHealth);
        UpdateHealthUI(); // sadece damage alýrken ve saðlýk yükseltirken çaðýrýrsak iyi olur
    }













    void UpdateHealthUI()
    {
        if (player_Stat != null)
        {
            healthSlider.fillAmount = player_Stat.CurrentHealth / player_Stat.baseHealth;
            int healthAsInt = Mathf.FloorToInt(player_Stat.CurrentHealth);
            healthText.text = healthAsInt.ToString();
        }

        if (player_Stat.isDead && !_isDead)
        {
            _isDead = true;
            StartCoroutine(Death_cor());
        }
    }



    public IEnumerator Death_cor()
    {
        //_isDead = false;
        death_Fade_.Close_Black_screen();
        yield return new WaitForSeconds(1.1f);

        Debug.Log("restart ");
        death_screen.gameObject.SetActive(true);
        yield return new WaitForSeconds(.1f);

        Debug.Log("hadi açýl ");
        death_Fade_.Open_Black_screen();
        yield return new WaitForSeconds(1f);


        Debug.Log("order deðiþ ");
        death_screen = death_screen.GetComponent<Canvas>();
        death_screen.sortingOrder = 5;
        //Time.timeScale = 0;
    }
}
