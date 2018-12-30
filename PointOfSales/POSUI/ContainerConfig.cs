using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using POSLibrary.Abstract;
using POSLibrary.DataAccess;
using POSLibrary.Models;
using System.Reflection;

namespace POSUI
{
    public static class ContainerConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();
            //builder.RegisterType<ProductData>().As<IProductData>();
            builder.RegisterType<CategoryData>().As<ICategoryData>();
            builder.RegisterAssemblyTypes(Assembly.Load(nameof(POSLibrary)))
                .Where(t => t.Namespace.Contains("DataAccess"))
                .As(t => t.GetInterfaces().FirstOrDefault(i=>i.Name=="I"+t.Name));
            return builder.Build();
        }
    }
}
