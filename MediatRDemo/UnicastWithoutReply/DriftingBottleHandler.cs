using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace MediatRDemo.UnicastWithoutReply
{
    /// <summary>
    /// 漂流瓶处理器
    /// </summary>
    public class DriftingBottleHandler : IRequestHandler<DriftingBottle>
    {
        /// <summary>
        /// 处理
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<Unit> Handle(DriftingBottle request, CancellationToken cancellationToken)
            => Task.Factory.StartNew(() =>
            {
                Console.WriteLine($"捡到一只漂流瓶：{request.Message}");
                return Unit.Value;
            });
    }
}
