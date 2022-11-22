using BookingApp.Models.Bookings.Contracts;
using BookingApp.Models.Hotels.Contacts;
using BookingApp.Models.Rooms.Contracts;
using BookingApp.Repositories;
using BookingApp.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookingApp.Models.Hotels
{
    public class Hotel : IHotel
    {
        private string fullName;
        private int category;
        private IRepository<IRoom> rooms;
        private IRepository<IBooking> bookings;

        public Hotel(string fullName, int category)
        {
            this.fullName = fullName;
            this.category = category;
            rooms = new RoomRepository();
            bookings = new BookingRepository();
        }

        public string FullName 
        {
            get => this.fullName;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException(Utilities.Messages.ExceptionMessages.HotelNameNullOrEmpty);
                }

                this.fullName = value;
            }
        }

        public int Category
        {
            get => this.category;
            private set
            {
                if (value < 1 || value > 5)
                {
                    throw new ArgumentException(Utilities.Messages.ExceptionMessages.InvalidCategory);
                }

                this.category = value;
            }
        }

        public double Turnover
        {
            get => Math.Round(Sum(), 2);
        }

        private double Sum()
        {
            double sum = 0;

            foreach (var booking in bookings.All())
            {
                sum += booking.ResidenceDuration * booking.Room.PricePerNight;
            }

            return sum;
        }

        public IRepository<IRoom> Rooms
        {
            get => this.rooms;
        }

        public IRepository<IBooking> Bookings
        {
            get => this.bookings;
        }
    }
}
