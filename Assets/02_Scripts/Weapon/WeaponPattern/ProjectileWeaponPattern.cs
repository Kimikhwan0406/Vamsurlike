using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class ProjectileWeaponPattern : IWeaponPattern
{
    public void Excute(WeaponContext context, RunTimeWeaponlData data)
    {
        CancellationToken token = context.OwnerTransform.GetCancellationTokenOnDestroy();

        SpawnAsync(context, data, token).Forget();
    }

    async UniTask SpawnAsync(WeaponContext context, RunTimeWeaponlData data, CancellationToken token)
    {
        for (int i = 0; i < data.ProjectileCount; i++)
        {
            if (token.IsCancellationRequested)
                return;

            Spawn(context, data);

            if(i < data.ProjectileCount - 1)
                await UniTask.Delay(System.TimeSpan.FromSeconds(data.RepeatInterval), cancellationToken: token);
        }
    }

    void Spawn(WeaponContext context, RunTimeWeaponlData data)
    {
        var direction = GameManager.Instance.GetPlayer().GetPlayerDir;

        float randomValue = Random.Range(-0.5f, 0.5f);
        Vector3 randomOffset = new Vector3(randomValue, randomValue, 0f);
        var proejectile = PoolManager.Instance.SpawnFromPool<Projectile>("Projectile", context.OwnerTransform.position + randomOffset);
        proejectile.Init(data, direction);
    }
}
