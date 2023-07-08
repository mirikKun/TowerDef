using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortarTower : Tower
{
    [SerializeField, Range(0.5f, 2f)] private float _shootsPerSeconds = 1f;
    [SerializeField, Range(0.5f, 3f)] private float _shellBlastRadious = 1f;
    [SerializeField, Range(1f, 100f)] private float _damage = 1f;
    [SerializeField] private Transform _mortar;
    public override TowerType Type => TowerType.Mortar;
    private float _launchSpeed;
    private float _launchedProgress;

    private void Awake()
    {
        OnValidate();
    }

    private void OnValidate()
    {
        float x = _targetingRange+0.251f;
        float y = _mortar.position.y;
        _launchSpeed = Mathf.Sqrt(9.81f * (y + Mathf.Sqrt(x * x + y * y)));
    }

    public override void GameUpdate()
    {
        _launchedProgress += Time.deltaTime * _shootsPerSeconds;
        while (_launchedProgress >= 1)
        {
            if (IsAcquireTarget(out TargetPoint target))
            {
                Launch(target);
                _launchedProgress -= 1;
            }
            else
            {
                _launchedProgress = 0.999f;
            }
        }
    }


    public void Launch(TargetPoint target)
    {
        
        Vector3 launchPoint = _mortar.position;
        Vector3 targetPoint = target.Position;
        targetPoint.y = 0;

        Vector2 dir;
        dir.x = targetPoint.x - launchPoint.x;
        dir.y = targetPoint.z - launchPoint.z;
        float x = dir.magnitude;
        float y = -launchPoint.y;
        dir /= x;
        float g = 9.81f;
        float s = _launchSpeed;
        float s2 = s * s;

        float r = s2 * s2 - g * (g * x * x + 2f * y * s2);
        float tanTheta = (s2 + Mathf.Sqrt(r)) / (g * x);
        float cosTheta = Mathf.Cos(Mathf.Atan(tanTheta));
        float sinTheta = cosTheta * tanTheta;
        _mortar.localRotation = Quaternion.LookRotation(new Vector3(dir.x, tanTheta, dir.y));
        Game.SpawnShell().Initialize(launchPoint, targetPoint,
            new Vector3(s * cosTheta * dir.x, s * sinTheta, s * cosTheta * dir.y),_shellBlastRadious,_damage);
      
    }
}