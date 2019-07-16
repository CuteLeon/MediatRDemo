using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Autofac;
using MediatR;
using MediatR.Pipeline;
using MediatRDemo.Multicast;
using MediatRDemo.Unicast;
using MediatRDemo.UnicastWithoutReply;

namespace MediatRDemo
{
    public partial class DemoForm : Form
    {
        /// <summary>
        /// 服务容器
        /// </summary>
        IContainer container = null;

        /// <summary>
        /// 中介者
        /// </summary>
        IMediator mediator = null;

        public DemoForm()
        {
            this.InitializeComponent();

            this.container = this.BuilderContainer();
            this.mediator = this.BuildMediator();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            var response = this.mediator
                .Send(new PingRequest($"测试单播请求-{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}"))
                .Result;

            Console.WriteLine(response.Message);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.mediator
                .Publish(new Quota(DateTime.Now.Ticks / 10000.0))
                .Wait();
        }

        /// <summary>
        ///创建服务容器
        /// </summary>
        /// <returns></returns>
        private IContainer BuilderContainer()
        {
            var builder = new ContainerBuilder();

            // 注册 IMediator
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly).AsImplementedInterfaces();

            // 自动注册程序集内中介者相关类型 (如果包含在同一程序集，仅注册这里一次即可)
            builder.RegisterAssemblyTypes(typeof(DriftingBottle).Assembly)
                .AsImplementedInterfaces();

            /* 上面已经注册，此处将重复注册
            
            // 显示注册单播类型
            builder.RegisterAssemblyTypes(typeof(PingRequest).Assembly, typeof(PongResponse).Assembly)
                .AsClosedTypesOf(typeof(IRequestHandler<,>))
                .AsImplementedInterfaces();

            // 显示注册多播类型
            builder.RegisterAssemblyTypes(typeof(Quota).Assembly)
                .AsClosedTypesOf(typeof(INotificationHandler<>))
                .AsImplementedInterfaces();
             */

            // 注册处理行为
            builder.RegisterGeneric(typeof(RequestPostProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(RequestPreProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));

            // 注册服务工厂
            builder.Register<ServiceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });

            return builder.Build();
        }

        /// <summary>
        /// 创建中介者
        /// </summary>
        /// <returns></returns>
        private IMediator BuildMediator()
            => this.container.Resolve<IMediator>();

        private void Button3_Click(object sender, EventArgs e)
        {
            var behaviors = this.container
                .Resolve<IEnumerable<IPipelineBehavior<PingRequest, PongResponse>>>()
                .ToList();

            Console.WriteLine(string.Join("\n", behaviors.Select(be => be.GetType().Name)));
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            _ = this.mediator
                .Send(new DriftingBottle($"漂流瓶-{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}"))
                .Result;
        }
    }
}
