using System;
using System.Collections.Generic;
using System.Timers;
using HotelSupervisorClient.Interfaces;
using HotelSupervisorClient.Objects.Commands;
using HotelSupervisorClient.Objects.Communication;

namespace HotelSupervisorClient.Managers
{
    internal class CommandManager
    {
        public CommandManager()
        { 
            
        }

        private List<ICommand> iCommandList=new List<ICommand>();
        private List<Timer> timerList=new List<Timer>();

        public void Init()
        {
            GetCommands();
            StartCommands();
        }

        public void StopCommand()
        {
            foreach (Timer timer in timerList)
            {
                timer.Enabled = false;
            }
            foreach (ICommand iCommand in iCommandList)
            {
                iCommand.SaveCommandResponseHolder();
            }
        }

        private void GetCommands()
        {
            iCommandList = new List<ICommand>();
            ICommand knockCommand = new KnockCommand();
            ICommand updateCommand = new UpdateCommand();
            iCommandList.Add(knockCommand);
            iCommandList.Add(updateCommand);
        }

        private void StartCommands()
        {
            timerList=new List<Timer>();
            foreach (ICommand iCommand in iCommandList)
            {
                Timer timer = new Timer(iCommand.CheckTime);
                timer.Elapsed += iCommand.Check;
                iCommand.InitCommandResponseHolder();
                iCommand.NeedProcess += iCommand_NeedProcess;
                iCommand.Check(null, null);
                timerList.Add(timer);
                timer.Enabled = true;
            }
        }

        void iCommand_NeedProcess(ICommand iCommand, List<MessageWhole> messageWholeList, bool firstReceive)
        {
            try
            {
                iCommand.Process(messageWholeList,firstReceive);
            }
            catch (Exception e)
            {
                LogManager.GlobalLogManager.CreateLog("命令执行出错。错误代码：" + e.Message);
            }
        }
    }
}
