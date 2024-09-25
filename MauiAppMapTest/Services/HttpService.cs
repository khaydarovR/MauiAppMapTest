using MauiAppMapTest.Infra;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MauiAppMapTest.Services
{
    public class HttpService
    {
        private readonly HttpClient httpClient;
        private readonly ILogger<HttpService> logger;


        public HttpService(HttpClient httpClient, ILogger<HttpService> logger)
        {
            this.httpClient = httpClient;
            this.logger = logger;
        }

        public async Task<Res<TResponse>> Post<TData, TResponse>(string url, TData data)
        {
            HttpResponseMessage httpResponseMessage;
            httpResponseMessage = await httpClient.PostAsJsonAsync(url, data);


            switch (httpResponseMessage.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    return await HandleBadRequest<TResponse>(url, httpResponseMessage);
                case HttpStatusCode.InternalServerError:
                    return HandleInternalServerError<TResponse>(url);
                case HttpStatusCode.Unauthorized:
                    return HandleUnauthorized<TResponse>(url);
                case HttpStatusCode.Forbidden:
                    return await HandleForbidden<TResponse>(url, httpResponseMessage);
                default:
                    return await HandleSuccessStatusCode<TResponse>(url, httpResponseMessage);
            }
        }


        public async Task<Res<TResponse>> Get<TResponse>(string url)
        {
            HttpResponseMessage httpResponseMessage;
            httpResponseMessage = await httpClient.GetAsync(url);
            switch (httpResponseMessage.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    return await HandleBadRequest<TResponse>(url, httpResponseMessage);
                case HttpStatusCode.InternalServerError:
                    return HandleInternalServerError<TResponse>(url);
                case HttpStatusCode.Unauthorized:
                    return HandleUnauthorized<TResponse>(url);
                case HttpStatusCode.Forbidden:
                    return await HandleForbidden<TResponse>(url, httpResponseMessage);
                default:
                    return await HandleSuccessStatusCode<TResponse>(url, httpResponseMessage);
            }
        }

        private async Task<Res<TResponse>> HandleBadRequest<TResponse>(string url, HttpResponseMessage httpResponseMessage)
        {
            ErrorResponse? errorResponse;
            try
            {
                errorResponse = await httpResponseMessage.Content.ReadFromJsonAsync<ErrorResponse>();
            }
            catch (Exception ex)
            {
                var errorMsg = $"Не удалось прочитать ошибки c сервера | URL: {url}";
                logger.LogError($"{errorMsg}\n{ex.Message}\nСервер должен отправить - {typeof(ErrorResponse).FullName}");
                return new Res<TResponse>(errorText: errorMsg);
            }

            if (errorResponse == null || errorResponse.Messages == null || errorResponse.Messages.Count == 0)
            {
                var txt = $"Сервер не отправил список ошибок | URL: {url}";
                return new Res<TResponse>(errorText: txt);
            }
            return new Res<TResponse>(errorTexts: errorResponse.Messages);
        }


        private async Task<Res<TResponse>> HandleSuccessStatusCode<TResponse>(string url, HttpResponseMessage httpResponseMessage)
        {
            TResponse? successRusult;
            try
            {
                successRusult = await httpResponseMessage.Content.ReadFromJsonAsync<TResponse>();
            }
            catch (Exception ex)
            {
                var json = await httpResponseMessage.Content.ReadAsStringAsync();
                var errTxt = $"Ошибка десериализации :\n{json}\n=>\n{typeof(TResponse).FullName}\nURL: {url}";
                logger.LogError(errTxt);
                return new Res<TResponse>(errorText: $"Не удалось преоброзвать полученный JSON в " + typeof(TResponse).Name + $" | URL: {url}");
            }

            return new Res<TResponse>(data: successRusult!);
        }


        private Res<TResponse> HandleInternalServerError<TResponse>(string url)
        {
            var errorMsg = $"Ошибка на стороне сервера | URL: {url}";
            logger.LogError($"{errorMsg}");
            return new Res<TResponse>(errorText: errorMsg);
        }

        private Res<TResponse> HandleUnauthorized<TResponse>(string url)
        {
            var errorMsg = $"Вы не авторизованы | URL: {url}";
            logger.LogError($"{errorMsg}");

            //TODO: обновить состояние авторизации, перенаправить в /login
            return new Res<TResponse>(errorText: errorMsg);
        }

        private async Task<Res<TResponse>> HandleForbidden<TResponse>(string url, HttpResponseMessage httpResponseMessage)
        {
            var res = await HandleBadRequest<TResponse>(url, httpResponseMessage);

            var errorMsg = $"Недостаточно прав: {res.ErrorList.Messages.First()}";
            logger.LogError($"{errorMsg}");
            return new Res<TResponse>(errorText: errorMsg);
        }
    }
}