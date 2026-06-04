using UnityEngine;

public class DrawGizmo : MonoBehaviour
{
    [SerializeField] Color _color = Color.cyan;
    [SerializeField] float _radius = 1f;
    [SerializeField] Vector3 positionOffset = Vector3.zero;

    void OnEnable()
    {
        Debug.Log("디버그용 스크립트입니다. 삭제해주세요");
    }

    void OnDrawGizmos()
    {
        Gizmos.color = _color;
        Gizmos.DrawWireSphere(transform.position + positionOffset, _radius);
    }
}
