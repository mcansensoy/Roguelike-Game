using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CharacterSelector : MonoBehaviour
{
    public Character CurrentCharacter;
    public CharacterDataList char_list;
    public int current_Char_index;

    public TextMeshProUGUI Char_Class;
    public TextMeshProUGUI HealthText, ArmorText, damage_text, AttackSpeedText, SpeedText, StoryText;
    public GameObject[] CharacterModels;













    #region Select Character

    public void SetNextCharacter()
    {
        if (char_list == null || char_list.char_Liste.Count == 0)
        {
            Debug.Log(char_list);
            Debug.Log(char_list.char_Liste.Count);
            Debug.LogError("No character data list assigned or list is empty!");
            return;
        }

        current_Char_index = (current_Char_index + 1) % char_list.char_Liste.Count;
        UpdateUI();
    }

    public void SetPreviousCharacter()
    {
        if (char_list == null || char_list.char_Liste.Count == 0)
        {
            Debug.LogError("No character data list assigned or list is empty!");
            return;
        }

        current_Char_index = (current_Char_index - 1
            + char_list.char_Liste.Count) % char_list.char_Liste.Count;
        UpdateUI();
    }




    #endregion







    private void UpdateUI()
    {
        CurrentCharacter = char_list.char_Liste[current_Char_index];

        if (CurrentCharacter == null) { Debug.LogError("No character selected!"); return; }

        //Char_Class.text = CurrentCharacter._Class.ToString();
        Char_Class.text = CurrentCharacter._Class.ToString();
        HealthText.text = CurrentCharacter.Health.ToString();
        ArmorText.text = CurrentCharacter.Armor.ToString();
        damage_text.text = CurrentCharacter.Damage.ToString();
        AttackSpeedText.text = CurrentCharacter.AttackSpeed.ToString();
        SpeedText.text = CurrentCharacter.Speed.ToString();
        StoryText.text = CurrentCharacter.Story.ToString();

        for (int i = 0; i < CharacterModels.Length; i++)
        {
            CharacterModels[i].SetActive(i == current_Char_index);
        }
    }












    #region next level








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

    #endregion


}