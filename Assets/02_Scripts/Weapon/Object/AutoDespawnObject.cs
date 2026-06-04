using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class AutoDespawnObject : MonoBehaviour
{
    CancellationTokenSource token;
    float timeOut = 5f;

    protected virtual void OnEnable()
    {
        token = new CancellationTokenSource();
    }

    protected virtual void Update()
    {
        TimeOut(token.Token).Forget();
    }

    protected virtual void Release()
    {
        CancelToken();
        PoolManager.Instance.DespawnToPool(this.gameObject);
    }


    async UniTask TimeOut(CancellationToken token)
    {
        await UniTask.Delay(System.TimeSpan.FromSeconds(timeOut), cancellationToken: token);
        Release();
    }

    void CancelToken()
    {
        token?.Cancel();
        token?.Dispose();
    }
}
