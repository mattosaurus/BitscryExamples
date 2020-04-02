using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OData.Edm;
using OData.Models.AdventureWorks;

namespace OData
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
            services.AddControllersWithViews();
            services.AddDbContext<Models.AdventureWorksContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DataConnection"))
                );

            services.AddOData();
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllers();
                endpoints.Select().Filter().OrderBy().Count().Expand().MaxTop(100);
                endpoints.MapODataRoute("api", "api", GetEdmModel());
            });
        }

        private IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Address>("Addresses");
            builder.EntitySet<Customer>("Customers");
            builder.EntitySet<CustomerAddress>("CustomerAddresses")
                .EntityType
                .HasKey(table => new { table.CustomerId, table.AddressId });
            builder.EntitySet<Product>("Products");
            builder.EntitySet<ProductCategory>("ProductCategories");
            builder.EntitySet<ProductDescription>("ProductDescriptions");
            builder.EntitySet<ProductModel>("ProductModels");
            builder.EntitySet<ProductModelProductDescription>("ProductModelProductDescriptions")
                .EntityType
                .HasKey(table => new { table.ProductModelId, table.ProductDescriptionId });
            builder.EntitySet<SalesOrderDetail>("SalesOrderDetails");
            builder.EntitySet<SalesOrderHeader>("SalesOrderHeaders")
                .EntityType
                .HasKey(table => new { table.SalesOrderId });
            return builder.GetEdmModel();
        }
    }
}
