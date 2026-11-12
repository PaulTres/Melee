namespace Animation.Tweening
{
    public class TweenFSMTest : ImprovedMonoBehaviour
    {
        public bool block;
        public bool toIdle;
        public bool stab;
        public TweenStateMachine stateMachine;
        public TweenBlockState TweenBlockState;
        public TweenStabWindupState stabState;

        private void Awake()
        {
            if (stateMachine == null) stateMachine = GetComponent<TweenStateMachine>();
        }
        private void Update()
        {
            if (block)
            {
                block = false;
                TweenBlockState.ChangeToThis();
            }
            else if (toIdle)
            {
                toIdle = false;
                stateMachine.ChangeState(stateMachine.DefaultState, true);
            }
            else if (stab)
            {
                stab = false;
                stabState.ChangeToThis();
            }
        }
    }
}
