using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace APIService1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //IdentityServer
            services.AddMvcCore().AddAuthorization().AddJsonFormatters();
            services.AddAuthentication(Configuration["Identity:Scheme"])
                        .AddIdentityServerAuthentication(options =>
                        {
                            options.RequireHttpsMetadata = false; //是否需要https
                            options.Authority = $"http://{Configuration["Identity:IP"]}:{Configuration["Identity:Port"]}";  //IdentityServer授权路径
                            options.ApiName = Configuration["Service:Name"];  //需要授权的服务名称
                        });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //启用Authentication中间件
            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
