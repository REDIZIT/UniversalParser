namespace InGame.Dynamics
{
    public abstract class DynamicParser
    {
        public bool IsWorking { get; private set; }

        public void Start()
        {
            OnStart();
            IsWorking = true;
        }
        public void Stop()
        {
            OnStop();
            IsWorking = false;
        }
        protected void SwitchWorkState()
        {
            if (IsWorking) Stop();
            else Start();
        }

        protected abstract void OnStart();
        protected abstract void OnStop();
    }
}