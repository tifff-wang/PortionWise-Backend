using PortionWise.Configs;

var builder = WebApplication.CreateBuilder(args);

// Configure services
ControllerConfig.config(builder);
MappingConfig.config(builder);
DatabaseConfig.config(builder);
DIConfig.config(builder);
SwaggerConfig.config(builder);
HttpClientConfig.config(builder);
UserSecretConfig.config(builder);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
