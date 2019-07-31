using Microsoft.AspNetCore.SignalR;
using Moq;
using NUnit.Framework;
using SignalRWebPack.Hubs;
using System.Threading;
using System.Threading.Tasks;

namespace SignalRWebPack.Test
{
    [TestFixture]
    class ChatHub_OnConnectedShould
    {
        [Test]
        public async Task ChatHub_OnConnect_ShouldReturnMessage()
        {
            Mock<IHubCallerClients> mockClients = new Mock<IHubCallerClients>();
            Mock<IClientProxy> mockClientProxy = new Mock<IClientProxy>();

            mockClients.Setup(clients => clients.All).Returns(mockClientProxy.Object);

            ChatHub hub = new ChatHub()
            {
                Clients = mockClients.Object
            };

            await hub.NewMessage(1, "message");

            mockClients.Verify(clients => clients.All, Times.Once);

            mockClientProxy.Verify(clientProxy => clientProxy.SendCoreAsync(
                                                                            "messageReceived",
                                                                            It.Is<object[]>(o => o != null && o.Length == 1 && ((object[])o[0]).Length == 2),
                                                                            default(CancellationToken)),
                                                                            Times.Once);
        }
    }
}
