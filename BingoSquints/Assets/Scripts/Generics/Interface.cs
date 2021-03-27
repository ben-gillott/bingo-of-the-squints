using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void ApplyDamage(int Damage, GameObject _Instagator);
    void ApplyHealing(int Healing, GameObject _Instagator);

}

public interface IInteract
{
    void InteractTrigger(GameObject _Instagator);
    void InteractFunction(GameObject _Instagator);


}