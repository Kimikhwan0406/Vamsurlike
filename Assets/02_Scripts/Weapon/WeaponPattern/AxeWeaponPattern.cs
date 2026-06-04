using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class AxeWeaponPattern : IWeaponPattern
{
    public void Excute(WeaponContext context, RunTimeWeaponlData data)
    {
        CancellationToken token = context.OwnerTransform.GetCancellationTokenOnDestroy();

        SpawnAxesAsync(context, data, token).Forget();
    }

    async UniTaskVoid SpawnAxesAsync(
        WeaponContext context,
        RunTimeWeaponlData data,
        CancellationToken token)
    {
        for (int i = 0; i < data.ProjectileCount; i++)
        {
            if (token.IsCancellationRequested)
                return;

            SpawnAxe(context, data, i);

            if (i < data.ProjectileCount - 1)
            {
                await UniTask.Delay(
                    TimeSpan.FromSeconds(0.2f),
                    cancellationToken: token
                );
            }
        }
    }

    void SpawnAxe(WeaponContext context, RunTimeWeaponlData data, int index)
    {
        if (context.OwnerTransform == null)
            return;

        Vector2 startPos = context.OwnerTransform.position + Vector3.up * 0.5f;

        float horizontalSpeed = index * 2f * GameManager.Instance.GetPlayer().GetPlayerXDir;
        float upwardSpeed = 7f + index * 0.4f;

        Vector2 initialVelocity = new Vector2(horizontalSpeed, upwardSpeed);


        var axeObj = PoolManager.Instance.SpawnFromPool<Axe>(data.WeaponId, startPos);
        axeObj.Init(initialVelocity, 10f, context.CombatQuerySystem, data);
    }
}
