using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private List<Transform> _goalLocations = new List<Transform>();
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private GameObject _player;

    private Element element;
    // Start is called before the first frame update

    void Update()
    {
        Vector2 currPosition = Vector2.zero;

        switch ((int) element)
        {
            case 0:
                GetComponent<SpriteRenderer>().enabled = false; // deactivate the arrow
                break;
            case 1:
                GetComponent<SpriteRenderer>().enabled = true; // activate the arrow since there is an element
                currPosition = _goalLocations[1].position;
                MoveTowardsPosition(_goalLocations[1].position);

                break;
            case 2:
                GetComponent<SpriteRenderer>().enabled = true;
                currPosition = _goalLocations[0].position;
                MoveTowardsPosition(_goalLocations[0].position);
                break;
            case 3:
                GetComponent<SpriteRenderer>().enabled = true;
                currPosition = _goalLocations[1].position;
                MoveTowardsPosition(_goalLocations[1].position);
                break;
        }
    }

    private void MoveTowardsPosition(Vector3 goalLocation)
    {
        // get the bounds of the screen
        Vector3 topRight = _mainCamera.ViewportToWorldPoint(new Vector3(0, 0, -10));
        Vector3 bottomLeft = _mainCamera.ViewportToWorldPoint(new Vector3(1, 1, -10));

        float topBound = topRight.y;
        float bottomBound = bottomLeft.y;
        float leftBound = bottomLeft.x;
        float rightBound = topRight.x;

        // get the direction of the goal position from the player
        Vector3 direction = (goalLocation - _player.transform.position).normalized;
        direction /= 20;

        Vector3 targetForward = Quaternion.LookRotation(direction) * Vector3.forward;
        transform.rotation = Quaternion.Euler(targetForward);

        // check that the position is within camera bounds
        if (goalLocation.y < topBound && goalLocation.y > bottomBound && goalLocation.x > leftBound && goalLocation.x < rightBound)
        {
            transform.position = goalLocation - 3 * direction;
        }
        else
        {
            Vector3 currPosition = _player.transform.position;
            Vector3 tmpPosition = currPosition;
            for (int i = 2; i < 500; i++)
            {
                
                tmpPosition += direction;
                if (tmpPosition.y > topBound || tmpPosition.y < bottomBound || tmpPosition.x < leftBound || tmpPosition.x > rightBound)
                {
                    currPosition = tmpPosition - direction * 20;
                    break;
                }
            }

            transform.position = currPosition;
        }
    }

    public void SetElement(Element element)
    {
        this.element = element;
    }
}
