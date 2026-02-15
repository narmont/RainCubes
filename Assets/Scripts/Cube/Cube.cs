using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Cube : MonoBehaviour
{
    private CubeColor _cubeColor;
    private Coroutine _lifeTimer;
    private bool _hasCollided = false;
    
    public event Action<Cube> LifeEnded;

    private void Awake()
    {
        _cubeColor = GetComponent<CubeColor>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_hasCollided) 
            return;

        if (collision.gameObject.TryGetComponent<Platform>(out _))
        {
            _hasCollided = true;

            if (_cubeColor != null)
            {
                _cubeColor.ChangeColor();
            }

            StartLifeTimer();
        }
    }

    public void ResetState()
    {
        _hasCollided = false;

        if (_lifeTimer != null)
        {
            StopCoroutine(_lifeTimer);
            _lifeTimer = null;
        }

        _cubeColor.ResetColor();
    }

    private void StartLifeTimer()
    {
        if (_lifeTimer != null)
        {
            StopCoroutine(_lifeTimer);
        }

        _lifeTimer = StartCoroutine(RunLifeTimer());
    }

    private IEnumerator RunLifeTimer()
    {
        float minRandomValue = 2f;
        float maxRandomValue = 5f;       
        float waitTime = Random.Range(minRandomValue, maxRandomValue);
        yield return new WaitForSeconds(waitTime);
        EndLife();
    }

    private void EndLife()
    {
        LifeEnded?.Invoke(this);
    }
}