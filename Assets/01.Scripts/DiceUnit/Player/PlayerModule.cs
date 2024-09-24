using UnityEngine;

public abstract class PlayerModule : MonoBehaviour
{
    protected Player _player = null;

    protected virtual void Awake()
    {
        _player = GetComponent<Player>();
        if(_player == null)
        {
            Debug.LogError("PlayerDiceUnit 존재하지 않음.");
        }
    }
}
