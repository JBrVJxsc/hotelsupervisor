using System;

namespace HotelSupervisorClient.Objects
{
    public class LocalGuest : IComparable
    {
        private string guestNumber = string.Empty;
        private string cardNumber = string.Empty;
        private string name = string.Empty;
        private string loginTime = string.Empty;
        private string loginRoom = string.Empty;

        public string GuestNumber
        {
            get
            {
                return guestNumber;
            }
            set
            {
                guestNumber = value;
            }
        }

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

        public string LoginTime
        {
            get
            {
                return loginTime;
            }
            set
            {
                loginTime = value;
            }
        }

        public string LoginRoom
        {
            get
            {
                return loginRoom;
            }
            set
            {
                loginRoom = value;
            }
        }

        #region IComparable 成员

        public int CompareTo(object obj)
        {
            LocalGuest localGuest = obj as LocalGuest;
            long a = Convert.ToInt64(guestNumber.Substring(guestNumber.Length - 18));
            long b = Convert.ToInt64(localGuest.GuestNumber.Substring(localGuest.GuestNumber.Length - 18));
            if (a > b)
            {
                return 1;
            }
            else if (a < b)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }

        #endregion
    }
}
