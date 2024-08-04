using UnityEngine;

public abstract class PlayerModule : MonoBehaviour
{
    protected PlayerDiceUnit _player = null;

    protected virtual void Awake()
    {
        _player = GetComponent<PlayerDiceUnit>();
        if(_player == null)
        {
            Debug.LogError("PlayerDiceUnit 존재하지 않음.");
        }
    }
}
