using System.Text;
using UnityEngine;

namespace Utils
{
    public class Timer
    {
        private bool isStopted;
        private float elapsedTime;
        private readonly StringBuilder stringBuilder = new StringBuilder(8);
            
        public void Update()
        {
            if (isStopted)
            {
                return;
            }

            elapsedTime += Time.deltaTime;

            if (elapsedTime <= 0)
            {
                elapsedTime = 0;
            }

            int minutes = Mathf.FloorToInt(elapsedTime / 60);
            int seconds = Mathf.FloorToInt(elapsedTime % 60);
            stringBuilder.Clear();
            stringBuilder.AppendFormat("{0:D2}:{1:D2}", minutes, seconds);
        }

        public override string ToString()
        {
            return stringBuilder.ToString();
        }

        public void Clear()
        {
            isStopted = false;
            elapsedTime = 0;
        }

        public void Stop()
        {
            isStopted = true;
        }
    }
}