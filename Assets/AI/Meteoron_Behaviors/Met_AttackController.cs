using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Met_AttackController : MonoBehaviour
{

    public bool IsRanged;
    public float projectileSpeed;

    public GameObject[] ProjectilePrefab;
    public Transform FireLocation;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("mIsRanged", IsRanged);
    }

    public void Attack(Transform target)
    {
        StartCoroutine(AttackOrShootProjectile(target));
    }

    IEnumerator AttackOrShootProjectile(Transform target)
    {
        var projIndex = animator.GetInteger("mNextAbility");
        yield return new WaitForSeconds(0.2f);

        var direction = target.transform.position - FireLocation.position;
        direction.y = target.GetComponent<BoxCollider>().size.y / 2;

        GameObject projectile = Instantiate(ProjectilePrefab[projIndex], FireLocation.position, Quaternion.LookRotation(-direction, Vector3.up));
        projectile.transform.position = FireLocation.transform.position;
        projectile.GetComponent<Rigidbody>().freezeRotation = true;
        projectile.GetComponent<Rigidbody>().AddForce(-projectile.transform.forward * projectileSpeed, ForceMode.Impulse);


    }


}
