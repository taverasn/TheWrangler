using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private Logger logger;

    public State state { get; protected set; } = null;
    protected List<State> states { get; set; } = new List<State>();
    [field:SerializeField] public Transform target { get; protected set; }
    private FollowerEntity follower;

    public Interactable interactable;
    public bool canInteract = true;
    public float interactCooldown = 30f;

    #region Inventory
    [SerializeField] private int inventorySize = 30;
    public Inventory inventory { get; protected set; }
    public CraftingTable craftingTable { get; protected set; }
    public List<RecipeSO> recipes = new List<RecipeSO>();
    public List<RecipeSO> craftableRecipes => interactable != null ? craftingTable.EvaluateCraftableRecipes(interactable.inventory) : new List<RecipeSO>();
    #endregion
    #region Events

    public event System.Action<System.Type> onTransitionTo;
    public void TransitionTo(System.Type state) => onTransitionTo?.Invoke(state);
    public event System.Action onDestinationReached;
    public void DestinationReached() => onDestinationReached?.Invoke();
    #endregion

    private Queue<State> transitionQueue = new Queue<State>();
    private bool transitionLock = false;

    protected virtual void Awake()
    {
        logger = LogManager.Instance.AddLogger("State Machine", LogLevel.INFO);

        inventory = new Inventory(inventorySize);
        craftingTable = new CraftingTable();
        craftingTable.AddRecipes(recipes);

        follower = GetComponent<FollowerEntity>();

        states.Add(new Idle(this));
        states.Add(new Wander(30f, this));
        states.Add(new Deciding(this));
        states.Add(new ItemCraft(this));
        states.Add(new ItemMove(this));

        onTransitionTo += HandleTransitionTo;
    }

    private void Start()
    {
        state = states.OfType<Idle>().FirstOrDefault();
        state.Arrive();
    }

    protected virtual void Update()
    {
        if (follower.reachedDestination)
        {
            DestinationReached();
        }


        state.Update(Time.deltaTime);
    }

    private void HandleTransitionTo(System.Type _state)
    {
        State newState = states.FirstOrDefault(s => s.GetType().ToString() == _state.Name);

        transitionQueue.Enqueue(newState);
        DoTransition();
    }

    private void DoTransition()
    {
        if (!transitionLock)
        {
            transitionLock = true;
            State prevState = state;

            state.Exit();

            state = transitionQueue.Dequeue();
            logger.Info($"Transitioning from {prevState.GetType().ToString()} to {state.GetType().ToString()}");

            state.Arrive();

            transitionLock = false;

            if (transitionQueue.Any())
            {
                DoTransition();
            }
        }

    }

    public IEnumerator StartCanInteractCoolDown()
    {
        canInteract = false;

        yield return new WaitForSeconds(interactCooldown);

        canInteract = true;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 30f);
    }
}
