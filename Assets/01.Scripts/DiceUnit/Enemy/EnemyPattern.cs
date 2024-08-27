public abstract class EnemyPattern
{
    protected EnemyDiceUnit _diceUnit = null;
    public bool isEnded { get; protected set; }

    public EnemyPattern(EnemyDiceUnit diceUnit)
    {
        _diceUnit = diceUnit;
        Initialize();
    }

    // 추후 아래같은 방식 대신 SO data 사용하도록 변경하기. Load하도록
    public abstract int GetPatternPriority();
    public abstract float GetCooltime();
    public abstract bool CanStartPattern();

    public abstract void Initialize();

    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}
