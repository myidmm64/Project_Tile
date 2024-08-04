using UnityEngine;

public class TestEnemy : DiceUnit
{
    protected override void Start()
    {
        base.Start();
        TestInit();
    }

    private void TestInit()
    {
        ChangeMyDice(new Vector2Int(2, 2));
        transform.position = dice.transform.position;
        SetSpriteSortingOrder();
    }


}