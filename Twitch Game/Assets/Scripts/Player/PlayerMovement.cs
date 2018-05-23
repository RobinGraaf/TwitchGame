using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpingForce;
    private Rigidbody _rb;
    private bool _isJumping = true;

    // Use this for initialization
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal") * _speed;
        float moveVertical = Input.GetAxis("Vertical") * _speed;

        moveHorizontal *= Time.deltaTime;
        moveVertical *= Time.deltaTime;
        transform.Translate(moveHorizontal, 0, moveVertical);

        if (!_isJumping)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _rb.AddForce(0, _jumpingForce, 0);
                _isJumping = true;
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Ground")
        {
            _isJumping = false;
        }
    }
}