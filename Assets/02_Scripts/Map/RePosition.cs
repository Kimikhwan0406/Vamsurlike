using UnityEngine;

public class RePosition : MonoBehaviour
{
    const int mapSizeOffSet = 60;

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area")) return;

        Vector3 playerPos = collision.gameObject.transform.position;
        Vector3 myPos = transform.position;

        float diffX = playerPos.x - myPos.x;
        float diffY = playerPos.y - myPos.y;

        int dirX = diffX > 0 ? 1 : -1;
        int dirY = diffY > 0 ? 1 : -1;

        float absDiffX = Mathf.Abs(diffX);
        float absDiffY = Mathf.Abs(diffY);

        float threshold = mapSizeOffSet * 0.5f;

        float moveX = absDiffX > threshold ? dirX * mapSizeOffSet * 2 : 0;
        float moveY = absDiffY > threshold ? dirY * mapSizeOffSet * 2 : 0;

        transform.Translate(new Vector3(moveX, moveY, 0));
    }
}
