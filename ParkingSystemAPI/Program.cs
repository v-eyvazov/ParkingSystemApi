
using Microsoft.EntityFrameworkCore;
using ParkingSystemAPI.Data;
using ParkingSystemAPI.Repositories;
using ParkingSystemAPI.Repositories.Impl;
using ParkingSystemAPI.Services;
using ParkingSystemAPI.Services.Impl;

namespace ParkingSystemAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .WithExposedHeaders("content-disposition");
            }));

            builder.Services.AddDbContext<ApplicationDbContext>(option =>
            {
                option.UseNpgsql(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
            });

            builder.Services.AddScoped<IParkingLotRepository, ParkingLotRepository>();
            builder.Services.AddScoped<ITicketService, TicketService>();
            builder.Services.AddSingleton<IQRGeneratorService, QRGeneratorService>();
            builder.Services.AddSingleton<IFilePathUtilService, FilePathUtilService>();
            builder.Services.AddSingleton<IPDFGeneratorService<SyncfusionPDFGeneratorService>, SyncfusionPDFGeneratorService>();
            builder.Services.AddSingleton<IPDFGeneratorService<PdfSharpCorePDFGeneratorService>, PdfSharpCorePDFGeneratorService>();
            builder.Services.AddSingleton<IQRReaderClientService, QRReaderClientService>();


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors("MyPolicy");

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}