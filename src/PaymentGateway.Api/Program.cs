global using PaymentGateway.Application.Dtos.CreatePayment;

using FluentValidation;
using FluentValidation.AspNetCore;

using MicroElements.Swashbuckle.FluentValidation.AspNetCore;

using Microsoft.AspNetCore.Mvc;

using PaymentGateway.Api.Filters;
using PaymentGateway.Application.Dtos.Validators;
using PaymentGateway.Application.Mapper;
using PaymentGateway.Application.Service;
using PaymentGateway.BankAcquirer.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(CustomModelValidationAttribute));
});
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddScoped<IValidator<CreatePaymentRequestDto>, CreatePaymentRequestDtoValidator>();
builder.Services.AddScoped<IPaymentProcessorService, PaymentProcessorService>();
builder.Services.AddScoped<IAcquirerService, AcquirerService>();
builder.Services.AddHttpClient<IAcquirerService, AcquirerService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration.GetSection("Acquirer:BaseUrl").Value);
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddFluentValidationRulesToSwagger();
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<AutoMapperProfile>());

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
