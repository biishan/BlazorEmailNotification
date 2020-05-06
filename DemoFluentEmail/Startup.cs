using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Mail;

namespace DemoFluentEmail
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();

            services.AddFluentEmail("support@domain.com", "Company Support")
                .AddRazorRenderer()
                .AddSmtpSender(() =>
                {
                    var isSpecifiedPickupDirectory = Configuration.GetValue<bool>("SmtpClient:IsSpecifiedPickupDirectory");

                    if (isSpecifiedPickupDirectory)
                    {
                        var pickupDirectoryLocation = Configuration.GetValue<string>("SmtpClient:PickupDirectoryLocation");
                        var host = Configuration.GetValue<string>("SmtpClient:Host");
                        return new SmtpClient()
                        {
                            DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,
                            PickupDirectoryLocation = pickupDirectoryLocation,
                            Host = host
                        };
                    }
                    else
                    {
                        var host = Configuration.GetValue<string>("SmtpClient:Host");
                        return new SmtpClient()
                        {
                            DeliveryMethod = SmtpDeliveryMethod.Network,
                            Host = host,
                            Port = 25
                        };
                    }
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
