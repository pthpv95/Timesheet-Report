﻿using Antlr.Runtime.Misc;
using Autofac;
using Autofac.Integration.Mvc;
using AutoMapper;
using MediatR;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using Owin;
using System.Collections.Generic;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using Timesheetreport.Infrastructure.TimeSheet;
using TimesheetReport.Core.Features.TRUsers;
using TimesheetReport.Core.Infrastructure.DAL;
using TimesheetReport.Core.Infrastructure.Handler.TimeSheets;
using TimesheetReport.Infrastructure.CompositionRoot;
using TimesheetReport.Model.Timesheet;
using AppCoreBootstrapper = TimesheetReport.Core.Bootstrapper;

namespace TimesheetReport.WebUI
{
    public partial class Startup
    {
        private void ConfigureCompositionRoot(IAppBuilder app)
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterModule(
                new TimeSheetReportModule
                {
                    TimeSheetReportConnectionStringName = "TimesheetReportContext"
                });

            AppCoreBootstrapper.Bootstrap(containerBuilder);

            RegisterAspNetMvcComponents(containerBuilder);
            RegisterMediatRComponents(containerBuilder);
            RegisterTimesheetReportContext(containerBuilder);
            RegisterAspNetIdentityComponents(containerBuilder, app);
            RegisterOtherComponents(containerBuilder);
            RegisterAutoMapperComponents(containerBuilder);
            RegisterTimeSheetImport(containerBuilder);

            var container = containerBuilder.Build();
            IntegrateDIContainerWithFrameworks(app, container);
        }

        private static void RegisterAspNetMvcComponents(ContainerBuilder containerBuilder)
        {
            // Register your MVC controllers.
            containerBuilder.RegisterControllers(typeof(MvcApplication).Assembly);

            // Register web abstractions like HttpContextBase.
            containerBuilder.RegisterModule<AutofacWebTypesModule>();

            // Enable property injection into action filters.
            containerBuilder.RegisterFilterProvider();
        }

        private static void RegisterMediatRComponents(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<Mediator>().As<IMediator>();

            containerBuilder.Register<SingleInstanceFactory>(componentContext =>
            {
                var context = componentContext.Resolve<IComponentContext>();
                return serviceType => context.Resolve(serviceType);
            });

            containerBuilder.Register<MultiInstanceFactory>(componentContext =>
            {
                var context = componentContext.Resolve<IComponentContext>();
                return serviceType => (IEnumerable<object>)context.Resolve(typeof(IEnumerable<>).MakeGenericType(serviceType));
            });
        }

        private void RegisterTimesheetReportContext(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<TimesheetReportContext>().AsSelf().InstancePerLifetimeScope();
        }

        private void RegisterAspNetIdentityComponents(ContainerBuilder containerBuilder, IAppBuilder app)
        {
            containerBuilder.RegisterType<ManageUsersDbContext>().AsSelf().InstancePerLifetimeScope();

            containerBuilder
                .RegisterType<UserStore<TRUser>>()
                .As<IUserStore<TRUser>>()
                .WithParameter((pi, ctx) => pi.ParameterType == typeof(DbContext), (pi, ctx) => ctx.Resolve<ManageUsersDbContext>())
                .InstancePerLifetimeScope();

            containerBuilder.RegisterType<ApplicationUserManager>().AsSelf().InstancePerLifetimeScope();
            containerBuilder.RegisterType<ApplicationSignInManager>().AsSelf().InstancePerLifetimeScope();
            containerBuilder.Register<IAuthenticationManager>(c => HttpContext.Current.GetOwinContext().Authentication).InstancePerLifetimeScope();
            containerBuilder.Register<IDataProtectionProvider>(c => app.GetDataProtectionProvider()).InstancePerLifetimeScope();
        }

        private void RegisterAutoMapperComponents(ContainerBuilder containerBuilder)
        {
            var configuration = new MapperConfiguration(config =>
            {
                config.AddProfiles(typeof(TimesheetReportContext));
                config.AddProfiles(typeof(TimeSheetReportModule));
                config.AddProfiles(typeof(Startup));
            });

            containerBuilder
                .RegisterInstance(configuration)
                .As<MapperConfiguration>()
                .ExternallyOwned()
                .As<IConfigurationProvider>()
                .ExternallyOwned();
            containerBuilder.Register(c => c.Resolve<MapperConfiguration>().CreateMapper(c.Resolve));
        }

        private void RegisterTimeSheetImport(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<DataTimeSheetReader>().As<IDataTimeSheetReader>();
            containerBuilder.RegisterType<WorkTimeDataValidator>().AsSelf();
            containerBuilder.RegisterType<SaveWorkTimeLookUpTable>().As<ISaveWorkTimeTable>();
            containerBuilder.RegisterType<TimeSheetDataValidator>().AsSelf();
            containerBuilder.RegisterType<TimeSheetLookUpTableFactory>().AsSelf();
            containerBuilder.RegisterType<SaveWorkTimeLookUpTable>().As<ISaveWorkTimeTable>();
        }
        private void RegisterOtherComponents(ContainerBuilder containerBuilder)
        {
            //containerBuilder
            //    .RegisterType<ResetPasswordEmailFactory>()
            //    .AsSelf()
            //    .SingleInstance();
        }

        private static void IntegrateDIContainerWithFrameworks(IAppBuilder app, IContainer container)
        {
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            // Note: Register the Autofac middleware FIRST, then the Autofac MVC middleware.
            app.UseAutofacMiddleware(container);
            app.UseAutofacMvc();
        }
    }
}