using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPoint : MonoBehaviour
{
    public Enemy Enemy { get; private set; }
    private bool _isEnabled;

    public bool IsEnabled
    {
        get { return _isEnabled; }
        set
        {
            _collider.enabled = value;
            _isEnabled = value;
        }
    }

    public Vector3 Position => transform.position;
    private const int ENEMY_LAYER_MASK = 1 << 9;
    private static Collider[] _buffer = new Collider[100];
    public static int BufferedCount { get; private set; }
    public float ColliderSize { get; private set; }
    private SphereCollider _collider;

    private void Awake()
    {
        Enemy = transform.root.GetComponent<Enemy>();
        _collider = GetComponent<SphereCollider>();
        ColliderSize = GetComponent<SphereCollider>().radius * transform.localScale.x;
    }

    public static bool FillBuffer(Vector3 position, float range)
    {
        Vector3 top = position;
        top.y += 3f;
        BufferedCount = Physics.OverlapCapsuleNonAlloc(position, top, range, _buffer, ENEMY_LAYER_MASK);
        return BufferedCount > 0;
    }

    public static TargetPoint GetBuffered(int index)
    {
        var taget = _buffer[index].GetComponent<TargetPoint>();
        return taget;
    }
}