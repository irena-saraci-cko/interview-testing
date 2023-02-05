global using PaymentGateway.Application.Dtos.CreatePayment;
using System.Text.Json.Serialization;
using FluentValidation;
using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Api.Filters;
using PaymentGateway.Application.Dtos.Validators;
using PaymentGateway.Application.Mapper;
using PaymentGateway.Application.Service;
using PaymentGateway.BankAcquirer.Services;


using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((ctx, lc) => lc
    .ReadFrom.Configuration(ctx.Configuration)
);

// Add services to the container.
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(CustomModelValidationAttribute));
})
.AddJsonOptions(cfg =>
{
    cfg.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
}

);
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddScoped<IValidator<CreatePaymentApiRequest>, CreatePaymentRequestDtoValidator>();
builder.Services.AddScoped<IPaymentProcessorService, PaymentProcessorService>();
builder.Services.AddScoped<IAcquirerService, AcquirerService>();
builder.Services.AddHttpClient<IAcquirerService, AcquirerService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration.GetSection("AcquirerBank:BaseUrl").Value);
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddFluentValidationRulesToSwagger();
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<AutoMapperProfile>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
