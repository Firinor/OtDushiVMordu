using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace FirTime
{
    public class Timer
    {
        private CancellationTokenSource _token;
        
        public async void Start(float sec, Action action)
        {
            if (_token is not null)
            {
                Debug.Log("Need create new Timer!!");
                return;
            }
            
            _token = new();

            Task timerTask = Task.Delay((int)(sec * 1000), //ms to sec
                _token.Token);
            await timerTask;
            if (timerTask.IsCompletedSuccessfully)
                action?.Invoke();
        }

        public void Stop()
        {
            _token.Cancel();
        }
    }
}