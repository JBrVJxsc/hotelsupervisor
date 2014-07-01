using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using HotelSupervisorClient.Interfaces;
using HotelSupervisorClient.Objects;
using HotelSupervisorClient.Objects.Communication;
using LumiSoft.Net;
using LumiSoft.Net.IMAP;
using LumiSoft.Net.IMAP.Client;
using LumiSoft.Net.Mail;
using LumiSoft.Net.MIME;

namespace HotelSupervisorClient.Managers
{
    internal class CommunicationManager
    {
        public CommunicationManager()
        {
            Init();
        }

        private IMAP_Client imapClient;
        private string defaultFolder = EncryptionManager.Decrypt(global::HotelSupervisorClient.Properties.Resources.MessageDefaultFolder);
        private List<MessageHeader> messageHeaderList = new List<MessageHeader>();
        private MessageHeader messageHeader;
        private List<MessageWhole> messageWholeList = new List<MessageWhole>();
        private SmtpClient smtpClient = new SmtpClient(EncryptionManager.Decrypt(global::HotelSupervisorClient.Properties.Resources.SendMessageServer), 25);
        private MailMessage mailMessage = new MailMessage();
        private string accountsString = EncryptionManager.Decrypt(global::HotelSupervisorClient.Properties.Resources.SendMessageAccounts);
        private string sendMessageServiceProvider = EncryptionManager.Decrypt(global::HotelSupervisorClient.Properties.Resources.SendMessageServiceProvider);
        string server = EncryptionManager.Decrypt(global::HotelSupervisorClient.Properties.Resources.ReceiveMessageServer);
        string sendRegisterMessageUser = EncryptionManager.Decrypt(global::HotelSupervisorClient.Properties.Resources.SendRegisterMessageUser);
        string sendRegisterMessagePassword = EncryptionManager.Decrypt(global::HotelSupervisorClient.Properties.Resources.SendRegisterMessagePassword);
        private TimeoutManager timeoutManager = new TimeoutManager();
        private TimeSpan timeOutTimeSpan = new TimeSpan(0, 0, 0, 60);
        private string[] accounts;
        private int currentAccountIndex = 0;

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
                throw new Exception("22。"+e.Message);
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
        public MessageWhole GetLastMessageWhole()
        {
            if (LoadMessageWholes("*:*") <= 0)
            {
                return null;
            }
            else
            {
                return messageWholeList[0];
            }
        }

        /// <summary>
        /// 获取最新的一封信息头。
        /// </summary>
        /// <returns>信息头实体。</returns>
        public MessageHeader GetLastMessageHeader()
        {
            if (LoadMessageHeaders("*:*") <= 0)
            {
                return null;
            }
            else
            {
                return messageHeaderList[0];
            }
        }

        public void SendNewGuestMessage(LocalGuest localGuest,HotelInfo hotelInfo)
        {
            try
            {
                string[] account = GetSendMessageAccount();
                smtpClient.Credentials = new NetworkCredential(account[0], account[1]);
                mailMessage.Subject = "&&" + global::HotelSupervisorClient.Properties.Resources.MessageNewGuestSubject;
                mailMessage.From = new MailAddress(account[0] + sendMessageServiceProvider);
                string body = localGuest.CardNumber + "&" + localGuest.Name + "&" + hotelInfo.ID + "&" + hotelInfo.Name + "&" + localGuest.LoginRoom + "&" + localGuest.LoginTime;
                mailMessage.Body = EncryptionManager.Encrypt(body) + "$";
                smtpClient.Send(mailMessage);
            }
            catch (Exception e)
            {
                throw new Exception("21。"+e.Message);
            }
        }

        public void SendCommandMessage(ICommand iCommand, string subject, string body)
        {
            try
            {
                smtpClient.Credentials = new NetworkCredential(iCommand.UserName, iCommand.Password);
                mailMessage.Subject = "&&" + subject;
                mailMessage.From = new MailAddress(iCommand.UserName + sendMessageServiceProvider);
                mailMessage.Body = body + "$";
                smtpClient.Send(mailMessage);
            }
            catch (Exception e)
            {
                throw new Exception("31。" + e.Message);
            }
        }

        public void SendRegisterMessage(HotelInfo hotelInfo)
        {
            try
            {
                sendRegisterMessageUser = EncryptionManager.Decrypt(global::HotelSupervisorClient.Properties.Resources.SendRegisterMessageUser);
                sendRegisterMessagePassword = EncryptionManager.Decrypt(global::HotelSupervisorClient.Properties.Resources.SendRegisterMessagePassword);
                smtpClient.Credentials = new NetworkCredential(sendRegisterMessageUser, sendRegisterMessagePassword);
                mailMessage.Subject ="&&"+ global::HotelSupervisorClient.Properties.Resources.MessageRegisterSubject;
                mailMessage.From = new MailAddress(sendRegisterMessageUser + sendMessageServiceProvider);
                string body = hotelInfo.ID + "&" + hotelInfo.Name + "&" + hotelInfo.Location + "&" + hotelInfo.Tel + "&" + Program.Version;
                mailMessage.Body = EncryptionManager.Encrypt(body) + "$";
                smtpClient.Send(mailMessage);
            }
            catch (Exception e)
            {
                throw new Exception("34。" + e.Message);
            }
        }

        /// <summary>
        /// 多组发送账号轮流使用。
        /// </summary>
        /// <returns>账号。</returns>
        private string[] GetSendMessageAccount()
        {
            string[] account;
            account = accounts[currentAccountIndex].Split(',');
            currentAccountIndex++;
            if (currentAccountIndex == accounts.Length)
            {
                currentAccountIndex = 0;
            }
            return account;
        }

        private void Init()
        {
            accounts = accountsString.Split(';');
            mailMessage.To.Add(EncryptionManager.Decrypt(global::HotelSupervisorClient.Properties.Resources.MessageServerLocation));
        }

        private void Clear()
        {
            messageHeaderList = new List<MessageHeader>();
            messageHeader = null;
            messageWholeList = new List<MessageWhole>();
        }

        private int LoadMessageWholes(string sequence)
        {
            if (imapClient == null || !imapClient.IsConnected)
            {
                return -1;
            }
            LoadMessageHeaders(sequence);
            try
            {
                foreach (MessageHeader header in messageHeaderList)
                {
                    LoadMessageWhole(header.UID);
                }
            }
            catch (Exception e)
            {
                throw new Exception("23。"+e.Message);
            }
            return messageWholeList.Count; ;
        }

        private int LoadMessageHeaders(string sequence)
        {
            try
            {
                Clear();
                imapClient.SelectFolder(defaultFolder);
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
            }
            catch (Exception e)
            {
                throw new Exception("3。" + e.Message);
            }
            return messageHeaderList.Count;
        }

        private long currentLoadMessageUID = -1;

        private void LoadMessageWhole(long uid)
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

        private  MemoryStream storeStream = new MemoryStream();

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
            messageWhole.Mime=mime;
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
                HotelSupervisorClient.Objects.Communication.Attachment attachment = new HotelSupervisorClient.Objects.Communication.Attachment();
                attachment.MimeEntity=entity;
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
    }
}
