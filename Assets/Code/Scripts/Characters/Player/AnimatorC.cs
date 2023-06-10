using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class AnimatorC : MonoBehaviour
{
    [SerializeField] private FirstPersonController _firstPerson;
    [Space]
    [SerializeField] private List<GameObject> _weapons;

    private GameObject _currentWeapon;
    private Animator _currentAnimator;

    private void Start()
    {
        ChangeWeapon(0);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            ChangeWeapon(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            ChangeWeapon(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            ChangeWeapon(2);
        else if (Input.GetKeyDown(KeyCode.Mouse0))
            _currentAnimator.SetTrigger("Shoot");
        else if (Input.GetKeyDown(KeyCode.R))
            _currentAnimator.SetTrigger("Reload");

        _currentAnimator.SetFloat("Speed", _firstPerson.CurrentSpeed);
    }

    private void ChangeWeapon(int index)
    {
        if(_currentWeapon != null)
            _currentWeapon.SetActive(false);
        _currentWeapon = _weapons[index];
        _currentAnimator = _currentWeapon.GetComponent<Animator>();
        _currentWeapon.SetActive(true);
    }
}

public class Inventory : MonoBehaviour
{
    #region Fields

    [SerializeField] private List<Weapon> _weapons;

    #endregion

    #region Public Methods

    public Weapon GetWeapon(Weapons weapons)
    {
        return _weapons[(int)weapons];
    }

    #endregion
}
public enum Weapons
{
    AK = 0,
    Pistol = 1,
    ShootGun = 2
}
public abstract class Weapon : MonoBehaviour
{
    #region Public Methods

    public abstract void Shoot();
    public abstract void Reload();

    #endregion
}
