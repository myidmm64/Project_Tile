using UnityEngine;

[CreateAssetMenu(menuName = "SO/GameObjectLoader/Data")]
public class GameObjectLoaderDataSO : ScriptableObject
{
    public string Prefix = "@";
    public string GameObjectName = string.Empty;
    public Color BackColor = Color.white;
    public Color FontColor = Color.black;

    public string ResourcePath = string.Empty;
}