using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyWaveUI : MonoBehaviour
{
    [SerializeField] EnemyWaveManager enemyWaveManager;
    [SerializeField] private TextMeshProUGUI waveNumberText;
    [SerializeField] private TextMeshProUGUI waveMessageText;
    [SerializeField] private RectTransform enemySpawnPositionIndicator;
    [SerializeField] private RectTransform enemyClosestPositionIndicator;

    private void Start()
    {
        enemyWaveManager.OnWaveNumberChanged += EnemyWaveManager_OnWaveNumberChanged;
        SetWaveNumberText($"Wave {enemyWaveManager.GetNextWaveSpawnTimer()}");
    }

    private void EnemyWaveManager_OnWaveNumberChanged(object sender, System.EventArgs e)
    {
        SetWaveNumberText($"Wave {enemyWaveManager.GetNextWaveSpawnTimer()}");
    }

    private void Update()
    {
        HandleNextWaveMessage();
        HandleEnemyWaveSpawnPositionIndicator();
        HandleEnemyClosestPositionIndicator();   
    }

    private void HandleNextWaveMessage()
    {
        float nextWaveSpawnTimer = enemyWaveManager.GetNextWaveSpawnTimer();
        if (nextWaveSpawnTimer <= 0f)
        {
            SetMessageText("");
        }
        else
        {
            SetMessageText($"Next wave in {nextWaveSpawnTimer.ToString("F1")}s");
        }
    }

    private void HandleEnemyWaveSpawnPositionIndicator()
    {
        Vector3 dirToNextSpawnPosition = (enemyWaveManager.GetSpawnPosition() - Camera.main.transform.position).normalized;

        enemySpawnPositionIndicator.anchoredPosition = dirToNextSpawnPosition * 300f;
        enemySpawnPositionIndicator.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVector(dirToNextSpawnPosition));

        float distanceToNextSpawnPosition = Vector3.Distance(enemyWaveManager.GetSpawnPosition(), Camera.main.transform.position);
        enemySpawnPositionIndicator.gameObject.SetActive(distanceToNextSpawnPosition > Camera.main.orthographicSize * 1.5f);
    }

    private void HandleEnemyClosestPositionIndicator()
    {
        float targetMaxRadius = 9999f;
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(Camera.main.transform.position, targetMaxRadius);
        Enemy targetEnemy = null;
        foreach (Collider2D collider2D in collider2DArray)
        {
            Enemy enemy = collider2D.GetComponent<Enemy>();
            if (enemy != null)
            {
                // Is an enemy!
                if (targetEnemy == null)
                {
                    targetEnemy = enemy;
                }
                else
                {
                    if (Vector3.Distance(transform.position, enemy.transform.position) < Vector3.Distance(transform.position, targetEnemy.transform.position))
                    {
                        // Closer.
                        targetEnemy = enemy;
                    }
                }
            }
        }
        if (targetEnemy != null)
        {
            Vector3 dirToClosestEnemy = (targetEnemy.transform.position - Camera.main.transform.position).normalized;

            enemyClosestPositionIndicator.anchoredPosition = dirToClosestEnemy * 250f;
            enemyClosestPositionIndicator.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVector(dirToClosestEnemy));

            float distanceToClosestEnemy = Vector3.Distance(enemyWaveManager.GetSpawnPosition(), Camera.main.transform.position);
            enemyClosestPositionIndicator.gameObject.SetActive(distanceToClosestEnemy > Camera.main.orthographicSize * 1.5f);
        }
        else
        {
            // No enemies alive
            enemyClosestPositionIndicator.gameObject.SetActive(false);
        }
    }

    private void SetMessageText(string message)
    {
        waveMessageText.SetText(message);
    }

    private void SetWaveNumberText(string text)
    {
        waveNumberText.SetText(text);
    }
}
