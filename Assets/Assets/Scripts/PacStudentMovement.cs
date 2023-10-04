using UnityEngine;
using System.Collections;

public class PacStudentController : MonoBehaviour
{
    public float moveSpeed = 5f; // Adjust the speed as needed.
    public Transform[] movePositions; // Define the key positions in the clockwise order.
    public float animationThreshold = 0.1f; // Threshold to trigger animations.

    private Animator pacStudentAnimator;
    private int currentMoveIndex = 0;

    void Start()
    {
        pacStudentAnimator = GetComponent<Animator>();
        StartCoroutine(MovePacStudent());
    }

    IEnumerator MovePacStudent()
    {
        while (true)
        {
            Vector3 targetPosition = movePositions[currentMoveIndex].position;
            float distance = Vector3.Distance(transform.position, targetPosition);

            // Calculate the time based on distance and moveSpeed to achieve consistent speed.
            float moveTime = distance / moveSpeed;

            // Calculate the direction to the target waypoint.
            Vector3 moveDirection = (targetPosition - transform.position).normalized;

            // Rotate PacStudent to face the target waypoint.
            //float targetRotation = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            //transform.rotation = Quaternion.Euler(0f, 0f, targetRotation);

            // Calculate the Horizontal and Vertical values based on direction.
            float horizontal = Mathf.Abs(moveDirection.x) > animationThreshold ? moveDirection.x : 0f;
            float vertical = Mathf.Abs(moveDirection.y) > animationThreshold ? moveDirection.y : 0f;

            // Set the animator parameters for walking animation.
            pacStudentAnimator.SetFloat("Horizontal", horizontal);
            pacStudentAnimator.SetFloat("Vertical", vertical);

            // Wait for a brief moment to allow rotation.
            yield return new WaitForSeconds(0.5f);

            // Tween PacStudent's position over time.
            float elapsedTime = 0f;
            Vector3 startPosition = transform.position;
            while (elapsedTime < moveTime)
            {
                transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Ensure PacStudent reaches the exact position.
            transform.position = targetPosition;

            // Reset animator parameters.
            pacStudentAnimator.SetFloat("Horizontal", 0f);
            pacStudentAnimator.SetFloat("Vertical", 0f);

            // Increment the move index in a circular manner.
            currentMoveIndex = (currentMoveIndex + 1) % movePositions.Length;

            // Wait for a brief pause between movements.
            yield return new WaitForSeconds(0.5f); // Adjust as needed.
        }
    }
}
