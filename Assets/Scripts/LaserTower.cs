using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTower : Tower
{
    [SerializeField, Range(1f, 100f)] private float _damagePerSecond = 15f;
    [SerializeField] private Transform _turret;
    [SerializeField] private Transform _lazerBeam;
    private Vector3 _laserBeamScale;
    private Vector3 _laserBeamStartPos;
    private TargetPoint _target;
    public override TowerType Type => TowerType.Laser;
    private void Awake()
    {
        _laserBeamScale = _lazerBeam.localScale;
        _laserBeamStartPos = _lazerBeam.localPosition;
    }
    private void Shoot()
    {
        var point = _target.Position;
        _turret.LookAt(point);
        _lazerBeam.localRotation = _turret.localRotation;
        var distance = Vector3.Distance(_turret.position, point);
        _laserBeamScale.z = distance;
        _lazerBeam.localScale = _laserBeamScale;
        _lazerBeam.localPosition = _laserBeamStartPos + 0.5f * distance * _lazerBeam.forward;
        _target.Enemy.TakeDamage(_damagePerSecond * Time.deltaTime);
    }
    public override void GameUpdate()
    {
        if (IsTargetTracked( ref _target)||IsAcquireTarget(out _target))
        {
            Shoot();
        }
        else
        {
            _lazerBeam.localScale=Vector3.zero;
        }
    }
}
