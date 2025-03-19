using Tourmine.NotificationService.Consumers;

var builder = WebApplication.CreateBuilder(args);

// Adiciona os serviços necessários para Swagger
builder.Services.AddControllers(); // Adiciona o controller
builder.Services.AddEndpointsApiExplorer(); // Adiciona o endpoint no Swagger
builder.Services.AddSwaggerGen();  // Adiciona o Swagger

builder.Services.AddSingleton<RabbitMqConsumer>(); // Adiciona o consumidor de mensagens RabbitMQ

var app = builder.Build();

if(app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Habilita o Swagger
    app.UseSwaggerUI(); // Habilita a interface gráfica do Swagger UI
}

//app.UseHttpsRedirection();

app.MapControllers();

var rabbitMqConsumer = app.Services.GetRequiredService<RabbitMqConsumer>();
await rabbitMqConsumer.StartListeningAsync(); // Inicia o consumo de mensagens

app.Run();
