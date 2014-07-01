using System;
using System.Collections.Generic;
using HotelSupervisorService.Exceptions;
using HotelSupervisorService.Objects;

namespace HotelSupervisorService.Managers
{
    public class SuspectManager
    {
        public event AlertHandle Alert;

        public void Check(Guest guest,Hotel hotel)
        {
            try
            {
                List<Suspect> suspectList = new List<Suspect>();
                hotel = DataBaseManager.GlobalDataBaseManager.GetHotelInfoByHotelCode(hotel.Code);
                List<Suspect> tempList = DataBaseManager.GlobalDataBaseManager.CheckSuspectFromCompare(guest.CardNumber);
                if (tempList != null)
                {
                    foreach (Suspect suspectTemp in tempList)
                    {
                        suspectTemp.Hotel = hotel;
                        suspectTemp.CardNumber = guest.CardNumber;
                        suspectTemp.Name = guest.Name;
                        suspectTemp.LastAppearHotelCode = hotel.Code;
                        suspectTemp.LastAppearHotelName = hotel.Name;
                        suspectTemp.LastAppearHotelRoom = guest.LogRoom;
                        suspectTemp.LastAppearTime = guest.LogTime;
                        suspectTemp.CheckSource = HotelSupervisorService.Enums.CheckSource.天网追逃;
                        suspectList.Add(suspectTemp);
                    }
                }
                Suspect suspect = DataBaseManager.GlobalDataBaseManager.CheckSuspectFromLocal(guest.CardNumber);
                if (suspect != null)
                {
                    suspect.Hotel = hotel;
                    suspect.CardNumber = guest.CardNumber;
                    suspect.Name = guest.Name;
                    suspect.LastAppearHotelCode = hotel.Code;
                    suspect.LastAppearHotelName = hotel.Name;
                    suspect.LastAppearHotelRoom = guest.LogRoom;
                    suspect.LastAppearTime = guest.LogTime;
                    suspect.CheckSource = HotelSupervisorService.Enums.CheckSource.特殊监控0;
                    suspectList.Add(suspect);
                }
                bool isSuspect = DataBaseManager.GlobalDataBaseManager.CheckSuspectFromLocalFuzzy(guest.CardNumber);
                if (isSuspect)
                {
                    suspect = new Suspect();
                    suspect.Hotel = hotel;
                    suspect.CardNumber = guest.CardNumber;
                    suspect.Name = guest.Name;
                    suspect.LastAppearHotelCode = hotel.Code;
                    suspect.LastAppearHotelName = hotel.Name;
                    suspect.LastAppearHotelRoom = guest.LogRoom;
                    suspect.LastAppearTime = guest.LogTime;
                    suspect.CheckSource = HotelSupervisorService.Enums.CheckSource.特殊监控1;
                    suspectList.Add(suspect);
                }
                if (suspectList.Count != 0)
                {
                    foreach (Suspect suspectTemp in suspectList)
                    {
                        DataBaseManager.GlobalDataBaseManager.InsertSuspectHistory(suspectTemp);
                    }
                    if (Alert != null)
                    {
                        Alert(suspectList);
                    }
                }
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(ExceptionPlus))
                {
                    throw;
                }
                throw new ExceptionPlus("107。", e);
            }
        }
    }

    public delegate void AlertHandle(List<Suspect> suspectList);
}
