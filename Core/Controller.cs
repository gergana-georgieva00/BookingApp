using BookingApp.Core.Contracts;
using BookingApp.Models.Bookings;
using BookingApp.Models.Hotels;
using BookingApp.Models.Hotels.Contacts;
using BookingApp.Models.Rooms;
using BookingApp.Models.Rooms.Contracts;
using BookingApp.Repositories;
using BookingApp.Utilities.Messages;
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
            this.hotels = new HotelRepository();
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
            if (!hotels.All().Any(h => h.Category == category))
            {
                return $"{category} star hotel is not available in our platform.";
            }

            var availableRooms = new Dictionary<IRoom, string>();

            foreach (var hotel in hotels.All().Where(x => x.Category == category).OrderBy(x => x.FullName))
                foreach (var room in hotel.Rooms.All())
                    if (room.PricePerNight > 0)
                        availableRooms.Add(room, hotel.FullName);

            IRoom roomToBook = null;
            string hotelNameToBook = string.Empty;
            int people = adults + children;

            foreach (var room in availableRooms.OrderBy(x => x.Key.BedCapacity))
            {
                if (room.Key.BedCapacity >= people)
                {
                    roomToBook = room.Key;
                    hotelNameToBook = room.Value;
                    break;
                }
            }

            if (roomToBook == null)
                return String.Format(OutputMessages.RoomNotAppropriate);

            IHotel hotelToBook = hotels.Select(hotelNameToBook);
            int newBookingNumber = hotelToBook.Bookings.All().Count + 1;

            Booking newBooking = new Booking(roomToBook, duration, adults, children, newBookingNumber);
            hotelToBook.Bookings.AddNew(newBooking);

            return String.Format(OutputMessages.BookingSuccessful, newBookingNumber, hotelNameToBook);
        }

        public string HotelReport(string hotelName)
        {
            if (!this.hotels.All().Any(h => h.FullName == hotelName))
            {
                return $"Profile {hotelName} doesn’t exist!";
            }

            var hotel = this.hotels.All().ToList().Find(h => h.FullName == hotelName);

            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Hotel name: {hotelName}");
            sb.AppendLine($"--{hotel.Category} star hotel");
            sb.AppendLine($"--Turnover: {hotel.Turnover:F2} $");
            sb.AppendLine($"--Bookings:");
            sb.AppendLine();

            if (hotel.Bookings.All().Count == 0)
            {
                return sb.ToString() + "none";
            }

            foreach (var reservation in hotel.Bookings.All())
            {
                sb.AppendLine(reservation.BookingSummary());
            }

            return sb.ToString().Trim();
        }

        public string SetRoomPrices(string hotelName, string roomTypeName, double price)
        {
            if (!hotels.All().Any(x => x.FullName == hotelName))
                return String.Format(OutputMessages.HotelNameInvalid, hotelName);

            if (!new string[] { "Apartment", "DoubleBed", "Studio" }.Contains(roomTypeName))
                throw new ArgumentException(ExceptionMessages.RoomTypeIncorrect);

            IHotel hotel = hotels.All().First(x => x.FullName == hotelName);
            if (!hotel.Rooms.All().Any(x => x.GetType().Name == roomTypeName))
                return OutputMessages.RoomTypeNotCreated;

            IRoom room = hotel.Rooms.All().First(x => x.GetType().Name == roomTypeName);
            if (room.PricePerNight != 0)
                throw new InvalidOperationException(ExceptionMessages.PriceAlreadySet);

            room.SetPrice(price);
            return string.Format(OutputMessages.PriceSetSuccessfully, roomTypeName, hotelName);
        }

        public string UploadRoomTypes(string hotelName, string roomTypeName)
        {
            if (!this.hotels.All().Any(h => h.FullName == hotelName))
            {
                return $"Profile {hotelName} doesn’t exist!";
            }

            var hotelCurr = this.hotels.All().ToList().Find(h => h.FullName == hotelName);

            if (hotelCurr.Rooms.All().Any(r => r.GetType().Name == roomTypeName))
            {
                return "Room type is already created!";
            }

            IRoom roomNew;
            
            switch (roomTypeName)
            {
                case "Apartment":
                    roomNew = new Apartment();
                    hotelCurr.Rooms.AddNew(roomNew);
                    return $"Successfully added Apartment room type in {hotelName} hotel!";
                case "DoubleBed":
                    roomNew = new DoubleBed();
                    hotelCurr.Rooms.AddNew(roomNew);
                    return $"Successfully added DoubleBed room type in {hotelName} hotel!";
                case "Studio":
                    roomNew = new Studio();
                    hotelCurr.Rooms.AddNew(roomNew);
                    return $"Successfully added Studio room type in {hotelName} hotel!";
                default:
                    throw new ArgumentException(Utilities.Messages.ExceptionMessages.RoomTypeIncorrect);
            }
        }
    }
}
