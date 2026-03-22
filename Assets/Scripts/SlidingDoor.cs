using UnityEngine;
using System.Collections;

public class SlidingDoor : MonoBehaviour
{
    [Header("Door Parts")]
    public Transform door;

    [Header("Positions")]
    public Vector3 closedLocalPosition;
    public Vector3 openLocalPosition;

    [Header("Settings")]
    public float moveSpeed = 2f;
    public float closeDelay = 0.5f;

    private Coroutine moveCoroutine;
    private int playerCount = 0;

    private void Start()
    {
        door.localPosition = closedLocalPosition;
    }

    public void PlayerEntered()
    {
        playerCount++;

        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        moveCoroutine = StartCoroutine(MoveDoor(openLocalPosition));
    }

    public void PlayerExited()
    {
        playerCount--;

        if (playerCount < 0)
            playerCount = 0;

        if (playerCount == 0)
        {
            if (moveCoroutine != null)
                StopCoroutine(moveCoroutine);

            moveCoroutine = StartCoroutine(CloseAfterDelay());
        }
    }
    
    private IEnumerator CloseAfterDelay()
    {
        yield return new WaitForSeconds(closeDelay);
        yield return MoveDoor(closedLocalPosition);
    }

    private IEnumerator MoveDoor(Vector3 targetPos)
    {
        while (Vector3.Distance(door.localPosition, targetPos) > 0.01f)
        {
            door.localPosition = Vector3.MoveTowards(
                door.localPosition,
                targetPos,
                moveSpeed * Time.deltaTime
                );

            yield return null;
        }

        door.localPosition = targetPos;
    }
}
