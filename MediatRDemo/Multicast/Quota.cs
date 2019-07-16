using System;
using MediatR;

namespace MediatRDemo.Multicast
{
    /// <summary>
    /// 行情
    /// </summary>
    public class Quota : INotification
    {
        public Quota(double price)
        {
            Console.WriteLine($"构造行情 {price} ({this.GetHashCode().ToString("X")})");
            this.Price = price;
        }

        /// <summary>
        /// 价格
        /// </summary>
        public double Price { get; }
    }
}
