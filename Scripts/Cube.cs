using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Cube : MonoBehaviour
{
    private Renderer _renderer;
    private int _lifeTime;
    private Color _standartColor;
    private bool _isReleased = false;

    public event Action<Cube> Felled;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();

        _standartColor = _renderer.material.color;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Platform>(out Platform platform) && TryRelease())
        {
            SetRandomColor();
            StartCoroutine(DieAfterDelay());
        }
    }

    private void OnDisable()
    {
        _isReleased = false;
    }

    private bool TryRelease()
    {
        if (_isReleased) 
        {
            return false;
        }

        else
        {
            _isReleased = true;
            return true;
        }
    }

    private void SetRandomColor()
    {
        _renderer.material.color = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
    }

    public void SetStandartColor()
    {
        _renderer.material.color = _standartColor;
    }

    private int CalculateLifeTime()
    {
        int minLifeTime = 2;
        int maxLifeTime = 5;

        _lifeTime = UnityEngine.Random.Range(minLifeTime, maxLifeTime + 1);
        return _lifeTime;
    }

    private IEnumerator DieAfterDelay()
    {
        yield return new WaitForSeconds(CalculateLifeTime());
        Felled?.Invoke(this);
    }
}
