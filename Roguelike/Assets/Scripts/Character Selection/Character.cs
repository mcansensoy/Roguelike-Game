using System.Collections.Generic;
using UnityEngine;

public enum Character_Class { Rogue = 1, Mage = 2, Warrior = 3 }

[CreateAssetMenu(fileName = "Character", menuName = "Scriptable Objects/Character", order = 1)]
public class Character : ScriptableObject
{
    [Space(10)]
    public Character_Class _Class;
    [Header("Base stat")]
    public int Health;
    public int Armor;
    public int Speed;
    [Space(10)]
    public int AttackSpeed;
    public int Damage;
    [Space(10)]
    //private GameObject Character_model;
    public string Story;
}