using UnityEngine;

// Garlic 기준으로 오브젝트 scale : drawRadius = 1 : 2.5 비율임

public class AreaEffect : MonoBehaviour
{
    [SerializeField] Transform objectTransform;

    public void SetRange(float range)
    {
        objectTransform.localScale = Vector3.one * range / 2.5f;
    }
}
