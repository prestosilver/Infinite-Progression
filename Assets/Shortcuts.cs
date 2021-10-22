// GENERATED AUTOMATICALLY FROM 'Assets/Shortcuts.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Shortcuts : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @Shortcuts()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Shortcuts"",
    ""maps"": [
        {
            ""name"": ""Screen"",
            ""id"": ""adda0f4a-1dce-4777-bd57-395cf83d1892"",
            ""actions"": [
                {
                    ""name"": ""ToggleFullscreen"",
                    ""type"": ""Button"",
                    ""id"": ""c8d895f7-5b2f-4f97-ba0a-e13f4e3dbbea"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Navigate"",
                    ""type"": ""Value"",
                    ""id"": ""bc43ce0a-bbd7-42e8-bcd6-1e010f2a8912"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Submit"",
                    ""type"": ""Button"",
                    ""id"": ""56ded574-315e-4a5f-9e49-183af5b5b014"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Point"",
                    ""type"": ""PassThrough"",
                    ""id"": ""8dc1aab7-644f-4101-bafe-d2f937337f39"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Click"",
                    ""type"": ""Button"",
                    ""id"": ""6f29d679-4369-40bf-a9d4-2420a7f9fa9f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ScrollWheel"",
                    ""type"": ""PassThrough"",
                    ""id"": ""3b4a3c22-e057-4450-b139-d6ddbaeb9d7f"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""25fa1c9f-460d-4143-aa43-eb7703ea19c9"",
                    ""path"": ""<Keyboard>/f11"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Debug"",
                    ""action"": ""ToggleFullscreen"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""c3aea061-f26c-456a-b33e-1edb9700ccce"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""8d42c670-f3e3-404f-8a8b-63bf085ae902"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Debug"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""060f4639-b538-4c18-a171-1b0238275aa0"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Debug"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""d96e4fbb-0c3e-4037-965e-02fd8d5595f0"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Debug"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""7aee497a-65c2-49db-ab40-6a79ea9e4f4b"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Debug"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""3d731cc6-bea3-4d85-96e9-ba49171e2ce5"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Debug"",
                    ""action"": ""Submit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""868a662c-9597-429f-b7fc-6c0c899ad5c7"",
                    ""path"": ""<Keyboard>/numpadEnter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Debug"",
                    ""action"": ""Submit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ef406e5c-4c24-46c1-ab07-a3868de28bdd"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Debug"",
                    ""action"": ""Point"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""80366dcb-f4ab-4a41-b3b1-7e3655667de1"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Debug"",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ca5de756-d374-4032-a9b1-483d084d227b"",
                    ""path"": ""<Mouse>/scroll"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Debug"",
                    ""action"": ""ScrollWheel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Debug"",
            ""id"": ""6dff2701-fa4f-4ee1-bb46-3c600586dee8"",
            ""actions"": [
                {
                    ""name"": ""ToggleDebug"",
                    ""type"": ""Button"",
                    ""id"": ""24a9591e-6b95-4d5a-aae1-9c153034748a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Return"",
                    ""type"": ""Button"",
                    ""id"": ""22de8b85-28e2-4f92-a0ad-86a48f32c22f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""9ff6af01-4d4a-4288-8c32-ba2a06df52df"",
                    ""path"": ""<Keyboard>/backquote"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Debug"",
                    ""action"": ""ToggleDebug"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d514f5d3-6dfc-4140-9b19-b35e937cdc91"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Debug"",
                    ""action"": ""Return"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""15b0afff-a8eb-4206-8a12-99c98f7316db"",
                    ""path"": ""<Keyboard>/numpadEnter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Debug"",
                    ""action"": ""Return"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Debug"",
            ""bindingGroup"": ""Debug"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Screen
        m_Screen = asset.FindActionMap("Screen", throwIfNotFound: true);
        m_Screen_ToggleFullscreen = m_Screen.FindAction("ToggleFullscreen", throwIfNotFound: true);
        m_Screen_Navigate = m_Screen.FindAction("Navigate", throwIfNotFound: true);
        m_Screen_Submit = m_Screen.FindAction("Submit", throwIfNotFound: true);
        m_Screen_Point = m_Screen.FindAction("Point", throwIfNotFound: true);
        m_Screen_Click = m_Screen.FindAction("Click", throwIfNotFound: true);
        m_Screen_ScrollWheel = m_Screen.FindAction("ScrollWheel", throwIfNotFound: true);
        // Debug
        m_Debug = asset.FindActionMap("Debug", throwIfNotFound: true);
        m_Debug_ToggleDebug = m_Debug.FindAction("ToggleDebug", throwIfNotFound: true);
        m_Debug_Return = m_Debug.FindAction("Return", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Screen
    private readonly InputActionMap m_Screen;
    private IScreenActions m_ScreenActionsCallbackInterface;
    private readonly InputAction m_Screen_ToggleFullscreen;
    private readonly InputAction m_Screen_Navigate;
    private readonly InputAction m_Screen_Submit;
    private readonly InputAction m_Screen_Point;
    private readonly InputAction m_Screen_Click;
    private readonly InputAction m_Screen_ScrollWheel;
    public struct ScreenActions
    {
        private @Shortcuts m_Wrapper;
        public ScreenActions(@Shortcuts wrapper) { m_Wrapper = wrapper; }
        public InputAction @ToggleFullscreen => m_Wrapper.m_Screen_ToggleFullscreen;
        public InputAction @Navigate => m_Wrapper.m_Screen_Navigate;
        public InputAction @Submit => m_Wrapper.m_Screen_Submit;
        public InputAction @Point => m_Wrapper.m_Screen_Point;
        public InputAction @Click => m_Wrapper.m_Screen_Click;
        public InputAction @ScrollWheel => m_Wrapper.m_Screen_ScrollWheel;
        public InputActionMap Get() { return m_Wrapper.m_Screen; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ScreenActions set) { return set.Get(); }
        public void SetCallbacks(IScreenActions instance)
        {
            if (m_Wrapper.m_ScreenActionsCallbackInterface != null)
            {
                @ToggleFullscreen.started -= m_Wrapper.m_ScreenActionsCallbackInterface.OnToggleFullscreen;
                @ToggleFullscreen.performed -= m_Wrapper.m_ScreenActionsCallbackInterface.OnToggleFullscreen;
                @ToggleFullscreen.canceled -= m_Wrapper.m_ScreenActionsCallbackInterface.OnToggleFullscreen;
                @Navigate.started -= m_Wrapper.m_ScreenActionsCallbackInterface.OnNavigate;
                @Navigate.performed -= m_Wrapper.m_ScreenActionsCallbackInterface.OnNavigate;
                @Navigate.canceled -= m_Wrapper.m_ScreenActionsCallbackInterface.OnNavigate;
                @Submit.started -= m_Wrapper.m_ScreenActionsCallbackInterface.OnSubmit;
                @Submit.performed -= m_Wrapper.m_ScreenActionsCallbackInterface.OnSubmit;
                @Submit.canceled -= m_Wrapper.m_ScreenActionsCallbackInterface.OnSubmit;
                @Point.started -= m_Wrapper.m_ScreenActionsCallbackInterface.OnPoint;
                @Point.performed -= m_Wrapper.m_ScreenActionsCallbackInterface.OnPoint;
                @Point.canceled -= m_Wrapper.m_ScreenActionsCallbackInterface.OnPoint;
                @Click.started -= m_Wrapper.m_ScreenActionsCallbackInterface.OnClick;
                @Click.performed -= m_Wrapper.m_ScreenActionsCallbackInterface.OnClick;
                @Click.canceled -= m_Wrapper.m_ScreenActionsCallbackInterface.OnClick;
                @ScrollWheel.started -= m_Wrapper.m_ScreenActionsCallbackInterface.OnScrollWheel;
                @ScrollWheel.performed -= m_Wrapper.m_ScreenActionsCallbackInterface.OnScrollWheel;
                @ScrollWheel.canceled -= m_Wrapper.m_ScreenActionsCallbackInterface.OnScrollWheel;
            }
            m_Wrapper.m_ScreenActionsCallbackInterface = instance;
            if (instance != null)
            {
                @ToggleFullscreen.started += instance.OnToggleFullscreen;
                @ToggleFullscreen.performed += instance.OnToggleFullscreen;
                @ToggleFullscreen.canceled += instance.OnToggleFullscreen;
                @Navigate.started += instance.OnNavigate;
                @Navigate.performed += instance.OnNavigate;
                @Navigate.canceled += instance.OnNavigate;
                @Submit.started += instance.OnSubmit;
                @Submit.performed += instance.OnSubmit;
                @Submit.canceled += instance.OnSubmit;
                @Point.started += instance.OnPoint;
                @Point.performed += instance.OnPoint;
                @Point.canceled += instance.OnPoint;
                @Click.started += instance.OnClick;
                @Click.performed += instance.OnClick;
                @Click.canceled += instance.OnClick;
                @ScrollWheel.started += instance.OnScrollWheel;
                @ScrollWheel.performed += instance.OnScrollWheel;
                @ScrollWheel.canceled += instance.OnScrollWheel;
            }
        }
    }
    public ScreenActions @Screen => new ScreenActions(this);

    // Debug
    private readonly InputActionMap m_Debug;
    private IDebugActions m_DebugActionsCallbackInterface;
    private readonly InputAction m_Debug_ToggleDebug;
    private readonly InputAction m_Debug_Return;
    public struct DebugActions
    {
        private @Shortcuts m_Wrapper;
        public DebugActions(@Shortcuts wrapper) { m_Wrapper = wrapper; }
        public InputAction @ToggleDebug => m_Wrapper.m_Debug_ToggleDebug;
        public InputAction @Return => m_Wrapper.m_Debug_Return;
        public InputActionMap Get() { return m_Wrapper.m_Debug; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(DebugActions set) { return set.Get(); }
        public void SetCallbacks(IDebugActions instance)
        {
            if (m_Wrapper.m_DebugActionsCallbackInterface != null)
            {
                @ToggleDebug.started -= m_Wrapper.m_DebugActionsCallbackInterface.OnToggleDebug;
                @ToggleDebug.performed -= m_Wrapper.m_DebugActionsCallbackInterface.OnToggleDebug;
                @ToggleDebug.canceled -= m_Wrapper.m_DebugActionsCallbackInterface.OnToggleDebug;
                @Return.started -= m_Wrapper.m_DebugActionsCallbackInterface.OnReturn;
                @Return.performed -= m_Wrapper.m_DebugActionsCallbackInterface.OnReturn;
                @Return.canceled -= m_Wrapper.m_DebugActionsCallbackInterface.OnReturn;
            }
            m_Wrapper.m_DebugActionsCallbackInterface = instance;
            if (instance != null)
            {
                @ToggleDebug.started += instance.OnToggleDebug;
                @ToggleDebug.performed += instance.OnToggleDebug;
                @ToggleDebug.canceled += instance.OnToggleDebug;
                @Return.started += instance.OnReturn;
                @Return.performed += instance.OnReturn;
                @Return.canceled += instance.OnReturn;
            }
        }
    }
    public DebugActions @Debug => new DebugActions(this);
    private int m_DebugSchemeIndex = -1;
    public InputControlScheme DebugScheme
    {
        get
        {
            if (m_DebugSchemeIndex == -1) m_DebugSchemeIndex = asset.FindControlSchemeIndex("Debug");
            return asset.controlSchemes[m_DebugSchemeIndex];
        }
    }
    public interface IScreenActions
    {
        void OnToggleFullscreen(InputAction.CallbackContext context);
        void OnNavigate(InputAction.CallbackContext context);
        void OnSubmit(InputAction.CallbackContext context);
        void OnPoint(InputAction.CallbackContext context);
        void OnClick(InputAction.CallbackContext context);
        void OnScrollWheel(InputAction.CallbackContext context);
    }
    public interface IDebugActions
    {
        void OnToggleDebug(InputAction.CallbackContext context);
        void OnReturn(InputAction.CallbackContext context);
    }
}
