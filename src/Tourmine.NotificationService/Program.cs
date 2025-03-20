//using Tourmine.NotificationService.Consumers;

//var builder = WebApplication.CreateBuilder(args);

//// Adiciona os servi�os necess�rios para Swagger
//builder.Services.AddControllers(); // Adiciona o controller
//builder.Services.AddEndpointsApiExplorer(); // Adiciona o endpoint no Swagger
//builder.Services.AddSwaggerGen();  // Adiciona o Swagger

//builder.Services.AddHttpClient<RabbitMqConsumer>();
//builder.Services.AddSingleton<RabbitMqConsumer>(); // Adiciona o consumidor de mensagens RabbitMQ

//var app = builder.Build();

//if(app.Environment.IsDevelopment())
//{
//    app.UseSwagger(); // Habilita o Swagger
//    app.UseSwaggerUI(); // Habilita a interface gr�fica do Swagger UI
//}

////app.UseHttpsRedirection();

//app.MapControllers();

//var rabbitMqConsumer = app.Services.GetRequiredService<RabbitMqConsumer>();
//await rabbitMqConsumer.StartListeningAsync(); // Inicia o consumo de mensagens

//app.Run();


// Cria��o do HttpClient, voc� pode configurar a URL base aqui, se necess�rio
using Tourmine.NotificationService.Consumers;

var httpClient = new HttpClient();

// Cria��o do consumidor RabbitMQ
var rabbitMqConsumer = new RabbitMqConsumer(httpClient);

// Iniciar o servi�o de escuta (aguardando mensagens)
await rabbitMqConsumer.StartListeningAsync();

// Deixar a aplica��o executando indefinidamente
// Assim o programa n�o vai fechar e continuar� ouvindo as mensagens
Console.WriteLine("Pressione qualquer tecla para sair...");
Console.ReadKey();