using UnityEngine;

namespace Iterium
{

    /// <summary>
    /// Rotate an object
    /// * Axis selection
    /// * Random speed
    /// * Random direction
    /// </summary>

    public class Rotate : MonoBehaviour
    {
        [SerializeField] float speed = 20f; //rotation speed
        [SerializeField] private float randomMaxSpeed; //upper value for random range
        [SerializeField] private bool reverseDirection;   // reverse roation toggle
        [SerializeField] private bool randomDirection = true; //random spin direction
        [SerializeField] private bool randomSpeed = true; //random spin speed
        [SerializeField] private bool rotationX;   // toggle rotation on X axis
        [SerializeField] private bool rotationY = true;   // toggle rotation on Y axis
        [SerializeField] private bool rotationZ;   // toggle rotation on Z axis
        private float finalSpeed; //final calculated spin speed

        private void Start()
        {
            if (randomSpeed)
            {
                finalSpeed = Random.Range(speed, randomMaxSpeed + 1);
            }

            if (reverseDirection)
            {
                finalSpeed -= finalSpeed * 2; // reverse direction
            }
            else if (randomDirection)
            {
                int rnd = Random.Range(1, 3);
                if (rnd == 2)
                {
                    finalSpeed -= finalSpeed * 2; // reverse direction
                }
            }
        }

        void Update()
        {
            if (rotationX)
            {
                transform.Rotate(finalSpeed * Time.deltaTime, 0, 0);
            }
            if (rotationY)
            {
                transform.Rotate(0, finalSpeed * Time.deltaTime, 0);
            }
            if (rotationZ)
            {
                transform.Rotate(0, 0, finalSpeed * Time.deltaTime);
            }
        }
    }
}