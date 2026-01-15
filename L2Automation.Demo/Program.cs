using L2Automation.Demo.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<EventStore>();
builder.Services.AddSingleton<MachineStateService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();