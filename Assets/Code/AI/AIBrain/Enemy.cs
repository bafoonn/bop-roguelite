using UnityEngine;

public class Enemy : MonoBehaviour
{
    private AgentMover agentMover;

    private Vector2 pointerInput, movementInput;

    public Vector2 PointerInput { get => pointerInput; set => pointerInput = value; }
    public Vector2 MovementInput { get => movementInput; set => movementInput = value; }

    private void Update()
    {
        agentMover.MovementInput = MovementInput;

        AnimateCharacter();
    }

    public void PerformAttack()
    {
        //TODO: DIFFRENT ATTACK THINGS
    }

    private void Awake()
    {
        agentMover = GetComponent<AgentMover>();
    }

    private void AnimateCharacter()
    {
        Vector2 lookDirection = PointerInput - (Vector2)transform.position;
    }
}