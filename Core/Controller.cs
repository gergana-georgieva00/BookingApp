using BookingApp.Core.Contracts;
using BookingApp.Models.Bookings;
using BookingApp.Models.Hotels;
using BookingApp.Models.Hotels.Contacts;
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

            var orderedHotels = this.hotels.All().OrderBy(h => h.FullName).ToList();
            var roomsWithSetPrice = new List<IRoom>();

            foreach (var hotel in orderedHotels)
            {
                foreach (var room in hotel.Rooms.All())
                {
                    if (room.PricePerNight > 0)
                    {
                        roomsWithSetPrice.Add(room);
                    }
                }
            }

            roomsWithSetPrice = roomsWithSetPrice.OrderBy(r => r.BedCapacity).ToList();
            int guestsCount = adults + children;
            var roomsLowestCapacity = roomsWithSetPrice.Where(r => r.BedCapacity >= guestsCount).ToList();

            

            if (!roomsLowestCapacity.Any())
            {
                return "We cannot offer appropriate room for your request.";
            }

            var roomFound = roomsLowestCapacity[0];
            IHotel hotelToBook;
            int bookingNumber = 0;
            string hotelName = "";
            foreach (var hotel in orderedHotels.Where(h => h.Category == category))
            {
                foreach (var room in hotel.Rooms.All().OrderBy(r => r.BedCapacity))
                {
                    if (room.PricePerNight > 0 && room.BedCapacity >= guestsCount)
                    {
                        hotelToBook = hotel;
                        bookingNumber = hotel.Bookings.All().Count + 1;
                        var booking = new Booking(room, duration, adults, children, bookingNumber);
                        hotelToBook.Bookings.AddNew(booking);
                        break;
                    }
                }
            }

            return $"Booking number {bookingNumber} for {hotelName} hotel is successful!";
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
            if (!this.hotels.All().Any(h => h.FullName == hotelName))
            {
                return $"Profile {hotelName} doesn’t exist!";
            }

            IRoom roomNew;
            foreach (var hotel in hotels.All())
            {
                foreach (var room in hotel.Rooms.All())
                {
                    if (room.GetType().Name == roomTypeName)
                    {
                        roomNew = room;

                        var currHotel = this.hotels.All().Where(h => h.FullName == hotelName).ToList()[0];
                        switch (roomTypeName)
                        {
                            case "Apartment":
                                if (!currHotel.Rooms.All().Any(r => r.GetType().Name == roomTypeName))
                                {
                                    return "Room type is not created yet!";
                                }

                                if (roomNew.PricePerNight == 0)
                                {
                                    roomNew.SetPrice(price);
                                    return $"Price of {roomTypeName} room type in {hotelName} hotel is set!";
                                }

                                throw new InvalidOperationException(Utilities.Messages.ExceptionMessages.PriceAlreadySet);
                            case "DoubleBed":
                                if (!currHotel.Rooms.All().Any(r => r.GetType().Name == roomTypeName))
                                {
                                    return "Room type is not created yet!";
                                }

                                if (roomNew.PricePerNight == 0)
                                {
                                    roomNew.SetPrice(price);
                                    return $"Price of {roomTypeName} room type in {hotelName} hotel is set!";
                                }

                                throw new InvalidOperationException(Utilities.Messages.ExceptionMessages.PriceAlreadySet);
                            case "Studio":
                                if (!currHotel.Rooms.All().Any(r => r.GetType().Name == roomTypeName))
                                {
                                    return "Room type is not created yet!";
                                }

                                if (roomNew.PricePerNight == 0)
                                {
                                    roomNew.SetPrice(price);
                                    return $"Price of {roomTypeName} room type in {hotelName} hotel is set!";
                                }

                                throw new InvalidOperationException(Utilities.Messages.ExceptionMessages.PriceAlreadySet);
                            default:
                                throw new ArgumentException(Utilities.Messages.ExceptionMessages.RoomTypeIncorrect);
                        }
                    }
                }
            }

            return null;
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
