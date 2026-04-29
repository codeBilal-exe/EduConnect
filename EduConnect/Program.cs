using EduConnect.Components;
using EduConnect.Interfaces;
using EduConnect.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// DIP: Register services as interfaces to enable dependency inversion
// SRP: Each service has a single responsibility and is registered separately
// builder.Services.AddScoped<IAuthStateService, AuthStateService>();
builder.Services.AddScoped<IAuthStateService, AuthStateService>();

// Data-carrying services are now Singletons to share state across all users in this in-memory demo
builder.Services.AddSingleton<IStudentService, StudentService>();
builder.Services.AddSingleton<ICourseService, CourseService>();
builder.Services.AddSingleton<IGradeService, GradeService>();
builder.Services.AddSingleton<INotificationService, NotificationService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
