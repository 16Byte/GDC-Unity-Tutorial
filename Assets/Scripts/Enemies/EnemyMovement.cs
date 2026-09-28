using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed;
    public float patrolRadius;
    public bool playerSpotted;

    Transform playerTransform;
    Vector3 patrolPoint;
    float timer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // this searches the entire heirarchy for the exact object named "Player". This is epensive so best called sparingly.
        var playerObject = GameObject.Find("Player");
        // and print a warning if no player found
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
            //Debug.Log($"{gameObject.name} could not fine a player object in the scene");
        }


        // setting this at start to avoid weird pathing
        patrolPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        // check for player line of sight (this linecast will return true if the line of sight is blocked)
        if (playerTransform == null || Physics.Linecast(transform.position, playerTransform.position))
        {
            Seek(patrolPoint);
            playerSpotted = false;
        }
        else
        {
            Vector3 targetPosition = Vector3.ProjectOnPlane(playerTransform.position, Vector3.up) + transform.position.y * Vector3.up;
            Seek(targetPosition);
            playerSpotted = true;
        }

        // the enemy should pick a random patrol point to go to after some time.

        timer -= Time.deltaTime;
        if (timer < 0)
        {
            // pick a random patrol point after ranomd time between 0 and 20 seconds;

            patrolPoint = Vector3.ProjectOnPlane(Random.insideUnitSphere * patrolRadius, Vector3.up) + transform.position.y * Vector3.up;
            timer = Random.value * 20f;
        }

    }

    void Seek(Vector3 target)
    {
        // go toward the target vector at move speed
        // also try to avoid colliding with scene geometry

        Vector3 targetDirection = target - transform.position;
        Vector3 moveDirection = targetDirection;

        // use a spherecast to check for obsacles

        RaycastHit hit;
        if (Physics.SphereCast(transform.position, 1f, targetDirection, out hit, 1f))
        {
            // go counter-clockwise around the object
            moveDirection = Vector3.Cross(hit.normal, Vector3.up);
        }
        
        // donlt move if already too close
        if (targetDirection.magnitude > .1f)
        {
            transform.position += moveDirection.normalized * moveSpeed * Time.deltaTime;
        }
        
    }

    private void OnDrawGizmos()
    {
        if (playerSpotted)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, playerTransform.position);
        }
        else
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, patrolPoint);
            Gizmos.DrawWireSphere(patrolPoint, 1f);
        }
        
    }

}
