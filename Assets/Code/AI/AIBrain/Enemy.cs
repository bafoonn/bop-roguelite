using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour
{
    private AgentMover agentMover;
    private AgentAnimations agentAnimations;
    private Vector2 pointerInput, movementInput;
    //private GameObject Weapon;
    private WeaponParent weaponParent;
    public Vector2 PointerInput { get => pointerInput; set => pointerInput = value; }
    public Vector2 MovementInput { get => movementInput; set => movementInput = value; }


    //[SerializeField] private GameObject AttackIndicatorArea;

    private void Update()
    {
        agentMover.MovementInput = MovementInput;

        AnimateCharacter();
        weaponParent.PointerPosition = pointerInput;

    }

    public void PerformAttack()
    {
        //TODO: DIFFRENT ATTACK THINGS
    }

    private void Awake()
    {
        agentAnimations = GetComponentInChildren<AgentAnimations>();
        agentMover = GetComponent<AgentMover>();
        weaponParent = GetComponentInChildren<WeaponParent>();
        //Weapon = gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject;
    }

    private void AnimateCharacter()
    {
        Vector2 lookDirection = PointerInput - (Vector2)transform.position;
        agentAnimations.RotateToPointer(lookDirection);
    }
}