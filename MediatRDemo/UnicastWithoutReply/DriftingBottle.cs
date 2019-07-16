using System;
using MediatR;

namespace MediatRDemo.UnicastWithoutReply
{
    /// <summary>
    /// 漂流瓶
    /// </summary>
    public class DriftingBottle : IRequest
    {
        public DriftingBottle(string message)
        {
            Console.WriteLine($"构造漂流瓶：{this.GetHashCode().ToString("X")}");
            this.Message = message;
        }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }
    }
}
