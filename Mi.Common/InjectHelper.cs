﻿using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;

namespace Mi.Common
{
    /// <summary>
    /// 类库注入帮助类
    /// </summary>
    public static class InjectHelper
    {
        /// <summary>
        /// Add Scoped from InterfaceAssembly and ImplementAssembly to the specified Microsoft.Extensions.DependencyInjection.IServiceCollection.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="interfaceAssembly"></param>
        /// <param name="implementAssembly"></param>
        public static void AddScoped(this IServiceCollection services, Assembly interfaceAssembly, Assembly implementAssembly)
        {
            var interfaces = interfaceAssembly.GetTypes().Where(t => t.IsInterface);
            var implements = implementAssembly.GetTypes();
            foreach (var item in interfaces)
            {
                var type = implements.FirstOrDefault(x => item.IsAssignableFrom(x));
                if (type != null)
                {
                    services.AddScoped(item, type);
                }
            }
        }

        /// <summary>
        /// Add AddSingleton from InterfaceAssembly and ImplementAssembly to the specified Microsoft.Extensions.DependencyInjection.IServiceCollection.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="interfaceAssembly"></param>
        /// <param name="implementAssembly"></param>
        public static void AddSingleton(this IServiceCollection services, Assembly interfaceAssembly, Assembly implementAssembly)
        {
            var interfaces = interfaceAssembly.GetTypes().Where(t => t.IsInterface);
            var implements = implementAssembly.GetTypes();
            foreach (var item in interfaces)
            {
                var type = implements.FirstOrDefault(x => item.IsAssignableFrom(x));
                if (type != null)
                {
                    services.AddSingleton(item, type);
                }
            }
        }

        /// <summary>
        /// Add AddTransient from InterfaceAssembly and ImplementAssembly to the specified Microsoft.Extensions.DependencyInjection.IServiceCollection.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="interfaceAssembly"></param>
        /// <param name="implementAssembly"></param>
        public static void AddTransient(this IServiceCollection services, Assembly interfaceAssembly, Assembly implementAssembly)
        {
            var interfaces = interfaceAssembly.GetTypes().Where(t => t.IsInterface);
            var implements = implementAssembly.GetTypes();
            foreach (var item in interfaces)
            {
                var type = implements.FirstOrDefault(x => item.IsAssignableFrom(x));
                if (type != null)
                {
                    services.AddTransient(item, type);
                }
            }
        }
    }
}
