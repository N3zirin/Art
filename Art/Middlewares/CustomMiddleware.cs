namespace Art.Middlewares
{
    public class CustomMiddleware
    {

            private readonly RequestDelegate _next;         // core.http den gelir - HTTP request'i isleyen funksiyadir
            private readonly ILogger _logger;               // loglamaq ucun olan tipdir
            public CustomMiddleware(RequestDelegate next, ILoggerFactory logger) //ILoggerFactory - Qeydiyyat sistemini konfiqurasiya etmək və
                                                                                 //qeydiyyatdan keçmiş "ILoggerProvider"-lərdən "ILogger" nümunələrini
                                                                                 //yaratmaq üçün istifadə olunan növü təmsil edir.
            {
                _next = next;
                _logger = logger.CreateLogger("CustomMiddleware"); //CreateLogger - yeni ILogger instance'i yaradir, return'u ILogger'dir
            }//logger - ILoggerFactory nin parametridir
            //HttpContext - (kapsulyasiya edir)bir http request haqda butun HTTP spesifik melumatlarini  
            public async Task Invoke(HttpContext httpContext)            // task - assinxron emeliyyati temsil edir
            {
                //httpContext. burda is goruruk
                _logger.LogWarning("CustomMiddleware initiated.");   // log message yazdiq
                await _next(httpContext); // bu hemise sonda olur
            }
        }

        // Extension method used to add the middleware to the HTTP request pipeline.
        public static class CustomMiddlewareExtensions
        {
            public static IApplicationBuilder UseCustomMiddleware(this IApplicationBuilder builder) //IApplicationBuilder-request pipeline'ninin mexanizmini ozunde saxlayir
            {
                return builder.UseMiddleware<CustomMiddleware>();                                   //UseMiddleware - pipeline'a middleware type'i elave edir
            }
        }
}
