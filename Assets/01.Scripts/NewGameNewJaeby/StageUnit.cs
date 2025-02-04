using UnityEngine;

// 스테이지는 이녀석들 소환 가능
public abstract class StageUnit : MonoBehaviour
{
    protected NewStage _stage => NewStageManager.Inst.curStage;
    public abstract void StartStage();
    public abstract void EndStage();
}
