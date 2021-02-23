using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace middlewares
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.Use(async (context, next) =>
            {
                await context.Response.WriteAsync("Before Invoke from 1st app.Use()\n");
                await next();
                await context.Response.WriteAsync("After Invoke from 1st app.Use()\n");
            });

            app.Use(async (context, next) =>
            {
                await context.Response.WriteAsync("Before Invoke from 2nd app.Use()\n");
                await next();
                await context.Response.WriteAsync("After Invoke from 2nd app.Use()\n");
            });

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello from 1st app.Run()\n");
            });

            // the following will never be executed    
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello from 2nd app.Run()\n");
            });

            app.Map("/newsegment/segment1", a =>
                a.Run(c => c.Response.WriteAsync("Running from the /newsegment/segment1 branch!")));

            app.Map("/newbranch", a => {
                a.Map("/branch1", brancha => brancha
                    .Run(c => c.Response.WriteAsync("Running from the newbranch/branch1 branch!")));
                a.Map("/branch2", brancha => brancha
                    .Run(c => c.Response.WriteAsync("Running from the newbranch/branch2 branch!")));

                a.Run(c => c.Response.WriteAsync("Running from the newbranch branch!"));
            });
        }
    }
}
