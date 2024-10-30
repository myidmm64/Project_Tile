using System;
using UnityEngine;

[Serializable]
public class StatData
{
    public int hp; // ü��

    public int physicalDamage; // ������
    public int magicalDamage; // ������

    public int strength; // ���� ���� ����
    public float takeDamage = 100; // �޴� ������ �߰�
    public float criticalChance = 0; // ũȮ
    public float criticalDamage = 120; // ũ��

    public float attackSpeed = 100; // ���ݼӵ� ����
    public float decreaseDP = 100; // DP ����
    public float drainDP = 100; // DP ȸ����
    public float decreaseCooltime = 100; // Ư��, ī���� ��ų ����

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
