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

        // Start is called before the first frame update
        void Start()
        {
            _rigidbody = gameObject.GetComponent<Rigidbody>();
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
                forwardSpeed -= Math.Sign(forwardSpeed) * acceleration*Time.deltaTime;
                if (forwardSpeed < 0.01f) forwardSpeed = 0;
            }

            if (Input.GetAxis("Horizontal") > 0.0 && forwardSpeed > 0)
            {
                horizontalSpeed += horAcceleration;
            }
            else if (Input.GetAxis("Horizontal") < 0.0 && forwardSpeed > 0)
            {
                horizontalSpeed -= horAcceleration;
            }

            Debug.Log(forwardSpeed);
            _rigidbody.AddForce(new Vector3(horizontalSpeed, 0, forwardSpeed), ForceMode.Force);
        }
    }
}