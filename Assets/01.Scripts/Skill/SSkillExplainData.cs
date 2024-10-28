using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SSkillExplainData // 스킬 설명용
{
    public float cooltime;
    public int maxDP;
    [TextArea]
    public string attackRange;

    public Sprite skillImage;
    public string skillName;
    public string explainText;
}

public class UseSkillData
{
    public DiceUnit owner;

    public UseSkillData(DiceUnit owner)
    {
        this.owner = owner;
    }
}

public class UsePlayerSkillData : UseSkillData
{
    public UsePlayerSkillData(DiceUnit owner) : base(owner)
    {
    }
}

public class UseEnemySkillData : UseSkillData
{
    public UseEnemySkillData(DiceUnit owner) : base(owner)
    {
    }
}

[System.Serializable]
public struct SUseSkillData
{
    public DiceUnit owner;

    private List<DiceUnit> _targets;
    public List<DiceUnit> targets { get
        {
            if(_targets == null)
                _targets = new List<DiceUnit>();
            return _targets;
        }
        set => _targets = value;
    }
    public Vector2Int spawnPosKey;
    public EDirection direction;
    public Dictionary<string, object> otherDatas;
}