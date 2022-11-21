using BookingApp.Models.Rooms.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookingApp.Models.Rooms
{
    public class Room : IRoom
    {
        private int bedCapacity;
        private double pricePerNight;

        public Room(int bedCapacity)
        {
            this.pricePerNight = 0;
            this.bedCapacity = bedCapacity;
        }

        public int BedCapacity 
        {
            get => this.bedCapacity;
        }

        public double PricePerNight
        {
            get => this.pricePerNight;
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentException(Utilities.Messages.ExceptionMessages.PricePerNightNegative);
                }

                this.pricePerNight = value;
            }
        }

        public void SetPrice(double price)
        {
            this.PricePerNight = price;
        }
    }
}
