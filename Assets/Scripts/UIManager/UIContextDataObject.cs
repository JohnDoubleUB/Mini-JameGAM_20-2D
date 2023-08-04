
using UnityEngine;

namespace UIManagerLibrary.Scripts
{
    [CreateAssetMenu(fileName = "UIDataContextObject", menuName = "UIManagerLibrary/UIDataContextObject", order = 1)]
    public class UIContextDataObject : ScriptableObject, IUIContextData
    {
        public string[] Contexts;
        public bool CaseSensitiveContexts = false; //This is incase you want your contexts to be case sensitive, defaults to off

        public string[] GetContexts()
        {
            return Contexts;
        }
    }

    [System.Serializable]
    public class UIContextData : IUIContextData 
    {
        public string[] Contexts;
        public bool CaseSensitiveContexts = false; //This is incase you want your contexts to be case sensitive, defaults to off

        public string[] GetContexts()
        {
            return Contexts;
        }
    }

    public interface IUIContextData
    {
        public string[] GetContexts();
    }
}
