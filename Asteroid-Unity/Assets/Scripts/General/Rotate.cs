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
        [SerializeField] float speed = 20f;
        [SerializeField] private float randomMaxSpeed;
        [SerializeField] private bool reverseDirection;
        [SerializeField] private bool randomDirection = true;
        [SerializeField] private bool randomSpeed = true;
        [SerializeField] private bool rotationX;
        [SerializeField] private bool rotationY = true;
        [SerializeField] private bool rotationZ;
        private float finalSpeed;

        private void Start()
        {
            if (randomSpeed)
            {
                finalSpeed = Random.Range(speed, randomMaxSpeed + 1);
            }

            if (reverseDirection)
            {
                finalSpeed -= finalSpeed * 2; 
            }
            else if (randomDirection)
            {
                int rnd = Random.Range(1, 3);
                if (rnd == 2)
                {
                    finalSpeed -= finalSpeed * 2;
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