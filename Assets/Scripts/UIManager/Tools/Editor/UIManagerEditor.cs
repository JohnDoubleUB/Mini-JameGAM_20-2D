using UnityEditor;

namespace UIManagerLibrary.Scripts
{
    [CustomEditor(typeof(UIManager))]
    public class UIManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var uiManager = target as UIManager;
            bool componentIsDirty = false;

            if (uiManager.TryGetUIContextData(out IUIContextData contextData))
            {
                string[] contexts = contextData.GetContexts();
                
                if (contexts != null && contexts.Length > 0) //This could be null
                {
                    uiManager._activeFlags = EditorGUILayout.MaskField("Enable Contexts On Startup", uiManager._activeFlags, contexts);
                }
            }

            // Draw the default inspector
            DrawDefaultInspector();


            if (componentIsDirty) EditorUtility.SetDirty(target);
        }
    }
}
