using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectManager.AppContext;
using ProjectManager.Models;
using ProjectManager.Repositories.Classses;
using ProjectManager.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>() 
       .AddEntityFrameworkStores<AppDbContext>()
       .AddDefaultTokenProviders();

// Add services to the container.


builder.Services.AddTransient<IAtividadeRepository, AtividadeRepository>();
builder.Services.AddTransient<IProjetoRepository, ProjetoRepository>();
builder.Services.AddTransient<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddTransient<IFeedRepository, FeedRepository>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); // utilização do IHttpContextAccessor para retorno de Url

builder.Services.AddControllersWithViews();

builder.Services.AddMemoryCache(); //Adicionando o serviço de cache na aplicação (utilizando p/ retorno Url)
builder.Services.AddSession(); //Adicionando o serviço de sessão na aplicação

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession(); //Adicionando o uso de sessão na aplicação

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "filtroProjeto",
    pattern: "Atividade/{action}/{projeto?}",
    defaults: new { controller = "Atividade", action = "Index" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Conta}/{action=Login}/{id?}");

app.Run();
