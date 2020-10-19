using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] GameObject controlledActor;

    public static PlayerInput instance;

    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode escapeKey = KeyCode.Escape;

    public string horizontalInputKey = "Horizontal";
    public string verticalInputKey = "Vertical";

    public PlayerMovement PlayerMovement { get; private set; }

    public GameObject ControlledActor
    {
        get { return controlledActor; }
        set
        {
            controlledActor = value;
            PlayerMovement = controlledActor.GetComponent<PlayerMovement>();
        }
    }

    private void Awake()
    {
        if (instance)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    private void Update()
    {
        if (PlayerMovement != null)
        {           
            PlayerMovement.Move(Input.GetAxis(verticalInputKey));

            if (!PlayerMovement.IsAbovePlatform && !PlayerMovement.IsLanded)
            {
                PlayerMovement.Rotate(Input.GetAxis(horizontalInputKey));
            }            
        }

        if (Input.GetKeyDown(sprintKey))
        {
            PlayerMovement.Boost();
        }
        else if (Input.GetKeyUp(sprintKey))
        {
            PlayerMovement.StopBoost();
        }

        if (Input.GetKeyDown(escapeKey))
        {
            // Return to main menu.
            SceneManager.LoadScene(0);
        }

        PlayerMovement.spaceInput = Input.GetButtonDown("Jump");
    }
}
