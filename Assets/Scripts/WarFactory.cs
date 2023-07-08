using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WarFactory : GameObjectFactory
{
    [SerializeField] private Shell _shellPrefab;
    [SerializeField] private Explosion _explosionPrefab;
    public Shell GetShell => Get(_shellPrefab);
    public Explosion GetExplosion => Get(_explosionPrefab);
    private T Get<T>(T prefab)where T:WarEntity
    {
        T inctance = CreateGameObjectInstance(prefab);
        inctance.OriginFactory = this;
        return inctance;
    }

    public void Reclaim(WarEntity entity)
    {
        Destroy(entity.gameObject);
    }
}
