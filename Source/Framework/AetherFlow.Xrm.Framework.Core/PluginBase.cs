using System;
using AetherFlow.Xml.Framework.Core.Interfaces;
using AetherFlow.Xrm.Framework.Core.Configuration;
using AetherFlow.Xrm.Framework.Core.Interfaces;
using AetherFlow.Xrm.Framework.Core.Processors;
using Microsoft.Xrm.Sdk;

namespace AetherFlow.Xrm.Framework.Core
{
    public abstract class PluginBase : IPlugin
    {
        private readonly ITraceConfiguration _traceConfiguration;

        protected PluginBase(ITraceConfiguration traceConfiguration = null)
        {
            _traceConfiguration = traceConfiguration ?? new TraceConfiguration();
        }

        protected virtual void ExecuteCrmPlugin(IDataverseContainer container) { }

        public void Execute(IServiceProvider serviceProvider)
        {
            // Get the associated services
            var executionContext = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            var tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            var notificationService = (IServiceEndpointNotificationService)serviceProvider.GetService(typeof(IServiceEndpointNotificationService));
            var serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            var organizationService = serviceFactory.CreateOrganizationService(executionContext.UserId);

            // Check to see if we need a new container or if we have been
            // passed one, for instance, from unit testing
            IDataverseContainer container = new DataverseContainer();
            if (executionContext.InputParameters.ContainsKey("DataverseContainer"))
                container = (IDataverseContainer)executionContext.InputParameters["DataverseContainer"];

            // Add services to the container
            container.Add<IServiceProvider>(serviceProvider);
            container.Add<IPluginExecutionContext>(executionContext);
            container.Add<ITracingService>(tracingService);
            container.Add<IServiceEndpointNotificationService>(notificationService);
            container.Add<IOrganizationServiceFactory>(serviceFactory);
            container.Add<IOrganizationService>(organizationService);
            container.Add<ITraceConfiguration>(_traceConfiguration);

            // Add generic interfaces to the container
            // Then get an instance of the log object
            RegisterGenericProcessors(container);
            var log = container.Get<ILog>();

            // Now, lets run the execute command, but wrap in a try catch
            try { ExecuteCrmPlugin(container); }
            catch (Exception ex)
            {
                // Ignore correctly thrown exception
                if (ex is InvalidPluginExecutionException) throw;

                // Deal with fatal unexpected exception
                log.Fatal("Unexpected Error: " + ex.Message, ex);
                throw new InvalidPluginExecutionException(
                    OperationStatus.Failed, 
                    "An unexpected error occurred - " + ex.Message
                );
            }
        }

        private void RegisterGenericProcessors(IDataverseContainer container)
        {
            container.Add<IBulkExecutor, BulkExecutor>();
            container.Add<IQueryPager, QueryPager>();
            container.Add<ILog, Log>();
        }
    }
}
