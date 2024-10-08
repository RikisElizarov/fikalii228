using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private Vector3 initialPlayerPosition;
    private Quaternion initialPlayerRotation;
    private Rigidbody playerRigidbody;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Сохранение начальной позиции и состояния игрока
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                initialPlayerPosition = player.transform.position;
                initialPlayerRotation = player.transform.rotation;
                playerRigidbody = player.GetComponent<Rigidbody>();
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void GameOver()
    {
        Debug.Log("Game Over!");
        Time.timeScale = 0f;
        ShowGameOverScreen();
    }

    private void ShowGameOverScreen()
    {
        DeathScreenManager deathScreenManager = FindObjectOfType<DeathScreenManager>();
        if (deathScreenManager != null)
        {
            deathScreenManager.ShowDeathScreen();
        }
    }

    public void RestartGame()
    {
        ResetLevel();
        ResetGameObjects();
    }

    private void ResetLevel()
    {
        Time.timeScale = 1f;

        // Восстановление позиции игрока и его параметров
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            player.transform.position = initialPlayerPosition;
            player.transform.rotation = initialPlayerRotation;

            if (playerRigidbody != null)
            {
                playerRigidbody.velocity = Vector3.zero;
                playerRigidbody.angularVelocity = Vector3.zero;
            }

            Debug.Log("Player position and state reset.");
        }

        // Вызов сброса в других объектах
        RoadGenerator roadGenerator = FindObjectOfType<RoadGenerator>();
        if (roadGenerator != null)
        {
            roadGenerator.ResetLevel();
        }

        ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
        if (scoreManager != null)
        {
            scoreManager.ResetGame();
        }

        AudioManager audioManager = FindObjectOfType<AudioManager>();
        if (audioManager != null)
        {
            audioManager.RestartGame();
        }
    }

    private void ResetGameObjects()
    {
        // Метод, который отвечает за сброс состояния всех объектов в игре
        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            player.ResetPlayer();
        }

        Obstacle[] obstacles = FindObjectsOfType<Obstacle>();
        foreach (Obstacle obstacle in obstacles)
        {
            obstacle.ResetObstacle();
        }
    }
}
