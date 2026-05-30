using UnityEngine;

public class RePosition : MonoBehaviour
{
    const int mapSizeOffSet = 60;

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area")) return;

        Vector3 playerPos = collision.gameObject.transform.position;

        float diffX = playerPos.x - transform.position.x;
        float diffY = playerPos.y - transform.position.y;

        int dirX = diffX > 0 ? 1 : -1;
        int dirY = diffY > 0 ? 1 : -1;

        diffX = Mathf.Abs(diffX);
        diffY = Mathf.Abs(diffY);

        if (diffX > diffY)
        {
            transform.Translate(Vector3.right * dirX * mapSizeOffSet * 2);
        }
        else if (diffX < diffY)
        {
            transform.Translate(Vector3.up * dirY * mapSizeOffSet * 2);
        }
        else
        {
            transform.Translate(new Vector3(dirX, dirY, 0) * mapSizeOffSet * 2);
        }
    }
}
