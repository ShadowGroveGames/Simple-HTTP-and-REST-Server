using UnityEngine;

namespace ShadowGroveGames.SimpleHttpAndRestServer.Examples
{
    public class Example1_PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        public float PlayerSpeed = 3.0F;
        public float GravityValue = -9.81f;

        [Header("Stats")]
        public float MaxHealth = 10;
        public float CurrentHealth = 9;

        private CharacterController _controller;
        private Vector3 _playerVelocity;

        // Start is called before the first frame update
        void Start()
        {
            _controller = GetComponent<CharacterController>();
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            _controller.Move(move * Time.deltaTime * PlayerSpeed);

            if (move != Vector3.zero)
            {
                gameObject.transform.forward = move;
            }

            _playerVelocity.y += GravityValue * Time.deltaTime;
            _controller.Move(_playerVelocity * Time.deltaTime);
        }
    }
}
