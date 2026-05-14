using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public Vector3 MoveInput;


    [SerializeField] float speed = 5f;


    void Update()
    {
        Move();
    }


    void Move()
    {
        transform.position += MoveInput * Time.deltaTime * speed;
    }


}
