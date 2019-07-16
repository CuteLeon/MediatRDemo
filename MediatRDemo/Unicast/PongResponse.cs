namespace MediatRDemo.Unicast
{
    /// <summary>
    ///  Pong 响应
    /// </summary>
    public class PongResponse
    {
        public PongResponse(string message)
        {
            this.Message = message;
        }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }
    }
}
