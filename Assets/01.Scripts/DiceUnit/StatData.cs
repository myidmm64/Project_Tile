using System;
using UnityEngine;

[Serializable]
public class StatData
{
    public int hp; // 체력

    public int physicalDamage; // 물공뎀
    public int magicalDamage; // 마공뎀

    public int strength; // 고정 피해 감소
    public float takeDamage = 100; // 받는 데미지 추가
    public float criticalChance = 0; // 크확
    public float criticalDamage = 120; // 크뎀

    public float attackSpeed = 100; // 공격속도 배율
    public float decreaseDP = 100; // DP 감소
    public float drainDP = 100; // DP 회수율
    public float decreaseCooltime = 100; // 특수, 카운터 스킬 쿨감율

    public StatData() { }

    public StatData(StatData copy)
    {
        this.hp = copy.hp;
        this.physicalDamage = copy.physicalDamage;
        this.magicalDamage = copy.magicalDamage;
        this.strength = copy.strength;
        this.takeDamage = copy.takeDamage;
        this.criticalChance = copy.criticalChance;
        this.criticalDamage = copy.criticalDamage;
        this.attackSpeed = copy.attackSpeed;
        this.decreaseDP = copy.decreaseDP;
        this.drainDP = copy.drainDP;
        this.decreaseCooltime = copy.decreaseCooltime;
    }
}
