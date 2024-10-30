using System;
using UnityEngine;

[Serializable]
public class StatData
{
    public int hp; // ü��

    public int strength; // ���� ���� ����
    public float takeDamageMultiflier; // �޴� ������ �߰�
    public int physicalDamage; // ������
    public int magicalDamage; // ������
    public float criticalChance; // ũȮ
    public float criticalDamage; // ũ��

    public float attackSpeedMultiflier; // ���ݼӵ� ����

    public float decreaseDP; // DP ����
    public float drainDP; // DP ȸ����
    public float decreaseCooltime; // Ư��, ī���� ��ų ����

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
