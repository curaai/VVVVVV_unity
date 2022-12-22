using UnityEngine;

namespace VVVVVV.UI
{
    public class PanelController : MonoBehaviour
    {
        private readonly string ANIMATOR_TRANSITION_KEY = "open";
        private readonly string IDLE_STATE_NAME = "IDLE";

        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void Toggle()
        {
            bool isPlayingNow()
            {
                var state = animator.GetCurrentAnimatorStateInfo(0);
                var isIdleState = state.IsName(IDLE_STATE_NAME);

                return !isIdleState && state.normalizedTime < 1;
            }
            if (isPlayingNow()) return;

            var openNow = !animator.GetBool(ANIMATOR_TRANSITION_KEY);
            animator.SetBool(ANIMATOR_TRANSITION_KEY, openNow);

            Debug.Log($"[Panel] OpenState: {openNow}");
        }
    }
}
