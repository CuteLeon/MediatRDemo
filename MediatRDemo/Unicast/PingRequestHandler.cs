using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace MediatRDemo.Unicast
{
    /// <summary>
    /// Ping 请求处理器
    /// </summary>
    public class PingRequestHandler : IRequestHandler<PingRequest, PongResponse>
    {
        public PingRequestHandler()
            => Console.WriteLine($"构造 Ping 请求处理器：{this.GetHashCode().ToString("X")}");

        /// <summary>
        /// 处理请求
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PongResponse> Handle(PingRequest request, CancellationToken cancellationToken)
            => Task.FromResult(new PongResponse($"我是请求 {request.GetHashCode().ToString("X")} 的响应。"));
    }
}
