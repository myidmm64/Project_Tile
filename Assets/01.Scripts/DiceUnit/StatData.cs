using System;
using UnityEngine;

[Serializable]
public class StatData
{
    public int hp; // 체력

    public int strength; // 고정 피해 감소
    public float takeDamageMultiflier; // 받는 데미지 추가
    public int physicalDamage; // 물공뎀
    public int magicalDamage; // 마공뎀
    public float criticalChance; // 크확
    public float criticalDamage; // 크뎀

    public float attackSpeedMultiflier; // 공격속도 배율

    public float decreaseDP; // DP 감소
    public float drainDP; // DP 회수율
    public float decreaseCooltime; // 특수, 카운터 스킬 쿨감율

    public static StatData operator +(StatData a, StatData b)
    {
        StatData result = new StatData();
        result.hp = a.hp + b.hp;
        result.strength = a.strength + b.strength;
        result.takeDamageMultiflier = a.takeDamageMultiflier + b.takeDamageMultiflier;
        result.physicalDamage = a.physicalDamage + b.physicalDamage;
        result.magicalDamage = a.magicalDamage + b.magicalDamage;
        result.criticalChance = a.criticalChance + b.criticalChance;
        result.criticalDamage = a.criticalDamage + b.criticalDamage;
        result.attackSpeedMultiflier = a.attackSpeedMultiflier + b.attackSpeedMultiflier;
        result.decreaseDP = a.decreaseDP + b.decreaseDP;
        result.drainDP = a.drainDP + b.drainDP;
        result.decreaseCooltime = a.decreaseCooltime - b.decreaseCooltime;
        return result;
    }
}
