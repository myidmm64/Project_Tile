using UnityEngine;

public class TestEnemy : DiceUnit
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        TestInit();
    }

    private void TestInit()
    {
        ChangeMyDice(new Vector2Int(2, 2));
        transform.position = dice.transform.position;
        SetSpriteSortingOrder(spriteRenderer);
    }


}
