using UnityEngine;

public class Player : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 7)
        {
            Debug.Log("Player is in contact with an enemy.");
        }
    }
}
