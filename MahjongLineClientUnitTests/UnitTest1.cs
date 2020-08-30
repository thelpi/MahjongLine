using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace MahjongLineClientUnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            ServerDto dto1 = new ServerDto { Aaaah = "Babar" };
            ServerDto dto2 = new ServerDto { Aaaah = "Toto" };

            List<Tuple<ServerDto, bool>> serverTuples =
                new List<Tuple<ServerDto, bool>>
                {
                    new Tuple<ServerDto, bool>(dto1, true),
                    new Tuple<ServerDto, bool>(dto2, false),
                };

            string stringTuples = JsonConvert.SerializeObject(serverTuples);

            var clientTuples = JsonConvert.DeserializeObject<List<Tuple<ClientDto, bool>>>(stringTuples);
        }
    }

    public class ServerDto
    {
        public string Aaaah { get; set; }
    }

    public class ClientDto
    {
        public string Aaaah { get; set; }
    }
}
