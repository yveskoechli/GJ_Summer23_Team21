using System;
using System.Collections.Generic;

using FMODUnity;

using UnityEditor;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public static event Action BaseMenuOpening;
    public static event Action BaseMenuClosed;
    
    #region Inspector

    [SerializeField] private string startScene = "Scenes/Level_001";
    [SerializeField] private string menuScene = "Scenes/MainMenu";

    [Tooltip("Base to be opened/closed with the ToggleMenu Action.\nThis is usually the main or pause menu.")]
    [SerializeField] private Menu baseMenu;

    [Tooltip("Prevent the base menu from being closed. Example -> in the main menu")]
    [SerializeField] private bool preventBaseClosing;

    [Tooltip("Hides the previous open menus when opening a new menu on-top.")]
    [SerializeField] private bool hidePreviousMenu;

    [Tooltip("The Background of the Pause/Options Menu.")]
    [SerializeField] private GameObject optionsBG;

    #endregion

    private GameInput input;

    private Stack<Menu> openedMenus;

    private StudioEventEmitter backSound;

    #region Unity Event Functions

    private void Awake()
    {
        input = new GameInput();

        input.UI.ToggleMenu.performed += ToggleMenu;
        input.UI.GoBackMenu.performed += GoBackMenu;

        openedMenus = new Stack<Menu>();

        backSound = GetComponent<StudioEventEmitter>();
        
        // Reset timescale on scene start.
        Time.timeScale = 1;

        if (preventBaseClosing)
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void Start()
    {
        // Add base menu to stack in case it was open on start. Example the main menu.
        if (baseMenu.gameObject.activeSelf)
        {
            openedMenus.Push(baseMenu);
        }
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    private void OnDestroy()
    {
        input.UI.ToggleMenu.performed -= ToggleMenu;
        input.UI.GoBackMenu.performed -= GoBackMenu;
    }

    #endregion

    #region Menu Functions

    public void StartGame()
    {
        SceneManager.LoadScene(startScene);
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene(menuScene);
    }
    
    public void LoadLevel1()
    {
        SceneManager.LoadScene("Level_001");
    }
    public void LoadLevel2()
    {
        SceneManager.LoadScene("Level_002");
    }

    public void LoadLevel3()
    {
        SceneManager.LoadScene("Level_003");
    }

    

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        
    }

    public void OpenMenu(Menu menu)
    {
        if (menu == baseMenu)
        {
            BaseMenuOpening?.Invoke();
            optionsBG.SetActive(true);
        }

        if (hidePreviousMenu && openedMenus.Count > 0)
        {
            openedMenus.Peek().Hide();
        }

        if (preventBaseClosing && openedMenus.Count == 1)
        {
            optionsBG.SetActive(true);
        }
        
        menu.Open();
        
        // Add menu to the stack.
        openedMenus.Push(menu);

    }

    public void CloseMenu()
    {
        if (openedMenus.Count == 0) { return; }
        
        // Prevent base menu from closing.
        if (preventBaseClosing && openedMenus.Count == 1 && openedMenus.Peek() == baseMenu) { return; }
        
        // Remove top most menu from the stack.
        Menu closingMenu = openedMenus.Pop();
        closingMenu.Close();


        if (hidePreviousMenu && openedMenus.Count > 0)
        {
            openedMenus.Peek().Show();
        }
        
        if (closingMenu == baseMenu)
        {
            BaseMenuClosed?.Invoke();
            optionsBG.SetActive(false);
        }

        if (preventBaseClosing && openedMenus.Count == 1)
        {
            optionsBG.SetActive(false);
        }
        
        //backSound.Play();
    }

    private void ToggleMenu(InputAction.CallbackContext _)
    {
        if (!baseMenu.gameObject.activeSelf && openedMenus.Count <=1)
        {
            OpenMenu(baseMenu);
        }
        else
        {
            GoBackMenu(_);  // _ oder null würde auch gehen, weils egal ist
        }
    }

    private void GoBackMenu(InputAction.CallbackContext _)
    {
        CloseMenu();
    }



    #endregion
}
