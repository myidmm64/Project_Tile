using System;
using UnityEngine;

public class LookMousePos : MonoBehaviour
{
    [SerializeField]
    private Transform pivotTrm = null;
    [SerializeField]
    private Vector2 _pivotAddPos = Vector2.zero;

    [SerializeField]
    private float _addzRot = 0f;
    [SerializeField]
    private float _radius = 1f;

    public bool isLock = false;

    private void Update()
    {
        if (isLock == false)
        {
            Look();
        }
    }

    private void Look()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = mousePos - ((Vector2)pivotTrm.position + _pivotAddPos);

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rot = Quaternion.Euler(0, 0, angle + _addzRot);

        Vector2 newPosition = ((Vector2)pivotTrm.position + _pivotAddPos) + (dir.normalized * _radius);

        transform.SetPositionAndRotation(newPosition, rot);
    }
}
