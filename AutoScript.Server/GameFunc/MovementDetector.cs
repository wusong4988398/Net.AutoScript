using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoScript.Share;

namespace AutoScript.Server.GameFunc
{

    public class MovementDetector
    {
        private Position _lastPosition;
        private DateTime _lastTimestamp;
        private readonly TimeSpan _thresholdDuration = TimeSpan.FromSeconds(2); // 阈值时间，例如2秒
        /// <summary>
        /// 画面是否停止
        /// </summary>
        public bool IsStoped {  get; set; }

        public MovementDetector(int threshold)
        {
            this._thresholdDuration=TimeSpan.FromSeconds(threshold);
        }

        public void UpdatePosition(Position newPosition)
        {
            var currentTime = DateTime.Now;
            if (_lastPosition != null && newPosition.Equals(_lastPosition) && (currentTime - _lastTimestamp) >= _thresholdDuration)
            {
                Trace.WriteLine("画面已经停止移动。");
                this.IsStoped=true;
            }
            else
            {
                _lastPosition = newPosition;
                _lastTimestamp = currentTime;
                this.IsStoped = false;
            }
        }
    }
}
