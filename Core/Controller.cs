using BookingApp.Core.Contracts;
using BookingApp.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookingApp.Core
{
    public class Controller : IController
    {
        private HotelRepository hotels;

        public Controller()
        {

        }

        public string AddHotel(string hotelName, int category)
        {
            throw new NotImplementedException();
        }

        public string BookAvailableRoom(int adults, int children, int duration, int category)
        {
            throw new NotImplementedException();
        }

        public string HotelReport(string hotelName)
        {
            throw new NotImplementedException();
        }

        public string SetRoomPrices(string hotelName, string roomTypeName, double price)
        {
            throw new NotImplementedException();
        }

        public string UploadRoomTypes(string hotelName, string roomTypeName)
        {
            throw new NotImplementedException();
        }
    }
}
