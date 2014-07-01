using System;
using HotelSupervisorService.Enums;
using HotelSupervisorService.Managers;

namespace HotelSupervisorService.Objects
{
    /// <summary>
    /// 嫌疑人。
    /// </summary>
    public class Suspect
    {
        public Suspect()
        {
            id = GuidManager.GetNewGuid();
        }

        private string id = string.Empty;
        private CheckSource checkSource = CheckSource.天网追逃;
        private string name = string.Empty;
        private string nameOther = string.Empty;
        private string cardNumber = string.Empty;
        private string homeLocation = string.Empty;
        private string homeLocationNow = string.Empty;
        private string charge = string.Empty;
        private string lastAppearHotelCode = string.Empty;
        private string lastAppearHotelName = string.Empty;
        private string lastAppearHotelRoom = string.Empty;
        private string lastAppearTime = string.Empty;
        private bool handled = false;
        private DateTime handleTime = new DateTime(0);
        private DateTime historyCreateTime = DateTime.Now;
        private string contacts = string.Empty;
        private string contactsTel = string.Empty;
        private Hotel hotel = new Hotel();

        public string ID
        {
            get 
            {
                return id;
            }
            set
            {
                id = value;
            }
        }

        /// <summary>
        /// 辨别来源。
        /// </summary>
        public CheckSource CheckSource
        {
            get
            {
                return checkSource;
            }
            set
            {
                checkSource = value;
            }
        }

        /// <summary>
        /// 姓名。
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        /// <summary>
        /// 其他姓名。
        /// </summary>
        public string NameOther
        {
            get
            {
                return nameOther;
            }
            set
            {
                nameOther = value;
            }
        }

        /// <summary>
        /// 身份证号。
        /// </summary>
        public string CardNumber
        {
            get
            {
                return cardNumber;
            }
            set
            {
                cardNumber = value;
            }
        }

        /// <summary>
        /// 户籍地。
        /// </summary>
        public string HomeLocation
        {
            get
            {
                return homeLocation;
            }
            set
            {
                homeLocation = value;
            }
        }

        /// <summary>
        /// 现住址。
        /// </summary>
        public string HomeLocationNow
        {
            get
            {
                return homeLocationNow;
            }
            set
            {
                homeLocationNow = value;
            }
        }

        /// <summary>
        /// 嫌疑罪名。
        /// </summary>
        public string Charge
        {
            get
            {
                return charge;
            }
            set
            {
                charge = value;
            }
        }

        /// <summary>
        /// 上次出现旅店编号。
        /// </summary>
        public string LastAppearHotelCode
        {
            get
            {
                return lastAppearHotelCode;
            }
            set
            {
                lastAppearHotelCode = value;
            }
        }

        /// <summary>
        /// 上次出现旅店名称。
        /// </summary>
        public string LastAppearHotelName
        {
            get
            {
                return lastAppearHotelName;
            }
            set
            {
                lastAppearHotelName = value;
            }
        }

        /// <summary>
        /// 上次出现旅店房间。
        /// </summary>
        public string LastAppearHotelRoom
        {
            get
            {
                return lastAppearHotelRoom;
            }
            set
            {
                lastAppearHotelRoom = value;
            }
        }

        /// <summary>
        /// 上次出现时间。
        /// </summary>
        public string LastAppearTime
        {
            get
            {
                return lastAppearTime;
            }
            set
            {
                lastAppearTime = value;
            }
        }

        public bool Handled
        {
            get
            {
                return handled;
            }
            set
            {
                handled = value;
            }
        }

        public DateTime HandleTime
        {
            get
            {
                return handleTime;
            }
            set
            {
                handleTime = value;
            }
        }

        public DateTime HistoryCreateTime
        {
            get
            {
                return historyCreateTime;
            }
            set
            {
                historyCreateTime = value;
            }
        }

        public string Contacts
        {
            get
            {
                return contacts;
            }
            set
            {
                contacts = value;
            }
        }

        public string ContactsTel
        {
            get
            {
                return contactsTel;
            }
            set
            {
                contactsTel = value;
            }
        }

        public Hotel Hotel
        {
            get
            {
                return hotel;
            }
            set
            {
                hotel = value;
            }
        }
    }
}
