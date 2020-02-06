using AutoMapper;
using ItSingular.RH.Application.DTOs.Infrastructure;
using ItSingular.RH.Application.Interfaces;
using ItSingular.RH.Application.Services;
using ItSingular.RH.Domain.Entities;
using ItSingular.RH.Infrastructure.Interfaces;
using ItSingular.RH.Persistence.Repositories;
using ItSingular.RH.WebApi.Data;
using ItSingular.RH.WebApi.Helpers;
using ItSingular.RH.WebApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using System;

namespace ItSingular.RH.WebApi
{
    public class Startup
    {
        //private readonly ILogger _logger;

        public Startup(IConfiguration configuration/*, ILogger<Startup> logger*/)
        {
            Configuration = configuration;
            //_logger = logger;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //nessa seção é onde injetamos todos os serviços que serão suportados por nossa api

            //injetando o CORS para permitir que o servidor Angular faça requisições a nossa API que roda em outra porta e pode rodar inclusive em outro domínio
            // Add service and create Policy with options
            //services.AddCors(option =>
            //{
            //    option.AddPolicy("CorsPolicy", buider => buider.AllowAnyOrigin()
            //                                        .AllowAnyMethod()
            //                                        .AllowAnyHeader()
            //                                        .AllowCredentials()
            //    );
            //});
            services.AddCors();


            services.AddControllers();

            // Configurando o uso da classe de contexto para
            // acesso às tabelas do ASP.NET Identity Core
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("LocalConnection")));

            services.AddDbContext<ItSingularDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("LocalConnection")));


            #region ASP.NET Identity

            // Ativando a utilização do ASP.NET Identity, a fim de
            // permitir a recuperação de seus objetos via injeção de
            // dependências
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            var signingConfigurations = new SigningConfigurations();
            services.AddSingleton(signingConfigurations);

            var tokenConfigurations = new TokenConfigurations();
            new ConfigureFromConfigurationOptions<TokenConfigurations>(
                Configuration.GetSection("TokenConfigurations"))
                    .Configure(tokenConfigurations);
            services.AddSingleton(tokenConfigurations);


            services.AddAuthentication(authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(bearerOptions =>
            {
                bearerOptions.SaveToken = true;

                var paramsValidation = bearerOptions.TokenValidationParameters;
                paramsValidation.IssuerSigningKey = signingConfigurations.Key;
                paramsValidation.ValidAudience = tokenConfigurations.Audience;
                paramsValidation.ValidIssuer = tokenConfigurations.Issuer;

                // Valida a assinatura de um token recebido
                paramsValidation.ValidateIssuerSigningKey = true;

                // Verifica se um token recebido ainda é válido
                paramsValidation.ValidateLifetime = true;

                // Tempo de tolerância para a expiração de um token (utilizado
                // caso haja problemas de sincronismo de horário entre diferentes
                // computadores envolvidos no processo de comunicação)
                paramsValidation.ClockSkew = TimeSpan.Zero;

            });


            // Ativa o uso do token como forma de autorizar o acesso
            // a recursos deste projeto
            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
                    .RequireAuthenticatedUser().Build());
            });

            #endregion


            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);


            //injeta como servico nossos repositórios
            services.AddScoped<IProfissionaisService, ProfissionaisService>();
            services.AddScoped<IProfissionaisRepository, ProfissionaisRepository>();


            // injeta o swagger para testes e documentação da api
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1.0",
                    new OpenApiInfo
                    {
                        Title = "ItSingular R.H.",
                        Version = "1.0",
                        Description = "ItSingular Recursos Humanos",
                        Contact = new OpenApiContact
                        {
                            Name = "ItSingular",
                            Email = "rh@itsingular.com.br",
                            Url = new Uri("https://itsingular.com.br/")
                        }
                    });
                c.IncludeXmlComments(System.IO.Path.Combine(System.AppContext.BaseDirectory, "ItSingular.RH.WebApi.xml"));
            });
            services.AddSwaggerGen(swagger =>
            {
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            //ILoggerFactory loggerFactory,
            ApplicationDbContext appContext,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //Ativa o cros 
            //app.UseCors("CorsPolicy");

            app.UseHttpsRedirection();


            // Ativando middlewares para uso do Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1.0/swagger.json", "ItSingular R.H. (V 1.0)");
            });

            // Criação de estruturas, usuários e permissões
            // na base do ASP.NET Identity Core (caso ainda não existam)
            new IdentityInitializer(appContext, userManager, roleManager)
                .Initialize();


            app.UseRouting();


            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            //app.UseAuthorization();

            //app.UseAuthentication();

            //app.UseMvc();


            app.UseEndpoints(endpoints =>
                     {
                         endpoints.MapControllers();
                     });
        }
    }
}
