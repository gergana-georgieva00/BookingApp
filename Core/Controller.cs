using BookingApp.Core.Contracts;
using BookingApp.Models.Hotels;
using BookingApp.Models.Rooms;
using BookingApp.Models.Rooms.Contracts;
using BookingApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
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
            Hotel hotel = new Hotel(hotelName, category);

            if (hotels.All().Any(h => h.FullName == hotelName))
            {
                return $"Hotel {hotelName} is already registered in our platform.";
            }

            this.hotels.AddNew(hotel);
            return $"{category} stars hotel {hotelName} is registered in our platform and expects room availability to be uploaded.";
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
            if (!this.hotels.All().Any(h => h.FullName == hotelName))
            {
                return $"Profile {hotelName} doesn’t exist!";
            }

            bool containsRoomType = false;
            foreach (var hotel in this.hotels.All())
            {
                foreach (var room in hotel.Rooms.All())
                {
                    if (room.GetType().Name == roomTypeName)
                    {
                        containsRoomType = true;
                        break;
                    }
                }
            }

            if (containsRoomType)
            {
                return "Room type is already created!";
            }

            IRoom room;
            switch (roomTypeName)
            {
                case "Apartment":
                    room = new Apartment();
                    break;
                case "DoubleBed":
                    room = new DoubleBed();
                    break;
                case "Studio":
                    room = new Studio();
                    break;
                default:
                    throw new ArgumentException(Utilities.Messages.ExceptionMessages.RoomTypeIncorrect);
            }
        }
    }
}
