using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]

public class Cube : MonoBehaviour
{
    private MeshRenderer _renedererColor;
    private Coroutine _coroutine;
    private float _minLifeTime = 2f;
    private float _maxLifeTime = 5f;
    private bool _isTouched = false;

    private void Awake()
    {
        _renedererColor = GetComponent<MeshRenderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isTouched == false)
        {
            if (collision.gameObject.TryGetComponent(out Platform wall))
            {
                _renedererColor.material.color = Random.ColorHSV();
                _coroutine = StartCoroutine(WaitForDestruction());
                _isTouched = true;
            }
        }
    }

    private IEnumerator WaitForDestruction()
    {
        float currentLifeTime = Random.Range(_minLifeTime, _maxLifeTime);
        yield return new WaitForSeconds(currentLifeTime);
        Destroy();
    }

    private void Destroy()
    {
        if(_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }

        Destroy(gameObject);
    }
}
