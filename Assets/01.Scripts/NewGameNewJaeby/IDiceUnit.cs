using UnityEngine;

public interface IDiceUnit
{
    public Vector2Int PosKey { get; }
    public Dice CurDice { get; }

    // public bool ChangeDice(Vector2Int targetPosKey) => {};
    public bool ChangeDice(Dice targetDice); // 변경 성공했을 때만 true 반환
}
