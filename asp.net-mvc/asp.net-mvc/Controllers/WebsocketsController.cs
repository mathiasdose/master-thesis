using System;
using System.Net;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
//using System.Web.WebSockets;

namespace asp.net_mvc.Controllers
{
    public class WebsocketsController : ApiController
    {
        public IHttpActionResult GetWebsockets()
        {
            HttpContext currentContext = HttpContext.Current;
            if (currentContext.IsWebSocketRequest ||
                currentContext.IsWebSocketRequestUpgrading)
            {
                currentContext.AcceptWebSocketRequest(ProcessWebsocketSession);
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.SwitchingProtocols));
            }
            return ResponseMessage(new HttpResponseMessage(HttpStatusCode.BadRequest));
        }

        private async Task ProcessWebsocketSession(WebSocketContext context)
        {
            var ws = context.WebSocket;
            
            const int maxMessageSize = 1024;
            byte[] receiveBuffer = new byte[maxMessageSize];

            while (ws.State == WebSocketState.Open)
            {
                var result = await ws.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
                
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                }
                else if (result.MessageType == WebSocketMessageType.Binary)
                {
                    await ws.CloseAsync(WebSocketCloseStatus.InvalidMessageType, "Cannot accept binary frame", CancellationToken.None);
                }
                else
                {
                    int count = result.Count;
                    while (result.EndOfMessage == false)
                    {
                        if (count >= maxMessageSize)
                        {
                            string closeMessage = string.Format("Maximum message size: {0} bytes.", maxMessageSize);
                            await
                                ws.CloseAsync(WebSocketCloseStatus.MessageTooBig, closeMessage, CancellationToken.None);
                            return;
                        }
                        result =
                            await
                                ws.ReceiveAsync(new ArraySegment<byte>(receiveBuffer, count, maxMessageSize - count),
                                    CancellationToken.None);
                        count += result.Count;
                    }
                    var receivedString = Encoding.UTF8.GetString(receiveBuffer, 0, count);
                    var echoString = "You said " + receivedString;
                    ArraySegment<byte> outputBuffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(echoString));

                    await ws.SendAsync(outputBuffer, WebSocketMessageType.Text, true, CancellationToken.None);
                    }
                /*ArraySegment<byte> outputBuffer = new ArraySegment<byte>(receiveBuffer);
                
                
                await ws.SendAsync(outputBuffer, WebSocketMessageType.Text, true, CancellationToken.None);
                    
                var result = await ws.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
                /*int i = 0;
                while (i < 10)
                {
                    i++;
                    var status = result.CloseStatusDescription;

                    
                }*/
                
            }
        }
    }
}
