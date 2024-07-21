using UnityEngine;

public abstract class DiceAction
{
    // dice �ȿ� ���� ���Դ�, �̷� �� �������� �̺�Ʈ�� �ش� Ŭ�������� �۵���.
    protected Dice _dice = null;
    public EDiceType eDiceType = EDiceType.None; // �߰��� ������ �ٲ��־�� ��.
    public DiceAction(Dice dice)
    {
        _dice = dice;
    }

    public abstract void OnDice();
}