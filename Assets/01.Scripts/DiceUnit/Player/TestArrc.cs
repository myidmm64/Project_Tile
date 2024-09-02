using UnityEngine;

public class TestArrc : MonoBehaviour
{
    public int segments = 50;         // ȣ�� �׸� �� ����� ���� ����
    public float maxRadius = 5f;      // ȣ�� �ִ� ������ (������ �ִ� ����)
    public float spreadAngle = 30f;   // ź ���� ����
    private LineRenderer lineRenderer;

    void Start()
    {
        // LineRenderer ������Ʈ ��������
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = segments + 1; // ���� ���� ����
        lineRenderer.useWorldSpace = true;         // ���� ��ǥ�踦 ����ϵ��� ����
    }

    void Update()
    {
        // ���콺 ��ġ�� ���� ��ǥ�� ��ȯ
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
        mousePosition.z = 0; // z ���� 0���� ���� (2D ȯ�濡���� ����� ����)

        // �÷��̾� ��ġ
        Vector3 playerPosition = transform.position;

        // ���콺�� �÷��̾� ��ġ ���� ���� ���
        float angleToMouse = Mathf.Atan2(mousePosition.y - playerPosition.y, mousePosition.x - playerPosition.x) * Mathf.Rad2Deg;

        // ź ������ �׸���
        DrawArcWithLineRenderer(playerPosition, angleToMouse);
    }

    void DrawArcWithLineRenderer(Vector3 center, float angleToMouse)
    {
        float angleStep = spreadAngle / segments; // ���� ���� (���� ������ �� ������ ������)

        for (int i = 0; i <= segments; i++)
        {
            // ���� ���� ���
            float currentAngle = angleToMouse - (spreadAngle / 2) + (i * angleStep);
            float currentAngleRad = Mathf.Deg2Rad * currentAngle; // ���� �������� ��ȯ

            // �� ��ġ ���
            float x = center.x + Mathf.Cos(currentAngleRad) * maxRadius;
            float y = center.y + Mathf.Sin(currentAngleRad) * maxRadius;

            // LineRenderer�� �� �߰�
            lineRenderer.SetPosition(i, new Vector3(x, y, 0));
        }
    }
}
