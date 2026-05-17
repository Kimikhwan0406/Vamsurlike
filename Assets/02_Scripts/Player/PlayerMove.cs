using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] PlayerInputHandler inputHandler;
    [SerializeField] float moveSpeed = 5f;


    void Update()
    {
        Move();
    }


    void Move()
    {
        transform.position += inputHandler.MoveInput * Time.deltaTime * moveSpeed;
    }


}
