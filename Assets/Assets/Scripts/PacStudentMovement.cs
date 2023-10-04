using UnityEngine;
using System.Collections;

public class PacStudentController : MonoBehaviour
{
    public float moveSpeed = 5f; // Adjust the speed as needed.
    public Transform[] movePositions; // Define the key positions in the clockwise order.

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
            float targetRotation = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;

            // Trigger the walking animation.
            pacStudentAnimator.SetBool("IsMoving", true);

            // Tween PacStudent's position and rotation over time.
            float elapsedTime = 0f;
            Vector3 startPosition = transform.position;
            Quaternion startRotation = transform.rotation;
            while (elapsedTime < moveTime)
            {
                transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveTime);
                transform.rotation = Quaternion.Lerp(startRotation, Quaternion.Euler(0f, 0f, targetRotation), elapsedTime / moveTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Ensure PacStudent reaches the exact position and rotation.
            transform.position = targetPosition;
            transform.rotation = Quaternion.Euler(0f, 0f, targetRotation);

            // Trigger the idle animation.
            pacStudentAnimator.SetBool("IsMoving", false);

            // Increment the move index in a circular manner.
            currentMoveIndex = (currentMoveIndex + 1) % movePositions.Length;

            // Wait for a brief pause between movements.
            yield return new WaitForSeconds(0.5f); // Adjust as needed.
        }
    }
}
