using UnityEngine;

namespace Script
{
    public class CameraMove : MonoBehaviour
    {
        [SerializeField] private GameObject car;

        [SerializeField] private Vector3 offset;

        // Start is called before the first frame update
        void Start()
        {
            offset = transform.position - car.transform.position;
        }

        // Update is called once per frame
        void LateUpdate()
        {
            transform.position = car.transform.position + offset;
        }
    }
}