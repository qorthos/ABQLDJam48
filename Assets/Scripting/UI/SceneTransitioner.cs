using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering.Universal;
using Pixelplacement;
using System;

public class SceneTransitioner : MonoBehaviour
{
    public Light2D GlobalLight;
    public Image GameOverImage;
    public Image BlackImage;
    public AudioSource MusicSource;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void GotoGameScene()
    {
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }

    void GotoTitleScene()
    {
        SceneManager.LoadScene("TitleScene", LoadSceneMode.Single);
    }

    void GotoScoreScene()
    {
        SceneManager.LoadScene("ScoreScene", LoadSceneMode.Single);
    }

    public void GameOver()
    {
        StartCoroutine("DimToTitle");
        GameOverImage.gameObject.SetActive(true);
        Tween.Color(GameOverImage, Color.white, 2f, 0f);

        Tween.Volume(MusicSource, 0, 2f, 0f);
    }

    public void StartGame()
    {
        StartCoroutine("DimToGame");
        Tween.Volume(MusicSource, 0, 2f, 0f);
    }

    public void ReturnToTitleFromVicory()
    {
        StartCoroutine("DimToTitle");
        Tween.Volume(MusicSource, 0, 2f, 0f);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    internal void FinishGame()
    {
        StartCoroutine("DimToVictory");
        Tween.Volume(MusicSource, 0, 2f, 0f);
    }

    IEnumerator DimToGame()
    {
        while (GlobalLight.intensity > 0)
        {
            GlobalLight.intensity -= 0.015f;
            var color = BlackImage.color;
            color.a += 0.05f;
            BlackImage.color = color;
            yield return new WaitForSeconds(0.1f);
        }

        GotoGameScene();
    }

    IEnumerator DimToTitle()
    {
        while (GlobalLight.intensity > 0)
        {
            GlobalLight.intensity -= 0.015f;
            var color = BlackImage.color;
            color.a += 0.05f;
            BlackImage.color = color;
            yield return new WaitForSeconds (0.1f);
        }

        GotoTitleScene();
    }

    IEnumerator DimToVictory()
    {
        while (GlobalLight.intensity > 0)
        {
            GlobalLight.intensity -= 0.015f;
            var color = BlackImage.color;
            color.a += 0.05f;
            BlackImage.color = color;
            yield return new WaitForSeconds(0.1f);
        }

        GotoScoreScene();
    }
}
