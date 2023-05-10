using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Xml;

namespace ACME.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AbastecimientoController : ControllerBase
    {
        [HttpPost]
        public IActionResult RealizarPedido([FromBody] dynamic pedidoJson)
        {
            // Transformar mensaje JSON a XML
            XmlDocument pedidoXml = new XmlDocument();
            XmlElement pedidoElement = pedidoXml.CreateElement("pedido");
            pedidoXml.AppendChild(pedidoElement);

            pedidoElement.AppendChild(pedidoXml.CreateElement("Cantidad")).InnerText = pedidoJson.cantidadPedido;
            pedidoElement.AppendChild(pedidoXml.CreateElement("EAN")).InnerText = pedidoJson.codigoEAN;
            pedidoElement.AppendChild(pedidoXml.CreateElement("Producto")).InnerText = pedidoJson.nombreProducto;
            pedidoElement.AppendChild(pedidoXml.CreateElement("Cedula")).InnerText = pedidoJson.numDocumento;
            pedidoElement.AppendChild(pedidoXml.CreateElement("Direccion")).InnerText = pedidoJson.direccion;

            string pedidoXmlString = pedidoXml.OuterXml;

            // Llamar al servicio SOAP para envío de pedido
            // Código de ejemplo: int codigoEnvio = ServicioSOAP.EnviarPedido(pedidoXmlString);

            // Transformar respuesta XML a JSON
            XmlDocument respuestaXml = new XmlDocument();
            XmlElement respuestaElement = respuestaXml.CreateElement("respuesta");
            respuestaXml.AppendChild(respuestaElement);

            respuestaElement.AppendChild(respuestaXml.CreateElement("codigoEnvio")).InnerText = "80375472";
            respuestaElement.AppendChild(respuestaXml.CreateElement("estado")).InnerText = "Entregado exitosamente al cliente";

            string respuestaXmlString = respuestaXml.OuterXml;

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(respuestaXmlString);

            string respuestaJsonString = JsonConvert.SerializeXmlNode(doc);

            dynamic respuestaJson = JsonConvert.DeserializeObject(respuestaJsonString);

            return Ok(respuestaJson);
        }
    }
}