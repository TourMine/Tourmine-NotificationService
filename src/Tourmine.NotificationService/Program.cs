//using Tourmine.NotificationService.Consumers;

//var builder = WebApplication.CreateBuilder(args);

//// Adiciona os serviços necessários para Swagger
//builder.Services.AddControllers(); // Adiciona o controller
//builder.Services.AddEndpointsApiExplorer(); // Adiciona o endpoint no Swagger
//builder.Services.AddSwaggerGen();  // Adiciona o Swagger

//builder.Services.AddHttpClient<RabbitMqConsumer>();
//builder.Services.AddSingleton<RabbitMqConsumer>(); // Adiciona o consumidor de mensagens RabbitMQ

//var app = builder.Build();

//if(app.Environment.IsDevelopment())
//{
//    app.UseSwagger(); // Habilita o Swagger
//    app.UseSwaggerUI(); // Habilita a interface gráfica do Swagger UI
//}

////app.UseHttpsRedirection();

//app.MapControllers();

//var rabbitMqConsumer = app.Services.GetRequiredService<RabbitMqConsumer>();
//await rabbitMqConsumer.StartListeningAsync(); // Inicia o consumo de mensagens

//app.Run();


// Criação do HttpClient, você pode configurar a URL base aqui, se necessário
using Tourmine.NotificationService.Consumers;

var httpClient = new HttpClient();

// Criação do consumidor RabbitMQ
var rabbitMqConsumer = new RabbitMqConsumer(httpClient);

// Iniciar o serviço de escuta (aguardando mensagens)
await rabbitMqConsumer.StartListeningAsync();

// Deixar a aplicação executando indefinidamente
// Assim o programa não vai fechar e continuará ouvindo as mensagens
Console.WriteLine("Pressione qualquer tecla para sair...");
Console.ReadKey();