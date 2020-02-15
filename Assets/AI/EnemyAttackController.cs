using System.Collections;
using UnityEngine;

public class EnemyAttackController : MonoBehaviour
{
    public CharacterType characterType;
    public bool IsRanged;
    public float projectileSpeed;

    public GameObject[] ProjectilePrefab;
    public Transform FireLocation;

    public GameObject placeHolder;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("mIsRanged", IsRanged);
    }

    public void Attack(Transform target)
    {
        if (!animator.GetBool("mIsRanged"))
            return;
        switch (characterType) {
            case CharacterType.Caveman:
                StartCoroutine(AttackOrShootProjectile(target));
                break;
            case CharacterType.Meteoron:
                StartCoroutine(AttackOrShootProjectileBoss(target));
                break;
            case CharacterType.Alien:
                StartCoroutine(ShootLasers(target));
                break;
        }
    }

    IEnumerator AttackOrShootProjectileBoss(Transform target)
    {

            var projIndex = animator.GetInteger("mNextAbility");


            var targetSize = target.GetComponent<BoxCollider>().size;

            var position = target.position;
            var direction = new Vector3(targetSize.x/2, 0.0f, targetSize.z/2);

            switch (projIndex)
            {
                case 0:
                    position.y += 0.5f;
                    direction.y = 1.0f;
                    break;
                case 1:
                    position.y += 10f;
                    direction.y = -1.0f;
                    break;
                case 2:
                    //the rock he's holdin ?
                    position = FireLocation.position;
                    direction = FireLocation.position - target.position;
                    ProjectilePrefab[projIndex].SetActive(true);
                    break;
            }
            var pos = target.position;
            pos.y = 0.1f;
            var go = Instantiate(placeHolder, pos, placeHolder.transform.rotation);
            yield return new WaitForSeconds(1.0f);
            GameObject projectile = Instantiate(ProjectilePrefab[projIndex],  position, Quaternion.LookRotation(direction, Vector3.up));

            projectile.GetComponent<Rigidbody>().freezeRotation = true;
            if (projIndex == 1)
                projectile.GetComponent<Rigidbody>().AddForce(projectile.transform.forward * projectileSpeed, ForceMode.Impulse);
            Destroy(go);
        
    }

    IEnumerator AttackOrShootProjectile(Transform target)
    {

            yield return new WaitForSeconds(0.2f);

            var direction = target.transform.position - FireLocation.position;
            direction.y = target.GetComponent<BoxCollider>().size.y / 2;

            GameObject projectile = Instantiate(ProjectilePrefab[0], FireLocation.position, Quaternion.LookRotation(-direction, Vector3.up));
            projectile.transform.position = FireLocation.transform.position;
            projectile.GetComponent<Rigidbody>().freezeRotation = true;
            projectile.GetComponent<Rigidbody>().AddForce(-projectile.transform.forward * projectileSpeed, ForceMode.Impulse);
        

    }

    IEnumerator ShootLasers(Transform target)
    {
        yield return new WaitForSeconds(0.2f);

        var direction = target.transform.position - FireLocation.position;
        direction.y = target.GetComponent<BoxCollider>().size.y / 2;

        GameObject projectile = Instantiate(ProjectilePrefab[0], FireLocation.position, Quaternion.LookRotation(-direction));
        projectile.transform.position = FireLocation.transform.position;
        projectile.GetComponent<Rigidbody>().freezeRotation = true;
        projectile.GetComponent<Rigidbody>().AddForce(-projectile.transform.forward * projectileSpeed, ForceMode.Impulse);

        yield return new WaitForSeconds(0.2f);

        projectile = Instantiate(ProjectilePrefab[0], FireLocation.position, Quaternion.LookRotation(-direction, Vector3.up));
        projectile.transform.position = FireLocation.transform.position;
        projectile.GetComponent<Rigidbody>().freezeRotation = true;
        projectile.GetComponent<Rigidbody>().AddForce(-projectile.transform.forward * projectileSpeed, ForceMode.Impulse); 
    }

}


public enum CharacterType{ Caveman, Alien, Meteoron, Dino };