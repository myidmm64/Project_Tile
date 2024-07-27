using UnityEngine;

public class TestEnemy : DiceUnit
{
    private void Start()
    {
        TestInit();
    }

    private void TestInit()
    {
        dice = _diceGrid.GetDice(new Vector2Int(2, 2));
        transform.position = dice.transform.position;
    }


}
