using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Social_Login.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<Social_LoginContext>
    (options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Social_LoginContextConnection")));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<Social_LoginContext>();
builder.Services.AddAuthentication().AddFacebook(x=>
{
    x.AppId = "983367856548233";
    x.AppSecret = "8c000fdb1d148a7906d2a0bccd558d3b";
});

builder.Services.AddAuthentication().AddGoogle(googleOptions =>
{
    googleOptions.ClientId = "587403257610-nnh2lrccgbmp9o090htnbp5k7ilnjnv4.apps.googleusercontent.com";
    googleOptions.ClientSecret = "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";
});
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = "LinkedIn"; 
})
.AddCookie()
.AddOAuth("LinkedIn", options =>
{
    options.ClientId = "77lsvb6qvqneaz";
    options.ClientSecret = "WD3CRrMNaWHZpkeS";
    options.CallbackPath = new PathString("/signin-linkedin");
    options.AuthorizationEndpoint = "https://www.linkedin.com/oauth/v2/authorization";
    options.TokenEndpoint = "https://www.linkedin.com/oauth/v2/accessToken";
    options.UserInformationEndpoint = "https://api.linkedin.com/v2/me";
    options.SaveTokens = true;
    options.Scope.Add("email");
});
builder.Services.AddAuthentication().AddTwitter(twitterOptions =>
{
    twitterOptions.ConsumerKey = "Gp8ajHFmSidtC87yz1kFvvarS";
    twitterOptions.ConsumerSecret = "pa2eQw7AYbu8TDcDmbCqc4C7lKU5Bax85GfZZy4ziVXOIirbL2";
});
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
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
