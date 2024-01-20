using System.Diagnostics;

namespace Tetris.Core
{
    public sealed class FpsCounter
    {
        private readonly Stopwatch _stopwatch;
        private int _frameCount;
        private double _fps;

        public FpsCounter()
        {
            _stopwatch = new();
            _frameCount = 0;
            _fps = 0;
        }

        public double Fps => _fps;

        public void Start() => _stopwatch.Start();

        public void Update()
        {
            var elapsed = _stopwatch.ElapsedMilliseconds;

            _frameCount++;

            if (elapsed >= 1000)
            {
                _fps = _frameCount / (elapsed / 1000);
                _frameCount = 0;
                _stopwatch.Restart();
                Debug.WriteLine($"Current fps: {_fps}.");
            }
        }
    }
}