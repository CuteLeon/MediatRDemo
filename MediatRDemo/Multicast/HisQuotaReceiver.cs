using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace MediatRDemo.Multicast
{
    /// <summary>
    /// 他的行情接收器
    /// </summary>
    public class HisQuotaReceiver : INotificationHandler<Quota>
    {
        public HisQuotaReceiver()
            => Console.WriteLine($"构造 {this.GetType().Name} ({this.GetHashCode().ToString("X")})");

        /// <summary>
        /// 处理
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task Handle(Quota quota, CancellationToken cancellationToken)
        {
            Console.WriteLine($"{this.GetType().Name} 接收到行情：{quota.Price.ToString("N4")}");
            return Task.CompletedTask;
        }
    }
}
