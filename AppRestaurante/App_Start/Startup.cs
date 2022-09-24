using AppRestaurante.ViewModel;
using AutoMapper;
using Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Owin;
using Owin;
using System;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(AppRestaurante.App_Start.Startup))]

namespace AppRestaurante.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
         
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
        }

    }
}
