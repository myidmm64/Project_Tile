using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/DiceAnimationDataSO")]
public class DiceAnimationDataSO : ScriptableObject
{
    public List<DiceAnimationData> diceAnimationDatas = new();
}