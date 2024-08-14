using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SSkillData
{
    public int maxMP;
    [TextArea]
    public string rangePattern;

    [SerializeField]
    private Sprite _skillImage;
    public Sprite SkillImage => _skillImage;
    [SerializeField]
    private string _skillName;
    public string skillName => _skillName;
}

[System.Serializable]
public struct SUseSkillData
{
    public DiceUnit owner;
    public DiceGrid grid => owner.diceGrid;

    public EDirection direction;
    public Dictionary<string, Action> specialActions;
}
