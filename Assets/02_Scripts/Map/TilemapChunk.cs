using System.Collections.Generic;
using UnityEngine;

public class TilemapChunk : MonoBehaviour
{
    const int mapSizeOffSet = 60;

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area")) return;

        Vector3 playerPos = GameManager.Instance.GetPlayer().transform.position;
        var moveInput = GameManager.Instance.GetPlayer().MoveInput;

        float diffX = playerPos.x - transform.position.x;
        float diffY = playerPos.y - transform.position.y;

        int dirX = diffX > 0 ? 1 : -1;
        int dirY = diffY > 0 ? 1 : -1;

        float absDiffX = Mathf.Abs(diffX);
        float absDiffY = Mathf.Abs(diffY);

        float moveX = Mathf.Abs(moveInput.x);
        float moveY = Mathf.Abs(moveInput.y);

        Vector3 rePosition = Vector3.zero;
        if ((moveX > 0 && moveY > 0))
        {
            if (Mathf.Abs(absDiffX - absDiffY) > 1f)
            {
                if (absDiffX > absDiffY)
                {
                    rePosition = new Vector3(transform.position.x + dirX * mapSizeOffSet * 2, transform.position.y, 0);
                }
                else
                {
                    rePosition = new Vector3(transform.position.x, transform.position.y + dirY * mapSizeOffSet * 2, 0);
                }
            }
            else
            {
                rePosition = new Vector3(transform.position.x + dirX * mapSizeOffSet * 2, transform.position.y + dirY * mapSizeOffSet * 2, 0);
            }
        }
        else if (moveX > 0)
        {
            rePosition = new Vector3(transform.position.x + dirX * mapSizeOffSet * 2, transform.position.y, 0);
        }
        else if (moveY > 0)
        {
            rePosition = new Vector3(transform.position.x, transform.position.y + dirY * mapSizeOffSet * 2, 0);
        }

        transform.position = rePosition;
    }
}
