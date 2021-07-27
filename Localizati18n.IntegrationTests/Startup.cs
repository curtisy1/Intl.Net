namespace Localizati18n.IntegrationTests {
  using System.Collections.Generic;
  using System.Globalization;
  using Microsoft.AspNetCore.Builder;
  using Microsoft.AspNetCore.Hosting;
  using Microsoft.AspNetCore.Localization;
  using Microsoft.Extensions.Configuration;
  using Microsoft.Extensions.DependencyInjection;
  using Microsoft.Extensions.Hosting;
  
  public class Startup {
    public Startup(IConfiguration configuration) {
      this.Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services) {
      services.AddControllersWithViews();
      services.AddLocalization();
      
      services.Configure<RequestLocalizationOptions>(options => {
        var defaultCulture = new CultureInfo("de-DE");
        var supportedCultures = new List<CultureInfo> {
          defaultCulture,
          new CultureInfo("en-US") {
            DateTimeFormat = defaultCulture.DateTimeFormat
          },
        };

        options.DefaultRequestCulture = new RequestCulture(defaultCulture);
        options.SupportedCultures = supportedCultures;
        options.SupportedUICultures = supportedCultures;
        options.RequestCultureProviders = new List<IRequestCultureProvider> {
          new CookieRequestCultureProvider(),
        };
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
      if (env.IsDevelopment()) {
        app.UseDeveloperExceptionPage();
      } else {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }

      app.UseHttpsRedirection();
      app.UseStaticFiles();

      app.UseRequestLocalization();
      app.UseRouting();

      app.UseAuthorization();

      app.UseEndpoints(endpoints => {
        endpoints.MapControllerRoute(name: "default",
                                     pattern: "{controller=Home}/{action=Index}/{id?}");
      });
    }
  }
}