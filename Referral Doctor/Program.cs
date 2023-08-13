using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Referral_Doctor.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// 添加表与数据库的连接 !!!!!
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 完成上一步后要执行数据迁移到数据库 add-migration AddTablesToDatabase 在Package manager Console运行
// AddTablesToDatabase 是迁移的名字
// 当数据迁移建立好后， run: update-database, 开始迁移
// 若在数据迁移之后有增加新的table, 则需run Add-Migration NewTableName, 然后 update-database



// 添加认证服务
builder.Services.AddAuthentication(options => {
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options => {
    options.LoginPath = "/Login";
    options.AccessDeniedPath = "/Forbidden";
});







//用于显示razor pages
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();


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

// 添加认证中间件
app.UseAuthentication();


app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
