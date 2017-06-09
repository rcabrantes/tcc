using Servidor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Servidor.Controllers
{
    public class HomeController : Controller
    {
        //Lista com o estado das portas, armazenado em memória (o static faz essa variavel se manter a mesma enquanto o servidor nao desliga)
        private static GPIOControl GPIOs= new GPIOControl();

        //index: route acessa por default
        public ActionResult Index()
        {
            //Cria uma lista para colocar o resultado
            var ports = new List<bool>();

            //Para cada porta na lista static, adicionar o estado na lista resultado
            GPIOs.Ports.ForEach(c => ports.Add(c.PortStatus));

            //Envia o resultado para a view, que vai acessar a lista como model
            return View(ports);
        }

        //Route para alterar estado da porta, recebe o numero da poarta
        [HttpPost]
        public ActionResult TogglePort(int portNum)
        {
            //Encontra a porta com o numero recebido, e inverte seu estado
            GPIOs.Ports[portNum].PortStatus = !GPIOs.Ports[portNum].PortStatus;

            //Retorna o estado da porta como JSON
            return Json(GPIOs.Ports[portNum].PortStatus);
        }

        //Route para ler o estado das portas
        public ActionResult getGPIOPortStatus()
        {
            //Criar uma nova lista para os resultados
            var ports = new List<bool>();

            //Para cada porta, adiciona o estado na lista resultado
            GPIOs.Ports.ForEach(c=>ports.Add(c.PortStatus));

            //Retorna o resultado, transformando em JSON
            return Json(ports, JsonRequestBehavior.AllowGet);
        }
    }
}