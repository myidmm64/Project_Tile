public abstract class EnemyPattern
{
    protected EnemyDiceUnit _diceUnit = null;
    public bool isEnded { get; protected set; }

    public EnemyPattern(EnemyDiceUnit diceUnit)
    {
        _diceUnit = diceUnit;
        Initialize();
    }

    // ���� �Ʒ����� ��� ��� SO data ����ϵ��� �����ϱ�. Load�ϵ���
    public abstract int GetPatternPriority();
    public abstract float GetCooltime();
    public abstract bool CanStartPattern();

    public abstract void Initialize();

    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}
