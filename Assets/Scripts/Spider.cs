using UnityEngine;

public class Spider : PlatformerAI
{
    private int patrolIndex;
    public Transform[] patrolPoints;
    public float timerBetweenPoints = 1.5f;

    private float currentTimer;
    private bool patrolWaitInitiated;

    protected new void FixedUpdate()
    {
        base.FixedUpdate();

        if (IsAlive == false || EntityActive == false) 
        {
            Move(Vector2.zero);
            return; 
        }


        if (patrolWaitInitiated)
        {
            if (currentTimer > 0)
            {
                currentTimer -= Time.deltaTime;
            }
            else 
            {
                patrolWaitInitiated = false;
                patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
            }
        }
        else 
        {
            Vector2 currentPosition = transform.position;
            Vector2 targetPosition = new Vector2(patrolPoints[patrolIndex].position.x, currentPosition.y);

            float distanceFromTarget = Vector2.Distance(currentPosition, targetPosition);

            if (distanceFromTarget > 0.2f)
            {
                Vector2 direction = targetPosition - currentPosition;

                direction.Normalize();
                print(direction);
                Move(direction * 30 * Time.deltaTime);

            }
            else
            {
                patrolWaitInitiated = true;
                currentTimer = timerBetweenPoints;
                Move(Vector2.zero);
            }
        }

    }


    public override void EntityReset() 
    {
        base.EntityReset();
        currentTimer = 0;
        patrolIndex = 0;
    }



}
