using UnityEngine;

public class AttackRange : MonoBehaviour
{
    [SerializeField]
    private Transform _centerTrm = null;
    [SerializeField]
    private Vector2 addPos = Vector2.zero;

    [SerializeField]
    private float _radius = 1f;

    private void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = mousePos - ((Vector2)_centerTrm.position + addPos);

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rot = Quaternion.Euler(0,0,angle);

        Vector2 newPosition = ((Vector2)_centerTrm.position + addPos) + (dir.normalized * _radius);

        transform.SetPositionAndRotation(newPosition,rot);
    }
}
