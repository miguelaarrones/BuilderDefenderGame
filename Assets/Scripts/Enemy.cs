using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static Enemy Create(Vector3 position)
    {
        Transform enemyPrefab = Resources.Load<Transform>("Enemy");
        Transform enemyTransform = Instantiate(enemyPrefab, position, Quaternion.identity);
        Enemy enemy = enemyTransform.GetComponent<Enemy>();
        return enemy;
    }

    private Transform targetTransform;
    private Rigidbody2D rb2D;
    private float lookForTargetTimer;
    private float lookForTargetTimerMax = .2f;
    private HealthSystem healthSystem;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        healthSystem = GetComponent<HealthSystem>();

        if (BuildingManager.Instance.GetHQBuilding() != null)
        {
            targetTransform = BuildingManager.Instance.GetHQBuilding().transform;
        }

        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        healthSystem.OnDied += HealthSystem_OnDied;

        lookForTargetTimer = Random.Range(0f, lookForTargetTimerMax);
    }

    private void HealthSystem_OnDamaged(object sender, System.EventArgs e)
    {
        SoundManager.Instance.PlaySound(SoundManager.Sound.EnemyHit);
        CinemachineShake.Instance.ShakeCamera(2f, .1f);
        ChromaticAberrationScript.Instance.SetWeight(.5f);
    }

    private void HealthSystem_OnDied(object sender, System.EventArgs e)
    {
        Instantiate(Resources.Load<Transform>("EnemyDieParticles"), transform.position, Quaternion.identity);

        Destroy(gameObject);
        SoundManager.Instance.PlaySound(SoundManager.Sound.EnemyDie);

        CinemachineShake.Instance.ShakeCamera(7f, .15f);
        ChromaticAberrationScript.Instance.SetWeight(.5f);
    }

    private void Update()
    {
        HandleMovement();

        HandleTargeting();
    }

    private void HandleMovement()
    {
        if (targetTransform != null)
        {
            Vector3 moveDirection = (targetTransform.position - transform.position).normalized;

            float moveSpeed = 6f;
            rb2D.velocity = moveDirection * moveSpeed;
        }
        else
        {
            rb2D.velocity = Vector3.zero;
        }
    }

    private void HandleTargeting()
    {
        lookForTargetTimer -= Time.deltaTime;
        if (lookForTargetTimer < 0f)
        {
            lookForTargetTimer += lookForTargetTimerMax;
            LookForTargets();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Building building = collision.gameObject.GetComponent<Building>();
        if (building != null)
        {
            // Collided with a building.
            HealthSystem healthSystem = building.GetComponent<HealthSystem>();
            healthSystem.Damage(10);
            this.healthSystem.Damage(999);
        }
    }

    private void LookForTargets()
    {
        float targetMaxRadius = 10f;
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(transform.position, targetMaxRadius);
        foreach(Collider2D collider2D in collider2DArray)
        {
            Building building = collider2D.GetComponent<Building>();
            if (building != null)
            {
                // Is a building!
                if (targetTransform == null)
                {
                    targetTransform = building.transform;
                } else
                {
                    if (Vector3.Distance(transform.position, building.transform.position) < Vector3.Distance(transform.position, targetTransform.position))
                    {
                        // Closer.
                        targetTransform = building.transform;
                    }
                }
            }
        }

        if (targetTransform == null)
        {
            // Found no targets within range.
            if (BuildingManager.Instance.GetHQBuilding() != null)
            {
                targetTransform = BuildingManager.Instance.GetHQBuilding().transform;
            }
        }
    }
}
