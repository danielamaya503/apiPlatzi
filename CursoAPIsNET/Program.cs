using CursoAPIsNET.Data;
using CursoAPIsNET.Middleware;
using CursoAPIsNET.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Scalar.AspNetCore;
using System.Reflection.Metadata;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Logging es para registrar eventos y mensajes de la aplicación
builder.Services.AddLogging();
builder.Services.AddControllers();

//--------------BASE DE DATOS EN MEMORIA------------------
//Configurar el contexto de la base de datos
//builder.Services.AddDbContext<AppDbContext>(option =>
//{
//    //Usar una base de datos en memoria para pruebas y desarrollo
//    option.UseInMemoryDatabase("CursoDb");
//});


//--------------BASE DE DATOS SQL SERVER------------------
//Agregar el contexto de la base de datos
//Agregaremos el servicio de DbContext para la aplicación
//para que pueda interactuar con la base de datos SQL Server
builder.Services.AddDbContext<AppDbContext>(option => {
    //Obtener la cadena de conexión desde el archivo de configuración
    //Configuration es una propiedad del objeto builder que proporciona acceso a la configuración de la aplicación
    //GetConnectionString es un método que obtiene la cadena de conexión con el nombre especificado
    var dbstring = builder.Configuration.GetConnectionString("SqlServerConnection");
    //Configurar el contexto de la base de datos para usar SQL Server con la cadena de conexión obtenida
    //UseSqlServer es un método de extensión que configura el proveedor de base de datos SQL Server
    option.UseSqlServer(dbstring);
});


//--------------------Base de datos con PostgreSQL--------------------

//builder.Services.AddDbContext<AppDbContext>(options =>
//    // Configurar el contexto de la base de datos para usar PostgreSQL
//    //UseNpgsql es un método de extensión que configura el proveedor de base de datos PostgreSQL
//    options.UseNpgsql(
//        builder.Configuration.GetConnectionString("PostgreSqlConnection"))
//);


//--------------------OpenAPI / Swagger / Scalar / CORS / DI--------------------

//OPENAPI es para documentar y probar APIs RESTful
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


//Libreria Swagger (Descargar el paquete NuGet Swashbuckle.AspNetCore)
//agregar servicios de Swagger
//Sirve para generar documentación interactiva de la API
builder.Services.AddSwaggerGen(option=> {

    //Obtener el archivo XML generado por la documentación de comentarios del código
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    //Combina la ruta base de la aplicación con el nombre del archivo XML
    var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);
    option.IncludeXmlComments(xmlPath);

    //Configuración para Swagger con autenticación básica
    option.AddSecurityDefinition("BasicAuth", new OpenApiSecurityScheme
    {
        //Definimos el esquema de seguridad para la autenticación básica
        Type = SecuritySchemeType.Http,
        //El esquema de autenticación es "basic"
        Scheme = "basic",
        //La ubicación del parámetro de autenticación es en el encabezado HTTP
        In = ParameterLocation.Header,
        //es una breve descripción del esquema de seguridad
        Description = "Autenticación básica para la API username / password"
    });

    //Definimos el requisito de seguridad para usar el esquema de autenticación básica
    option.AddSecurityRequirement(document => new OpenApiSecurityRequirement() {
        //Indicamos que el esquema de seguridad
        [new OpenApiSecuritySchemeReference("BasicAuth", document)] = []
    });
});



//---------------------------INTERFACES GRAFICAS----------------------------------------
//SWAGGER
//SCALAR

//CORS Policy
//Cualquier cliente puede consumir la API como scalar o swagger
var MyAllowOrigins = "MyAllorOrigins";

//Funciona para configurar CORS
builder.Services.AddCors(options =>
{
    //Definimos la politica de CORS
    options.AddPolicy(name: MyAllowOrigins,
            policy =>
            {
                policy.AllowAnyOrigin()
                    .AllowAnyHeader();
            });
});

//---------------------------INYECCIÓN DE DEPENDENCIAS----------------------------------------

//AddScoped es para registrar servicios con un tiempo de vida por solicitud
//UserService implementa la interfaz IUserService
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITaskItemService, TaskItemService>();


//CORS Policy
//es para que cualquier cliente pueda consumir la API
builder.Services.AddCors();



var app = builder.Build();

//----------------------------Middleware--------------------------------
//es un software que se encuentra entre el sistema operativo y las aplicaciones
//es un componente de software que se encarga de procesar las solicitudes HTTP entrantes


// Configure the HTTP request pipeline.
//pipeline es una serie de componentes de software que procesan las solicitudes HTTP entrantes

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    //Libreria Scalar INSTALAR el paquete NuGet Scalar.AspNetCore
    //Sirve para mapear la referencia de la API Scalar
    app.MapScalarApiReference();
    //UseSwagger y UseSwaggerUI son necesarios para habilitar Swagger en la aplicación
    //genera la documentación Swagger en formato JSON
    app.UseSwagger();
    //proporciona una interfaz de usuario web para interactuar con la documentación Swagger
    //genera una interfaz de usuario web para explorar y probar los endpoints de la API
    app.UseSwaggerUI();
}

//configurar CORS
app.UseCors(MyAllowOrigins);


app.UseHttpsRedirection();

app.UseBasicAuth();

app.UseAuthorization();

//Custom Middleware
//es para registrar las solicitudes entrantes

//Agregar el middleware personalizado al pipeline de la aplicación
app.UseRequestLogging();

app.MapControllers();

app.Run();
