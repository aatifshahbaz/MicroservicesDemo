namespace Common.Settings
{
    public class KafkaProducer
    {
        public string Server { get; set; } = "localhost";
        public int Port { get; set; } = 9092;

        public string BootstraServers
        {
            get { return $"{Server}:{Port}"; }
        }

        public string Acks { get; set; } = "Leader";   //Acknowledgement form ALL, Leader or None partitions within a Topic


        //Following are optional setting, could be provider for better fine tuning of bandwidth, latency and through-put

        public string? ClientId { get; set; }   //Could be used for debugging to uniquely identify a producer

        public string? SecurityProtocol { get; set; }      //Sasl_Ssl or other protocol to authenticate producer

        public string? MessageTimeoutMs { get; set; }      //Timeout in msec to wait for Broker to respond in case of acknowledgement

        public string? BatchNumMessages { get; set; }     //Size of batch if producer is utilizing batch-sending mechanism

        public string? LingerMs { get; set; }        //Time in msec that producer wait to accumulate messages in batch   

        public string? CompressionType { get; set; }  //Gzip or any other compression type can be used before sending
    }


    public class KafkaConsumer
    {
        public string Server { get; set; } = "localhost";
        public int Port { get; set; } = 9092;

        public string BootstraServers
        {
            get { return $"{Server}:{Port}"; }
        }

        public string GroupId { get; set; } = "Default Group";      //Consumers GroupId, No 2 consumers from same Consumer Group are not allowed to read from same partition

        public string AutoOffsetReset { get; set; } = "Earliest";   //If a consumer goes down, then after it get back again, it will start from oldest available message

        public string EnableAutoCommit { get; set; } = "True";      //Consumer will autocommit the offset after reading message from partition, and comitted offset will be marked as read. Offset are intexed position in partitions


        //Following are optional setting for Authentication

        public string? ClientId { get; set; }   //Could be used for debugging to uniquely identify a producer

        public string? SecurityProtocol { get; set; }      //Sasl_Ssl or other protocol to authenticate producer

        public string? SaslMechanism { get; set; }

        public string? SaslUsername { get; set; }

        public string? SaslPassword { get; set; }
    }
}
