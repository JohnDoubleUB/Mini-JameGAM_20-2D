using UnityEngine;


namespace UIManagerLibrary.Scripts
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIContextObject : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup CanvasGroup;

        public bool DisableGameObjectOnInactive = true;

        public bool Display
        {
            get { return display; }
        }

        [SerializeField]
        private bool display;

        public float FadeSpeed = 3f;

        private float currentTransparency = 0f;

        public void SetDisplayAndActive(bool active, bool immediate = false)
        {
            display = active;
            CanvasGroup.blocksRaycasts = active;


            if (immediate)
            {
                SetActiveImmediate(active);
            }
            else if (display && !gameObject.activeInHierarchy)
            {
                gameObject.SetActive(true);
            }
        }

        private void SetActiveImmediate(bool active)
        {
            if (DisableGameObjectOnInactive) gameObject.SetActive(active);
            currentTransparency = active ? 1f : 0f;
            CanvasGroup.alpha = currentTransparency;
        }

        // Update is called once per frame
        protected void Update()
        {
            bool transparencyHasChanged = false;
            if (!display && currentTransparency != 0f)
            {

                currentTransparency = Mathf.Max(currentTransparency - (Time.deltaTime * FadeSpeed), 0f);
                transparencyHasChanged = true;
            }
            else if (display && currentTransparency != 1f)
            {
                currentTransparency = Mathf.Min(currentTransparency + (Time.deltaTime * FadeSpeed), 1f);
                transparencyHasChanged = true;
            }


            if (transparencyHasChanged) CanvasGroup.alpha = currentTransparency;

            if (!display && currentTransparency == 0f)
            {
                if (DisableGameObjectOnInactive && gameObject.activeInHierarchy) gameObject.SetActive(false);
            }
        }

        private void Reset()
        {
            CanvasGroup = GetComponent<CanvasGroup>();
        }
    }
}