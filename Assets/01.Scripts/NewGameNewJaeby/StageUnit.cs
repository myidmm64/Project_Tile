using UnityEngine;

// ���������� �̳༮�� ��ȯ ����
public abstract class StageUnit : MonoBehaviour
{
    protected NewStage _stage => NewStageManager.Inst.curStage;
    public abstract void StartStage();
    public abstract void EndStage();
}
