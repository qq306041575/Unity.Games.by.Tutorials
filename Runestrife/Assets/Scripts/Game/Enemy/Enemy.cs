using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float maxHealth = 100f;
    public float health = 100f;
    public float moveSpeed = 3f;
    public int goldDrop = 10;
    public int pathIndex = 0;
    public float timeEnemyStaysFrozenInSeconds = 2f;
    public bool frozen;

    private float freezeTimer;
    private int wayPointIndex = 0;

    void Start()
    {
        EnemyManager.Instance.RegisterEnemy(this);
    }

    void OnGotToLastWayPoint()
    {
        GameManager.Instance.OnEnemyEscape();
        Die();
    }

    public void TakeDamage(float amountOfDamage)
    {
        health -= amountOfDamage;

        if (health <= 0)
        {
            DropGold();
            Die();
        }
    }

    void DropGold()
    {
        GameManager.Instance.gold += goldDrop;
    }

    void Die()
    {
        if (gameObject != null)
        {
            EnemyManager.Instance.UnRegister(this);
            gameObject.AddComponent<AutoScaler>().scaleSpeed = -2;
            enabled = false;
            Destroy(gameObject, 0.3f);
        }
    }

    void Update()
    {
        if (wayPointIndex < WayPointManager.Instance.Paths[pathIndex].WayPoints.Count)
        {
            UpdateMovement();
        }
        else
        {
            OnGotToLastWayPoint();
        }
        if (frozen)
        {
            freezeTimer += Time.deltaTime;
            if (freezeTimer >= timeEnemyStaysFrozenInSeconds)
            {
                Defrost();
            }
        }
    }

    private void UpdateMovement()
    {
        Vector3 targetPosition = WayPointManager.Instance.Paths[pathIndex].WayPoints[wayPointIndex].position;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        transform.localRotation = UtilityMethods.SmoothlyLook(transform, targetPosition);
        if (Vector3.Distance(transform.position, targetPosition) < .1f)
        {
            wayPointIndex++;
        }
    }

    public void Freeze()
    {
        if (!frozen)
        {
            frozen = true;
            moveSpeed /= 2;
        }
    }

    void Defrost()
    {
        freezeTimer = 0f;
        frozen = false;
        moveSpeed *= 2;
    }
}
