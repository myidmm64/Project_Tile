using UnityEngine;

public class PlayerAttackModule : MonoBehaviour
{
    // �÷��̾� �⺻ ����
    private PlayerDiceUnit _player = null;

    [SerializeField]
    private float _attackDelay = 0.5f;
    private float _attackTimer = 0f;
    private void Awake()
    {
        _player = GetComponent<PlayerDiceUnit>();
    }

    public void Attack()
    {
        // �����Ÿ� �� ���� �ִٸ� �ڵ�����
        _attackTimer += Time.deltaTime;

        if (_player.moveModule.isMoving) return;
        if (_attackTimer >= _attackDelay)
        {
            if (_player.diceGrid.diceUnitGrid.ContainsKey(_player.positionKey + Vector2Int.left))
            {
                PopupText popup = PoolManager.Inst.Pop(EPoolType.PopupText) as PopupText;
                popup.Popup(_player.diceGrid.grid[_player.positionKey].dicePip.ToString()
                    , _player.diceGrid.grid[_player.positionKey + Vector2Int.left].transform.position + Vector3.up * 0.3f);
                Debug.Log("Left Attack");
                _attackTimer = 0f;
                _player.animator.Play("NormalAttack");
            }
            else if (_player.diceGrid.diceUnitGrid.ContainsKey(_player.positionKey + Vector2Int.right))
            {
                PopupText popup = PoolManager.Inst.Pop(EPoolType.PopupText) as PopupText;
                popup.Popup(_player.diceGrid.grid[_player.positionKey].dicePip.ToString()
                    , _player.diceGrid.grid[_player.positionKey + Vector2Int.right].transform.position + Vector3.up * 0.3f);
                Debug.Log("Right Attack");
                _attackTimer = 0f;
                _player.animator.Play("Move");
                _player.animator.Play("NormalAttack");
            }
        }
    }
}
