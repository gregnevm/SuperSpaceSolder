using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Gun : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _shotVisualEffect;
    [SerializeField] private GameObject _hitEffect;
    [Header("Settings")]
    [SerializeField] private float _hitEffectDuration = 2.0f;
    [SerializeField] private float _shotEffectDuration = 0.1f;

    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();       
    }

    public void FireGun()
    {
        Ray ray = new Ray(transform.position, transform.up);        

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            IDamageable damageableObject = hit.collider.GetComponent<IDamageable>();
            if (damageableObject != null)
            { 
                damageableObject.TakeDamage();
            }

            if (_hitEffect != null)
            {
               StartCoroutine(InstantiateAndDestroyWithDelay(_hitEffect, hit.point, Quaternion.LookRotation(hit.normal), _hitEffectDuration));
            }

            if (_shotVisualEffect != null)
            {
               StartCoroutine(ActivateAndDeactivateShotEffect());
            }
        }

        _audioSource.Play();
    }

    private IEnumerator InstantiateAndDestroyWithDelay(GameObject effect, Vector3 position, Quaternion rotation, float duration)
    {
        GameObject instantiatedEffect = Instantiate(effect, position, rotation);
        yield return new WaitForSeconds(duration);        
        Destroy(instantiatedEffect);
    }

    private IEnumerator ActivateAndDeactivateShotEffect()
    {
        _shotVisualEffect.SetActive(true);
        yield return new WaitForSeconds(_shotEffectDuration);
        _shotVisualEffect.SetActive(false);
    }
}
