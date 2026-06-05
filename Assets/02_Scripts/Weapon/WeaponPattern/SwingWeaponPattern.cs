using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class SwingWeaponPattern : IWeaponPattern
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

            var facingDir = GameManager.Instance.GetPlayer().CurrentFacingDir;
            facingDir = facingDir * (i % 2 == 0 ? 1 : -1);

            SpawnWeapon(context, data, facingDir);

            if(i < data.ProjectileCount - 1)
                await UniTask.Delay(TimeSpan.FromSeconds(data.RepeatInterval), cancellationToken: token);
        }
    }

    void SpawnWeapon(WeaponContext context, RunTimeWeaponlData data, int facingDir)
    {
        var effect = PoolManager.Instance.SpawnFromPool<EllipseObject>(data.WeaponId, context.OwnerTransform.position);
        effect.GetComponent<EllipseObject>().Init(data, facingDir);
    }
}
