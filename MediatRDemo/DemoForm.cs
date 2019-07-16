using System;
using System.Reflection;
using System.Windows.Forms;
using Autofac;
using MediatR;
using MediatR.Pipeline;
using MediatRDemo.Unicast;

namespace MediatRDemo
{
    public partial class DemoForm : Form
    {
        /// <summary>
        /// 单播中介者
        /// </summary>
        IMediator unicastMediator = BuildUnicastMediator();

        public DemoForm()
        {
            this.InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            var response = this.unicastMediator
                .Send(new PingRequest($"测试单播请求-{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}"))
                .Result;

            Console.WriteLine(response.Message);
        }

        /// <summary>
        /// 创建单播中介者
        /// </summary>
        /// <returns></returns>
        private static IMediator BuildUnicastMediator()
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly).AsImplementedInterfaces();

            var mediatrOpenTypes = new[]
            {
                typeof(IRequestHandler<,>),
                typeof(INotificationHandler<>),
            };

            foreach (var mediatrOpenType in mediatrOpenTypes)
            {
                builder
                    .RegisterAssemblyTypes(typeof(PingRequest).GetTypeInfo().Assembly)
                    .AsClosedTypesOf(mediatrOpenType)
                    .AsImplementedInterfaces();
            }

            builder.RegisterGeneric(typeof(RequestPostProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(RequestPreProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));

            builder.Register<ServiceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });

            var container = builder.Build();

            // The below returns:
            //  - RequestPreProcessorBehavior
            //  - RequestPostProcessorBehavior
            //  - GenericPipelineBehavior

            //var behaviors = container
            //    .Resolve<IEnumerable<IPipelineBehavior<Ping, Pong>>>()
            //    .ToList();

            var mediator = container.Resolve<IMediator>();

            return mediator;
        }
    }
}
