using UnityEngine;

public interface IDiceUnit
{
    public Vector2Int PosKey { get; }
    public Dice CurDice { get; }

    // public bool ChangeDice(Vector2Int targetPosKey) => {};
    public bool ChangeDice(Dice targetDice); // ���� �������� ���� true ��ȯ
}
