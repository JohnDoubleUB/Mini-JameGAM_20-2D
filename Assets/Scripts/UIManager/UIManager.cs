using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UIManagerLibrary.Scripts
{
    public class UIManager : MonoBehaviour
    {
        [HideInInspector]
        public int _activeFlags; //Used to track flags with DataContext

        public static UIManager current;

        [SerializeField]
        private bool AssignAsCurrentSingleton = true; //Incase we just want to use a UIManager for a specific thing

        //Use one or the other!
        [Header("Data Contexts - Use one or the other, not both")]
        public UIContextDataObject UIDataContextObject; //This is used mainly for defining enum for initially active contexts as well
        public UIContextData UIDataContext; //This is for defining it entirely within the inspector without a separate context
        private bool ContextIsCaseSensitive => UIDataContextObject != null ? UIDataContextObject.CaseSensitiveContexts : UIDataContext.CaseSensitiveContexts;
        [Header("UIContext Objects - Drag and drop ui elements with UIContexts")]
        public List<SerializedPair<string, UIContextObject>> UIContexts = new List<SerializedPair<string, UIContextObject>>();

        private Dictionary<string, List<UIContextObject>> _UIContexts = new Dictionary<string, List<UIContextObject>>();

        #region Unity Lifecycle

        private void Awake()
        {
            if (AssignAsCurrentSingleton == false) return;
            if (current != null) Debug.LogWarning("Oops! it looks like there might already be a " + GetType().Name + " in this scene!");
            current = this;
        }

        private void Start()
        {
            _InitializeContexts();
        }

        private void OnEnable()
        {
            _UpdateUIContexts();
        }

        //Editor based
        private void OnValidate()
        {
            _UpdateUIContexts();
        }

        private void Reset()
        {
            _ClearUIContexts();
        }

        //Helper methods
        private void _InitializeContexts()
        {
            foreach (var (kVP, i) in _UIContexts.Select((value, i) => (value, i)))
            {
                int bitMask = 1 << i;
                bool contextEnabled = (_activeFlags & bitMask) != 0;
                foreach (UIContextObject contextObject in kVP.Value) contextObject.SetDisplayAndActive(contextEnabled, true);
            }
        }

        private void _ClearUIContexts()
        {
            UIContexts.Clear();
            _UIContexts.Clear();
        }

        private void _UpdateUIContexts()
        {
            _UIContexts.Clear();

            foreach (SerializedPair<string, UIContextObject> sKVP in UIContexts)
            {
                if (sKVP.Value == null) continue;

                string contextKey = ContextIsCaseSensitive ? sKVP.Key : sKVP.Key.ToLower();

                if (_UIContexts.ContainsKey(contextKey)) //If key already exists then add the value into the existing dictionary list
                {
                    _UIContexts[contextKey].Add(sKVP.Value);
                }
                else //If not then create a whole new object in the dictionary and then assign the current canvas group into that context
                {
                    _UIContexts.Add(contextKey, new List<UIContextObject> { sKVP.Value });
                }
            }
        }

        #endregion

        public bool TryGetUIContextData(out IUIContextData contextData)
        {
            contextData = null;

            if (UIDataContextObject != null) //Always prioritize using the ScriptableObject
            {
                contextData = UIDataContextObject;
                return true;
            }
            else if (UIDataContext != null) 
            {
                contextData = UIDataContext;
                return true;
            }

            return false;
        }

        public void SetActiveContexts(bool active, params string[] contexts)
        {
            foreach (string context in contexts)
            {
                SetAllContextsOfType(context, active);
            }
        }

        public void SetActiveContexts(bool active, bool immediate, params string[] contexts)
        {
            foreach (string context in contexts)
            {
                SetAllContextsOfType(context, active, immediate);
            }
        }

        public void ToggleActiveContexts(params string[] contexts)
        {
            foreach (string context in contexts)
            {
                ToggleContext(context);
            }
        }

        //These are incase we want to directly refer to the UIManager in buttons and call this in onclick event for the UI
        public void ToggleContext(string context)
        {
            ToggleAllContextsOfType(context);
        }

        public void EnableContext(string context)
        {
            SetAllContextsOfType(context, true);
        }

        public void DisableContext(string context)
        {
            SetAllContextsOfType(context, false);
        }

        //Helper methods
        private void SetAllContextsOfType(string context, bool active, bool immediate = false)
        {
            if (ContextIsCaseSensitive == false)
            {
                context = context.ToLower();
            }

            if (_UIContexts.ContainsKey(context) == false)
            {
                Debug.LogWarning($"Setting Context For: \"{context}\" could not be performed. Have you properly configured this context?");
                return;
            }

            foreach (UIContextObject contextObject in _UIContexts[context]) 
            { 
                contextObject.SetDisplayAndActive(active, immediate); 
            }
        }

        private void ToggleAllContextsOfType(string context)
        {
            if (ContextIsCaseSensitive == false) 
            {
                context = context.ToLower();
            }

            if (_UIContexts.ContainsKey(context) == false)
            {
                Debug.LogWarning($"Toggle Context For: \"{context}\" could not be performed. Have you properly configured this context?");
                return;
            }

            foreach (UIContextObject contextObject in _UIContexts[context]) 
            { 
                contextObject.SetDisplayAndActive(!contextObject.Display); 
            }
        }
    }
}