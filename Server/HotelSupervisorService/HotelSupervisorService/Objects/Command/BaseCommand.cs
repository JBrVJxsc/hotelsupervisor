using HotelSupervisorService.Managers;

namespace HotelSupervisorService.Objects.Command
{
    /// <summary>
    /// 命令的基础实体。
    /// </summary>
    public class BaseCommand
    {
        protected CommunicationManager communicationManager = new CommunicationManager();
    }
}
