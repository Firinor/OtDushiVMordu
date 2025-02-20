public class AnimationsState : BattleState
{
    private AnimationController _animationController;
    private float _timeLimit;
    
    public AnimationsState(BattleManager manager)
    {
        _animationController = manager.AnimationController;
        _timeLimit = manager.StatesTimeConfig.VSLifeTime;
    }
    public override void OnEnter()
    {
        _animationController.StartAnimations(_timeLimit);
    }
}