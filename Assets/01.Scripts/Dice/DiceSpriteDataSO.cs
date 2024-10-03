using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/DiceSpriteData")]
public class DiceSpriteDataSO : ScriptableObject
{
    public List<DiceSpriteData> diceSpriteDatas = new List<DiceSpriteData>();
}

[System.Serializable]
public class DiceSpriteData
{
    public Sprite sprite = null;
    public Color normalColor = Color.white;
    public Color accentColor = Color.white;
}
