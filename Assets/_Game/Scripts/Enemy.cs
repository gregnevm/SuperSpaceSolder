using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    private float _healthPoints = 5;
    public void TakeDamage()
    {
        if (_healthPoints > 1)
        {
            Debug.Log("Damage taken");
            _healthPoints--;
            return;
        }
        if (_healthPoints == 1)
        {
            _healthPoints--;
            Dead();
            return;
        }
        else Dead();       
    }
    private void Dead()
    {
        Debug.Log("Is dead");
        //TODO: add state machine and dead animation.
    }
}
