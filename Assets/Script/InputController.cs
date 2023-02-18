using System;
using UnityEngine;

namespace Script
{
    public class InputController : MonoBehaviour
    {
        [SerializeField] private float acceleration;
        [SerializeField] private float horAcceleration;
        [SerializeField] private float autoBrake;

        private float forwardSpeed;
        private float horizontalSpeed;
        private Rigidbody _rigidbody;

        private Vector3 _direction;
        private Vector3 _eurlerAngles;

        // Start is called before the first frame update
        void Start()
        {
            _rigidbody = gameObject.GetComponent<Rigidbody>();
            _direction = new Vector3(0, 0, 1);
            _eurlerAngles = new Vector3(0, 10, 0);
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 move = new Vector3();
            if (Input.GetAxis("Forward") > 0)
            {
                forwardSpeed += acceleration;
            }
            else if (Input.GetAxis("Forward") < 0)
            {
                forwardSpeed -= acceleration;
            }
            else
            {
                /*
                forwardSpeed -= Math.Sign(forwardSpeed) * acceleration*Time.deltaTime;
                if (forwardSpeed < 0.01f) forwardSpeed = 0;
                */
                if (forwardSpeed < 0.1f)
                {
                    forwardSpeed = 0.0f;
                }
                else
                {
                    forwardSpeed /= 1.1f;
                }
            }

            //
            if (Input.GetAxis("Horizontal") > 0.0 && _rigidbody.velocity.sqrMagnitude > 0)
            {
                Quaternion deltaRotation = Quaternion.Euler(_eurlerAngles * Time.fixedDeltaTime);
                _rigidbody.MoveRotation(_rigidbody.rotation * deltaRotation);
                _direction = deltaRotation * _direction;
            }
            else if (Input.GetAxis("Horizontal") < 0.0 && _rigidbody.velocity.sqrMagnitude > 0)
            {
                Quaternion deltaRotation = Quaternion.Euler(-_eurlerAngles * Time.fixedDeltaTime);
                _rigidbody.MoveRotation(_rigidbody.rotation * deltaRotation);
                _direction = deltaRotation * _direction;
            }

            Debug.Log("Power: " + forwardSpeed + "; velocity: " + _rigidbody.velocity);
            _rigidbody.AddForce(forwardSpeed * _direction, ForceMode.Force);
        }
    }
}
