using System;
using System.Reflection;
using System.Windows.Forms;
using Autofac;
using MediatR;
using MediatR.Pipeline;
using MediatRDemo.Multicast;
using MediatRDemo.Unicast;

namespace MediatRDemo
{
    public partial class DemoForm : Form
    {
        /// <summary>
        /// 中介者
        /// </summary>
        IMediator mediator = BuildMediator();

        public DemoForm()
        {
            this.InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            var response = this.mediator
                .Send(new PingRequest($"测试单播请求-{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}"))
                .Result;

            Console.WriteLine(response.Message);
        }

        /// <summary>
        /// 创建中介者
        /// </summary>
        /// <returns></returns>
        private static IMediator BuildMediator()
        {
            var builder = new ContainerBuilder();

            // 注册 IMediator
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly).AsImplementedInterfaces();

            // 注册单播类型
            builder.RegisterAssemblyTypes(typeof(PingRequest).Assembly, typeof(PongResponse).Assembly)
                .AsClosedTypesOf(typeof(IRequestHandler<,>))
                .AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(typeof(PingRequest).Assembly)
                .AsClosedTypesOf(typeof(INotificationHandler<>))
                .AsImplementedInterfaces();

            // 注册处理行为
            builder.RegisterGeneric(typeof(RequestPostProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(RequestPreProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));

            // 注册服务工厂
            builder.Register<ServiceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });

            var container = builder.Build();

            //var behaviors = container
            //    .Resolve<IEnumerable<IPipelineBehavior<PingRequest, PongResponse>>>()
            //    .ToList();

            var mediator = container.Resolve<IMediator>();

            return mediator;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.mediator
                .Publish(new Quota(DateTime.Now.Ticks / 10000.0))
                .Wait();
        }
    }
}
