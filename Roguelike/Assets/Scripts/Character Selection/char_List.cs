using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterDataList",
    menuName = "Scriptable Objects/Character Data List", order = 2)]
public class CharacterDataList : ScriptableObject
{
    public List<Character> char_Liste;
}