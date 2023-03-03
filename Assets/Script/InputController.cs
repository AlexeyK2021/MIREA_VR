using System;
using UnityEngine;

namespace Script
{
    public class InputController : MonoBehaviour
    {
        private Vector3 tractionForce;                        // F(traction) = engineForce * dir
        private const float ENGINE_FORCE = 40000;
        private float engineForce;
        private Vector3 airResistForce;                       // F(drag) = -const * velocity * |velocity| 
        private const float AIR_RESIST = 0.2257f;      
        private Vector3 frictionForce;                        // F(rr) = -const * velocity
        private const float FRICTION = AIR_RESIST * 30;
        private Vector3 F;                                    // F(long) = F(traction) + F(drag) * F(rr)
        private Vector3 breakingForce;                        // F(br) = -breakingForce * dir
        private const float BREAKING = 10000;
        [SerializeField] private float TURNING_ANGLE = 5;
        [SerializeField] private float VEHICLE_LENGHT = 3;
        private float R;
        //private float Wf;                                     // weight on the front axle
        //private float Wr;                                     // weight on the back axle
        //private const float G = 9.8f;
        //private Vector3 acceleration = Vector3.zero;
        //private Vector3 lastVelocity = Vector3.zero;

        private Rigidbody _rigidbody;

        private Vector3 _eurlerAngles;

        // Start is called before the first frame update
        void Start()
        {
            _rigidbody = gameObject.GetComponent<Rigidbody>();
            _eurlerAngles = new Vector3(0, 10, 0);
            R = VEHICLE_LENGHT / (float)Math.Sin(TURNING_ANGLE) * 100;
        }

        // Update is called once per frame
        void Update()
        {
            /* For the future
            acceleration = (_rigidbody.velocity - lastVelocity) / Time.deltaTime;
            Wf = 0.5f * _rigidbody.mass * G - (0.2f / 2.72f) * _rigidbody.mass * acceleration.sqrMagnitude;
            Wr = 0.5f * _rigidbody.mass * G + (0.2f / 2.72f) * _rigidbody.mass * acceleration.sqrMagnitude;
            */

            tractionForce = ENGINE_FORCE * _rigidbody.transform.forward;
            airResistForce = -AIR_RESIST * _rigidbody.velocity * _rigidbody.velocity.sqrMagnitude;
            frictionForce = -FRICTION * _rigidbody.velocity;
            breakingForce = -BREAKING * _rigidbody.velocity.normalized;

            if (Input.GetAxis("Forward") > 0) // Move forward
            {
                F = tractionForce + airResistForce + frictionForce;
            }
            else if (Input.GetAxis("Forward") < 0) // Move back
            {
                F = -(tractionForce / 1.25f) + airResistForce + frictionForce;
            }
            else // do nothing
            {
                F = airResistForce + frictionForce;
            }

            if (Input.GetAxis("Jump") > 0.0f && _rigidbody.velocity.sqrMagnitude > 0.5) // Breaking
            {
                F = breakingForce + airResistForce + frictionForce;
            }


            /*
            Debug.Log("TractionForce: " + tractionForce +
                "\nAirResistForce: " + airResistForce +
                "\nFrictionForce: " + frictionForce +
                "\nF: " + F +
                "\nVelocity: " + _rigidbody.velocity);
            */
            Debug.Log("AngVel: " + _rigidbody.angularVelocity + "\nVel: " + _rigidbody.velocity);
            _rigidbody.AddForce(F, ForceMode.Force);
            //_rigidbody.AddForceAtPosition(F, transform.TransformPoint(new Vector3(0, 0, -1)), ForceMode.Force);

            // Rotation
            if (Input.GetAxis("Horizontal") > 0.0 && _rigidbody.velocity.sqrMagnitude > 0)
            {
                //Quaternion deltaRotation = Quaternion.Euler(_eurlerAngles * Time.fixedDeltaTime);
                //_rigidbody.MoveRotation(_rigidbody.rotation * deltaRotation);
                _rigidbody.angularVelocity = new Vector3(0.0f, -_rigidbody.velocity.sqrMagnitude / R, 0.0f); 
            }
            else if (Input.GetAxis("Horizontal") < 0.0 && _rigidbody.velocity.sqrMagnitude > 0)
            {
                //Quaternion deltaRotation = Quaternion.Euler(-_eurlerAngles * Time.fixedDeltaTime);
                //_rigidbody.MoveRotation(_rigidbody.rotation * deltaRotation);
                _rigidbody.angularVelocity = new Vector3(0.0f, _rigidbody.velocity.sqrMagnitude / R, 0.0f); 
            }
        }
    }
}
