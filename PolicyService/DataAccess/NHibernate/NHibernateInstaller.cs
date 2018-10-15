﻿using Microsoft.Extensions.DependencyInjection;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Connection;
using NHibernate.Bytecode;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NHibernate;
using PolicyService.Domain;

namespace PolicyService.DataAccess.NHibernate
{
    public static class NHibernateInstaller
    {
        public static IServiceCollection AddNHibernate(this IServiceCollection services)
        {
            var cfg = new Configuration();

            cfg.DataBaseIntegration(db =>
            {
                db.Dialect<MsSql2012Dialect>();
                db.Driver<SqlClientDriver>();
                db.ConnectionProvider<DriverConnectionProvider>();
                db.BatchSize = 500;
                db.IsolationLevel = System.Data.IsolationLevel.ReadCommitted;
                db.LogSqlInConsole = false;
                db.ConnectionString = @"Server=localhost\SQLEXPRESS;Database=lab_policy_service;Trusted_Connection=True;";
                db.Timeout = 30;/*seconds*/
            });

            cfg.Proxy(p => p.ProxyFactoryFactory<DefaultProxyFactoryFactory>());

            cfg.Cache(c => c.UseQueryCache = false);

            cfg.AddAssembly(typeof(NHibernateInstaller).Assembly);

            services.AddSingleton<ISessionFactory>(cfg.BuildSessionFactory());

            //services.AddScoped<ISession>(svc => svc.GetService<ISessionFactory>().OpenSession());
            services.AddSingleton<IUnitOfWorkProvider, UnitOfWorkProvider>();

            return services;
        }
    }
}
