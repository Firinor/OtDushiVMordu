namespace FirAnimations
{
    public interface IFirAnimation
    {
        public void Initialize();
        public void Play();
        public void Stop();
        public void ToStartPoint();
        public void ToEndPoint();
    }
}