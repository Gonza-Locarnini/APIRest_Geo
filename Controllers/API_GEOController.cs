using System;
using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using APIRest_Geo.Models;
using APIRest_Geo.Services;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace APIRest_Geo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class API_GEOController : ControllerBase
    {
        public GeoRepository _repo { get; set; }

        [HttpGet]
        public object Get()
        {
            _repo = new GeoRepository();
            return _repo.Get();
        }
        [HttpGet("id")]
        public object Get(int id)
        {
            _repo = new GeoRepository();
            return _repo.Get(id);
        }

        [HttpPost]
        public object Post(Geo pModel)
        {
            if (ModelState.IsValid)
            {
                _repo = new GeoRepository();
                var newId = _repo.Insert(pModel);

                var message = JsonSerializer.Serialize(pModel, typeof(Geo));
                Task t = InvokeAsync(message, (model, ea) =>
                {
                    _repo = new GeoRepository();
                    var body = ea.Body.ToArray();
                    var jsonReply = Encoding.UTF8.GetString(body);
                    if (jsonReply == ""){
                        pModel.Estado = Geo.eEstados.ErrorOSM;
                        _repo.Update(pModel);
                        return;
                    }
                    var g = JsonSerializer.Deserialize(jsonReply, typeof(Geo));
                    if (g is Geo)
                    {
                        (g as Geo).Estado = Geo.eEstados.Terminado;
                        _repo.Update(g as Geo);
                    }
                    return;
                });

                return "{Id: " + newId + "}";
            }
            return "Error en la carga!";
        }


        private static async Task InvokeAsync(string n, EventHandler<BasicDeliverEventArgs> CallBack)
        {
            var rpcClient = new RpcClient(CallBack);
            var response = await rpcClient.CallAsync(n.ToString());
            rpcClient.Close();
        }
    }
}
