using UnityEngine;

public abstract class DiceAction
{
    // dice 안에 뭐가 들어왔다, 이럴 때 직접적인 이벤트는 해당 클래스에서 작동함.
    protected Dice _dice = null;
    public EDiceType eDiceType = EDiceType.None; // 추가될 때마다 바꿔주어야 함.
    public DiceAction(Dice dice)
    {
        _dice = dice;
    }

    public abstract void OnDice();
}