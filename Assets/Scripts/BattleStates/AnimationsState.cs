public class AnimationsState : BattleState
{
    private AnimationController _animationController;

    public AnimationsState(AnimationController animationController)
    {
        _animationController = animationController;
    }
    public override void OnEnter()
    {
        _animationController.StartAnimations();
    }
}