using UnityEngine;

[CreateAssetMenu(menuName = "SO/DiceGenerateData")]
public class DiceGenerateDataSO : ScriptableObject
{
    [TextArea]
    public string diceMapStr = null;
    public Vector2 diceCenterPosition = Vector2.zero;
    public Vector2 dicePositionDistance = Vector2.one;
}
