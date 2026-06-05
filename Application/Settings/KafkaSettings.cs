namespace Application.Settings;

public class KafkaSettings
{
    public const string Section = "Kafka";
    public string BootstrapServers { get; set; }
    public string Topic { get; set; }
    public string GroupId { get; set; }
}