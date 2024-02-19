using Pasta;
using System.Collections;
using UnityEngine;

public class WeaponParent : MonoBehaviour
{
    //public SpriteRenderer characterRenderer, weaponRenderer;
    public Vector2 EnemyWeaponPos { get; set; } // Replace this!

    private GameObject AttackIndicatorImage;
    [SerializeField] private Projectile projectile;
    [SerializeField] public GameObject ProjectileSpawnPoint;
    public Vector2 direction;
    private AIData aidata;
    private Vector3 enemyDirectionLocal;
    private AgentAnimations animations;
    //public Transform circleOrigin;
    public float radius;
    [SerializeField] private GameObject detectors;
    private SpriteRenderer spriteRend;
    [SerializeField] private GameObject attackColliderHolder;
    [SerializeField] private Collider2D attackCollider;
    public bool Aim = true;
    [SerializeField] private Transform spriteTransform;
    private Vector2 weaponScale;
    private Projectile projectileScript;
    private EnemyAi enemyAI;
    public bool Scoot = false;
    private Transform parent;
    private RangedAI rangedAI;

    //private CleavingWeaponAnimations weaponAnimations;
    private void Start()
    {
        parent = transform.parent;
        //AttackIndicatorImage = gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject;
        aidata = GetComponentInParent<AIData>();
        spriteRend = parent.GetComponentInChildren<SpriteRenderer>();
        animations = GetComponentInParent<AgentAnimations>();
        weaponScale = transform.localScale;
        enemyAI = GetComponentInParent<EnemyAi>();
        //weaponAnimations = GetComponentInParent<CleavingWeaponAnimations>();
    }

    private void Update()
    {


        if (Aim)
        {
            Vector3 weaponPos = EnemyWeaponPos; // THIS WHOLE THING IS A SHITSHOW BUT IT WORKS!
            direction = new Vector2(weaponPos.x - transform.position.x, weaponPos.y - transform.position.y);

            //weaponAnimations.SetAim(direction);

            if (aidata.currentTarget != null)
            {
                enemyDirectionLocal = transform.InverseTransformPoint(aidata.currentTarget.transform.position); // Gets players position from aidata script.
            }

            Vector2 scale = attackColliderHolder.transform.localScale;
            transform.right = direction;
            if (enemyDirectionLocal.x < 0)
            {

                //Debug.Log("LEFT");
                scale.x = -1f;
                weaponScale.x = -1f;
                weaponScale.y = 0f;
                spriteRend.flipY = true;
                if (rangedAI != null)
                {
                    if (spriteTransform != null) // TEMP FOR ERROR MANAGEMENT
                    {
                        spriteTransform.localPosition = weaponScale; // Changes weapon sprites "side"
                    }
                }

                attackColliderHolder.transform.localScale = scale; // Changing child since editing parents scale fucks direction check
            }
            else if (enemyDirectionLocal.x > 0)
            {

                //Debug.Log("RIGHT");
                scale.x = 1f;
                weaponScale.x = 1f;
                weaponScale.y = 0f;
                spriteRend.flipY = false;
                if (rangedAI != null)
                {
                    if (spriteTransform != null) // TEMP FOR ERROR MANAGEMENT
                    {
                        spriteTransform.localPosition = weaponScale; // Changes weapon sprites "side"
                    }
                }
                attackColliderHolder.transform.localScale = scale; // Changing child since editing parents scale fucks direction check
            }
        }


    }


    public void Attack()
    {
        if (attackCollider != null) attackCollider.enabled = true;
        StartCoroutine(StopAttack());
    }
    private IEnumerator StopAttack() // TEST STUFF
    {
        yield return new WaitForSeconds(0.1f);

        if (attackCollider != null) attackCollider.enabled = false;

        Aim = true;
        animations.aim = true;
    }
    public void RangedAttack()
    {
        var Projectile = Instantiate(projectile, ProjectileSpawnPoint.transform.position, Quaternion.identity);
        Vector2 direction = Player.Current.transform.position - transform.position;
        direction.Normalize();
        Projectile.Launch(direction);
        if (rangedAI != null)
        {
            Projectile.damage = rangedAI.damage; // Get projectiles damage from enemyscript so no need to change damage on multiple places.
        }
        else
        {
            Projectile.damage = 15f;
        }

        StartCoroutine(StopAttack());
        Scoot = true;
    }
    public void ImmunityBeam()
    {
        LineRenderer lineRenderer;
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer = GetComponent<LineRenderer>();
        Vector3[] positions = new Vector3[2];
        positions[0] = gameObject.transform.position;
        positions[1] = enemyAI.supportEnemyTarger.position;
        lineRenderer.SetPositions(positions);
    }



}
