using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using HotelSupervisorService.Exceptions;
using HotelSupervisorService.Interfaces;
using HotelSupervisorService.Objects.Command;
using HotelSupervisorService.Objects.Communication;
using HotelSupervisorService.Objects.Setting;
using LumiSoft.Net;
using LumiSoft.Net.IMAP;
using LumiSoft.Net.IMAP.Client;
using LumiSoft.Net.Mail;
using LumiSoft.Net.MIME;

namespace HotelSupervisorService.Managers
{
    /// <summary>
    /// 通信操作类。
    /// </summary>
    /// IMAP 地址imap.163.com SSL端口993 非SSL端口143
    /// SMTP 地址smtp.163.com SSL端口465/994 非SSL端口25
    /// POP3 地址pop.163.com SSL端口995 非SSL端口110
    public class CommunicationManager
    {
        public CommunicationManager()
        {
            Init();
        }

        private IMAP_Client imapClient;
        private List<MessageHeader> messageHeaderList = new List<MessageHeader>();
        private MessageHeader messageHeader;
        private List<MessageWhole> messageWholeList = new List<MessageWhole>();
        private SmtpClient smtpClient = new SmtpClient(EncryptionManager.Decrypt(global::HotelSupervisorService.Properties.Resources.SendMessageServer), 25);
        private MailMessage mailMessage = new MailMessage();
        private string sendMessageServiceProvider = EncryptionManager.Decrypt(global::HotelSupervisorService.Properties.Resources.SendMessageServiceProvider);
        string server = EncryptionManager.Decrypt(global::HotelSupervisorService.Properties.Resources.ReceiveMessageServer);
        private CommunicationSetting communicationSetting = new CommunicationSetting();
        private TimeoutManager timeoutManager = new TimeoutManager();
        private TimeSpan timeOutTimeSpan = new TimeSpan(0, 0, 0, 10);

        /// <summary>
        /// 连接。
        /// </summary>
        /// <param name="communicationParameter">通信参数实体。</param>
        /// <returns>true为连接成功；false为连接失败。</returns>
        public bool Connect(CommunicationParameter communicationParameter)
        {
            try
            {
                Disconnect();
                imapClient = new IMAP_Client();
                if (communicationParameter.UseSSL)
                {
                    timeoutManager.Do = null;
                    timeoutManager.Do = ConnectWithSSL;
                }
                else
                {
                    timeoutManager.Do = null;
                    timeoutManager.Do = ConnectWithoutSSL;
                }
                bool isTimeout = timeoutManager.DoWithTimeout(timeOutTimeSpan);
                if (isTimeout)
                {
                    return false;
                }
                imapClient.Login(communicationParameter.UserName, communicationParameter.Password);
                return true;
            }
            catch (Exception e)
            {
                throw new ExceptionPlus("83。", e);
            }
        }

        private void ConnectWithSSL()
        {
            imapClient.Connect(server, WellKnownPorts.IMAP4_SSL, true);
        }

        private void ConnectWithoutSSL()
        {
            imapClient.Connect(server, WellKnownPorts.IMAP4, false);
        }

        /// <summary>
        /// 断开连接。
        /// </summary>
        public void Disconnect()
        {
            if (imapClient != null)
            {
                if (imapClient.IsConnected)
                {
                    imapClient.Disconnect();
                }
            }
        }

        /// <summary>
        /// 获取最新的一封信息。
        /// </summary>
        /// <returns>信息实体。</returns>
        public MessageWhole GetLastMessage(IFolderProcessor iFolderProcessor)
        {
            try
            {
                if (LoadMessages("*:*", iFolderProcessor) <= 0)
                {
                    return null;
                }
                else
                {
                    return messageWholeList[0];
                }
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(ExceptionPlus))
                {
                    throw;
                }
                throw new ExceptionPlus("84。", e);
            }
        }

        /// <summary>
        /// 获取所有信息。
        /// </summary>
        /// <returns>信息实体列表。</returns>
        public List<MessageWhole> GetAllMessage(IFolderProcessor iFolderProcessor)
        {
            try
            {
                if (LoadMessages("1:*", iFolderProcessor) <= 0)
                {
                    return null;
                }
                else
                {
                    return messageWholeList;
                }
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(ExceptionPlus))
                {
                    throw;
                }
                throw new ExceptionPlus("85。", e);
            }
        }

        /// <summary>
        /// 获取指定序号的信息。
        /// </summary>
        /// <param name="sequence">序号。</param>
        /// <returns>信息实体列表。</returns>
        public List<MessageWhole> GetMessages(string sequence, IFolderProcessor iFolderProcessor)
        {
            try
            {
                if (LoadMessages(sequence, iFolderProcessor) <= 0)
                {
                    return null;
                }
                else
                {
                    return messageWholeList;
                }
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(ExceptionPlus))
                {
                    throw;
                }
                throw new ExceptionPlus("86。", e);
            }
        }

        public void SendCommandMessage(ICommand iCommand,CommandObject commandObject, string content,string[] fileNames)
        {
            try
            {
                smtpClient.Credentials = new NetworkCredential(iCommand.CommunicationParameter.UserName, iCommand.CommunicationParameter.Password);
                mailMessage.Subject = EncryptionManager.Encrypt(commandObject.CommandType.ToString() + "&" + commandObject.CommandID);
                mailMessage.Subject = "&&" + mailMessage.Subject;
                mailMessage.From = new MailAddress(iCommand.CommunicationParameter.UserName + sendMessageServiceProvider);
                mailMessage.To.Clear();
                mailMessage.To.Add(iCommand.TargetAddress);
                if (content != null)
                {
                    mailMessage.Body = content;
                }
                if (fileNames != null && fileNames.Length != 0)
                {
                    mailMessage.Attachments.Clear();
                    for (int i = 0; i < fileNames.Length; i++)
                    {
                        string fileName = fileNames[i];
                        if (File.Exists(fileName))
                        {
                            System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(fileName);
                            attachment.ContentDisposition.FileName= Path.GetFileName(fileName);
                            mailMessage.Attachments.Add(attachment);
                        }
                    }
                }
                smtpClient.Send(mailMessage);
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(ExceptionPlus))
                {
                    throw;
                }
                throw new ExceptionPlus("87。", e);
            }
        }

        private void Init()
        {
            communicationSetting.InitSetting();
        }

        private void Clear()
        {
            messageHeaderList = new List<MessageHeader>();
            messageHeader = null;
            messageWholeList = new List<MessageWhole>();
        }

        private int LoadMessages(string sequence, IFolderProcessor iFolderProcessor)
        {
            if (imapClient == null || !imapClient.IsConnected)
            {
                return -1;
            }
            Clear();
            imapClient.SelectFolder(iFolderProcessor.OperatingFolderName);
            IMAP_Client_FetchHandler fetchHandler = new IMAP_Client_FetchHandler();
            fetchHandler.NextMessage += FetchHandlerNextMessage;
            fetchHandler.Envelope += FetchHandlerEnvelope;
            fetchHandler.Flags += delegate { };
            fetchHandler.InternalDate += FetchHandlerInternalDate;
            fetchHandler.Rfc822Size += FetchHandlerRfc822Size;
            fetchHandler.UID += FetchHandlerUID;

            IMAP_SequenceSet seqSet = new IMAP_SequenceSet();
            seqSet.Parse(sequence);

            imapClient.Fetch(false, seqSet, new IMAP_Fetch_DataItem[]
				                             	{
				                             		new IMAP_Fetch_DataItem_Envelope(),
				                             		new IMAP_Fetch_DataItem_Flags(),
				                             		new IMAP_Fetch_DataItem_InternalDate(),
				                             		new IMAP_Fetch_DataItem_Rfc822Size(),
				                             		new IMAP_Fetch_DataItem_Uid()
				                             	},
                          fetchHandler);

            if (messageHeader != null)
            {
                messageHeaderList.Add(messageHeader);
            }

            foreach (MessageHeader header in messageHeaderList)
            {
                LoadMessage(header.UID);
            }
            return messageWholeList.Count; ;
        }

        private long currentLoadMessageUID = -1;

        private void LoadMessage(long uid)
        {
            currentLoadMessageUID = uid;
            try
            {
                IMAP_Client_FetchHandler fetchHandler = new IMAP_Client_FetchHandler();
                fetchHandler.Rfc822 += FetchHandlerRfc822;

                IMAP_SequenceSet seqSet = new IMAP_SequenceSet();
                seqSet.Parse(uid.ToString());
                imapClient.Fetch(
                    true,
                    seqSet,
                    new IMAP_Fetch_DataItem[]{
                        new IMAP_Fetch_DataItem_Rfc822()
                    },
                    fetchHandler
                );
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void ImapMessageExpunged(object sender, EventArgs<IMAP_r_u_Expunge> e)
        {
            if (messageHeaderList.Count >= e.Value.SeqNo)
            {
                messageHeaderList.RemoveAt(e.Value.SeqNo - 1);
            }
        }

        private void FetchHandlerNextMessage(object s, EventArgs e)
        {
            if (messageHeader != null)
            {
                messageHeaderList.Add(messageHeader);
            }
            messageHeader = new MessageHeader();
        }

        private void FetchHandlerEnvelope(object s, EventArgs<IMAP_Envelope> e)
        {
            IMAP_Envelope envelope = e.Value;
            string from = string.Empty;
            if (envelope.From != null)
            {
                for (int i = 0; i < envelope.From.Length; i++)
                {
                    if (i == envelope.From.Length - 1)
                    {
                        from += envelope.From[i].ToString();
                    }
                    else
                    {
                        from += envelope.From[i] + ";";
                    }
                }
            }
            else
            {
                from = "<none>";
            }
            messageHeader.From = from;
            messageHeader.Subject = envelope.Subject ?? "<none>";
        }

        private void FetchHandlerInternalDate(object s, EventArgs<DateTime> e)
        {
            messageHeader.Received = e.Value.ToString();
        }

        private void FetchHandlerRfc822Size(object s, EventArgs<int> e)
        {
            messageHeader.Size = (e.Value / (decimal)1000).ToString("f2") + " kb";
        }

        private void FetchHandlerUID(object s, EventArgs<long> e)
        {
            messageHeader.UID = e.Value;
        }

        private MemoryStream storeStream = new MemoryStream();

        private void FetchHandlerRfc822(object s, IMAP_Client_Fetch_Rfc822_EArgs e)
        {
            storeStream = new MemoryStream();
            e.Stream = storeStream;
            e.StoringCompleted += ImapClientFetchRfc822EArgsStoringCompleted;
        }

        private void ImapClientFetchRfc822EArgsStoringCompleted(object s1, EventArgs e1)
        {
            storeStream.Position = 0;
            Mail_Message mime = Mail_Message.ParseFromStream(storeStream);
            MessageWhole messageWhole = new MessageWhole();
            messageWhole.Mime = mime;
            messageWhole.UID = currentLoadMessageUID;
            if (mime.From != null)
            {
                messageWhole.From = mime.From.ToString();
            }
            if (!String.IsNullOrEmpty(mime.Subject))
            {
                messageWhole.Subject = mime.Subject;
            }
            if (mime.BodyText != null)
            {
                messageWhole.BodyText = mime.BodyText;
            }
            if (mime.BodyHtmlText != null)
            {
                messageWhole.BodyHTMLText = mime.BodyHtmlText;
            }
            foreach (MIME_Entity entity in mime.Attachments)
            {
                HotelSupervisorService.Objects.Communication.Attachment attachment = new HotelSupervisorService.Objects.Communication.Attachment();
                attachment.MimeEntity = entity;
                if (entity.ContentDisposition != null && entity.ContentDisposition.Param_FileName != null)
                {
                    attachment.Text = entity.ContentDisposition.Param_FileName;
                }
                else
                {
                    attachment.Text = "untitled";
                }
                messageWhole.AttachmentList.Add(attachment);
            }
            messageWholeList.Add(messageWhole);
        }

        public void DeleteMessage(long uid,string folder)
        {
            IMAP_SequenceSet sequenceSet = new IMAP_SequenceSet();
            sequenceSet.Parse(uid.ToString());
            imapClient.SelectFolder(folder);
            imapClient.StoreMessageFlags(true, sequenceSet, IMAP_Flags_SetType.Add, IMAP_MessageFlags.Deleted);
            imapClient.Expunge();
        }

        public void MoveMessage(long uid,string fromFolder, string toTargetFolder)
        {
            if (CopyMessage(uid, fromFolder, toTargetFolder))
            {
                DeleteMessage(uid, fromFolder);
            }
        }

        private bool CopyMessage(long uid, string fromFolder,string toTargetFolder)
        {
            IMAP_SequenceSet sequenceSet = new IMAP_SequenceSet();
            sequenceSet.Parse(uid.ToString());
            imapClient.SelectFolder(fromFolder);
            imapClient.CopyMessages(true, sequenceSet, toTargetFolder);
            return true;
        }
    }
}
