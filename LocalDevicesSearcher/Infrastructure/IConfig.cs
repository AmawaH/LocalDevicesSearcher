namespace LocalDevicesSearcher.Infrastructure
{
    public interface IConfig
    {
        string ConnectionString { get; }
        bool Range { get; }
        int MinSubnetRange { get; }
        int MaxSubnetRange { get; }
        int[] SubnetCollection { get; }
    }
}