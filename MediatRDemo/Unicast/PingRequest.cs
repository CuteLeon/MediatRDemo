using System;
using MediatR;

namespace MediatRDemo.Unicast
{
    /// <summary>
    /// Ping 请求
    /// </summary>
    public class PingRequest : IRequest<PongResponse>
    {
        public PingRequest(string message)
        {
            Console.WriteLine($"构造 Ping 请求：{this.GetHashCode().ToString("X")}");
            this.Message = message;
        }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }
    }
}
