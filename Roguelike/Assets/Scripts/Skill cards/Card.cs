using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardCategory { Dagger, Trap, Ultimate }

[CreateAssetMenu(fileName = "New Skill Card", menuName = " Skill Card")]
public class Card : ScriptableObject
{
    //public string skill;
    public CardCategory category;
    public string skillName;
    public string description;
    public string subclass;
}
